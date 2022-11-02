using ElectricityDataApp.DataParser.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.DataParser.Interfaces
{
    public interface IHtmlParser
    {
        List<TableData> ParseTable(string htmlData);
    }
}
