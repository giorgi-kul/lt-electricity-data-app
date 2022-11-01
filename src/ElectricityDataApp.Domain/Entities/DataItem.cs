using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.Domain.Entities
{
    public class DataItem : BaseEntity
    {
        public int RegionId { get; set; }

        public Region Region { get; set; }

        public string ObjGvTipas { get; set; }

        public long ObjNumeris { get; set; }

        public decimal? PPlus { get; set; }

        public DateTime Date { get; set; }

        public decimal? PMinus { get; set; }


       public DataItem SetRegionId(int regionId)
        {
            RegionId = regionId;

            return this;
        }
    }
}