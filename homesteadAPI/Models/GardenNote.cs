using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace homesteadAPI.Models
{

    public class GardenNote
    {
        public long ID { get; set; }
        public string Title { get; set; }

        public string Body {get; set;}

        
        [JsonIgnoreAttribute]
        public DateTime CreatedOn { get; set; }

        public virtual Garden Garden { get; set; }

    }

}