using System.Collections.Generic;
using System;

namespace homesteadAPI.Models
{

    public class GardenNote
    {
        public long ID { get; set; }
        public string Title { get; set; }

        public string Body {get; set;}

        public DateTime CreatedOn { get; set; }

        public int GardenId { get; set; }
        public virtual Garden Garden { get; set; }

    }

}