using FoodStore.Server.Infrastructure.DataModels;

namespace FoodStore.Domain.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; } = Guid.NewGuid().ToString().Substring(0, 10).ToUpper();
        public int UserId { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Processing, Shipped, Delivered, Cancelled
        public string PaymentStatus { get; set; } = "Pending"; // Pending, Paid, Failed
        public string PaymentMethod { get; set; }
        public string ShippingAddress { get; set; }
        public string Notes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }

        // Navigation Properties
        public User User { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}