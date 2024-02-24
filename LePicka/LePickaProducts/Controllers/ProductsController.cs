using MediatR;
using Microsoft.AspNetCore.Mvc;
using LePickaProducts.Application.Queries.Products;
using LePickaProducts.Application.Commands;

namespace LePickaProducts.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;
        private readonly IMediator _mediator;
        
        public ProductsController(ILogger<ProductsController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> Test() 
        {
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> TestAdd([FromBody] AddProductRequest request)
        {
            var prod = await _mediator.Send(new AddProductCommand(request.Name, request.Description, request.Category, request.Price));

            return Ok(prod);
        }
    }
}
