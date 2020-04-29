using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace homesteadAPI.Models
{

    public class GardenPlantHarvest
    {
        [Key]
        public long ID { get; set; }

        public int AmountHarvested { get; set; }
        public DateTime HarvestDate { get; set; }


        [JsonIgnoreAttribute]
        public DateTime CreatedOn { get; set; }

        public virtual GardenPlant GardenPlant { get; set; }
        public virtual long GardenPlantID { get; set; }

    }

}