using System.Collections.Generic;

namespace homesteadAPI.Models
{

    public class Garden
    {
        public long ID {get; set;}
        public string Name {get; set;}

        public virtual ICollection<GardenPlants> GardenPlants { get; set; }

    }

}