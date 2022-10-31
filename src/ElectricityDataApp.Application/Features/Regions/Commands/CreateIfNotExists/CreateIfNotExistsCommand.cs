using ElectricityDataApp.Application.Features.ElectricityData.Commands.ProcessElectricityData;
using ElectricityDataApp.Application.Interfaces;
using ElectricityDataApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.Application.Features.Regions.Commands.CreateIfNotExists
{
    public record CreateIfNotExistsCommand(string Name) : IRequest<int>;

    public class ProcessElectricityDataCommandHandler : IRequestHandler<CreateIfNotExistsCommand, int>
    {
        private readonly IDataContext _context;

        public ProcessElectricityDataCommandHandler(IDataContext context)
        {
            _context = context;
        }

        public async Task<int> Handle(CreateIfNotExistsCommand request, CancellationToken cancellationToken)
        {
            var region = await _context.Regions.FirstOrDefaultAsync(r => r.Name.ToLower() == request.Name.ToLower());

            if (region == null)
            {
                _context.Regions.Add(region = new Region
                {
                    Name = request.Name
                });

                await _context.SaveChangesAsync(cancellationToken);
            }

            return region.Id;
        }
    }
}
