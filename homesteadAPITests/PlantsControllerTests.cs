using System;
using Xunit;
using homesteadAPI.Controllers;
using homesteadAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.Sqlite;
using System.Collections.Generic;

namespace homesteadAPITests
{
    public class PlantsControllerTests
    {
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
                    context.Database.EnsureCreated();
                    List<PlantGroup> defaultPlantGroups = new List<PlantGroup>()
                    {
                        new PlantGroup(){
                            Name = "Kale",
                            Description = "kale"
                        },
                        new PlantGroup(){
                            Name = "Basil"
                        },
                        new PlantGroup(){
                            Name = "Mint",
                            Description = "Makes a delicious tea"
                        }

                    };

                    context.PlantGroups.AddRange(defaultPlantGroups);
                    context.SaveChanges();


                    List<Plant> defaultPlants = new List<Plant>()
                    {
                        new Plant(){
                            Name = "Dinosaur Kale",
                            Description = "winter kale",
                            ID = 1,
                            PlantGroupId = 1
                        },
                        new Plant(){
                            Name = "Sweet Basil",
                            Description = "the pesto-bilities are endless",
                            ID = 2,
                            PlantGroupId = 2
                        },
                        new Plant(){
                            Name = "Peppermint",
                            Description = "Makes a delicious tea",
                            ID = 3,
                            PlantGroupId = 3
                        }

                    };
                    context.Plants.AddRange(defaultPlants);
                    context.SaveChanges();

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
                    context.Database.EnsureCreated();
                    List<PlantGroup> defaultPlantGroups = new List<PlantGroup>()
                    {
                        new PlantGroup(){
                            Name = "Kale",
                            Description = "kale"
                        },
                        new PlantGroup(){
                            Name = "Basil"
                        },
                        new PlantGroup(){
                            Name = "Mint",
                            Description = "Makes a delicious tea"
                        }

                    };

                    context.PlantGroups.AddRange(defaultPlantGroups);
                    context.SaveChanges();


                    List<Plant> defaultPlants = new List<Plant>()
                    {
                        new Plant(){
                            Name = "Dinosaur Kale",
                            Description = "winter kale",
                            ID = 1,
                            PlantGroupId = 1
                        },
                        new Plant(){
                            Name = "Sweet Basil",
                            Description = "the pesto-bilities are endless",
                            ID = 2,
                            PlantGroupId = 2
                        },
                        new Plant(){
                            Name = "Peppermint",
                            Description = "Makes a delicious tea",
                            ID = 3,
                            PlantGroupId = 3
                        }

                    };
                    context.Plants.AddRange(defaultPlants);
                    context.SaveChanges();

                    PlantsController x = new PlantsController(context);

                    //act
                    var result = x.DeletePlant(1);

                    //assert. get the plant that was deleted, it should no longer be in the context
                    Assert.Equal(1, result.Result.Value.ID);
                    Assert.False(context.Plants.AnyAsync(p => p.ID == 1).Result);

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
                    context.Database.EnsureCreated();
                    List<PlantGroup> defaultPlantGroups = new List<PlantGroup>()
                    {
                        new PlantGroup(){
                            Name = "Kale",
                            Description = "kale"
                        },
                        new PlantGroup(){
                            Name = "Basil"
                        }

                    };

                    context.PlantGroups.AddRange(defaultPlantGroups);
                    context.SaveChanges();


                    List<Plant> defaultPlants = new List<Plant>()
                    {
                        new Plant(){
                            Name = "Dinosaur Kale",
                            Description = "winter kale",
                            ID = 1,
                            PlantGroupId = 1
                        },
                        new Plant(){
                            Name = "Sweet Basil",
                            Description = "the pesto-bilities are endless",
                            ID = 2,
                            PlantGroupId = 2
                        }

                    };
                    context.Plants.AddRange(defaultPlants);
                    context.SaveChanges();

                    PlantsController x = new PlantsController(context);

                    //act
                    var result = x.DeletePlant(3);

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
                    //Create a plant associated with a gardenplant, then try to delete it
                    context.Database.EnsureCreated();
                    List<PlantGroup> defaultPlantGroups = new List<PlantGroup>()
                    {
                        new PlantGroup(){
                            Name = "Kale",
                            Description = "kale"
                        }
                    };

                    context.PlantGroups.AddRange(defaultPlantGroups);
                    context.SaveChanges();


                    List<Plant> defaultPlants = new List<Plant>()
                    {
                        new Plant(){
                            Name = "Dinosaur Kale",
                            Description = "winter kale",
                            ID = 1,
                            PlantGroupId = 1
                        }
                    };
                    context.Plants.AddRange(defaultPlants);
                    context.SaveChanges();

                    List<Garden> defaultGardens = new List<Garden>()
                    {
                      new Garden(){
                        Name = "Joe's Garden",
                        ID = 1
                      }

                    };

                    context.Gardens.AddRange(defaultGardens);
                    context.SaveChanges();

                    List<GardenPlant> defaultGardenPlants = new List<GardenPlant>()
                    {
                        new GardenPlant(){
                            Name = "Dinosaur Kale",
                            Plant = defaultPlants[0],
                            Count = 5,
                            Garden = defaultGardens[0]
                        }

                    };

                    context.GardenPlants.AddRange(defaultGardenPlants);
                    context.SaveChanges();


                    PlantsController x = new PlantsController(context);

                    //act
                    var result = x.DeletePlant(1);

                    //assert
                    Assert.NotNull(result.Exception);
                    
                }

            }
            finally
            {
                connection.Close();
            }
        }

    }
}
