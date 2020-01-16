using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using homesteadAPI.Models;

namespace homesteadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGarden(long id, Garden garden)
        {
            if (id != garden.ID)
            {
                return BadRequest();
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
            _context.Gardens.Add(garden);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGarden", new { id = garden.ID }, garden);
        }

        // DELETE: api/Gardens/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Garden>> DeleteGarden(long id)
        {
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
