

namespace homesteadAPI.Models
{

    public class Plant
    {
        public long ID {get; set;}
        public string Name {get; set;}
        public string Description {get; set;}

        public long PlantGroupId {get; set;}
        public virtual PlantGroup PlantGroup {get; set;}

    }

}