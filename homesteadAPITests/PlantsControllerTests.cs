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
    public class PlantsControllerTests
    {
        [Fact]
        public void GetPlantsTest()
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


                    var mockConfig = new Mock<IConfiguration>();
                    var mockLog = new Mock<ILogger<PlantsController>>();
                    SetupDataContextAndConfig(context);

                    //mock the controller so I can set the GetPersonEmail() to whatever I want without worrying about access tokens
                    var controllerMock = new Mock<PlantsController>(context, mockConfig.Object, mockLog.Object);
                    controllerMock.Setup(m => m.GetPersonEmail()).Returns("dan.thurow@gmail.com");
                    controllerMock.CallBase = true;


                    //act
                    var result = controllerMock.Object.GetPlants();

                    //assert
                    Assert.Equal(3, result.Result.Value.Count());

                }

            }
            finally
            {
                connection.Close();
            }

        }


        [Fact]
        public void GetPlantByIDTest()
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


                    var mockConfig = new Mock<IConfiguration>();
                    var mockLog = new Mock<ILogger<PlantsController>>();
                    SetupDataContextAndConfig(context);

                    //mock the controller so I can set the GetPersonEmail() to whatever I want without worrying about access tokens
                    var controllerMock = new Mock<PlantsController>(context, mockConfig.Object, mockLog.Object);
                    controllerMock.Setup(m => m.GetPersonEmail()).Returns("jim@bob.com");
                    controllerMock.CallBase = true;


                    //act
                    Task<ActionResult<Plant>> result = controllerMock.Object.GetPlant(1);

                    //assert
                    Assert.Equal("Dinosaur Kale", result.Result.Value.Name);

                }

            }
            finally
            {
                connection.Close();
            }

        }

        [Fact]
        public void DeletePlantByIDTest()
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
                    var mock = new Mock<IConfiguration>();
                    var mockLog = new Mock<ILogger<PlantsController>>();
                    SetupDataContextAndConfig(context);

                    //mock the controller so I can set the GetPersonEmail() to whatever I want without worrying about access tokens
                    var controllerMock = new Mock<PlantsController>(context, mock.Object, mockLog.Object);
                    controllerMock.Setup(m => m.GetPersonEmail()).Returns("jim@bob.com");
                    controllerMock.CallBase = true;

                    //act
                    var result = controllerMock.Object.DeletePlant(4);

                    //assert. get the plant that was deleted, it should no longer be in the context
                    Assert.Equal(4, result.Result.Value.ID);
                    Assert.False(context.Plants.Any(p => p.ID == 4));

                }

            }
            finally
            {
                connection.Close();
            }

        }

        [Fact]
        public void DeletePlantThatDoesntExistByIDTest()
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
                    var mock = new Mock<IConfiguration>();
                    var mockLog = new Mock<ILogger<PlantsController>>();
                    SetupDataContextAndConfig(context);

                    //mock the controller so I can set the GetPersonEmail() to whatever I want without worrying about access tokens
                    var controllerMock = new Mock<PlantsController>(context, mock.Object, mockLog.Object);
                    controllerMock.Setup(m => m.GetPersonEmail()).Returns("dan.thurow@gmail.com");
                    controllerMock.CallBase = true;

                    //act
                    var result = controllerMock.Object.DeletePlant(10);

                    //assert. If a non-existent plant is requested to be deleted, just return notfound and
                    //a null plant
                    Assert.Null(result.Result.Value);
                    Assert.Equal(typeof(Microsoft.AspNetCore.Mvc.NotFoundResult), result.Result.Result.GetType());

                }

            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void DeletePlantByIDThatHasGardenPlantsAssociatedWithItTest()
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
                    var mock = new Mock<IConfiguration>();
                    var mockLog = new Mock<ILogger<PlantsController>>();
                    SetupDataContextAndConfig(context);

                    //mock the controller so I can set the GetPersonEmail() to whatever I want without worrying about access tokens
                    var controllerMock = new Mock<PlantsController>(context, mock.Object, mockLog.Object);
                    controllerMock.Setup(m => m.GetPersonEmail()).Returns("jim@bob.com");
                    controllerMock.CallBase = true;

                    //act
                    var result = controllerMock.Object.DeletePlant(1);

                    //assert
                    Assert.NotNull(result.Exception);

                }

            }
            finally
            {
                connection.Close();
            }
        }



        private void SetupDataContextAndConfig(HomesteadDataContext context)
        {
            homesteadAPI.Config dbconfig = new homesteadAPI.Config(context);
            dbconfig.InitializeDatabase();


        }


    }
}
