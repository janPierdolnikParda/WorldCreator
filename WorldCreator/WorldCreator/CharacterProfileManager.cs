using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace WorldCreator
{
    public class CharacterProfileManager
    {
        static public CharacterProfile character;

        static public Dictionary<String, CharacterProfile> D;

        public CharacterProfileManager()
        {
            D = new Dictionary<String, CharacterProfile>();
            character = new CharacterProfile();
            character.BodyMass = 70;
            character.BodyScaleFactor = new Vector3(1.5f, 1, 1.5f);
            character.HeadOffset = new Vector3(0, 0.8f, 0);
            character.MeshName = "Man.mesh";
            character.WalkSpeed = 1.85f;
            character.PictureMaterial = "AdamMaterial";

            D.Add("iCharacter", character);
        }
    }
}
