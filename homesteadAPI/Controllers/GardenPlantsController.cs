using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using homesteadAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace homesteadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GardenPlantsController : BaseController
    {
        private readonly ILogger<GardenPlantsController> _logger;

        #region constructors
        public GardenPlantsController(HomesteadDataContext context, IConfiguration configuration, ILogger<GardenPlantsController> logger)
       : base(context, configuration)
        {
            _logger = logger;
        }
        #endregion

        #region http methods
        // GET: api/GardenPlants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<GardenPlant>>> GetGardenPlants()
        {
            try
            {
                string personEmail = GetPersonEmail();

                if (string.IsNullOrEmpty(personEmail))
                {
                    return NoContent();
                }
                else
                {
                    var query = _context.GardenPlants.Where(gardenPlant => gardenPlant.Plant.Person.Email == personEmail);
                    return await query.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }
        }

        // GET: api/GardenPlants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GardenPlant>> GetGardenPlant(long id)
        {
            try
            {
                string personEmail = GetPersonEmail();

                if (string.IsNullOrEmpty(personEmail))
                {
                    return NoContent();
                }

                var gardenPlant = await GetGardenPlant(personEmail, id);

                if (gardenPlant == null)
                {
                    return NotFound();
                }

                return gardenPlant;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }
        }

        // PUT: api/GardenPlants/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GardenPlant>> PutGardenPlant(long id, GardenPlant gardenPlant)
        {
            try
            {
                string personEmail = GetPersonEmail();

                if (string.IsNullOrEmpty(personEmail))
                {
                    return NoContent();
                }

                if (id != gardenPlant.ID)
                {
                    return BadRequest();
                }
                var dbGardenPlant = await GetGardenPlant(personEmail, gardenPlant.ID);
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }
        }

        // POST: api/GardenPlants
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<GardenPlant>> PostGardenPlant(GardenPlant gardenPlant)
        {
            try
            {
                long personID = GetPersonID();                

                if (gardenPlant.PlantID == 0 || gardenPlant.GardenID == 0)
                {
                    return BadRequest();
                }

                //make sure they don't try to link to the wrong plant owned by someone else
                var plant = await _context.Plants.FindAsync(gardenPlant.PlantID);
                if (plant.PersonID != personID){
                    //they're trying to steal plants!
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save value");
                return BadRequest();
            }
        }

        // DELETE: api/GardenPlants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GardenPlant>> DeleteGardenPlant(long id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete value");
                return BadRequest();
            }
        }

        #endregion

        #region helper methods

        private bool GardenPlantExists(long id)
        {
            return _context.GardenPlants.Any(e => e.ID == id);
        }

        private Task<GardenPlant> GetGardenPlant(string personEmail, long plantID)
        {

            var query = from plant in _context.GardenPlants
                        where plant.Plant.Person.Email == personEmail
                        && plant.ID == plantID
                        select plant;

            return query.FirstOrDefaultAsync();

        }

        #endregion

    }
}
