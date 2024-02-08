using AutoMapper;
using Azure;
using GatesVillaAPI.DataAcess.Data;
using GatesVillaAPI.DataAcess.Repo.IRepo;
using GatesVillaAPI.Models.Models.APIResponde;
using GatesVillaAPI.Models.Models.DTOs.VillaDTOs;
using GatesVillaAPI.Models.Models.MyModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace GatesVilla_API.Controllers
{
    [Route("api/VillaController")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        protected APIResponse response;

        public VillaController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            response = new();
        }

        [HttpGet("{id:int}", Name = "GetVillaById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                var villa = await unitOfWork.Villa.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { $"Villa with ID {id} not found." };
                    response.Result = null;
                    return NotFound(response);
                }

                VillaDTO villaDTO = mapper.Map<VillaDTO>(villa);

                response.StatusCode = HttpStatusCode.OK;
                response.ErrorMessages = null;
                response.Result = villaDTO;
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


        [HttpGet("GetVillas")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> GetVillas()
        {
            try
            {
                var villas = await unitOfWork.Villa.GetAllAsync();
                if (villas == null || !villas.Any())
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.ErrorMessages = new List<string> { $"Villas not found." };
                    response.IsSuccess = false;
                    response.Result = null;
                    return NotFound(response);
                }

                List<VillaDTO> villasDTO = mapper.Map<List<VillaDTO>>(villas);
                response.StatusCode = HttpStatusCode.OK;
                response.ErrorMessages = null;
                response.Result = villasDTO;
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


        [HttpPost("AddVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Create([FromBody] VillaCreateDTO villaCreateDTO)
        {
            try
            {
                if (villaCreateDTO == null)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ErrorMessages = new List<string> { $"Villas not Created." };
                    response.IsSuccess = false;
                    response.Result = null;
                    return BadRequest(response);
                }

                var existingVilla = await unitOfWork.Villa.GetAsync(x => x.Name == villaCreateDTO.Name);
                if (existingVilla != null)
                {
                    response.StatusCode = HttpStatusCode.Conflict;
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { "Villa Already exists" };
                    response.Result = null;
                    return Conflict(response);
                }

                Villa newVilla = mapper.Map<Villa>(villaCreateDTO);
                await unitOfWork.Villa.AddAsync(newVilla);
                await unitOfWork.SaveChangesAsync();
                return Ok(villaCreateDTO);
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


        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    response.StatusCode = HttpStatusCode.BadRequest;
                    response.ErrorMessages = new List<string> { $"ID can't equal 0" };
                    response.IsSuccess = false;
                    response.Result = null;
                    return BadRequest(response); 
                }

                var villa = await unitOfWork.Villa.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.ErrorMessages = new List<string> { $"Villa with ID {id} not found." };
                    response.IsSuccess = false;
                    response.Result = null;
                    return NotFound(response); 
                }

                await unitOfWork.Villa.DeleteAsync(villa);
                await unitOfWork.SaveChangesAsync();

                response.StatusCode = HttpStatusCode.NoContent;
                response.ErrorMessages = null;
                response.Result = $"Villa {villa.Name} DELETED";

                return response;
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


        [HttpPut("UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> Update(int id, [FromBody] VillaUpdateDTO villaUpdateDTO)
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
                var foundedVilla = await unitOfWork.Villa.GetAsync(x => x.Id == id);

                if (foundedVilla == null)
                {
                    response.StatusCode = HttpStatusCode.NotFound;
                    response.IsSuccess = false;
                    response.ErrorMessages = new List<string> { $"Villa with ID {id} not found." };
                    response.Result = null;
                    return NotFound(response);

                }

                mapper.Map(villaUpdateDTO, foundedVilla);

                unitOfWork.Villa.Update(foundedVilla);
                await unitOfWork.SaveChangesAsync();
                response.StatusCode = HttpStatusCode.NoContent;
                response.IsSuccess = true;
                response.Result = foundedVilla;
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

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> PatchVilla(int id, [FromBody] JsonPatchDocument<VillaUpdateDTO> patchDocument)
        {
            try
            {
                var villa = unitOfWork.Villa.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    return NotFound($"Villa with ID {id} not found.");
                }

                VillaUpdateDTO villaUpdateDTO = mapper.Map<VillaUpdateDTO>(villa);


                patchDocument.ApplyTo(villaUpdateDTO, ModelState);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                await mapper.Map(villaUpdateDTO, villa);

                await unitOfWork.SaveChangesAsync();

                return Ok($"Villa with ID {id} patched successfully.");
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
