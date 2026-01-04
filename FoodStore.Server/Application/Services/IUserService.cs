using ErrorOr;
using FoodStore.Server.Application.Users.Commands;
using FoodStore.Server.Infrastructure.DataModels;
using MediatR;

namespace FoodStore.Server.Application.Services
{
    public interface IUserService
    {
        Task<ErrorOr<RegisterUser.Response>> RegisterAsync(RegisterUser.Request registerRequest);
        Task<ErrorOr<LoginUser.Response>> LoginAsync(LoginUser.Request loginRequest);
        Task<ErrorOr<Success>> AddRoleAsync(AddRole.Request addRoleRequest);
        Task<ErrorOr<LoginUserWithRefreshToken.Response>> LoginUserWithRefreshTokenAsync(LoginUserWithRefreshToken.Request request);
        Task<ErrorOr<Success>> RevokeRefreshTokensAsync(RevokeRefreshTokens.Request request);
        string? GetCurrentUserName();
        Task<ErrorOr<Success>> DeleteUserAsync(string userId);
        Task<ErrorOr<Success>> LogoutAsync();
    }
}
