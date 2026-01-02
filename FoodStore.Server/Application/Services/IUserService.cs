using FoodStore.Server.Infrastructure.DataModels;

namespace FoodStore.Server.Application.Services
{
    public interface IUserService
    {
        Task<Authentication> RegisterAsync(Register registerRequest);
        Task<Authentication> LoginAsync(Login loginRequest);
        Task<string> AddRoleAsync(AddRole addRoleRequest);
    }
}
