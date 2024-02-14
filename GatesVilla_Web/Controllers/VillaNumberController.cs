using AutoMapper;
using GatesVilla_Web.Services;
using GatesVilla_Web.Services.IServices;
using GatesVillaAPI.Models.Models.APIResponde;
using GatesVillaAPI.Models.Models.DTOs.VillaDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace GatesVilla_Web.Controllers
{
    public class VillaNumberController : Controller
    {
        private readonly IVillaNumberService villaNumberServices;
        private readonly IMapper mapper;
        private readonly VillaService villaService;

        public VillaNumberController(IVillaNumberService villaNumberServices, IMapper mapper,VillaService villaService)
        {
            this.villaNumberServices = villaNumberServices;
            this.mapper = mapper;
            this.villaService = villaService;
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
        [HttpPost]
        public async Task<IActionResult> CreateVillaNumber()
        {
            return View();
        }


        public async Task<IActionResult> CreateVillaNumber(VillaNumberCreateDTO villaNumberCreateDTO)
        {
            var response = await villaNumberServices.CreateAsync<APIResponse>(villaNumberCreateDTO);
            if (response != null && response.IsSuccess)
            {
                villaNumberCreateDTO = JsonConvert.DeserializeObject<VillaNumberCreateDTO>(Convert.ToString(response.Result));
            }
            return View(villaNumberCreateDTO);
        }

        private async Task<List<SelectListItem>> GetAvailableVillas()
        {
            List<VillaDTO> villaList = new();
            var response = await villaService.GetAllAsync<APIResponse>();

            if (response != null && response.IsSuccess)
            {
                villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }

            return villaList.Select(v => new SelectListItem { Value = v.Id.ToString(), Text = v.Name }).ToList();
        }

    }



}
