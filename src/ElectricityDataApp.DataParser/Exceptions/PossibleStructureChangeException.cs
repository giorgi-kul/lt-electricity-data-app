using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.DataParser.Exceptions
{
    public class PossibleStructureChangeException : Exception
    {
        public PossibleStructureChangeException(string paramName)
            : base($"Possible structure change, parameter: {paramName}")
        {

        }
    }
}
