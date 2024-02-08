using AutoMapper;
using GatesVillaAPI.Models.Models.DTOs.VillaDTOs;
using GatesVillaAPI.Models.Models.MyModels;
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
            CreateMap<Villa,VillaNumberDTO>().ReverseMap();
            CreateMap<Villa,VillaNumberCreateDTO>().ReverseMap();
            CreateMap<Villa,VillaNumberUpdateDTO>().ReverseMap();
        }
    }
}
