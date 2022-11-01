using ElectricityDataApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.Application.Features.ElectricityData.Queries
{
    public class ElectricityDataVm
    {
        public string Region { get; set; }

        public string ObjGvTipas { get; set; }

        public long ObjNumeris { get; set; }

        public decimal? PPlus { get; set; }

        public DateTime Date { get; set; }

        public decimal? PMinus { get; set; }
    }
}
