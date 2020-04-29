using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.ComponentModel.DataAnnotations;

namespace homesteadAPI.Models
{
    public enum MeasurementType
    {
        feet,
        inches,
        meters,
        centimeters
    }

    public class Garden
    {

        [Required]
        [Key]
        public long ID {get; set;}

        [Required]
        public string Name {get; set;}

        public string GrowingDateRange {get; set;}

        public DateTime GrowingSeasonStartDate {get; set;}

        public DateTime GrowingSeasonEndDate {get; set;}

        public decimal Width{get; set;}
        public decimal Length {get; set;}

        [JsonConverter(typeof(StringEnumConverter))]
        public MeasurementType MeasurementType{get; set;}

        [JsonIgnoreAttribute]
        public DateTime CreatedOn {get; set;}

        [Required]
         public long PersonID {get; set;}
         
         
        public virtual Person Person {get; set;}

        public virtual ICollection<GardenPlant> GardenPlants { get; set; }

        public virtual ICollection<GardenNote> GardenNotes { get; set; }

    }

}