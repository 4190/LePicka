using MediatR;
using Microsoft.AspNetCore.Mvc;
using LePickaProducts.Application.Queries.Products;
using AutoMapper;
using LePickaProducts.Application.Commands.Products;
using LePickaProducts.Infrastructure.MessageBus;
using LePickaProducts.Application.Dtos;
using LePickaProducts.Domain.Products;

namespace LePickaProducts.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductsController : ControllerBase
    {

        private readonly ILogger<ProductsController> _logger;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;
        private readonly IMessageBusClient _messageBusClient;

        public ProductsController(ILogger<ProductsController> logger, IMediator mediator, IMapper mapper, IMessageBusClient messageBusClient)
        {
            _logger = logger;
            _mediator = mediator;
            _mapper = mapper;
            _messageBusClient = messageBusClient;
        }

        [HttpGet]
        //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult> Test()
        {
            _messageBusClient.PublishTestEvent();
            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }
      
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {
            ProductDto product = await _mediator.Send(new GetProductQuery() { Id = id });
            return Ok(product);
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            List<ProductDto> products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [HttpPost]
        public async Task<ActionResult> Create(AddProductCommand command)
        {
            ProductResponse products = await _mediator.Send(command);
            return Ok(products);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(EditProductCommand command)
        {
            ProductResponse product = await _mediator.Send(command);
            _messageBusClient.PublishProductEdit(product.Product);
            return Ok(product.Product);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            ProductDto product = await _mediator.Send(new DeleteProductCommand() { Id = id });
            return Ok(product);
        }
    }
}
