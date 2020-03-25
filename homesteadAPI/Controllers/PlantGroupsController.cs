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
    public class PlantGroupsController : BaseController
    {
        private readonly ILogger<PlantGroupsController> _logger;

        public PlantGroupsController(HomesteadDataContext context, IConfiguration configuration, ILogger<PlantGroupsController> logger)
        : base(context, configuration)
        {
            _logger = logger;
        }

       
        // GET: api/PlantGroups
        [HttpGet]
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

          // GET: api/PlantGroups/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PlantGroup>> GetPlantGroup(long id)
        {
            try
            {
                string personEmail = GetPersonEmail();

                if (string.IsNullOrEmpty(personEmail))
                {
                    return NoContent();
                }

                var plantGroup = await _context.PlantGroups.FindAsync(id);

                if (plantGroup == null)
                {
                    return NotFound();
                }

                if (!plantGroup.Person.Email.Equals(GetPersonEmail(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return NoContent();
                }

                return plantGroup;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }
        }



        // PUT: api/PlantGroups/5
        [HttpPut("{id}")]
        public async Task<ActionResult<PlantGroup>> PutPlantGroup(long id, PlantGroup plantGroup)
        {
            try
            {
                if (id != plantGroup.ID)
                {
                    return BadRequest();
                }
                var dbplantGroup = _context.PlantGroups.Find(plantGroup.ID);
                if (!dbplantGroup.Person.Email.Equals(GetPersonEmail(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return BadRequest();
                }
                if (dbplantGroup != null)
                {
                    dbplantGroup.Name = plantGroup.Name;
                    dbplantGroup.Description = plantGroup.Description;
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlantGroupExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return dbplantGroup;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update value");
                return BadRequest();
            }
        }

        // POST: api/PlantGroups

        [HttpPost]

        public async Task<ActionResult<PlantGroup>> PostPlantGroup(PlantGroup plantGroup)
        {
            try
            {
                var dbplantGroup = new PlantGroup();
                var person = _context.Persons.FirstOrDefault(p => p.Email == GetPersonEmail());

                if (person != null)
                {
                    dbplantGroup.Name = plantGroup.Name;
                    dbplantGroup.Description = plantGroup.Description;
                    dbplantGroup.PersonID = person.ID;
                    dbplantGroup.CreatedOn = DateTime.Now;

                }
                else
                {
                    return BadRequest();
                }

                _context.PlantGroups.Add(dbplantGroup);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetPlantGroup", new { id = dbplantGroup.ID }, dbplantGroup);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create value");
                return BadRequest();
            }
        }

        // DELETE: api/PlantGroups/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PlantGroup>> DeletePlantGroup(long id)
        {
            try
            {
                //make sure they're only deleting plants that belong to them
                var plantGroup = await _context.PlantGroups.FindAsync(id);
                if (plantGroup == null)
                {
                    return NotFound();
                }
                if (!plantGroup.Person.Email.Equals(GetPersonEmail(), StringComparison.CurrentCultureIgnoreCase))
                {
                    return BadRequest();
                }

                //if the plant has any associated garden plants to it, can't delete
                if (plantGroup.Plants != null && plantGroup.Plants.Any())
                {
                    return Problem("Plant Group must have no associated Plants associated with it", statusCode: 428);
                }

                _context.PlantGroups.Remove(plantGroup);
                await _context.SaveChangesAsync();

                return plantGroup;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete value");
                return BadRequest();
            }
        }

        private bool PlantGroupExists(long id)
        {
            return _context.PlantGroups.Any(e => e.ID == id);
        }


    }
}
