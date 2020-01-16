using System.Collections.Generic;
using System;

namespace homesteadAPI.Models
{

    public class Person
    {
        public long ID {get; set;}
        public string Name {get; set;}
        public string Email {get; set;}
        public DateTime CreatedOn {get; set;}

        #region foreign key relations
       public virtual ICollection<Garden> Gardens { get; set; }
        #endregion
        
    }

}