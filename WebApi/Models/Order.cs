namespace WebApi.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<OrderItem> Items { get; set; } = [];
    }
}