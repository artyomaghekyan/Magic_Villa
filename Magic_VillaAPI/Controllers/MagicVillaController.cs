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
        private readonly AppliactionDbContext _db;

        public MagicVillaController (ILogger<MagicVillaController> logger, AppliactionDbContext db)
        {
            _logger = logger;
            _db = db;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<VillaDTO> GetVillas()
        {
            _logger.LogInformation("Geting all villas");
            return Ok(_db.Villas.ToList());
        }
        [HttpGet("{id}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<VillaDTO> GetVilla(int id)
        {
            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);
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
            if (_db.Villas.FirstOrDefault(x => x.Name.ToLower() == villa.Name.ToLower()) != null) {
                //ModelState.AddModelError("", $"{villa.Name} already exists");
                _logger.LogError($"{villa.Name} already exists, Controller: AddVilla");
                return BadRequest(ModelState);
            }
            if (villa is null || villa.Name.Length == 0)
            {
                _logger.LogError("Request Validation Error In Addvilla");
                return BadRequest();
            }
            Villa model = new()
            {
                //Id = villa.Id,
                Name = villa.Name,
                Details = villa.Details,
                Amenity = villa.Amenity,
                ImageUrl = villa.ImageUrl,
                Occupance = villa.Occupancy,
                Rate = villa.Rate,
                Sqft = villa.Sqft,

            };

            _logger.LogInformation("Creating a new villa info");

            _db.Villas.Add(model);
            _db.SaveChanges();
            return Ok(model);
        }

        [HttpDelete("{id:int}", Name = "DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDTO> DeleteVilla(int id)
        {
            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);

            if (villa is null || id <= 0)
            {
                _logger.LogError("Request Validation Error");
                return NotFound();
            }

            _db.Villas.Remove(villa);
            _db.SaveChanges();
            _logger.LogInformation("Deleting a villa");
            return Ok("Item was deleted");
        }
        [HttpPut("{id:int}", Name = "UpdateVillaAll")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        
        public IActionResult UpdateVillaAll(int id, VillaDTO villaDTO)
        {
            if (villaDTO == null)
            {
                _logger.LogError("Request Validation Error In UpdateVillaAll Controller");
                return BadRequest();
            }

            var villa = _db.Villas.FirstOrDefault(x => x.Id == id);

            if (villa == null)
            {
                _logger.LogError("Villa is null");
                return BadRequest();
            }
            _db.Villas.Update(villa);
            _db.SaveChanges();
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
            var findVilla = _db.Villas.FirstOrDefault(x => x.Id == id);

            if (findVilla is null)
            {
                _logger.LogError("Request Validation Error In UpdateVillaPartial Controller");
                return NotFound();
            }
            VillaDTO villaDTO = new()
            {
                Id = findVilla.Id,
                Name = findVilla.Name,
                Details = findVilla.Details,
                Amenity = findVilla.Amenity,
                ImageUrl = findVilla.ImageUrl,
                Occupancy = findVilla.Occupance,
                Rate = findVilla.Rate,
                Sqft = findVilla.Sqft,

            };

            villa.ApplyTo(villaDTO, ModelState);

            Villa model = new()
            {
                Id = villaDTO.Id,
                Name = villaDTO.Name,
                Details = villaDTO.Details,
                Amenity = villaDTO.Amenity,
                ImageUrl = villaDTO.ImageUrl,
                Occupance = villaDTO.Occupancy,
                Rate = villaDTO.Rate,
                Sqft = villaDTO.Sqft,

            };

            _db.Villas.Update(model);   
            _db.SaveChanges();
            _logger.LogInformation("Updating Partial Villa's Fields");
            return NoContent();


        }



    }

}
