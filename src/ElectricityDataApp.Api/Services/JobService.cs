using ElectricityDataApp.Application.Features.ElectricityData.Commands.ProcessElectricityData;
using MediatR;

namespace ElectricityDataApp.Api.Services
{
    public class JobService : IJobService
    {
        private readonly IMediator _mediator;

        public JobService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task ProcessElectricityData()
        {
            await _mediator.Send(new ProcessElectricityDataCommand());
        }
    }
}
