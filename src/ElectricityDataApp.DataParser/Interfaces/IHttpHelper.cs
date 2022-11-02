using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.DataParser.Interfaces
{
    public interface IHttpHelper
    {
        Task<string> GetContentAsString(string url);
    }
}
