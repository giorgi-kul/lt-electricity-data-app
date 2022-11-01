using AutoMapper;
using ElectricityDataApp.Application.Interfaces;
using ElectricityDataApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.Application.Features.ElectricityData.Queries
{
    public record GetElectricityDataQuery(string? RegionName) : IRequest<IEnumerable<ElectricityDataVm>>;

    public class GetElectricityDataQueryHandler : IRequestHandler<GetElectricityDataQuery, IEnumerable<ElectricityDataVm>>
    {
        private readonly IDataContext _context;
        private readonly IMapper _mapper;

        public GetElectricityDataQueryHandler(
            IMapper mapper,
            IDataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        public async Task<IEnumerable<ElectricityDataVm>> Handle(GetElectricityDataQuery request, CancellationToken cancellationToken)
        {
            IQueryable<DataItem> items = _context.DataItems
                .Include(i => i.Region)
                .AsNoTracking();

            if (!string.IsNullOrWhiteSpace(request.RegionName))
            {
                items = items.Where(i => i.Region.Name.ToLower() == request.RegionName.ToLower());
            }

            return _mapper.Map<IEnumerable<ElectricityDataVm>>(await items.ToListAsync());
        }
    }
}
