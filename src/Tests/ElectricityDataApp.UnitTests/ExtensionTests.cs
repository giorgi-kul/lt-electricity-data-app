using ElectricityDataApp.DataParser.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.UnitTests
{
    public class ExtensionTests
    {
        [Theory]
        [InlineData("2022.11", true)]
        [InlineData("2022-11", false)]
        [InlineData("2022/11", false)]
        [InlineData("11.2022", false)]
        public void ToNormalizedDateTime_Should_Return_DateTime(string date, bool shouldConvertSuccessfully)
        {
            try
            {
                date.ToNormalizedDate();

                Assert.True(shouldConvertSuccessfully);
            }
            catch
            {
                Assert.False(shouldConvertSuccessfully);
            }
        }
    }
}
