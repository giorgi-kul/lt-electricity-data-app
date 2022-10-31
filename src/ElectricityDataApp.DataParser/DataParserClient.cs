using CsvHelper;
using ElectricityDataApp.DataParser.Exceptions;
using ElectricityDataApp.DataParser.Extensions;
using ElectricityDataApp.DataParser.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System.Globalization;

namespace ElectricityDataApp.DataParser
{
    public class DataParserClient : IDataParserClient
    {
        private readonly HttpClient _client;
        private readonly DataParserClientOptions _options;

        public DataParserClient(HttpClient client,
            IOptions<DataParserClientOptions> options)
        {
            _client = client;
            _options = options.Value;
        }


        public async Task<List<Record>> ParseCsvData(string csvUrl)
        {
            using var response = await _client.GetAsync(csvUrl);

            response.EnsureSuccessStatusCode();

            using var content = response.Content;

            using var reader = new StreamReader(await content.ReadAsStreamAsync());

            using var csv = new CsvReader(reader, CultureInfo.InvariantCulture);

            return csv.GetRecords<Record>().ToList();
        }

        public async Task<IEnumerable<TableData>> GetDataUrlsToProcess(int monthsToProcess)
        {
            List<TableData> data = new();

            using var response = await _client.GetAsync(_options.DataUrl);

            response.EnsureSuccessStatusCode();

            using var content = response.Content;

            var result = await content.ReadAsStringAsync();

            var document = new HtmlDocument();

            document.LoadHtml(result);

            var nodes = document.DocumentNode.SelectNodes("//table[@id='resource-table']/tbody/tr");

            if (nodes?.Count == 0)
            {
                throw new PossibleStructureChangeException(nameof(nodes));
            }

            foreach (var trNode in nodes)
            {
                var tdNodes = trNode.ChildNodes.Where(cn => cn.Name == "td");

                if (tdNodes?.Count() == 0)
                {
                    throw new PossibleStructureChangeException(nameof(tdNodes));
                }

                string dateValue = tdNodes.ElementAt(_options.DateColumnIndex).InnerText;

                if (string.IsNullOrEmpty(dateValue))
                {
                    throw new PossibleStructureChangeException(nameof(dateValue));
                }

                HtmlNode? downloadNode = tdNodes.ElementAt(_options.DownloadNodeTableIndex)
                    ?.ChildNodes.First(cn => cn.Name == "div")
                    ?.ChildNodes.Last(cn => cn.Name == "a");

                if (downloadNode == null)
                {
                    throw new PossibleStructureChangeException(nameof(downloadNode));
                }

                string downloadUrl = $"{_options}{downloadNode.GetAttributeValue("href", "")}";

                var date = dateValue.ToNormalizedDate();

                data.Add(new TableData()
                {
                    DataUrl = downloadUrl,
                    Date = date
                });
            }

            return data.OrderByDescending(d => d.Date)
                       .Take(monthsToProcess);
        }
    }
}
