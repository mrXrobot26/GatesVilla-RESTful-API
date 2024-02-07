using AutoMapper;
using GatesVillaAPI.Models.Models;
using GatesVillaAPI.Models.Models.DTOs.VillaDTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GatesVillaAPI.Models.Mapper
{
    public class MappingConfig :Profile
    {
        public MappingConfig()
        {
            CreateMap<Villa,VillaDTO>().ReverseMap();
            CreateMap<Villa,VillaCreateDTO>().ReverseMap();
            CreateMap<Villa,VillaUpdateDTO>().ReverseMap();
        }
    }
}
