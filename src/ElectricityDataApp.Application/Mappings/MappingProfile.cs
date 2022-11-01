using AutoMapper;
using ElectricityDataApp.Application.Features.ElectricityData.Queries;
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
            CreateMap<DataItem, ElectricityDataVm>().ForMember(dest => dest.Region, opt => opt.MapFrom(dest => dest.Region.Name));
        }
    }
}
