using MediatR;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Commands.ProductInventory.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly ApplicationDbContext _context;

        public CreateProductCommandHandler(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            var product = new ItemInventory
            {
                ProductId = Guid.NewGuid(),
                ProductName = request.ProductName,
                StockQty = request.StockQty,
                Price = request.Price
            };

            _context.Inventories.Add(product);
            await _context.SaveChangesAsync(cancellationToken);

            return product.ProductId;
        }
    }
}
