using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace WorldCreator
{
    public abstract class SelectableObject : GameObject
    {
        public abstract String DisplayName
        {
            get;
            set;
        }
        public abstract String Description
        {
            get;
            set;
        }
        public abstract Vector3 DisplayNameOffset
        {
            get;
            set;
        }

        public bool ShowName;
    }
}
