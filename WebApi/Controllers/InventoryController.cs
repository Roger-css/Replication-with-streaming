using MediatR;
using Microsoft.AspNetCore.Mvc;
using WebApi.Commands.ProductInventory.CreateProduct;
using WebApi.Queries.ProductInventory.GetAllInventories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemInventoryController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ItemInventoryController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        public async Task<IActionResult> CreateItemInventory([FromBody] CreateProductCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }
        [HttpGet]
        public async Task<IActionResult> GetAllInventories()
        {
            var query = new GetAllInventoriesQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
