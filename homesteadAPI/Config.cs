using homesteadAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;

namespace homesteadAPI
{

    public class Config
    {
        private readonly HomesteadDataContext _context;
        private readonly ILogger<Startup> _logger;

        public Config(HomesteadDataContext context, ILogger<Startup> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void InitializeDatabase()
        {
            _logger.LogInformation("Starting initialization of db");
            _context.Database.OpenConnection();
            _context.Database.EnsureCreated();

            if (_context.Plants.Any())
            {
                //db already seeded
                _logger.LogInformation("database already seeded");
                return;
            }

            List<Person> defaultPersons = new List<Person>()
            {
                new Person()
                {
                     Name = "Jim Bob",
                     Email = "jim@bob.com",
                     CreatedOn = DateTime.Now,
                     ID = 3

                },
                new Person()
                {
                    Name = "Danielle Thurow",
                     Email = "dan.thurow@gmail.com",
                     CreatedOn = DateTime.Now,
                     ID = 1
                },
                new Person()
                {
                     Name = "Dee Thurow",
                     Email = "husky0420@gmail.com",
                     CreatedOn = DateTime.Now,
                     ID = 2
                }

            };
            _context.Persons.AddRange(defaultPersons);
            _context.SaveChanges();
            _logger.LogInformation("Saved test person records");

            List<FoodCategory> defaultFoodCategories = new List<FoodCategory>(){
                        new FoodCategory(){
                            ID=1,
                            Name="citrus/tomato",
                            CreatedOn= DateTime.Now
                            },
                            new FoodCategory(){
                            ID=2,
                            Name="grn/ylw veg",
                            CreatedOn= DateTime.Now
                            },
                            new FoodCategory(){
                            ID=3,
                            Name="potatoes",
                            CreatedOn= DateTime.Now
                            },
                            new FoodCategory(){
                            ID=4,
                            Name="other frt/veg",
                            CreatedOn= DateTime.Now
                            },
                            new FoodCategory(){
                            ID=5,
                            Name="grains",
                            CreatedOn= DateTime.Now
                            },
                            new FoodCategory(){
                            ID=6,
                            Name="dry bns/peas/nuts",
                            CreatedOn= DateTime.Now
                            },
                };

            _context.FoodCategories.AddRange(defaultFoodCategories);
            _context.SaveChanges();
            _logger.LogInformation("saved test food categories");


            List<PlantGroup> defaultPlantGroups = new List<PlantGroup>(){
                        new PlantGroup(){
                            Name = "Kale",
                            Description = "kale",
                            PersonID = 2,
                        },
                        new PlantGroup(){
                            Name = "Basil",
                            PersonID = 2,
                        },
                        new PlantGroup(){
                            Name = "Mint",
                            Description = "Makes a delicious tea",
                            PersonID = 1
                        },
                         new PlantGroup(){
                            Name = "Kale",
                            Description = "kale",
                            PersonID = 1,
                        },
                        new PlantGroup(){
                            Name = "Basil",
                            PersonID = 1,
                        },
                        new PlantGroup(){
                            Name = "Mint",
                            Description = "Makes a delicious tea",
                            PersonID = 1
                        }

                };

            _context.PlantGroups.AddRange(defaultPlantGroups);
            _context.SaveChanges();
            _logger.LogInformation("saved test plant groups");


            List<Plant> defaultPlants = new List<Plant>(){
                        new Plant(){
                            Name = "Dinosaur Kale",
                            Description = "winter kale, delicious sauteed in butter",
                            PlantGroupID = 1,
                            PersonID = 1,
                            FoodCategoryID = 2,
                            ID = 1
                        },
                         new Plant(){
                            Name = "King Kale",
                            Description = "winter kale",
                            PlantGroupID = 1,
                            PersonID = 1,
                            FoodCategoryID = 2,
                            ID = 2
                        },
                         new Plant(){
                            Name = "Winter Red Kale",
                            Description = "winter kale",
                            PlantGroupID = 1,
                            PersonID = 1,
                            FoodCategoryID = 2,
                            ID = 3
                        },
                        new Plant(){
                            Name = "Sweet Basil",
                            Description = "the pesto-bilities are endless",
                            PlantGroupID = 2,
                            PersonID = 1,
                            FoodCategoryID = 1,
                            ID = 4
                        },
                         new Plant(){
                            Name = "Thai Basil",
                            Description = "pho-ndamentally delicious",
                            PlantGroupID = 2,
                            PersonID = 2,
                            FoodCategoryID = 3,
                            ID = 5
                        },
                        new Plant(){
                            Name = "Peppermint",
                            Description = "Makes a delicious tea",
                            PlantGroupID = 3,
                            PersonID = 2,
                            FoodCategoryID = 2,
                            ID = 6
                        },
                        new Plant(){
                            Name = "Spearmint",
                            Description = "Makes a delicious tea",
                            PlantGroupID = 3,
                            PersonID = 2,
                            FoodCategoryID = 1,
                            ID = 7
                        }

                };

            _context.Plants.AddRange(defaultPlants);
            _context.SaveChanges();
            _logger.LogInformation("saved test plants");

            List<Garden> defaultGardens = new List<Garden>(){
                      new Garden(){
                        Name = "Joe's Garden",
                        ID = 1,
                        GrowingSeasonStartDate = new System.DateTime(2019, 1, 15),
                        GrowingSeasonEndDate = new System.DateTime(2019, 10, 31),
                        Width = 20,
                        Length = 10,
                        MeasurementType = MeasurementType.feet,
                        PersonID = 1
                      }

                };

            _context.Gardens.AddRange(defaultGardens);
            _context.SaveChanges();
            _logger.LogInformation("saved test gardens");


            List<GardenPlant> defaultGardenPlants = new List<GardenPlant>()
            {
                new GardenPlant(){
                     Name = "Dinosaur Kale",
                     Plant = defaultPlants[0],
                     AmountPlanted = 5,
                     YieldEstimatedPerAmountPlanted = 2,
                     YieldType = YieldType.Ounces,
                     Garden = defaultGardens[0],
                     FinishedHarvesting = false
                },
                new GardenPlant(){
                     Name = "Sweet Basil",
                     Plant = defaultPlants[1],
                     AmountPlanted = 6,
                      Garden = defaultGardens[0],
                      FinishedHarvesting = false
                },
                new GardenPlant(){
                     Name = "Peppermint",
                     Plant = defaultPlants[2],
                     AmountPlanted = 1,
                     Garden = defaultGardens[0],
                     FinishedHarvesting = false
                }

            };

            _context.GardenPlants.AddRange(defaultGardenPlants);
            _context.SaveChanges();
            _logger.LogInformation("saved test garden plants");


        }

    }

}