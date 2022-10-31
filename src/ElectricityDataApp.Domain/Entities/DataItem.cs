using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.Domain.Entities
{
    public class DataItem : BaseEntity
    {
        public Region Region { get; set; }

        public string ObjGvTipas { get; set; }

        public int ObjNumeris { get; set; }

        public decimal PPlus { get; set; }

        public DateTime Date { get; set; }

        public decimal PMinus { get; set; }
    }
}