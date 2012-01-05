﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreNewt;

namespace WorldCreator
{
    public class ItemSword : DescribedProfile
    {
        public bool InUse;
        public float Damage;
        public Vector3 HandleOffset;

        public new ItemSword Clone()
        {
            return (ItemSword)MemberwiseClone();
        }
    }
}
