using ElectricityDataApp.DataParser.Models;

namespace ElectricityDataApp.DataParser
{
    public interface IDataParserClient
    {
        Task<IEnumerable<TableData>> GetDataUrlsToProcess(int monthsToProcess);
        Task<List<Record>> ParseCsvData(string csvUrl);
    }
}
