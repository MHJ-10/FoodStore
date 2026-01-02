using FoodStore.Server.Domain.Enums;
using FoodStore.Server.Domain.Valueobjects;
using FoodStore.Server.Infrastructure.DataModels;
using FoodStore.Server.Settings;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using NuGet.Packaging;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace FoodStore.Server.Application.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly Jwt _jwt;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<Jwt> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<string> RegisterAsync(Register register)
        {

            var usernameResult = Username.Create(register.Username);
            var emailResult = Email.Create(register.Email);
            var passwordResult = Password.Create(register.Password);


            if (usernameResult.IsError)
            {
                return usernameResult.Errors[0].Description;
            }

            if (emailResult.IsError)
            {
                return emailResult.Errors[0].Description;
            }

            if (passwordResult.IsError)
            {
                return passwordResult.Errors[0].Description;
            }

            var user = new ApplicationUser
            {
                UserName = register.Username,
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
                Address = register.Address,
            };

            var userWithSameUsername = await _userManager.FindByNameAsync(register.Username);
            var userWithSameEmail = await _userManager.FindByEmailAsync(register.Email);

            if (userWithSameUsername != null)
            {
                return $"Username {user.UserName} is already taken.";
            }

            if (userWithSameEmail != null)
            {
                return $"Email {user.Email} is already registered.";
            }

            var result = await _userManager.CreateAsync(user, register.Password);
            if (result.Succeeded)
            {
                var roleExists = await _roleManager.RoleExistsAsync(UserRole.Customer.ToString());
                await _userManager.AddToRoleAsync(user, UserRole.Customer.ToString());
            }

            return $"Success: User registered with email {user.Email}";
        }

        public async Task<Authentication> GetTokenAsync(TokenRequest tokenRequest)
        {
            var authenticationModel = new Authentication();
            var user = await _userManager.FindByEmailAsync(tokenRequest.Email);


            if (user == null)
            {

                authenticationModel.IsAuthenticated = false;
                authenticationModel.Message = $"No Accounts Registered with {tokenRequest.Email}.";
                return authenticationModel;
            }
            if (await _userManager.CheckPasswordAsync(user, tokenRequest.Password))
            {
                authenticationModel.IsAuthenticated = true;
                JwtSecurityToken jwtSecurityToken = await CreateJwtToken(user);

                authenticationModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
                authenticationModel.Email = user.Email!;
                authenticationModel.Username = user.UserName!;

                var roles = await _userManager.GetRolesAsync(user);
                authenticationModel.Role = roles.FirstOrDefault() ?? UserRole.Customer.ToString();

                return authenticationModel;
            }
            authenticationModel.IsAuthenticated = false;
            authenticationModel.Message = $"Incorrect Credentials for user {user.Email}.";
            return authenticationModel;
        }
        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? UserRole.Customer.ToString();


            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email!),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName!),
                new Claim(ClaimTypes.Role, role),
                new Claim("uid", user.Id)
            };

            claims.AddRange(userClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwt.DurationInMinutes),
                signingCredentials: signingCredentials);
            return jwtSecurityToken;
        }
    }
}
