using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace homesteadAPI.Models
{

    public class GardenNote
    {
        [Key]
        public long ID { get; set; }

        public string Note {get; set;}

        
        [JsonIgnoreAttribute]
        public DateTime CreatedOn { get; set; }

        public virtual Garden Garden { get; set; }

    }

}