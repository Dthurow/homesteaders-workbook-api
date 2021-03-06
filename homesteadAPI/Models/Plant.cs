using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace homesteadAPI.Models
{


    public enum AmountType
    {
        ounces,
        grams,
        milligrams,
        seeds
    }



    public class Plant
    {
        [Key]
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public TimeSpan SeedLife { get; set; }

        public int Amount { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public AmountType AmountType { get; set; }

        public DateTime BuyDate { get; set; }

        [JsonIgnoreAttribute]
        public DateTime CreatedOn { get; set; }

        [JsonIgnoreAttribute]
        public virtual Person Person { get; set; }

        public virtual long PersonID { get; set; }

        public long PlantGroupID { get; set; }
        public virtual PlantGroup PlantGroup { get; set; }

        public long FoodCategoryID { get; set; }
        public virtual FoodCategory FoodCategory { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<GardenPlant> GardenPlants { get; set; }


    }

}