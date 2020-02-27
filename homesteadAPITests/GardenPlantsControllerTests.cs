using System;
using Xunit;
using homesteadAPI.Controllers;
using homesteadAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;
using System.Linq;
using Moq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace homesteadAPITests
{
    public class GardenPlantsControllerTests
    {

        private void SetupDataContextAndConfig(HomesteadDataContext context)
        {
            homesteadAPI.Config dbconfig = new homesteadAPI.Config(context);
            dbconfig.InitializeDatabase();


        }


    }
}
