using homesteadAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System;

namespace homesteadAPI
{

    public class Config
    {
        private readonly HomesteadDataContext _context;

        public Config(HomesteadDataContext context)
        {
            _context = context;
        }

        public void InitializeDatabase()
        {
            _context.Database.EnsureCreated();

            if (_context.Plants.Any())
            {
                //db already seeded
                return;
            }

             List<Person> defaultPersons = new List<Person>()
            {
                new Person()
                {
                     Name = "Jim Bob",
                     Email = "jim@bob.com",
                     CreatedOn = DateTime.Now

                },
                new Person()
                {
                    Name = "Danielle Thurow",
                     Email = "dan.thurow@gmail.com",
                     CreatedOn = DateTime.Now
                },
                new Person()
                {
                     Name = "Jeb Thurow",
                     Email = "husky0420@gmail.com",
                     CreatedOn = DateTime.Now
                }

            };
            _context.Persons.AddRange(defaultPersons);
            _context.SaveChanges();

             List<FoodCategory> defaultFoodCategories = new List<FoodCategory>(){
                        new FoodCategory(){
                            Name = "Yellow"
                        },
                        new FoodCategory(){
                            Name = "Green"
                        },
                        new FoodCategory(){
                            Name = "Stuff"
                        }

                };

            _context.FoodCategories.AddRange(defaultFoodCategories);
            _context.SaveChanges();


            List<PlantGroup> defaultPlantGroups = new List<PlantGroup>(){
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

            _context.PlantGroups.AddRange(defaultPlantGroups);
            _context.SaveChanges();


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



            List<GardenPlant> defaultGardenPlants = new List<GardenPlant>()
            {
                new GardenPlant(){
                     Name = "Dinosaur Kale",
                     Plant = defaultPlants[0],
                     AmountPlanted = 5,
                     YieldEstimatedPerAmountPlanted = 2,
                     YieldType = YieldType.Ounces,
                     Garden = defaultGardens[0]
                },
                new GardenPlant(){
                     Name = "Sweet Basil",
                     Plant = defaultPlants[1],
                     AmountPlanted = 6,
                      Garden = defaultGardens[0]
                },
                new GardenPlant(){
                     Name = "Peppermint",
                     Plant = defaultPlants[2],
                     AmountPlanted = 1,
                     Garden = defaultGardens[0]
                }

            };

            _context.GardenPlants.AddRange(defaultGardenPlants);
            _context.SaveChanges();


        }

    }

}