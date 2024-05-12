using LePickaOrders.Application.Queries.Products;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LePickaOrders.API.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IMediator _mediator;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get()
        {
            var x = await _mediator.Send(new GetProductQuery() { Id = 1 });
            return Ok(x);
        }
    }
}
