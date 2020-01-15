using System.Collections.Generic;

namespace homesteadAPI.Models
{

    public class PlantGroup
    {
        public long ID {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}

        public virtual ICollection<Plant> Plants { get; set; }

    }

}