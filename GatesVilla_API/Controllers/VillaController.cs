using GatesVillaAPI.DataAcess.Data;
using GatesVillaAPI.DataAcess.Repo.IRepo;
using GatesVillaAPI.Models.Models;
using GatesVillaAPI.Models.Models.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GatesVilla_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;

        public VillaController(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        [HttpGet("{id:int}", Name ="GetVillaById")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            try
            {
                var villa = unitOfWork.Villa.Get(x => x.Id == id);
                if (villa == null)
                {
                    return NotFound($"Villa with ID {id} not found.");
                }

                var villaDTO = new VillaDTO();
                villaDTO.Id = id;
                villaDTO.Name = villa.Name;

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
        public ActionResult<IEnumerable<VillaDTO>> GetVillas()
        {
            try
            {
                var villas = unitOfWork.Villa.GetAll();
                if (villas == null || !villas.Any())
                {
                    return NotFound("No villas found.");
                }

                List<VillaDTO> villasDTO = new List<VillaDTO>();

                foreach (var villa in villas)
                {
                    var villaDTO = new VillaDTO
                    {
                        Id = villa.Id,
                        Name = villa.Name
                    };
                    villasDTO.Add(villaDTO);
                }

                return Ok(villasDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost("{id : int}" , Name ="UpdateVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDTO> Create([FromBody] VillaDTO newVillaDTO)
        {
            try
            {

                if (newVillaDTO.Id > 0)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError);
                }
                if (newVillaDTO == null)
                {
                    return BadRequest(newVillaDTO);
                }
                if (unitOfWork.Villa.Get(x=>x.Name == newVillaDTO.Name) != null)
                {
                    return BadRequest("Villa Already exist");
                }
                Villa newVilla = new Villa()
                {
                    Id = newVillaDTO.Id,
                    Name = newVillaDTO.Name,
                    CreatedTime= DateTime.Now,
                };
                unitOfWork.Villa.Add(newVilla);
                unitOfWork.SaveChanges();
                return Ok(newVillaDTO);
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
        public IActionResult Delete(int id)
        {
            try
            {
                if (id == 0)
                {
                    return BadRequest("Id must not equal 0");

                }
                var villa = unitOfWork.Villa.Get(x => x.Id == id);
                if (villa == null)
                {
                    return NotFound($"Villa with ID {id} not found.");
                }
                unitOfWork.Villa.Delete(villa);
                unitOfWork.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPut]
        public IActionResult Update(int id, [FromBody]VillaDTO villaDTO)
        {
            try
            {
                
                var villa = unitOfWork.Villa.Get(x => x.Id == id);
                if (villa == null)
                {
                    return NotFound($"Villa with ID {id} not found.");
                }

                villa.Name = villaDTO.Name;

                unitOfWork.SaveChanges();

                return Ok($"Villa with ID {id} updated successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }


    }
}
