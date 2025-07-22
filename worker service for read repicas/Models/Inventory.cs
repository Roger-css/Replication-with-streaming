namespace worker_service_for_read_repicas.Models
{
    public class ItemInventory
    {
        public Guid ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public int StockQty { get; set; }
        public decimal Price { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = [];
    }
}