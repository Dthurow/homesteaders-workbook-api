using System.Collections.Generic;
using System;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace homesteadAPI.Models
{

    public class PlantGroup
    {
        [Key]
        public long ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        [JsonIgnoreAttribute]
        public DateTime CreatedOn { get; set; }

         [JsonIgnoreAttribute]
        public virtual Person Person { get; set; }

        public virtual long PersonID { get; set; }

        [JsonIgnoreAttribute]
        public virtual ICollection<Plant> Plants { get; set; }

    }

}