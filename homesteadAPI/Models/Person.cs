using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace homesteadAPI.Models
{

    public class Person
    {
        public long ID {get; set;}
        public string Name {get; set;}
        public string Email {get; set;}

        [JsonIgnoreAttribute]
        public DateTime CreatedOn {get; set;}

        #region foreign key relations

        [JsonIgnoreAttribute]
       public virtual ICollection<Garden> Gardens { get; set; }

        public virtual ICollection<PersonPlant> PersonPlants { get; set; }
        #endregion
        
    }

}