using Microsoft.EntityFrameworkCore;

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
        public DbSet<GardenPlants> GardenPlants { get; set; }
        public DbSet<PlantGroup> PlantGroups { get; set; }
    }
}