using MediatR;
using WebApi.Models;

namespace WebApi.Queries.ProductInventory.GetAllInventories
{
    public class GetAllInventoriesQuery : IRequest<IEnumerable<ItemInventory>>
    {
    }
}
