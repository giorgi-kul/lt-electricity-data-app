using CsvHelper;
using ElectricityDataApp.DataParser.Exceptions;
using ElectricityDataApp.DataParser.Extensions;
using ElectricityDataApp.DataParser.Interfaces;
using ElectricityDataApp.DataParser.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace ElectricityDataApp.DataParser
{
    public class DataParserClient : IDataParserClient
    {
        private readonly IHtmlParser _htmlParser;
        private readonly IHttpHelper _httpHelper;
        private readonly DataParserClientOptions _options;

        public DataParserClient(
            IHtmlParser htmlParser,
            IHttpHelper httpHelper,
            IOptions<DataParserClientOptions> options)
        {
            _htmlParser = htmlParser;
            _httpHelper = httpHelper;
            _options = options.Value;
        }


        public async Task<List<Record>> ParseCsvData(string csvUrl)
        {
            var stringContent = await _httpHelper.GetContentAsString(csvUrl);

            using TextReader reader = new StringReader(stringContent);

            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            var records = csv.GetRecords<Record>().ToList();

            return records;
        }

        public async Task<IEnumerable<TableData>> GetDataUrlsToProcess(DateTime? lastProcessedDate)
        {
            var htmlData = await _httpHelper.GetContentAsString(_options.DataUrl);

            List<TableData> data = _htmlParser.ParseTable(htmlData);

            if (lastProcessedDate.HasValue)
            {
                data = data.FindAll(d => d.Date > lastProcessedDate.Value);
            }

            return data.OrderByDescending(d => d.Date)
                       .Take(_options.LastMonthCountToProcess);
        }
    }
}