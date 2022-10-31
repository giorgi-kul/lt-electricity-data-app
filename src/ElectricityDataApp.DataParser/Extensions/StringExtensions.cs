using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.DataParser.Extensions
{
    public static class StringExtensions
    {
        public static DateTime ToNormalizedDate(this string date)
        {
            return DateTime.ParseExact($"{date}.01", "yyyy.MM.dd", CultureInfo.InvariantCulture);
        }
    }
}
