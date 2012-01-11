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

        static public Dictionary<String, CharacterProfile> C;

		public CharacterProfileManager()
		{
			C = new Dictionary<String, CharacterProfile>();
			character = new CharacterProfile();
			character.BodyMass = 70;
			character.BodyScaleFactor = new Vector3(1.5f, 1, 1.5f);
			character.HeadOffset = new Vector3(0, 0.8f, 0);
			character.MeshName = "Man.mesh";
			character.WalkSpeed = 1.85f;
			character.PictureMaterial = "AdamMaterial";
			character.DisplayNameOffset = new Vector3(0, 1, 0);
			character.DisplayName = "Andrzej";
			C.Add("cAndrzej", character);
		}
    }
}
