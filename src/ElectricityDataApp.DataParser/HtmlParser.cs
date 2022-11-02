using ElectricityDataApp.DataParser.Exceptions;
using ElectricityDataApp.DataParser.Extensions;
using ElectricityDataApp.DataParser.Interfaces;
using ElectricityDataApp.DataParser.Models;
using HtmlAgilityPack;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.DataParser
{
    public class HtmlParser : IHtmlParser
    {
        private readonly DataParserClientOptions _options;

        public HtmlParser(IOptions<DataParserClientOptions> options)
        {
            _options = options.Value;
        }

        public List<TableData> ParseTable(string htmlData)
        {
            List<TableData> data = new();

            var document = new HtmlDocument();

            document.LoadHtml(htmlData);

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

                string downloadUrl = $"{_options.BaseUrl}{downloadNode.GetAttributeValue("href", "")}";

                var date = dateValue.ToNormalizedDate();

                data.Add(new TableData()
                {
                    DataUrl = downloadUrl,
                    Date = date
                });
            }

            return data;
        }
    }
}
