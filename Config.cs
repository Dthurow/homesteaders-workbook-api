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
                            Description = "winter kale",
                            PlantGroupId = 1
                        },
                        new Plant(){
                            Name = "Sweet Basil",
                            Description = "the pesto-bilities are endless",
                            PlantGroupId = 2
                        },
                        new Plant(){
                            Name = "Peppermint",
                            Description = "Makes a delicious tea",
                            PlantGroupId = 3
                        }

                };

            _context.Plants.AddRange(defaultPlants);
            _context.SaveChanges();


            List<Garden> defaultGardens = new List<Garden>(){
                      new Garden(){
                        Name = "Joe's Garden"
                      }

                };

            _context.Gardens.AddRange(defaultGardens);
            _context.SaveChanges();



            List<GardenPlants> defaultGardenPlants = new List<GardenPlants>()
            {
                new GardenPlants(){
                     Name = "Dinosaur Kale",
                     PlantID = 1,
                     Count = 5,
                     GardenId = 1
                },
                new GardenPlants(){
                     Name = "Sweet Basil",
                     PlantID = 2,
                     Count = 5,
                     GardenId = 1
                },
                new GardenPlants(){
                     Name = "Peppermint",
                     PlantID = 2,
                     Count = 1,
                     GardenId = 1
                }

            };

            _context.GardenPlants.AddRange(defaultGardenPlants);
            _context.SaveChanges();


        }

    }

}