using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.Domain.Entities
{
    public class Region : BaseEntity
    {
        public string Name { get; set; }

        public ICollection<DataItem> Data { get; set; }

        public Region()
        {
            Data = new HashSet<DataItem>();
        }
    }
}
