using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace homesteadAPI.Models
{

    public class GardenNote
    {
        public long ID { get; set; }

        public string Note {get; set;}

        
        [JsonIgnoreAttribute]
        public DateTime CreatedOn { get; set; }

        public virtual Garden Garden { get; set; }

    }

}