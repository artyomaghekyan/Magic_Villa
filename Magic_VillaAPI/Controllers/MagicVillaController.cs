using Magic_VillaAPI.Data;
using Magic_VillaAPI.Dto;
using Magic_VillaAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace Magic_VillaAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MagicVillaController : ControllerBase
    {
        private readonly ILogger<MagicVillaController> _logger;

        public MagicVillaController (ILogger<MagicVillaController> logger)
        {
            _logger = logger;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<VillaDTO> GetVillas()
        {
            _logger.LogInformation("Geting all villas");
            return Ok(VillaStore.villaList);
        }
        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            if (id <= 0 || villa is null)
            {
                _logger.LogError("Request Validation Error In GetVilla Controller");
                return BadRequest();
            }
            _logger.LogInformation("Geting a single villa by ID");
            _logger.LogDebug($"Id = {id}");
            
            return Ok(villa);


        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]

        public ActionResult<VillaDTO> AddVilla(VillaDTO villa)
        {
            if (VillaStore.villaList.FirstOrDefault(x => x.Name.ToLower() == villa.Name.ToLower()) != null) {
                //ModelState.AddModelError("", $"{villa.Name} already exists");
                _logger.LogError($"{villa.Name} already exists, Controller: AddVilla");
                return BadRequest(ModelState);
            }
            if (villa is null || villa.Name.Length == 0)
            {
                _logger.LogError("Request Validation Error In Addvilla");
                return BadRequest();
            }

            villa.Id = VillaStore.villaList.OrderByDescending(x => x.Id).FirstOrDefault().Id + 1;
            VillaStore.villaList.Add(villa);
            _logger.LogInformation("Creating a new villa info");
            return CreatedAtRoute("GetVilla", new { id = villa.Id }, villa);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> DeleteVilla(int id)
        {
            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            if (villa is null || id <= 0)
            {
                _logger.LogError("Request Validation Error");
                return NotFound();
            }
            VillaStore.villaList.Remove(villa);
            _logger.LogInformation("Deleting a villa");
            return Ok("Item was deleted");
        }
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut("{id:int}", Name = "UpdateVillaAll")]
        public IActionResult UpdateVillaAll(int id, VillaDTO villaDTO)
        {
            if (villaDTO == null || villaDTO.Id != id)
            {
                _logger.LogError("Request Validation Error In UpdateVillaAll Controller");
                return BadRequest();
            }

            var villa = VillaStore.villaList.FirstOrDefault(x => x.Id == id);
            villa.Id = villaDTO.Id;
            villa.Name = villaDTO.Name;
            villa.Occupancy = villaDTO.Occupancy;
            villa.Sqft = villaDTO.Sqft;
            _logger.LogInformation("Updating villa's all fields");

            return Ok(villa);


        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("{id:int}", Name = "UpdateVillaPartial")]
        public IActionResult UpdateVillaPartial(int id, JsonPatchDocument<VillaDTO> villa) {
            if (villa is null || id <= 0)
            {
                _logger.LogError("Request Validation Error In UpdateVillaPartial Controller");
                return BadRequest();
            }
            var findVilla = VillaStore.villaList.FirstOrDefault(x => x.Id == id);

            if (findVilla is null)
            {
                _logger.LogError("Request Validation Error In UpdateVillaPartial Controller");
                return NotFound();
            }
            villa.ApplyTo(findVilla, ModelState);
            _logger.LogInformation("Updating Partial Villa's Fields");
            return NoContent();


        }



    }

}
