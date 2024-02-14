using AutoMapper;
using GatesVilla_Web.Services;
using GatesVilla_Web.Services.IServices;
using GatesVillaAPI.Models.Models.APIResponde;
using GatesVillaAPI.Models.Models.DTOs.VillaDTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GatesVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService villaNumberServices;
        private readonly IMapper mapper;

        public VillaNumberController(IVillaNumberService villaNumberServices, IMapper mapper)
        {
            this.villaNumberServices = villaNumberServices;
            this.mapper = mapper;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> villaNumberList = new();
            var response = await villaNumberServices.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaNumberList = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(villaNumberList);
        }




    }
}
