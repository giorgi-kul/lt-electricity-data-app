using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.DataParser
{
    public class DataParserClientOptions
    {
        public string BaseUrl { get; set; }
        public string DataUrl { get; set; }
        public int DateColumnIndex { get; set; }
        public int DownloadNodeTableIndex { get; set; }
        public int? HttpRequestTimeoutInMinutes { get; set; }
    }
}
