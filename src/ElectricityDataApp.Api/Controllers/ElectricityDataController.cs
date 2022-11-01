using ElectricityDataApp.Application.Features.ElectricityData.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElectricityDataApp.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ElectricityDataController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ElectricityDataController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet(Name = "GetElectricityData")]
        public async Task<IActionResult> Get([FromQuery] GetElectricityDataQuery request)
        {
            try
            {
                return Ok(await _mediator.Send(request));
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }
    }
}