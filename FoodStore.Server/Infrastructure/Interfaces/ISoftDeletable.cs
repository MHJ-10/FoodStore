namespace FoodStore.Server.Infrastructure.Interfaces;

public interface ISoftDeletable
{
    bool IsDeleted { get; set; }

    DateTime? DeletedOnUtc { get; set; }
}
