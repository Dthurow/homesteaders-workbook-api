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
    public class PlantsController : BaseController
    {
        private readonly ILogger<PlantsController> _logger;

        public PlantsController(HomesteadDataContext context, IConfiguration configuration, ILogger<PlantsController> logger)
        : base(context, configuration)
        {
            _logger = logger;
        }

        // GET: api/Plants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlants()
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
                    var query = await _context.Plants.Where(plant => plant.Person.Email == personEmail).ToListAsync();
                    return query;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }
        }

        // GET: api/Plants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(long id)
        {
            try
            {
                string personEmail = GetPersonEmail();

                if (string.IsNullOrEmpty(personEmail))
                {
                    return NoContent();
                }

                var plant = await _context.Plants.FindAsync(id);

                if (plant == null)
                {
                    return NotFound();
                }

                if (!plant.Person.Email.Equals(GetPersonEmail(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return NoContent();
                }

                return plant;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }
        }

        // GET: api/Plants
        [HttpGet("plantgroups")]
        public async Task<ActionResult<IEnumerable<PlantGroup>>> GetPlantGroups()
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
                    var query = await _context.PlantGroups.Where(plantGroup => plantGroup.Person.Email == personEmail).ToListAsync();
                    return query;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }
        }


        // PUT: api/Plants/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Plant>> PutPlant(long id, Plant plant)
        {
            try
            {
                if (id != plant.ID)
                {
                    return BadRequest();
                }
                var dbplant = _context.Plants.Find(plant.ID);
                if (!dbplant.Person.Email.Equals(GetPersonEmail(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return BadRequest();
                }
                if (dbplant != null)
                {
                    dbplant.Name = plant.Name;
                    dbplant.Description = plant.Description;
                    dbplant.PlantGroupID = plant.PlantGroupID;
                    dbplant.SeedLife = plant.SeedLife;
                    dbplant.BuyDate = plant.BuyDate;
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update value");
                return BadRequest();
            }
        }

        // POST: api/Plants

        [HttpPost]

        public async Task<ActionResult<Plant>> PostPlant(Plant plant)
        {
            try
            {
                var dbplant = new Plant();
                var person = _context.Persons.FirstOrDefault(p => p.Email == GetPersonEmail());

                if (person != null)
                {
                    dbplant.Name = plant.Name;
                    dbplant.Description = plant.Description;
                    dbplant.PersonID = person.ID;
                    dbplant.PlantGroupID = plant.PlantGroupID;
                    dbplant.SeedLife = plant.SeedLife;
                    dbplant.BuyDate = plant.BuyDate;
                    dbplant.FoodCategoryID = plant.FoodCategoryID;

                }
                else
                {
                    return BadRequest();
                }

                _context.Plants.Add(dbplant);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPlant", new { id = dbplant.ID }, dbplant);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create value");
                return BadRequest();
            }
        }

        // DELETE: api/Plants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Plant>> DeletePlant(long id)
        {
            try
            {
                //make sure they're only deleting plants that belong to them
                var plant = await _context.Plants.FindAsync(id);
                if (plant == null)
                {
                    return NotFound();
                }
                if (!plant.Person.Email.Equals(GetPersonEmail(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return BadRequest();
                }

                //if the plant has any associated garden plants to it, can't delete
                if (plant.GardenPlants != null && plant.GardenPlants.Any())
                {
                    return Problem("Plant must have no associated Garden Plants associated with it", statusCode: 428);
                }

                _context.Plants.Remove(plant);
                await _context.SaveChangesAsync();

                return plant;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete value");
                return BadRequest();
            }
        }

        private bool PlantExists(long id)
        {
            return _context.Plants.Any(e => e.ID == id);
        }


    }
}
