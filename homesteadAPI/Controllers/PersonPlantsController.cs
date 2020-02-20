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
    public class PersonPlantsController : ControllerBase
    {
        private readonly HomesteadDataContext _context;

        public PersonPlantsController(HomesteadDataContext context)
        {
            _context = context;
        }

        

        // GET: api/PersonPlants/5
        [HttpGet("{id}")]
        public async Task<ActionResult<PersonPlant>> GetPersonPlant(long id)
        {
            var personPlant = await _context.PersonPlants.FindAsync(id);

            if (personPlant == null)
            {
                return NotFound();
            }

            return personPlant;
        }

        // PUT: api/PersonPlants/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<ActionResult<PersonPlant>> PutPersonPlant(long id, PersonPlant personPlant)
        {
            if (id != personPlant.ID)
            {
                return BadRequest();
            }

           var dbPersonPlant = _context.PersonPlants.Find(personPlant.ID);
            if (dbPersonPlant != null){
                dbPersonPlant.Name = personPlant.Name;
                dbPersonPlant.Amount = personPlant.Amount;
                dbPersonPlant.AmountType = personPlant.AmountType;
                dbPersonPlant.BuyDate = personPlant.BuyDate;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonPlantExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return dbPersonPlant;
        }

        // POST: api/PersonPlants
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<PersonPlant>> PostPersonPlant(PersonPlant personPlant)
        {
            _context.PersonPlants.Add(personPlant);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPersonPlant", new { id = personPlant.ID }, personPlant);
        }

        // DELETE: api/PersonPlants/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<PersonPlant>> DeletePersonPlant(long id)
        {
            var personPlant = await _context.PersonPlants.FindAsync(id);
            if (personPlant == null)
            {
                return NotFound();
            }

            _context.PersonPlants.Remove(personPlant);
            await _context.SaveChangesAsync();

            return personPlant;
        }

        private bool PersonPlantExists(long id)
        {
            return _context.PersonPlants.Any(e => e.ID == id);
        }
    }
}