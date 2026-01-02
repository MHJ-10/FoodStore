using FoodStore.Server.Domain.Enums;
using FoodStore.Server.Domain.Valueobjects;
using FoodStore.Server.Infrastructure.DataModels;
using FoodStore.Server.Settings;
using Microsoft.AspNetCore.Identity;
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
        private readonly Jwt _jwt;

        public UserService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<Jwt> jwt)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _jwt = jwt.Value;
        }

        public async Task<Authentication> RegisterAsync(Register registerRequest)
        {
            var validationError = ValidateRegister(registerRequest);
            if (validationError != null)
                return AuthFailed(validationError);

            if (await _userManager.FindByNameAsync(registerRequest.Username) != null)
                return AuthFailed($"Username {registerRequest.Username} is already taken.");

            if (await _userManager.FindByEmailAsync(registerRequest.Email) != null)
                return AuthFailed($"Email {registerRequest.Email} is already registered.");

            var user = new ApplicationUser
            {
                UserName = registerRequest.Username,
                FirstName = registerRequest.FirstName,
                LastName = registerRequest.LastName,
                Email = registerRequest.Email,
                PhoneNumber = registerRequest.PhoneNumber,
                Address = registerRequest.Address
            };

            var result = await _userManager.CreateAsync(user, registerRequest.Password);
            if (!result.Succeeded)
                return AuthFailed(result.Errors.First().Description);

            if (!await _roleManager.RoleExistsAsync(UserRole.Customer.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(UserRole.Customer.ToString()));

            await _userManager.AddToRoleAsync(user, UserRole.Customer.ToString());

            return await GenerateAuthentication(user);
        }
        public async Task<Authentication> LoginAsync(Login loginRequest)
        {
            var user = await _userManager.FindByEmailAsync(loginRequest.Email);
            if (user == null || !await _userManager.CheckPasswordAsync(user, loginRequest.Password))
                return AuthFailed($"Incorrect credentials for {loginRequest.Email}.");

            return await GenerateAuthentication(user);
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
        private string? ValidateRegister(Register register)
        {
            var usernameResult = Username.Create(register.Username);
            if (usernameResult.IsError) return usernameResult.Errors[0].Description;

            var emailResult = Email.Create(register.Email);
            if (emailResult.IsError) return emailResult.Errors[0].Description;

            var passwordResult = Password.Create(register.Password);
            if (passwordResult.IsError) return passwordResult.Errors[0].Description;

            return null;
        }
        private Authentication AuthFailed(string message) => new()
        {
            IsAuthenticated = false,
            Message = message
        };
        private async Task<Authentication> GenerateAuthentication(ApplicationUser user)
        {
            var token = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);

            return new Authentication
            {
                IsAuthenticated = true,
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Email = user.Email!,
                Username = user.UserName!,
                Role = roles.FirstOrDefault() ?? UserRole.Customer.ToString(),
                Message = "Success"
            };
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
