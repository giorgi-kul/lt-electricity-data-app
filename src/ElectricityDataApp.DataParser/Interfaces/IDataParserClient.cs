using ElectricityDataApp.DataParser.Models;

namespace ElectricityDataApp.DataParser.Interfaces
{
    public interface IDataParserClient
    {
        Task<IEnumerable<TableData>> GetDataUrlsToProcess(DateTime? lastProcessedDate);
        Task<List<Record>> ParseCsvData(string csvUrl);
    }
}
