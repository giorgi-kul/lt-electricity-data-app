using AutoMapper;
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
        private readonly IMapper _mapper;

        public ProcessElectricityDataCommandHandler(
            IMapper mapper,
            IMediator mediator,
            IDataContext context,
            IDataParserClient dataParser)
        {
            _mapper = mapper;
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

            int totalProcessedRecords = 0;

            try
            {
                foreach (var item in completedData)
                {
                    var grouppedData = item
                        .Where(i => i.ObtPavadinimas == "Butas")
                        .GroupBy(i => new { i.Tinklas, i.ObjGvTipas });

                    foreach (var regionData in grouppedData)
                    {
                        int regionId = await _mediator.Send(new CreateIfNotExistsCommand(regionData.Key.Tinklas));

                        DataItem dataItem = new();

                        dataItem.RegionId = regionId;
                        dataItem.ObjGvTipas = regionData.Key.ObjGvTipas;
                        dataItem.ObjNumeris = regionData.Sum(rd => rd.ObjNumeris);
                        dataItem.PPlus = regionData.Sum(rd => rd.PPlus);
                        dataItem.PMinus = regionData.Sum(rd => rd.PMinus);
                        dataItem.Date = regionData.First().PlT;

                        _context.DataItems.Add(dataItem);

                        await _context.SaveChangesAsync(cancellationToken);
                    }

                    totalProcessedRecords += grouppedData.Count();
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return totalProcessedRecords;
        }
    }
}
