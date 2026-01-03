using FoodStore.Domain.Entities;

namespace FoodStore.Application.Common.Interfaces
{
    public interface IProductRepository : IRepository<Product>
    {
        Task<IEnumerable<Product>> GetProductsByCategoryAsync(int categoryId);
        Task<IEnumerable<Product>> GetFeaturedProductsAsync(int count);
        Task<IEnumerable<Product>> SearchProductsAsync(string searchTerm);
        Task UpdateStockAsync(int productId, int quantity);
    }
}