using homesteadAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

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
                            PlantGroupId = 1
                        },
                         new Plant(){
                            Name = "King Kale",
                            Description = "winter kale",
                            PlantGroupId = 1
                        },
                         new Plant(){
                            Name = "Winter Red Kale",
                            Description = "winter kale",
                            PlantGroupId = 1
                        },
                        new Plant(){
                            Name = "Sweet Basil",
                            Description = "the pesto-bilities are endless",
                            PlantGroupId = 2
                        },
                         new Plant(){
                            Name = "Thai Basil",
                            Description = "pho-ndamentally delicious",
                            PlantGroupId = 2
                        },
                        new Plant(){
                            Name = "Peppermint",
                            Description = "Makes a delicious tea",
                            PlantGroupId = 3
                        },
                        new Plant(){
                            Name = "Spearmint",
                            Description = "Makes a delicious tea",
                            PlantGroupId = 3
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
                        MeasurementType = MeasurementType.feet
                      }

                };

            _context.Gardens.AddRange(defaultGardens);
            _context.SaveChanges();



            List<GardenPlant> defaultGardenPlants = new List<GardenPlant>()
            {
                new GardenPlant(){
                     Name = "Dinosaur Kale",
                     Plant = defaultPlants[0],
                     Count = 5,
                     Garden = defaultGardens[0]
                },
                new GardenPlant(){
                     Name = "Sweet Basil",
                     Plant = defaultPlants[1],
                     Count = 6,
                      Garden = defaultGardens[0]
                },
                new GardenPlant(){
                     Name = "Peppermint",
                     Plant = defaultPlants[2],
                     Count = 1,
                     Garden = defaultGardens[0]
                }

            };

            _context.GardenPlants.AddRange(defaultGardenPlants);
            _context.SaveChanges();


        }

    }

}