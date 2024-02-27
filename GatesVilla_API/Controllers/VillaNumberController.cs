using AutoMapper;
using GatesVilla_Utility;
using GatesVillaAPI.DataAcess.Repo.IRepo;
using GatesVillaAPI.Models.Models.APIResponde;
using GatesVillaAPI.Models.Models.DTOs.VillaDTOs;
using GatesVillaAPI.Models.Models.MyModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GatesVilla_API.Controllers
{
    [Route("api/VillaNumberAPI")]
    [ApiController]
    public class VillaNumberController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        protected APIResponse response;
        public VillaNumberController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            response = new();
        }
        [HttpGet("{id:int}", Name = "GetVillaNumberById")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Get(int id)
        {
            try
            {
                if (id <= 0)
                {
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { "Invalid VillaNumber ID" }, null, false);
                    return NotFound(response);
                }

                var villaNumber = await unitOfWork.VillaNumber.GetAsync(x => x.VillaNum == id);
                if (villaNumber == null)
                {
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { $"VillaNumber with ID {id} not found" }, null, false);
                    return NotFound(response);
                }

                VillaNumberDTO villaNumberDTO = mapper.Map<VillaNumberDTO>(villaNumber);
                response.SetResponseInfo(HttpStatusCode.OK, null, villaNumberDTO, true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetResponseInfo(HttpStatusCode.BadRequest, new List<string> { $"{ex.Message}" }, null, false);
                return BadRequest(response);
            }
        }
        //[Authorize(Roles = SD.Admin)]
        [HttpGet("GetAllVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> GetAll()
        {
            try
            {
                var villaNumbers = await unitOfWork.VillaNumber.GetAllAsync(includes : "villa");
                if (villaNumbers == null)
                {
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { "There are no villaNumbers" }, null, false);
                    return NotFound(response);
                }

                List<VillaNumberDTO> villaNumbersDTO = mapper.Map<List<VillaNumberDTO>>(villaNumbers);
                response.SetResponseInfo(HttpStatusCode.OK, null, villaNumbersDTO, true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetResponseInfo(HttpStatusCode.BadRequest, new List<string> { $"{ex.Message}" }, null, false);
                return BadRequest(response);
            }
        }
        //[Authorize(Roles = SD.Admin)]
        [HttpPost("AddVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] VillaNumberCreateDTO villaNumberCreateDTO)
        {
            try
            {
                if (villaNumberCreateDTO == null)
                {
                    response.SetResponseInfo(HttpStatusCode.BadRequest, new List<string> { "VillaNumber not Created." }, null, false);
                    ModelState.AddModelError("ErrorMessages", "VillaNumber not Created.");

                    return BadRequest(response);
                }

                var existingVillaNumber = await unitOfWork.VillaNumber.GetAsync(x => x.VillaNum == villaNumberCreateDTO.VillaNum);
                if (existingVillaNumber != null)
                {
                    response.SetResponseInfo(HttpStatusCode.Conflict, new List<string> { "VillaNumber Already exists" }, null, false);
                    ModelState.AddModelError("ErrorMessages", "Villa Number already Exists!");
                    return Conflict(response);
                }

                var existingVilla = await unitOfWork.Villa.GetAsync(x => x.Id == villaNumberCreateDTO.VillaId);
                if (existingVilla == null)
                {
                    response.SetResponseInfo(HttpStatusCode.Conflict, new List<string> { $"No Villa Has Id {villaNumberCreateDTO.VillaId}" }, null, false);
                    ModelState.AddModelError("ErrorMessages", "Villa ID is Invalid!");
                    return Conflict(response);
                }

                VillaNumber newVillaNumber = mapper.Map<VillaNumber>(villaNumberCreateDTO);
                await unitOfWork.VillaNumber.AddAsync(newVillaNumber);
                await unitOfWork.SaveChangesAsync();

                response.SetResponseInfo(HttpStatusCode.OK, null, newVillaNumber, true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetResponseInfo(HttpStatusCode.InternalServerError, new List<string> { $"{ex.Message}" }, null, false);
                return response;
            }
        }

        //[Authorize(Roles = SD.Admin)]
        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Update(int id, [FromBody] VillaNumberUpdateDTO villaNumberUpdate)
        {
            try
            {
                if (id <= 0)
                {
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { "Invalid VillaNumber ID" }, null, false);
                    return NotFound(response);
                }

                var villaNumber = await unitOfWork.VillaNumber.GetAsync(x => x.VillaNum == id);

                if (villaNumber == null)
                {
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { $"VillaNumber with ID {id} not found" }, null, false);
                    return NotFound(response);
                }
                var villa = await unitOfWork.Villa.GetAsync(x => x.Id == villaNumber.VillaId);
                if (villa == null)
                {
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { $"Villa with ID {id} not found" }, null, false);
                    return NotFound(response);
                }
                villaNumberUpdate.VillaNum = id;
                mapper.Map(villaNumberUpdate, villaNumber);
                response.SetResponseInfo(HttpStatusCode.OK, null, villaNumber, true);
                unitOfWork.VillaNumber.Update(villaNumber);
                await unitOfWork.SaveChangesAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetResponseInfo(HttpStatusCode.BadRequest, new List<string> { $"{ex.Message}" }, null, false);
                return BadRequest(response);
            }
        }

        //[Authorize(Roles = SD.Admin)]
        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                if (id <= 0)
                {
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { "Invalid VillaNumber ID" }, null, false);
                    return NotFound(response);
                }

                var villaNumber = await unitOfWork.VillaNumber.GetAsync(x => x.VillaNum == id);
                if (villaNumber == null)
                {
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { $"VillaNumber with ID {id} not found" }, null, false);
                    return NotFound(response);
                }

                await unitOfWork.VillaNumber.DeleteAsync(villaNumber);
                await unitOfWork.SaveChangesAsync();

                response.SetResponseInfo(HttpStatusCode.NoContent, null, $"Villa {villaNumber.VillaNum} DELETED", true);
                return response;
            }
            catch (Exception ex)
            {
                response.SetResponseInfo(HttpStatusCode.BadRequest, new List<string> { $"{ex.Message}" }, null, false);
                return BadRequest(response);
            }
        }

    }
}
