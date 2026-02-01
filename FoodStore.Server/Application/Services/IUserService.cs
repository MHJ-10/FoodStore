using ErrorOr;
using FoodStore.Server.Application.Users.Commands;
using FoodStore.Server.Application.Users.Queries;

namespace FoodStore.Server.Application.Services
{
    public interface IUserService
    {
        Task<ErrorOr<RegisterUser.Response>> RegisterAsync(RegisterUser.Request registerRequest);
        Task<ErrorOr<LoginUser.Response>> LoginAsync(LoginUser.Request loginRequest);
        Task<ErrorOr<Success>> AddRoleAsync(AddRole.Request addRoleRequest);
        Task<ErrorOr<LoginUserWithRefreshToken.Response>> LoginUserWithRefreshTokenAsync(LoginUserWithRefreshToken.Request request);
        Task<ErrorOr<Success>> RevokeRefreshTokenAsync(RevokeRefreshToken.Request request);
        string? GetCurrentUserName();
        Task<ErrorOr<Success>> DeleteUserAsync(string userId);
        Task<ErrorOr<Success>> LogoutAsync();
        Task<ErrorOr<Success>> ConfirmEmailAsync(ConfirmEmail.Request confirmEmailRequest);
        Task<ErrorOr<IList<GetAllUsers.Response>>> GetAllUsersAsync(CancellationToken cancellationToken);
        Task<ErrorOr<GetUserByEmail.Response>> GetUserByEmailAsync(string email, CancellationToken cancellationToken);
        Task<ErrorOr<GetUserById.Response>> GetUserByIdAsync(string id, CancellationToken cancellationToken);
        Task<ErrorOr<UpdateUser.Response>> UpdateUserAsync(UpdateUser.Request updateUserRequest, CancellationToken cancellationToken);
    }
}
