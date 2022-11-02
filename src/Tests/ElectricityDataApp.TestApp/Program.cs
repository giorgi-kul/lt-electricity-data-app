using ElectricityDataApp.DataParser;
using ElectricityDataApp.DataParser.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

DataParserClientOptions _options = new();
using IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration((hostingContext, configuration) =>
    {
        configuration.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true);

        IConfigurationRoot configurationRoot = configuration.Build();

        configurationRoot.GetSection("DataParser").Bind(_options);
    })
    .ConfigureServices((_, services) =>
    {
        services.AddDataParser(opt =>
        {
            opt.DataUrl = _options.DataUrl;
            opt.BaseUrl = _options.BaseUrl;
            opt.DateColumnIndex = _options.DateColumnIndex;
            opt.DownloadNodeTableIndex = _options.DownloadNodeTableIndex;
            opt.HttpRequestTimeoutInMinutes = _options.HttpRequestTimeoutInMinutes;
            opt.LastMonthCountToProcess = _options.LastMonthCountToProcess;
        });
    })
    .Build();



HttpClient httpClient = host.Services.GetRequiredService<HttpClient>();

IDataParserClient dataParserClient = host.Services.GetRequiredService<IDataParserClient>();

var tableData = await dataParserClient.GetDataUrlsToProcess(null);

var finalData = await Task.WhenAll(tableData.Select(td => dataParserClient.ParseCsvData(td.DataUrl)));

foreach (var item in finalData)
{
    foreach (var rec in item)
    {
        Console.WriteLine(rec.Tinklas);
    }
}

Console.ReadKey();