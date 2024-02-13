using AutoMapper;
using GatesVilla_Web.Services;
using GatesVilla_Web.Services.IServices;
using GatesVillaAPI.Models.Models.APIResponde;
using GatesVillaAPI.Models.Models.DTOs.VillaDTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace GatesVilla_Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService villaServices;
        private readonly IMapper mapper;

        public VillaController(IVillaService villaServices, IMapper mapper)
        {
            this.villaServices = villaServices;
            this.mapper = mapper;
        }

        public async Task<IActionResult> IndexVilla()
        {
            List<VillaDTO> villaList = new();
            var response = await villaServices.GetAllAsync<APIResponse>();
            if (response != null && response.IsSuccess)
            {
                villaList = JsonConvert.DeserializeObject<List<VillaDTO>>(Convert.ToString(response.Result));
            }
            return View(villaList);
        }

        public async Task<IActionResult> CreateVilla()
        { 
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateVilla(VillaCreateDTO villaCreate)
        {
            var response = await villaServices.CreateAsync<APIResponse>(villaCreate);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));

            }
            return View(villaCreate);
        }
		public async Task<IActionResult> UpdateVilla(int villaId)
		{
			var response = await villaServices.GetAsync<APIResponse>(villaId);
			if (response != null && response.IsSuccess)
			{
				VillaDTO model = JsonConvert.DeserializeObject<VillaDTO>(Convert.ToString(response.Result));
				return View(mapper.Map<VillaUpdateDTO>(model));
			}
			return NotFound();
		}


        [HttpPut]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateVilla(VillaUpdateDTO villaUpdate)
        {
            var response = await villaServices.UpdateAsync<APIResponse>(villaUpdate);
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(IndexVilla));

            }
            return View(villaUpdate);
        }


    }
}
