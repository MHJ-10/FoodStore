using ErrorOr;
using FoodStore.Server.Application.Users.Commands;
using FoodStore.Server.Domain.Enums;
using FoodStore.Server.Domain.Valueobjects;
using FoodStore.Server.Identity;
using FoodStore.Server.Identity.DataModels;
using FoodStore.Server.Infrastructure.DataModels;
using FoodStore.Server.Settings;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Versioning;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodStore.Server.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly TokenProvider _tokenProvider;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly UserDbContext _userDbContext;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _httpContextAccessor = httpContextAccessor;
        }
        private string? GetCurrentUserId()
        {
            var userIdString = _httpContextAccessor.HttpContext?
                .User
                .FindFirstValue(ClaimTypes.NameIdentifier);
            return userIdString;
        }
        public string ?GetCurrentUserName()
        {
            var userName = _httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Name);
            return userName;
        }

        public async Task<ErrorOr<RegisterUser.Response>> RegisterAsync(RegisterUser.Request registerRequest)
        {
            var email = Email.Create(registerRequest.Email);
            var password = Password.Create(registerRequest.Password);
            if (email.IsError)
                return email.Errors;
          
            if (password.IsError)
                return password.Errors;

            PhoneNumber? phoneNumber = null;
            if (!string.IsNullOrWhiteSpace(registerRequest.PhoneNumber))
            {
                var phoneResult = PhoneNumber.Create(registerRequest.PhoneNumber);
                if (phoneResult.IsError)
                    return phoneResult.Errors;

                phoneNumber = phoneResult.Value;
            }
           

            var user = new ApplicationUser()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.UserName,
                PhoneNumber = phoneNumber?.Value,
                Email = email.Value.Value,
                Address = registerRequest.Address,
            };
            var result = await _userManager.CreateAsync(user, password.Value.Value);
            if (!result.Succeeded)
            {
                return result.Errors
                    .Select(e => Error.Validation(
                        code: e.Code,
                        description: e.Description))
                    .ToList();
            }
            return new RegisterUser.Response()
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
            };

        }
        public async Task<ErrorOr<LoginUser.Response>> LoginAsync(LoginUser.Request loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
                return Error.NotFound("Invalid Email or Password");

            string acessToken = await _tokenProvider.GenerateAcessTokenAsync(user);
            string refreshToken = await GetOrCreateRefreshTokenAsync(user);

            return new LoginUser.Response(acessToken, refreshToken);
        }
        private async Task<string> GetOrCreateRefreshTokenAsync(ApplicationUser user)
        {
            var existingToken = await _userDbContext.RefreshTokens
                .FirstOrDefaultAsync(r => r.UserId == user.Id);

            if (existingToken is null || existingToken.ExpiresOnUtc < DateTime.UtcNow)
            {
                // Create new or update expired refresh token
                if (existingToken is null)
                {
                    existingToken = new RefreshToken
                    {
                        Id = Guid.NewGuid(),
                        UserId = user.Id,
                        ExpiresOnUtc = DateTime.UtcNow.AddDays(7),
                        Token = _tokenProvider.GenerateRefreshToken()
                    };
                    _userDbContext.RefreshTokens.Add(existingToken);
                }
                else
                {
                    existingToken.Token = _tokenProvider.GenerateRefreshToken();
                    existingToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);
                }

                await _userDbContext.SaveChangesAsync();
            }

            // Return the valid refresh token
            return existingToken.Token;
        }
        public async Task<ErrorOr<Success>> AddRoleAsync(AddRole.Request addRoleRequest)
        {
            var user = await _userManager.FindByEmailAsync(addRoleRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, addRoleRequest.Password))
                return Error.NotFound($"Incorrect Email or Password");

            if (!Enum.TryParse<UserRole>(addRoleRequest.Role, true, out var validRole))
                return Error.NotFound($"Role {addRoleRequest.Role} not found.");

            if (validRole == UserRole.Admin)
                return Error.Forbidden($"You can't assign {addRoleRequest.Role} role to any user.");

            var existingRoles = await _userManager.GetRolesAsync(user);
            if (existingRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, existingRoles);
                if (!removeResult.Succeeded)
                    return Error.Failure($"Failed to remove old roles.");
            }

            var addResult = await _userManager.AddToRoleAsync(user, validRole.ToString());
            if (!addResult.Succeeded)
                return Error.Failure($"Failed to add role.");

            return Result.Success;
        }

        public async Task<ErrorOr<LoginUserWithRefreshToken.Response>> LoginUserWithRefreshTokenAsync(LoginUserWithRefreshToken.Request request)
        {

            RefreshToken? refreshToken = await _userDbContext.RefreshTokens
                 .Include(r => r.User)
                 .FirstOrDefaultAsync(rt => rt.Token == request.RefreshToken);
            if (refreshToken is null || refreshToken.ExpiresOnUtc < DateTime.UtcNow)
            {
                return Error.NotFound("RefreshToken.Invalid", "The provided refresh token is invalid or has expired.");
            }

            string newAcessToken = await _tokenProvider.GenerateAcessTokenAsync(refreshToken.User);
            string newRefreshTokenString = _tokenProvider.GenerateRefreshToken();
            // Update the existing refresh token
            refreshToken.Token = newRefreshTokenString;
            refreshToken.ExpiresOnUtc = DateTime.UtcNow.AddDays(7);
            await _userDbContext.SaveChangesAsync();
            return new LoginUserWithRefreshToken.Response(newAcessToken, newRefreshTokenString);
        }
        public async Task<ErrorOr<Success>> RevokeRefreshTokensAsync(RevokeRefreshTokens.Request request)
        {
            // Get the logged-in user's ID from JWT
            var currentUserId = GetCurrentUserId();
            if (currentUserId is null)
            {
                return Error.Forbidden("Auth.InvalidUser", "Could not determine the current user.");
            }

            // Ensure the user is only revoking their own refresh tokens
            if (request.UserId != currentUserId)
            {
                return Error.Forbidden("RevokingRefreshToken", "You cannot revoke refresh tokens for another user.");
            }

            // Convert to GUID to match the RefreshToken.UserId property
            if (!Guid.TryParse(request.UserId, out Guid parsedUserId))
            {
                return Error.Validation("User.InvalidId", "User ID is not a valid GUID.");
            }

            // Perform fast SQL DELETE
            await _userDbContext.RefreshTokens
                .Where(r => r.UserId == request.UserId)
                .ExecuteDeleteAsync();

            return Result.Success;
        }

    }
}
