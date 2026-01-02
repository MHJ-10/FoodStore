using ErrorOr;
using FoodStore.Server.Application.Users.Commands;
using FoodStore.Server.Infrastructure.DataModels;

namespace FoodStore.Server.Application.Services
{
    public interface IUserService
    {
        Task<ErrorOr<RegisterUser.Response>> RegisterAsync(RegisterUser.Request registerRequest);
        Task<ErrorOr<LoginUser.Response>> LoginAsync(LoginUser.Request loginRequest);
        Task<string> AddRoleAsync(AddRole addRoleRequest);
    }
}
