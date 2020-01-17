using System;
using Xunit;
using homesteadAPI.Controllers;
using homesteadAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;

namespace homesteadAPITests
{
    public class PlantsControllerTests
    {
        [Fact]
        public void Test1()
        {
            // In-memory database only exists while the connection is open
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<HomesteadDataContext>()
                    .UseSqlite(connection)
                    .Options;

                // Run the test against one instance of the context
                using (var context = new HomesteadDataContext(options))
                {
                    //arrange
                    homesteadAPI.Config customConfig = new homesteadAPI.Config(context);
                    customConfig.InitializeDatabase();

                    PlantsController x = new PlantsController(context);

                    //act
                    Task<ActionResult<Plant>> result = x.GetPlant(1);
                    
                    //assert
                    Assert.Equal("Dinosaur Kale", result.Result.Value.Name);

                }

            }
            finally
            {
                connection.Close();
            }

        }
    }
}
