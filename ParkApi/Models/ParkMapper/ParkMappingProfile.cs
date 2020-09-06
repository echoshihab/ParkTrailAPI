using AutoMapper;
using ParkApi.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkApi.Models.ParkMapper
{
    public class ParkMappingProfile : Profile
    {
        public ParkMappingProfile()
        {
            CreateMap<NationalPark, NationalParkDto>().ReverseMap();
            CreateMap<Trail, TrailDto>().ReverseMap();
            CreateMap<Trail, TrailCreateDto>().ReverseMap();
            CreateMap<Trail, TrailUpdateDto>().ReverseMap();
        }
    }
}
