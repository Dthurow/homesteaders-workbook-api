using System;
using System.Collections.Generic;

namespace homesteadAPI.Models
{

    public enum YieldType
    {
        Fruit,
        Pounds,
        Kilograms,
        Bushels
    }

    public class Plant
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public YieldType YieldType { get; set; }
        public DateTime CreatedOn { get; set; }

        public long PlantGroupId { get; set; }
        public virtual PlantGroup PlantGroup { get; set; }
        public virtual ICollection<GardenPlant> GardenPlants { get; set; }

    }

}