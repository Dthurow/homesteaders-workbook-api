using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace homesteadAPI.Models
{

    public enum PlantingType
    {
        rowFeet,
        individual
    }

    public enum YieldType
    {
        Pounds,
        Kilograms,
        Bushels,
        Ounces
    }

    public class GardenPlant
    {
        public long ID {get; set;}
        public string Name {get; set;}
        public int AmountPlanted {get; set;}

        public int YieldEstimatedPerAmountPlanted { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public PlantingType AmountPlantedType { get; set; }

        [JsonConverter(typeof(StringEnumConverter))]
        public YieldType YieldType { get; set; }



        [JsonIgnoreAttribute]
        public DateTime CreatedOn {get; set;}

        #region foreign key relations
        public virtual Plant Plant {get; set;}

        public virtual long PlantID {get; set;}

        [JsonIgnoreAttribute]
        public virtual Garden Garden {get; set;}

        public virtual long GardenID {get; set;}

        #endregion

    }

}