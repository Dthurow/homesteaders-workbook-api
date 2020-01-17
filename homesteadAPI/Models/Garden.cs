using System.Collections.Generic;
using System;

namespace homesteadAPI.Models
{

    public class Garden
    {
        public long ID {get; set;}
        public string Name {get; set;}

        public string GrowingDateRange {get; set;}

        public DateTime GrowingSeasonStartDate {get; set;}

        public DateTime GrowingSeasonEndDate {get; set;}

        public DateTime CreatedOn {get; set;}

         public int PersonID {get; set;}
         
        public virtual Person Person {get; set;}

        public virtual ICollection<GardenPlant> GardenPlants { get; set; }

        public virtual ICollection<GardenNote> GardenNotes { get; set; }

    }

}