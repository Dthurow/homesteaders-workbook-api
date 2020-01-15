

namespace homesteadAPI.Models
{

    public class GardenPlants
    {
        public long ID {get; set;}
        public string Name {get; set;}
        public int Count {get; set;}
        public int PlantID {get; set;}
        public virtual Plant Plant {get; set;}
        public int GardenId {get; set;}
        public virtual Garden Garden {get; set;}

    }

}