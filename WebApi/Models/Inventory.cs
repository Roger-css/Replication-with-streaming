namespace WebApi.Models
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