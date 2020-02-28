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
    public class GardensController : BaseController
    {
        private readonly ILogger<GardensController> _logger;

        public GardensController(HomesteadDataContext context, IConfiguration configuration, ILogger<GardensController> logger)
       : base(context, configuration)
        {
            _logger = logger;
        }

        // GET: api/Gardens
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Garden>>> GetGardens()
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
                    var query = _context.Gardens.Where(garden => garden.Person.Email == personEmail);
                    return await query.ToListAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }

        }

        // GET: api/Gardens/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Garden>> GetGarden(long id)
        {
            try
            {
                string personEmail = GetPersonEmail();

                if (string.IsNullOrEmpty(personEmail))
                {
                    return NoContent();
                }

                var garden = await GetGarden(personEmail, id);

                if (garden == null)
                {
                    return NotFound();
                }

                return garden;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to return value");
                return BadRequest();
            }

        }


        // PUT: api/Gardens/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Garden>> PutGarden(long id, Garden garden)
        {
            try
            {
                string personEmail = GetPersonEmail();

                if (string.IsNullOrEmpty(personEmail))
                {
                    return NoContent();
                }

                if (id != garden.ID)
                {
                    return BadRequest();
                }

                var dbGarden = await GetGarden(personEmail, garden.ID);
                if (dbGarden != null)
                {
                    dbGarden.Name = garden.Name;
                    dbGarden.GrowingSeasonStartDate = garden.GrowingSeasonStartDate;
                    dbGarden.GrowingSeasonEndDate = garden.GrowingSeasonEndDate;
                    dbGarden.Length = garden.Length;
                    dbGarden.Width = garden.Width;
                    dbGarden.MeasurementType = garden.MeasurementType;

                }

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

                return dbGarden;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to update value");
                return BadRequest();
            }
        }

        // POST: api/Gardens
        [HttpPost]
        public async Task<ActionResult<Garden>> PostGarden(Garden garden)
        {
            try
            {
                long personID = GetPersonID();


                var newGarden = new Garden();
                if (newGarden != null)
                {
                    newGarden.Name = garden.Name;
                    newGarden.GrowingSeasonStartDate = garden.GrowingSeasonStartDate;
                    newGarden.GrowingSeasonEndDate = garden.GrowingSeasonEndDate;
                    newGarden.Length = garden.Length;
                    newGarden.Width = garden.Width;
                    newGarden.MeasurementType = garden.MeasurementType;
                    newGarden.PersonID = personID;

                }



                _context.Gardens.Add(newGarden);
                await _context.SaveChangesAsync();

                return CreatedAtAction("GetGarden", new { id = newGarden.ID }, newGarden);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to save value");
                return BadRequest();
            }


        }

        // DELETE: api/Gardens/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Garden>> DeleteGarden(long id)
        {
              try
            {
                string personEmail = GetPersonEmail();

                if (string.IsNullOrEmpty(personEmail))
                {
                    return NoContent();
                }

                var garden = await GetGarden(personEmail, id);
                if (garden == null)
                {
                    return NotFound();
                }
               
                _context.Gardens.Remove(garden);
                await _context.SaveChangesAsync();

                return garden;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to delete value");
                return BadRequest();
            }
        }

        #region helper methods
        private bool GardenExists(long id)
        {
            return _context.Gardens.Any(e => e.ID == id);
        }

        private Task<Garden> GetGarden(string personEmail, long gardenID)
        {

            var query = from garden in _context.Gardens
                        where garden.Person.Email == personEmail
                        && garden.ID == gardenID
                        select garden;

            return query.FirstOrDefaultAsync();

        }

        #endregion

    }
}
