using ErrorOr;
using FoodStore.Server.Application.Users.Commands;
using FoodStore.Server.Domain.Enums;
using FoodStore.Server.Domain.Valueobjects;
using FoodStore.Server.Identity;
using FoodStore.Server.Identity.DataModels;
using FoodStore.Server.Infrastructure.DataModels;
using FoodStore.Server.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
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

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IHttpContextAccessor httpContextAccessor)
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

        public async Task<ErrorOr<RegisterUser.Response>> RegisterAsync(RegisterUser.Request registerRequest)
        {
            var user = new ApplicationUser()
            {
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                UserName = registerRequest.UserName,
                PhoneNumber = registerRequest.PhoneNumber,
                Email = registerRequest.Email,
                Address = registerRequest.Address
            };
            var result = await _userManager.CreateAsync(user, registerRequest.Password);
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
        public async Task<string> AddRoleAsync(AddRole addRoleRequest)
        {
            var user = await _userManager.FindByEmailAsync(addRoleRequest.Email);
            if (user == null)
                return $"No account registered with {addRoleRequest.Email}.";

            if (!await _userManager.CheckPasswordAsync(user, addRoleRequest.Password))
                return $"Incorrect credentials for user {user.Email}.";

            if (!Enum.TryParse<UserRole>(addRoleRequest.Role, true, out var validRole))
                return $"Role {addRoleRequest.Role} not found.";

            if (validRole == UserRole.Admin)
                return $"You can't assign {addRoleRequest.Role} role to any user.";

            var existingRoles = await _userManager.GetRolesAsync(user);
            if (existingRoles.Any())
            {
                var removeResult = await _userManager.RemoveFromRolesAsync(user, existingRoles);
                if (!removeResult.Succeeded)
                    return $"Failed to remove old roles.";
            }

            var addResult = await _userManager.AddToRoleAsync(user, validRole.ToString());
            if (!addResult.Succeeded)
                return $"Failed to add role.";

            return $"Success: Role {validRole} assigned to user {addRoleRequest.Email}.";
        }
        

 
    }
}
