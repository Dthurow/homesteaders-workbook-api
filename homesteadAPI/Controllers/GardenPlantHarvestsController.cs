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
    public class GardenPlantHarvestsController : BaseController
    {
        private readonly ILogger<GardenPlantHarvestsController> _logger;

        #region constructors
        public GardenPlantHarvestsController(HomesteadDataContext context, IConfiguration configuration, ILogger<GardenPlantHarvestsController> logger)
       : base(context, configuration)
        {
            _logger = logger;
        }
        #endregion

        #region http methods

        // GET: api/GardenPlantHarvests
         [HttpGet]
        public async Task<ActionResult<IEnumerable<GardenPlantHarvest>>> GetGardenPlantHarvests()
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
                    //get plant harvests that are related to this person and also are related to the given garden plant
                    var query = _context.GardenPlantHarvests.Where(GardenPlantHarvest => GardenPlantHarvest.GardenPlant.Plant.Person.Email == personEmail);
                    return await query.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }
        }


        // GET: api/GardenPlantHarvests/gardenplant/5
        [HttpGet("gardenplant/{id}")]
        public async Task<ActionResult<IEnumerable<GardenPlantHarvest>>> GetGardenPlantHarvestsForPlant(int id)
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
                    //get plant harvests that are related to this person and also are related to the given garden plant
                    var query = _context.GardenPlantHarvests.Where(GardenPlantHarvest => GardenPlantHarvest.GardenPlant.Plant.Person.Email == personEmail && GardenPlantHarvest.GardenPlant.ID == id);
                    return await query.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }
        }

        // GET: api/GardenPlantHarvests/5
        [HttpGet("{id}")]
        public async Task<ActionResult<GardenPlantHarvest>> GetGardenPlantHarvest(long id)
        {
            try
            {
                string personEmail = GetPersonEmail();

                if (string.IsNullOrEmpty(personEmail))
                {
                    return NoContent();
                }

                var GardenPlantHarvest = await GetGardenPlantHarvest(personEmail, id);

                if (GardenPlantHarvest == null)
                {
                    return NotFound();
                }

                return GardenPlantHarvest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }
        }

        // PUT: api/GardenPlantHarvests/5
        [HttpPut("{id}")]
        public async Task<ActionResult<GardenPlantHarvest>> PutGardenPlantHarvest(long id, GardenPlantHarvest GardenPlantHarvest)
        {
            try
            {
                string personEmail = GetPersonEmail();

                if (string.IsNullOrEmpty(personEmail))
                {
                    return NoContent();
                }

                if (id != GardenPlantHarvest.ID)
                {
                    return BadRequest();
                }
                var dbGardenPlantHarvest = await GetGardenPlantHarvest(personEmail, GardenPlantHarvest.ID);
                if (dbGardenPlantHarvest != null)
                {
                    dbGardenPlantHarvest.AmountHarvested = GardenPlantHarvest.AmountHarvested;
                    dbGardenPlantHarvest.GardenPlant = GardenPlantHarvest.GardenPlant;
                    dbGardenPlantHarvest.HarvestDate = GardenPlantHarvest.HarvestDate;
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GardenPlantHarvestExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return dbGardenPlantHarvest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }
        }

        // POST: api/GardenPlantHarvests
        [HttpPost]
        public async Task<ActionResult<GardenPlantHarvest>> PostGardenPlantHarvest(GardenPlantHarvest GardenPlantHarvest)
        {
            try
            {
                long personID = GetPersonID();

                if (GardenPlantHarvest.GardenPlantID == 0)
                {
                    return BadRequest();
                }

                //make sure they don't try to link to the wrong plant owned by someone else
                var plant = await _context.GardenPlants.Where(x=> x.ID == GardenPlantHarvest.GardenPlantID).Select(x=> x.Plant.PersonID).FirstOrDefaultAsync();
                if (plant != personID)
                {
                    //they're trying to steal plants!
                    return BadRequest();
                }


                GardenPlantHarvest newPlantHarvest = new GardenPlantHarvest();
                newPlantHarvest.AmountHarvested = GardenPlantHarvest.AmountHarvested;
                newPlantHarvest.GardenPlantID = GardenPlantHarvest.GardenPlantID;
                newPlantHarvest.HarvestDate = GardenPlantHarvest.HarvestDate;

                _context.GardenPlantHarvests.Add(newPlantHarvest);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGardenPlantHarvest", new { id = GardenPlantHarvest.ID }, newPlantHarvest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save value");
                return BadRequest();
            }
        }

        // DELETE: api/GardenPlantHarvests/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<GardenPlantHarvest>> DeleteGardenPlantHarvest(long id)
        {
            try
            {
                string personEmail = GetPersonEmail();

                if (string.IsNullOrEmpty(personEmail))
                {
                    return NoContent();
                }

                var GardenPlantHarvest = await GetGardenPlantHarvest(personEmail, id);
                if (GardenPlantHarvest == null)
                {
                    return NotFound();
                }

                _context.GardenPlantHarvests.Remove(GardenPlantHarvest);
                await _context.SaveChangesAsync();

                return GardenPlantHarvest;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete value");
                return BadRequest();
            }
        }

        #endregion

        #region helper methods

        private bool GardenPlantHarvestExists(long id)
        {
            return _context.GardenPlantHarvests.Any(e => e.ID == id);
        }

        // make sure the harvest they get is related to the person
        private Task<GardenPlantHarvest> GetGardenPlantHarvest(string personEmail, long plantHarvestID)
        {

            var query = from plantHarvest in _context.GardenPlantHarvests
                        where plantHarvest.GardenPlant.Plant.Person.Email == personEmail
                        && plantHarvest.ID == plantHarvestID
                        select plantHarvest;

            return query.FirstOrDefaultAsync();

        }

        #endregion

    }
}
