﻿using AutoMapper;
using GatesVillaAPI.DataAcess.Data;
using GatesVillaAPI.DataAcess.Repo.IRepo;
using GatesVillaAPI.Models.Models;
using GatesVillaAPI.Models.Models.DTOs.VillaDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace GatesVilla_API.Controllers
{
    [Route("api/VillaController")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public VillaController(IUnitOfWork unitOfWork,IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        [HttpGet("{id:int}", Name = "GetVillaById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [Produces("application/json")] 

        public async Task<ActionResult<VillaDTO>> GetVilla(int id)
        {
            try
            {
                var villa = await unitOfWork.Villa.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    return NotFound($"Villa with ID {id} not found.");
                }

                VillaDTO villaDTO = mapper.Map<VillaDTO>(villa);

                return Ok(villaDTO);

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("GetVillas")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<VillaDTO>>> GetVillas()
        {
            try
            {
                var villas = await unitOfWork.Villa.GetAllAsync();
                if (villas == null )
                {
                    return NotFound("No villas found.");
                }
                List<VillaDTO> villasDTO = mapper.Map<List<VillaDTO>>(villas);

                return Ok(villasDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("AddVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaDTO>> Create([FromBody] VillaCreateDTO villaCreateDTO)
        {
            try
            {

                if (villaCreateDTO == null)
                {
                    return BadRequest(villaCreateDTO);
                }
                if (await unitOfWork.Villa.GetAsync(x => x.Name == villaCreateDTO.Name) != null)
                {
                    return BadRequest("Villa Already exist");
                }
                Villa newVilla = mapper.Map<Villa>(villaCreateDTO);
                await unitOfWork.Villa.AddAsync(newVilla);
                await unitOfWork.SaveChangesAsync();
                return Ok(villaCreateDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Id must not equal 0");

                }
                var villa = await unitOfWork.Villa.GetAsync(x => x.Id == id);
                if (villa == null)
                {
                    return NotFound($"Villa with ID {id} not found.");
                }
                await unitOfWork.Villa.DeleteAsync(villa);
                await unitOfWork.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut("UpdateVilla")]
        public async Task<IActionResult> Update(int id, [FromBody] VillaUpdateDTO villaUpdateDTO)
        {
            try
            {
                var foundedVilla = await unitOfWork.Villa.GetAsync(x => x.Id == id);

                if (foundedVilla == null)
                {
                    return NotFound($"Villa with ID {id} not found.");
                }

                mapper.Map(villaUpdateDTO, foundedVilla);

                await unitOfWork.Villa.UpdateAsync(foundedVilla);

                return Ok($"Villa with ID {id} updated successfully.");

            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
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
                return StatusCode(500, "Internal server error.");
            }
        }


    }
}
