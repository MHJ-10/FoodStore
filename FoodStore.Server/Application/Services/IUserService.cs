using FoodStore.Server.Infrastructure.DataModels;

namespace FoodStore.Server.Application.Services
{
    public interface IUserService
    {
        Task<string> RegisterAsync(Register register);
        Task<Authentication> GetTokenAsync(TokenRequest tokenRequest);
    }
}
