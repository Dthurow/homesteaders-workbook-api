using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace homesteadAPI.Models
{

    public enum YieldType
    {
        Pounds,
        Kilograms,
        Bushels,
        Ounces
    }

    public class Plant
    {
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public YieldType YieldType { get; set; }

        [JsonIgnoreAttribute]
        public DateTime CreatedOn { get; set; }

        [JsonIgnoreAttribute]
        public long PlantGroupId { get; set; }
        public virtual PlantGroup PlantGroup { get; set; }
        
        [JsonIgnoreAttribute]
        public virtual ICollection<GardenPlant> GardenPlants { get; set; }


    }

}