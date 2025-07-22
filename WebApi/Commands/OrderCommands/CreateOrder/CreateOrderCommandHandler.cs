using MediatR;
using WebApi.Data;
using WebApi.Models;

namespace WebApi.Commands.OrderCommands.CreateOrder
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Guid>
    {
        private readonly ApplicationDbContext _dbContext;
        public CreateOrderCommandHandler(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<Guid> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var order = new Order
                {
                    Status = "Pending",
                    TotalAmount = request.TotalAmount,
                    UserId = request.UserId,
                };
                await _dbContext.Orders.AddAsync(order, cancellationToken);
                await _dbContext.SaveChangesAsync(cancellationToken);
                return order.Id;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
