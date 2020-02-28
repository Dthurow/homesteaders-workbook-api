using Microsoft.AspNetCore.Mvc;
using homesteadAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
using System;

namespace homesteadAPI.Controllers
{

    public class BaseController : ControllerBase
    {
        protected readonly HomesteadDataContext _context;
        protected IConfiguration Configuration { get; }

        public BaseController(HomesteadDataContext context, IConfiguration configuration)
        {
            _context = context;
            Configuration = configuration;
        }

        public virtual string GetPersonEmail()
        {
            string audience = Configuration["Auth0:Audience"];
            return HttpContext.User.Claims.FirstOrDefault(c => c.Type == audience + "email")?.Value;
        }

        public virtual long GetPersonID(){
            string email = GetPersonEmail();
            if (!string.IsNullOrEmpty(email)){
                var person = _context.Persons.FirstOrDefault(x=> x.Email == email);
                if (person != null){
                    return person.ID;
                }
            }
            //if it gets here, something's wrong
            throw new ApplicationException("Cannot find person with email '" + email + "'");
        }


    }

}