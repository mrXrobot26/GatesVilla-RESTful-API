using GatesVillaAPI.DataAcess.Data;
using GatesVillaAPI.DataAcess.Repo.IRepo;
using GatesVillaAPI.Models.Models;
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


        [HttpGet("{id:int}")]
        public IActionResult GetVilla(int id)
        {
            try
            {
                var villa = unitOfWork.Villa.Get(x => x.Id == id);
                if (villa == null)
                {
                    return NotFound($"Villa with ID {id} not found.");
                }
                return Ok(villa);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpGet("GetVillas")]
        public IActionResult GetVillas()
        {
            try
            {
                var villas = unitOfWork.Villa.GetAll();
                if (villas == null || !villas.Any())
                {
                    return NotFound("No villas found.");
                }
                return Ok(villas);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }

        [HttpPost]
        public IActionResult Create([FromBody] Villa newVilla)
        {
            try
            {
                unitOfWork.Villa.Add(newVilla);
                unitOfWork.SaveChanges();
                return Ok(newVilla);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }


        }

        [HttpDelete("{id:int}")]
        public IActionResult Delete(int id)
        {
            try
            {
                var villa = unitOfWork.Villa.Get(x => x.Id == id);

                if (villa == null)
                {
                    return NotFound($"Villa with ID {id} not found.");
                }

                unitOfWork.Villa.Delete(villa);
                unitOfWork.SaveChanges();

                return Ok($"Villa with ID {id} deleted successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error.");
            }
        }





    }
}
