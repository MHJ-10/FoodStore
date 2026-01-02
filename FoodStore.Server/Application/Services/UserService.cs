using FoodStore.Server.Domain.Enums;
using FoodStore.Server.Domain.Valueobjects;
using FoodStore.Server.Infrastructure.DataModels;
using FoodStore.Server.Settings;
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
                return emailResult.Errors[0].Description;
            }

            if (passwordResult.IsError)
            {
                return passwordResult.Errors[0].Description;
            }


            var user = new ApplicationUser
            {
                FirstName = register.FirstName,
                LastName = register.LastName,
                Email = register.Email,
                PhoneNumber = register.PhoneNumber,
                Address = register.Address,
            };
            var userWithSameEmail = await _userManager.FindByEmailAsync(register.Email);
            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, register.Password);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, UserRole.Customer.ToString());
                }
                return $"User registered with email {user.email}";
            }
            else
            {
                return $"Email {user.Email} is already registered.";
            }
        }
    }
}
