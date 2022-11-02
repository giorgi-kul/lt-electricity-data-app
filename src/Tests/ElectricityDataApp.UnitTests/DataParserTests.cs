using ElectricityDataApp.DataParser;
using ElectricityDataApp.DataParser.Interfaces;
using ElectricityDataApp.DataParser.Models;
using Microsoft.Extensions.Options;
using Moq;
using System.Globalization;

namespace ElectricityDataApp.UnitTests
{
    public class DataParserTests
    {
        private readonly IOptions<DataParserClientOptions> _options;
        public DataParserTests()
        {
            _options = Options.Create<DataParserClientOptions>(new DataParserClientOptions()
            {
                LastMonthCountToProcess = 2
            });
        }

        [Theory]
        [InlineData(null, 2)]
        [InlineData("2022-05-01", 0)]
        [InlineData("2022-04-01", 1)]
        [InlineData("2022-03-01", 2)]
        public async Task GetDataUrlsToProcess_Should_Return_Maximum_Last_Two_Months(string lastProcessDate, int correctDataCountToProcess)
        {
            DateTime? lastProcessDateVal = null;
            if (!string.IsNullOrWhiteSpace(lastProcessDate))
            {
                lastProcessDateVal = DateTime.ParseExact(lastProcessDate, "yyyy-MM-dd", CultureInfo.InvariantCulture);
            }

            IHttpHelper httpHelperMock = GetMockedHttpHelperForGetDataUrlsToProcess();
            IHtmlParser htmlParserMock = GetHtmlParserMock();

            var client = new DataParserClient(htmlParserMock, httpHelperMock, _options);

            var result = await client.GetDataUrlsToProcess(lastProcessDateVal);

            Assert.Equal(result.Count(), correctDataCountToProcess);
        }

        private IHttpHelper GetMockedHttpHelperForGetDataUrlsToProcess()
        {
            var httpHelperMock = new Mock<IHttpHelper>();
            httpHelperMock
               .Setup(opt => opt.GetContentAsString(_options.Value.DataUrl))
               .ReturnsAsync("test-result");

            return httpHelperMock.Object;
        }
        private IHtmlParser GetHtmlParserMock()
        {
            var htmlParserMock = new Mock<IHtmlParser>();
            htmlParserMock
                .Setup<List<TableData>>(s => s.ParseTable("test-result"))
                .Returns(MockTableDataResult());

            return htmlParserMock.Object;
        }
        private List<TableData> MockTableDataResult()
        {
            return new List<TableData>()
            {
               new TableData(){ Date = new DateTime(2022, 5, 1), DataUrl = "http://localhost" },
               new TableData(){ Date = new DateTime(2022, 4, 1), DataUrl = "http://localhost" },
               new TableData(){ Date = new DateTime(2022, 3, 1), DataUrl = "http://localhost" },
               new TableData(){ Date = new DateTime(2022, 2, 1), DataUrl = "http://localhost" },
               new TableData(){ Date = new DateTime(2022, 1, 1), DataUrl = "http://localhost" }
            };
        }
    }
}
