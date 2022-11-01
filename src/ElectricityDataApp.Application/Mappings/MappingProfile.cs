using AutoMapper;
using ElectricityDataApp.DataParser.Models;
using ElectricityDataApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectricityDataApp.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Record, DataItem>();
        }
    }
}
