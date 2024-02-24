using MediatR;
using Microsoft.AspNetCore.Mvc;
using LePickaProducts.Application.Queries.Products;
using AutoMapper;
using LePickaProducts.Application.Commands.Products;

namespace LePickaProducts.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        public ProductsController(ILogger<ProductsController> logger, IMediator mediator, IMapper mapper)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
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
            AddProductCommand command = _mapper.Map<AddProductCommand>(request);
            var prod = await _mediator.Send(command);

            return Ok(prod);
        }
    }
}
