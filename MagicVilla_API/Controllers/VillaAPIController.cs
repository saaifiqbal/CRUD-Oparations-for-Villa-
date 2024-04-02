using MagicVilla_API.Data;
using MagicVilla_API.Models;
using MagicVilla_API.Models.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace MagicVilla_API.Controllers
{
    [Route("api/villa")]
    [ApiController]
    public class VillaAPIController : ControllerBase
    {
        [HttpGet]
        public ActionResult<IEnumerable <VillaDTO>> GetVillas()
        {
            return Ok(VillaStore.villaList);
        }
        [HttpGet("{id:int}", Name = "GetVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult <VillaDTO> GetVilla(int id)
        {
            if (id == 0) return BadRequest();
            var villa= Ok(VillaStore.villaList.FirstOrDefault(u => u.Id == id));
            if(villa == null) return NotFound();
            return Ok(villa);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult <VillaDTO> CreateVilla([FromBody] VillaDTO villaDTO) { 
            if (VillaStore.villaList.FirstOrDefault(u=>u.Name.ToLower() == villaDTO.Name.ToLower()) != null)
            {
                ModelState.AddModelError("CustomError", "Villa Already Exists!");
                return BadRequest(ModelState);
            }
            if(!ModelState.IsValid) return BadRequest(ModelState);
            if (villaDTO == null) { return BadRequest(villaDTO); }
            if(villaDTO.Id > 0){ return StatusCode(StatusCodes.Status500InternalServerError); }
            villaDTO.Id = VillaStore.villaList.OrderByDescending(u => u.Id).FirstOrDefault().Id+1;
            VillaStore.villaList.Add(villaDTO);
            return CreatedAtRoute("GetVilla", new {  id= villaDTO.Id},villaDTO);
        }
        [HttpDelete("{id:int}", Name ="DeleteVilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult DeleteVilla(int id)
        {
            if(id == 0)  return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault(u=> u.Id == id);
            if(villa == null) return NotFound();
            VillaStore.villaList.Remove(villa);
            return NoContent();
        }

        [HttpPut("{id:int}",Name = "UpdateVillaPut")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult UpdateVillaPut(int id, [FromBody]VillaDTO villaDTO)
        {
            if(villaDTO == null || id != villaDTO.Id) return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault(u=> u.Id == id);
            if(villa == null) return NotFound();
            villa.Name = villaDTO.Name;
            return NoContent() ;
        }

        [HttpPatch("{id:int}", Name ="UpdateVillaPatch")]
        public IActionResult UpdateVillaPatch(int id, JsonPatchDocument<VillaDTO> villaDTO)
        {
            if (villaDTO == null || id == 0) return BadRequest();
            var villa = VillaStore.villaList.FirstOrDefault(u => u.Id == id);
            if (villa == null) return NotFound();
            villaDTO.ApplyTo(villa, ModelState);
            if(!ModelState.IsValid) return BadRequest(ModelState);
            return NoContent();
        }
    }
}
