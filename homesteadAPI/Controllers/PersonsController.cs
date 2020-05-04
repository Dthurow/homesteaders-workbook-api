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
    public class PersonsController : BaseController
    {

        public PersonsController(HomesteadDataContext context, IConfiguration configuration) : base(context, configuration)
        {
        }

        [HttpGet("loggedin")]
        public async Task<ActionResult<Person>> LogInCheck(string name)
        {
            bool exists = false;
            try
            {
                GetPersonID();
                exists = true;
            }
            catch
            {
                //happens if person doesn't exist
            }
            if (!exists)
            {
                //person doesn't exist in database, create them
                string email = GetPersonEmail();

                Person person = new Person();
                person.Email = email;
                person.CreatedOn = DateTime.Now;
                person.Name = name;

                _context.Persons.Add(person);
                await _context.SaveChangesAsync();
                return CreatedAtAction("GetPerson", new { id = person.ID }, person);
            }
            else{
                return Ok();
            }
        }

        // GET: api/Persons
        [HttpGet]
        [Authorize("admin_user")]
        public async Task<ActionResult<IEnumerable<Person>>> GetPersons()
        {
            return await _context.Persons.ToListAsync();
        }

        // GET: api/Persons/5
        [HttpGet("{id}")]
        [Authorize("admin_user")]
        public async Task<ActionResult<Person>> GetPerson(long id)
        {
            var person = await _context.Persons.FindAsync(id);

            if (person == null)
            {
                return NotFound();
            }

            return person;
        }

        // PUT: api/Persons/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        [Authorize("admin_user")]
        public async Task<IActionResult> PutPerson(long id, Person person)
        {
            if (id != person.ID)
            {
                return BadRequest();
            }

            _context.Entry(person).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PersonExists(id))
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

        // POST: api/Persons
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        [Authorize("admin_user")]
        public async Task<ActionResult<Person>> PostPerson(Person person)
        {
            _context.Persons.Add(person);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetPerson", new { id = person.ID }, person);
        }

        // DELETE: api/Persons/5
        [HttpDelete("{id}")]
        [Authorize("admin_user")]
        public async Task<ActionResult<Person>> DeletePerson(long id)
        {
            var person = await _context.Persons.FindAsync(id);
            if (person == null)
            {
                return NotFound();
            }

            _context.Persons.Remove(person);
            await _context.SaveChangesAsync();

            return person;
        }

        private bool PersonExists(long id)
        {
            return _context.Persons.Any(e => e.ID == id);
        }
    }
}
