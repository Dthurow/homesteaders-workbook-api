using System;
using Newtonsoft.Json;

namespace homesteadAPI.Models
{

    public enum PersonPlantAmountType
    {
        ounces,
        seedlings
    }

    public class PersonPlant
    {
        public long ID {get; set;}
        public string Name {get; set;}
        public int Amount {get; set;}
        public PersonPlantAmountType AmountType {get; set;}

        public DateTime BuyDate {get; set;}

        [JsonIgnoreAttribute]
        public DateTime CreatedOn {get; set;}

        #region foreign key relations
        public virtual Plant Plant {get; set;}

        public virtual long PlantID {get; set;}

        [JsonIgnoreAttribute]
        public virtual Person Person {get; set;}

        public virtual long PersonID {get; set;}

        #endregion

    }

}