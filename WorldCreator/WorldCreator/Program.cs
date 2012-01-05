using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;



namespace WorldCreator
{
    class Program
    {
        static void Main(string[] args)
        {
            Engine.Singleton.Initialise();

            Engine.Singleton.CurrentLevel = new Level();
            Engine.Singleton.CurrentLevel.LoadLevel("Karczma");

            Light light = Engine.Singleton.SceneManager.CreateLight();
            light.Type = Light.LightTypes.LT_DIRECTIONAL;
            light.Direction = new Vector3(1, -3, 1).NormalisedCopy;
            light.DiffuseColour = new ColourValue(0.2f, 0.2f, 0.2f);
            Engine.Singleton.SceneManager.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_MODULATIVE;

            Engine.Singleton.HumanController.User = Engine.Singleton.User;
            Engine.Singleton.User.WalkSpeed = 0.1f;
            Engine.Singleton.User.Camera.Position = new Vector3(0, 0, 0);
            //Engine.Singleton.User.Camera.Angle = 45;

            while (true)
            {
                Engine.Singleton.Update();

                if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
                    break;
            }

            Engine.Singleton.Root.Dispose();
        }
    }
}
