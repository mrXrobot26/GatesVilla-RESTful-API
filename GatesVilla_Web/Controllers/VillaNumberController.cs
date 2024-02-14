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
        private readonly IVillaService villaNumberServices;
        private readonly IMapper mapper;

        public VillaNumberController(IVillaService villaServices, IMapper mapper)
        {
            this.villaNumberServices = villaServices;
            this.mapper = mapper;
        }

        public async Task<IActionResult> IndexVillaNumber()
        {
            List<VillaNumberDTO> villaList = new();
            var response = await villaNumberServices.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaList = JsonConvert.DeserializeObject<List<VillaNumberDTO>>(Convert.ToString(response.Result));
            }
            return View(villaList);
        }




    }
}
