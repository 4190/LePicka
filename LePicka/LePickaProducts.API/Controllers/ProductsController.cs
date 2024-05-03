using MediatR;
using Microsoft.AspNetCore.Mvc;
using LePickaProducts.Application.Queries.Products;
using AutoMapper;
using LePickaProducts.Application.Commands.Products;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using LePickaProducts.Infrastructure.MessageBus;
using LePickaProducts.Application.Dtos;

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

        [HttpPost]
        public async Task<ActionResult> TestAdd([FromBody] AddProductRequest request)
        {
            AddProductCommand command = _mapper.Map<AddProductCommand>(request);
            var prod = await _mediator.Send(command);

            return Ok(prod);
        }
      
        [HttpGet("{id}")]
        public async Task<ActionResult> Get(int id)
        {

            var product = await _mediator.Send(new GetProductQuery() { Id = id });
            return Ok(product);
        }
        [HttpGet]

        public async Task<ActionResult> GetAll()
        {

            var products = await _mediator.Send(new GetAllProductsQuery());
            return Ok(products);
        }

        [HttpPut]
        public async Task<ActionResult> Edit(ProductDto productdto)
        {
            EditProductCommand command = _mapper.Map<EditProductCommand>(productdto);
            var prod = await _mediator.Send(command);
            _messageBusClient.PublishProductEdit(prod);
            return Ok(prod);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {

            var product = await _mediator.Send(new DeleteProductCommand() { Id = id });
            return Ok(product);
        }
    }
}
