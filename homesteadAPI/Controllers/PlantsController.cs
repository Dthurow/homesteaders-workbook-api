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
    public class PlantsController : ControllerBase
    {
        private readonly HomesteadDataContext _context;

        public PlantsController(HomesteadDataContext context)
        {
            _context = context;
        }

        // GET: api/Plants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlants()
        {
            return await _context.Plants.ToListAsync();
        }

        // GET: api/Plants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(long id)
        {
            var plant = await _context.Plants.FindAsync(id);

            if (plant == null)
            {
                return NotFound();
            }

            return plant;
        }

        // PUT: api/Plants/5
        [HttpPut("{id}")]
        [Authorize("admin_user")]
        public async Task<ActionResult<Plant>> PutPlant(long id, Plant plant)
        {
            if (id != plant.ID)
            {
                return BadRequest();
            }
            var dbplant = _context.Plants.Find(plant.ID);
            if (dbplant != null){
                dbplant.Name = plant.Name;
                dbplant.Description = plant.Description;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return dbplant;
        }

        // POST: api/Plants
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize("admin_user")]
        public async Task<ActionResult<Plant>> PostPlant(Plant plant)
        {
            _context.Plants.Add(plant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlant", new { id = plant.ID }, plant);
        }

        // DELETE: api/Plants/5
        [HttpDelete("{id}")]
        [Authorize("admin_user")]
        public async Task<ActionResult<Plant>> DeletePlant(long id)
        {
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
            {
                return NotFound();
            }

            //if the plant has any associated garden plants to it, can't delete
            if (plant.GardenPlants != null && plant.GardenPlants.Any()){
                return Problem("Plant must have no associated Garden Plants associated with it", statusCode: 428);
            }

            _context.Plants.Remove(plant);
            await _context.SaveChangesAsync();

            return plant;
        }

        private bool PlantExists(long id)
        {
            return _context.Plants.Any(e => e.ID == id);
        }
    }
}
