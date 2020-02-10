using System;
using Newtonsoft.Json;

namespace homesteadAPI.Models
{

    public class GardenPlant
    {
        public long ID {get; set;}
        public string Name {get; set;}
        public int Count {get; set;}
        public int YieldEstimated{get; set;}
        public int? YieldActual {get; set;}

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