using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace WorldCreator
{
    public class DescribedProfile
    {
        public String MeshName;
        public String Description;
        public String DisplayName;
        public Vector3 DisplayNameOffset;
        public Vector3 BodyScaleFactor;
        public Single Mass;
        public Boolean IsPickable;
        public String InventoryPictureMaterial;
        public String ProfileName;
        public bool IsEquipment;



        public DescribedProfile Clone()
        {
            return (DescribedProfile)MemberwiseClone();
        }
    }
}
