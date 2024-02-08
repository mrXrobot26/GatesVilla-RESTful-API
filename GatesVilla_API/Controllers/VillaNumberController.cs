using AutoMapper;
using GatesVillaAPI.DataAcess.Repo.IRepo;
using GatesVillaAPI.Models.Models.APIResponde;
using GatesVillaAPI.Models.Models.DTOs.VillaDTOs;
using GatesVillaAPI.Models.Models.MyModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GatesVilla_API.Controllers
{
    [Route("api/[controller]")]
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

        [HttpGet("{id:int}" , Name ="GetVillaNumberById")]
        public async Task<ActionResult<APIResponse>> Get(int id)
        {

            try
            {
                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.ErrorMessages = new List<string>() { "there is no villaNumber id 0" };
                    response.IsSuccess = false;
                    response.Result = null;
                    return NotFound(response);
                }
                var villaNumber = await unitOfWork.VillaNumber.GetAsync(x => x.VillaNum == id);
                if (villaNumber == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.ErrorMessages = new List<string>() { "there is no villaNumber" };
                    response.IsSuccess = false;
                    response.Result = null;
                    return NotFound(response);
                }
                VillaNumberDTO villaNumberDTO = mapper.Map<VillaNumberDTO>(villaNumber);
                response.Result = villaNumberDTO;
                response.StatusCode = HttpStatusCode.OK;
                response.ErrorMessages = null;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { $"{ex.Message}" };
                response.Result = null;
                return BadRequest(response);
            }
        }

        [HttpGet("GetAllVillaNumber")]
        public async Task<ActionResult<APIResponse>> GetAll()
        {

            try
            {

                var villaNumbers = await unitOfWork.VillaNumber.GetAllAsync();
                if (villaNumbers == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.ErrorMessages = new List<string>() { "there is no villaNumbers" };
                    response.IsSuccess = false;
                    response.Result = null;
                    return NotFound(response);
                }
                List<VillaNumberDTO> villaNumbersDTO = mapper.Map<List<VillaNumberDTO>>(villaNumbers);
                response.Result = villaNumbersDTO;
                response.StatusCode = HttpStatusCode.OK;
                response.ErrorMessages = null;

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { $"{ex.Message}" };
                response.Result = null;
                return BadRequest(response);
            }
        }

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
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ErrorMessages = new List<string> { $"VillaNumber not Created." };
                    response.IsSuccess = false;
                    response.Result = null;
                    return BadRequest(response);
                }

                var existingVilla = await unitOfWork.VillaNumber.GetAsync(x => x.VillaNum == villaNumberCreateDTO.VillaNum);
                if (existingVilla != null)
                {
                    response.StatusCode = HttpStatusCode.Conflict;
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { "VillaNumber Already exists" };
                    response.Result = null;
                    return Conflict(response);
                }

                VillaNumber newVillaNumber = mapper.Map<VillaNumber>(villaNumberCreateDTO);
                await unitOfWork.VillaNumber.AddAsync(newVillaNumber);
                await unitOfWork.SaveChangesAsync();
                response.StatusCode = HttpStatusCode.OK;
                response.Result = newVillaNumber;
                response.ErrorMessages = null;
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { $"{ex.Message}" };
                response.Result = null;
                return response;
            }
        }
















        [HttpPut("{id:int}", Name = "UpdateVillaNumber")]
        public async Task<ActionResult<APIResponse>> Update(int id, [FromBody] VillaNumberUpdateDTO villaNumberUpdate)
        {

            try
            {
                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.ErrorMessages = new List<string>() { "there is no villaNumber id 0" };
                    response.IsSuccess = false;
                    response.Result = null;
                    return NotFound(response);
                }
                var villaNumber = await unitOfWork.VillaNumber.GetAsync(x => x.VillaNum == id);
                if (villaNumber == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.ErrorMessages = new List<string>() { "there is no villaNumber" };
                    response.IsSuccess = false;
                    response.Result = null;
                    return NotFound(response);
                }
                mapper.Map(villaNumberUpdate, villaNumber); 
                response.StatusCode = HttpStatusCode.OK;
                response.ErrorMessages = null;
                response.Result = villaNumber;
                unitOfWork.VillaNumber.Update(villaNumber);
                await unitOfWork.SaveChangesAsync();

                return Ok(response);
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { $"{ex.Message}" };
                response.Result = null;
                return BadRequest(response);
            }
        }



        [HttpDelete("{id:int}", Name = "DeleteVillaNumber")]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {

            try
            {
                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.ErrorMessages = new List<string>() { "there is no villaNumber id 0" };
                    response.IsSuccess = false;
                    response.Result = null;
                    return NotFound(response);
                }
                var villaNumber = await unitOfWork.VillaNumber.GetAsync(x => x.VillaNum == id);
                if (villaNumber == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.ErrorMessages = new List<string>() { "there is no villaNumber" };
                    response.IsSuccess = false;
                    response.Result = null;
                    return NotFound(response);
                }
                await unitOfWork.VillaNumber.DeleteAsync(villaNumber);
                await unitOfWork.SaveChangesAsync();

                response.StatusCode = HttpStatusCode.NoContent;
                response.ErrorMessages = null;
                response.Result = $"Villa {villaNumber.VillaNum} DELETED";

                return response;
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                response.IsSuccess = false;
                response.ErrorMessages = new List<string> { $"{ex.Message}" };
                response.Result = null;
                return BadRequest(response);
            }
        }

    }
}
