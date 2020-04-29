using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace homesteadAPI.Models
{

    public class FoodCategory
    {
        [Key]
        public long ID { get; set; }

        public string Name {get; set;}

        
        [JsonIgnoreAttribute]
        public DateTime CreatedOn { get; set; }

    }

}