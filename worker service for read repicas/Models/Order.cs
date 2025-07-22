namespace worker_service_for_read_repicas.Models
{
    public class Order
    {
        public Guid Id { get; set; }
        public Guid? UserId { get; set; }
        public string Status { get; set; } = null!;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public ICollection<OrderItem> Items { get; set; } = [];
    }
}