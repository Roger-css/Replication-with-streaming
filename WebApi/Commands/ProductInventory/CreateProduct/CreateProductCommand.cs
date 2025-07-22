using MediatR;

namespace WebApi.Commands.ProductInventory.CreateProduct
{
  
    public class CreateProductCommand : IRequest<Guid>
    {
        public string ProductName { get; set; } = null!;
        public int StockQty { get; set; }
        public decimal Price { get; set; }
    }
}