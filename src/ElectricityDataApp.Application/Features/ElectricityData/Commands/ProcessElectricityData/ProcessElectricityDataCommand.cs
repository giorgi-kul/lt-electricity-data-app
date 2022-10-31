using ElectricityDataApp.Application.Features.Regions.Commands.CreateIfNotExists;
using ElectricityDataApp.Application.Interfaces;
using ElectricityDataApp.DataParser;
using ElectricityDataApp.DataParser.Models;
using ElectricityDataApp.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.Application.Features.ElectricityData.Commands.ProcessElectricityData
{
    public class ProcessElectricityDataCommand : IRequest<int>
    {
    }

    public class ProcessElectricityDataCommandHandler : IRequestHandler<ProcessElectricityDataCommand, int>
    {
        private readonly IDataParserClient _dataParser;
        private readonly IDataContext _context;
        private readonly IMediator _mediator;

        public ProcessElectricityDataCommandHandler(
            IMediator mediator,
            IDataContext context,
            IDataParserClient dataParser)
        {
            _mediator = mediator;
            _dataParser = dataParser;
            _context = context;
        }

        public async Task<int> Handle(ProcessElectricityDataCommand request, CancellationToken cancellationToken)
        {
            var lastProcessedDate = await _context.DataItems.MaxAsync(item => (DateTime?)item.Date);

            var tableData = await _dataParser.GetDataUrlsToProcess(lastProcessedDate);

            var tasks = tableData.Select(td => _dataParser.ParseCsvData(td.DataUrl));

            var completedData = await Task.WhenAll(tableData.Select(td => _dataParser.ParseCsvData(td.DataUrl)));

            int totalProcessedData = 0;

            try
            {
                foreach (var item in completedData)
                {
                    var grouppedData = item
                        .Where(i => i.ObtPavadinimas == "Butas")
                        .GroupBy(i => i.Tinklas);

                    foreach (var regionData in grouppedData)
                    {
                        int regionId = await _mediator.Send(new CreateIfNotExistsCommand(regionData.Key));

                        _context.DataItems.AddRange(regionData.Select(rd => new DataItem()
                        {
                            Date = rd.PlT,
                            PPlus = rd.PPlus,
                            PMinus = rd.PMinus,
                            ObjNumeris = rd.ObjNumeris,
                            ObjGvTipas = rd.ObjGvTipas,
                            RegionId = regionId
                        }));
                        await _context.SaveChangesAsync(cancellationToken);

                        totalProcessedData += regionData.Count();
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            

            return totalProcessedData;
        }
    }
}
