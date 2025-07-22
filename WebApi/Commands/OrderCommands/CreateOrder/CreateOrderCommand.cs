using MediatR;

namespace WebApi.Commands.OrderCommands.CreateOrder
{
    public class CreateOrderCommand : IRequest<Guid>
    {
        public decimal TotalAmount { get; set; }
        public Guid UserId { get; set; }
    }
}
