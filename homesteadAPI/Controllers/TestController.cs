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
    public class TestController : BaseController
    {

        private readonly ILogger<TestController> _logger;

        public TestController(HomesteadDataContext context, IConfiguration configuration, ILogger<TestController> logger)
      : base(context, configuration)
        {
            _logger = logger;
            _logger.LogInformation("testcontroller construct");
        }

        [HttpGet("AccessDb")]
        public async Task<string> AccessDb()
        {
            try
            {
                var query = await _context.Plants.ToListAsync();
                if (query.Count > 0){
                    return "Found " + query.Count + " plants";
                }
                return "Found zero plants";

            }
            catch (Exception ex)
            {
                return "Error message: " + ex.Message + ex.StackTrace;
            }
        }


        [HttpGet("IsUp")]
        public string IsUp()
        {
            _logger.LogInformation("isup func");
            return "YES";
        }

    }
}
