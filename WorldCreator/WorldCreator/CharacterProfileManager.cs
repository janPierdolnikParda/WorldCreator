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
        static public Dictionary<String, CharacterProfile> E;

		public CharacterProfileManager()
		{
			C = new Dictionary<String, CharacterProfile>();
            E = new Dictionary<String, CharacterProfile>();
			character = new CharacterProfile();
			character.BodyMass = 70;
			character.BodyScaleFactor = new Vector3(1.5f, 1, 1.5f);
			character.HeadOffset = new Vector3(0, 0.8f, 0);
			character.MeshName = "Man.mesh";
			character.WalkSpeed = 1.85f;
			character.PictureMaterial = "AdamMaterial";
			character.DisplayNameOffset = new Vector3(0, 1, 0);
			character.DisplayName = "Andrzej";
            character.ProfileName = "cAndrzej";
			C.Add("cAndrzej", character);

            CharacterProfile enemy0 = new CharacterProfile();
            enemy0.BodyMass = 70;
            enemy0.BodyScaleFactor = new Vector3(1.5f, 1, 1.5f);
            enemy0.HeadOffset = new Vector3(0, 0.8f, 0);
            enemy0.MeshName = "Man.mesh";
            enemy0.WalkSpeed = 1.85f;
            enemy0.PictureMaterial = "AdamMaterial";
            enemy0.DisplayNameOffset = new Vector3(0, 1, 0);
            enemy0.ProfileName = "eAdam";
            enemy0.DisplayName = "ADAM!";
            E.Add("eAdam", enemy0);
		}
    }
}
