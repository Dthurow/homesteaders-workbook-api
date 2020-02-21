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

namespace homesteadAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PlantsController : ControllerBase
    {
        private readonly HomesteadDataContext _context;
        private IConfiguration Configuration { get; }

        public PlantsController(HomesteadDataContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        // GET: api/Plants
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Plant>>> GetPlants()
        {
            string personEmail = GetPersonEmail();

            if (string.IsNullOrEmpty(personEmail))
            {
                return NoContent();
            }
            else
            {
                var query = _context.Plants.Where(plant => plant.Person.Email == personEmail);
                return await query.ToListAsync();
            }
        }

        // GET: api/Plants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Plant>> GetPlant(long id)
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

            if (plant.Person.Email != GetPersonEmail())
            {
                return NoContent();
            }

            return plant;
        }

        // PUT: api/Plants/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Plant>> PutPlant(long id, Plant plant)
        {
            if (id != plant.ID)
            {
                return BadRequest();
            }
            var dbplant = _context.Plants.Find(plant.ID);
            if (dbplant.Person.Email != GetPersonEmail())
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

        // POST: api/Plants

        [HttpPost]

        public async Task<ActionResult<Plant>> PostPlant(Plant plant)
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
            }
            else
            {
                return BadRequest();
            }

            _context.Plants.Add(dbplant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPlant", new { id = plant.ID }, plant);
        }

        // DELETE: api/Plants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Plant>> DeletePlant(long id)
        {
            //TODO - make sure they're only deleting plants that belong to them
            var plant = await _context.Plants.FindAsync(id);
            if (plant == null)
            {
                return NotFound();
            }
            if (plant.Person.Email != GetPersonEmail()){
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

        private bool PlantExists(long id)
        {
            return _context.Plants.Any(e => e.ID == id);
        }

        private string GetPersonEmail()
        {
            string audience = Configuration["Auth0:Audience"];
            return HttpContext.User.Claims.FirstOrDefault(c => c.Type == audience + "email")?.Value;
        }
    }
}
