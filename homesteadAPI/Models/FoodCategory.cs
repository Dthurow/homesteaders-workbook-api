using System.Collections.Generic;
using System;
using Newtonsoft.Json;

namespace homesteadAPI.Models
{

    public class FoodCategory
    {
        public long ID { get; set; }

        public string Name {get; set;}

        
        [JsonIgnoreAttribute]
        public DateTime CreatedOn { get; set; }

    }

}