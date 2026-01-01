using FoodStore.Server.Domain.Enums;
using FoodStore.Server.Domain.Valueobjects;
using FoodStore.Server.Infrastructure.DataModels;
using FoodStore.Server.Settings;
using Humanizer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

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

            var emailResult = Email.Create(register.Email);
            var passwordResult = Password.Create(register.Password);

            if (emailResult.IsError)
            {
                return emailResult.Errors.Humanize();
            }

            if (passwordResult.IsError)
            {
                return passwordResult.Errors.Humanize();
            }


            var user = new ApplicationUser
            {
                UserName = register.Username,
                Email = register.Email,
                FirstName = register.FirstName,
                LastName = register.LastName
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(register.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRole.Customer.ToString());
                }
                return $"User Modeled with username {user.UserName}";
            }
            else
            {
                return $"Email {user.Email} is already Modeled.";
            }
        }
    }
}
