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
    [Route("api/villaAPI")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        protected APIResponse response;

        public VillaAPIController(IUnitOfWork unitOfWork,IMapper mapper)
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
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { $"Villa with ID {id} not found." }, null, false);
                    return NotFound(response);
                }

                VillaDTO villaDTO = mapper.Map<VillaDTO>(villa);

                response.SetResponseInfo(HttpStatusCode.OK, null, villaDTO, true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetResponseInfo(HttpStatusCode.BadRequest, new List<string> { $"{ex.Message}" }, null, false);
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
                if (!villas.Any())
                {
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { "Villas not found." }, null, false);
                    return NotFound(response);
                }

                List<VillaDTO> villasDTO = mapper.Map<List<VillaDTO>>(villas);
                response.SetResponseInfo(HttpStatusCode.OK, null, villasDTO, true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetResponseInfo(HttpStatusCode.BadRequest, new List<string> { $"{ex.Message}" }, null, false);
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
                    response.SetResponseInfo(HttpStatusCode.BadRequest, new List<string> { "Villa not Created." }, null, false);
                    return BadRequest(response);
                }

                var existingVilla = await unitOfWork.Villa.GetAsync(x => x.Name == villaCreateDTO.Name);
                if (existingVilla != null)
                {
                    response.SetResponseInfo(HttpStatusCode.Conflict, new List<string> { "Villa Already exists" }, null, false);
                    return Conflict(response);
                }

                Villa newVilla = mapper.Map<Villa>(villaCreateDTO);
                await unitOfWork.Villa.AddAsync(newVilla);
                await unitOfWork.SaveChangesAsync();

                response.SetResponseInfo(HttpStatusCode.OK, null, villaCreateDTO, true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetResponseInfo(HttpStatusCode.InternalServerError, new List<string> { $"{ex.Message}" }, null, false);
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
                    response.SetResponseInfo(HttpStatusCode.BadRequest, new List<string> { "ID can't equal 0" }, null, false);
                    return BadRequest(response);
                }

                var villa = await unitOfWork.Villa.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { $"Villa with ID {id} not found." }, null, false);
                    return NotFound(response);
                }

                await unitOfWork.Villa.DeleteAsync(villa);
                await unitOfWork.SaveChangesAsync();

                response.SetResponseInfo(HttpStatusCode.NoContent, null, new { Message = $"Villa {villa.Name} DELETED" }, true);
                return response;
            }
            catch (Exception ex)
            {
                response.SetResponseInfo(HttpStatusCode.InternalServerError, new List<string> { $"{ex.Message}" }, null, false);
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
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { "There is no villa with ID 0" }, null, false);
                    return NotFound(response);
                }

                var foundedVilla = await unitOfWork.Villa.GetAsync(x => x.Id == id);

                if (foundedVilla == null)
                {
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { $"Villa with ID {id} not found." }, null, false);
                    return NotFound(response);
                }

                mapper.Map(villaUpdateDTO, foundedVilla);

                unitOfWork.Villa.Update(foundedVilla);
                await unitOfWork.SaveChangesAsync();

                response.SetResponseInfo(HttpStatusCode.NoContent, null, foundedVilla, true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetResponseInfo(HttpStatusCode.BadRequest, new List<string> { $"{ex.Message}" }, null, false);
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
                var villa = await unitOfWork.Villa.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    response.SetResponseInfo(HttpStatusCode.NotFound, new List<string> { $"Villa with ID {id} not found." }, null, false);
                    return NotFound(response);
                }

                VillaUpdateDTO villaUpdateDTO = mapper.Map<VillaUpdateDTO>(villa);

                patchDocument.ApplyTo(villaUpdateDTO, ModelState);

                if (!ModelState.IsValid)
                {
                    response.SetResponseInfo(HttpStatusCode.BadRequest, ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList(), null, false);
                    return BadRequest(response);
                }

                mapper.Map(villaUpdateDTO, villa);

                await unitOfWork.SaveChangesAsync();

                response.SetResponseInfo(HttpStatusCode.OK, null, $"Villa with ID {id} patched successfully.", true);
                return Ok(response);
            }
            catch (Exception ex)
            {
                response.SetResponseInfo(HttpStatusCode.BadRequest, new List<string> { $"{ex.Message}" }, null, false);
                return BadRequest(response);
            }
        }



    }
}
