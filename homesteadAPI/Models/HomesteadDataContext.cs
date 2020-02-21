using Microsoft.EntityFrameworkCore;
using homesteadAPI.Models;

namespace homesteadAPI.Models
{
    public class HomesteadDataContext : DbContext
    {
        public HomesteadDataContext(DbContextOptions<HomesteadDataContext> options)
            : base(options)
        {
        }

        public DbSet<Plant> Plants { get; set; }
        public DbSet<Garden> Gardens { get; set; }
        public DbSet<GardenPlant> GardenPlants { get; set; }
        public DbSet<PlantGroup> PlantGroups { get; set; }
        public DbSet<GardenNote> GardenNotes { get; set; }
        public DbSet<Person> Persons { get; set; }
        public DbSet<FoodCategory> FoodCategories { get; set; }
        public DbSet<GardenPlantHarvest> GardenPlantHarvests { get; set; }

    }
}