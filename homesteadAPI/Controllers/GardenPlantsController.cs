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
    public class GardenPlantsController : ControllerBase
    {
        private readonly HomesteadDataContext _context;

        public GardenPlantsController(HomesteadDataContext context)
        {
            _context = context;
        }

        // GET: api/GardenPlants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GardenPlant>>> GetGardenPlants()
        {
            return await _context.GardenPlants.ToListAsync();
        }

        // GET: api/GardenPlants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GardenPlant>> GetGardenPlant(long id)
        {
            var gardenPlant = await _context.GardenPlants.FindAsync(id);

            if (gardenPlant == null)
            {
                return NotFound();
            }

            return gardenPlant;
        }

        // PUT: api/GardenPlants/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GardenPlant>> PutGardenPlant(long id, GardenPlant gardenPlant)
        {
            if (id != gardenPlant.ID)
            {
                return BadRequest();
            }
            var dbGardenPlant = _context.GardenPlants.Find(gardenPlant.ID);
            if (dbGardenPlant != null)
            {
                dbGardenPlant.Name = gardenPlant.Name;
                dbGardenPlant.AmountPlanted = gardenPlant.AmountPlanted;
                dbGardenPlant.YieldEstimatedPerAmountPlanted = gardenPlant.YieldEstimatedPerAmountPlanted;
                dbGardenPlant.YieldType = gardenPlant.YieldType;
                dbGardenPlant.AmountPlantedType = gardenPlant.AmountPlantedType;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!GardenPlantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return dbGardenPlant;
        }

        // POST: api/GardenPlants
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<GardenPlant>> PostGardenPlant(GardenPlant gardenPlant)
        {

            if (gardenPlant.PlantID == 0 || gardenPlant.GardenID == 0){
                return BadRequest();
            }
            GardenPlant newPlant = new GardenPlant();
            newPlant.Name = gardenPlant.Name;
            newPlant.GardenID = gardenPlant.GardenID;
            newPlant.PlantID = gardenPlant.PlantID;
            newPlant.YieldType = gardenPlant.YieldType;
            newPlant.AmountPlanted = gardenPlant.AmountPlanted;
            newPlant.AmountPlantedType = gardenPlant.AmountPlantedType;
            newPlant.YieldEstimatedPerAmountPlanted = gardenPlant.YieldEstimatedPerAmountPlanted;
            newPlant.AmountPlanted = gardenPlant.AmountPlanted;
            

            _context.GardenPlants.Add(newPlant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetGardenPlant", new { id = gardenPlant.ID }, newPlant);
        }

        // DELETE: api/GardenPlants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GardenPlant>> DeleteGardenPlant(long id)
        {
            //TODO - make sure they're only deleting garden plants that belong to them
            var gardenPlant = await _context.GardenPlants.FindAsync(id);
            if (gardenPlant == null)
            {
                return NotFound();
            }

            _context.GardenPlants.Remove(gardenPlant);
            await _context.SaveChangesAsync();

            return gardenPlant;
        }

        private bool GardenPlantExists(long id)
        {
            return _context.GardenPlants.Any(e => e.ID == id);
        }
    }
}
