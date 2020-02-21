using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using homesteadAPI.Models;
using Microsoft.AspNetCore.Authorization;


namespace homesteadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GardensController : ControllerBase
    {
        private readonly HomesteadDataContext _context;
        

        public GardensController(HomesteadDataContext context)
        {
            _context = context;
        }

        // GET: api/Gardens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Garden>>> GetGardens()
        {
            return await _context.Gardens.ToListAsync();
        }

        // GET: api/Gardens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Garden>> GetGarden(long id)
        {
            var garden = await _context.Gardens.FindAsync(id);

            if (garden == null)
            {
                return NotFound();
            }

            return garden;
        }

        // GET: api/Gardens/5/GardenPlants
        [HttpGet("{id}/GardenPlants")]
        public async Task<ActionResult<ICollection<GardenPlant>>> GetGardensGardenPlants(long id)
        {
            var garden = await _context.Gardens.FindAsync(id);

            if (garden == null)
            {
                return NotFound();
            }

            return Ok(garden.GardenPlants);
        }


        // PUT: api/Gardens/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGarden(long id, Garden garden)
        {
            if (id != garden.ID)
            {
                return BadRequest();
            }

             var dbGarden = _context.Gardens.Find(garden.ID);
            if (dbGarden != null)
            {
                dbGarden.Name = garden.Name;
                dbGarden.GrowingSeasonStartDate = garden.GrowingSeasonStartDate;
                dbGarden.GrowingSeasonEndDate = garden.GrowingSeasonEndDate;
                dbGarden.Length = garden.Length;
                dbGarden.Width = garden.Width;
                dbGarden.MeasurementType = garden.MeasurementType;
               
            }


            _context.Entry(garden).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GardenExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Gardens
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Garden>> PostGarden(Garden garden)
        {
            
             var dbGarden = new Garden();
            if (dbGarden != null)
            {
                dbGarden.Name = garden.Name;
                dbGarden.GrowingSeasonStartDate = garden.GrowingSeasonStartDate;
                dbGarden.GrowingSeasonEndDate = garden.GrowingSeasonEndDate;
                dbGarden.Length = garden.Length;
                dbGarden.Width = garden.Width;
                dbGarden.MeasurementType = garden.MeasurementType;
                //TODO - pull personID from their access token, not from their posted garden
                dbGarden.PersonID = garden.PersonID;
               
            }


            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGarden", new { id = garden.ID }, garden);
        }

        // DELETE: api/Gardens/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Garden>> DeleteGarden(long id)
        {
            
            //TODO - make sure they're only deleting gardens that belong to them
            var garden = await _context.Gardens.FindAsync(id);
            if (garden == null)
            {
                return NotFound();
            }

            _context.Gardens.Remove(garden);
            await _context.SaveChangesAsync();

            return garden;
        }

        private bool GardenExists(long id)
        {
            return _context.Gardens.Any(e => e.ID == id);
        }
    }
}
