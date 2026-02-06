using ErrorOr;


namespace FoodStore.Server.Application.Users.Errors
{
    public static class UserErrors
    {
        public static Error NoUsersAvailable =>
        Error.NotFound(
            code: "User.NoUsersAvailable",
            description: "No users are available in the store");
    }
}
