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
            bool Kibel = false;

            Console.WriteLine("***************************************\n");
            Console.Write("\tPodaj nazwe mapy: ");
            String MapName;
            MapName = Console.ReadLine();
            Console.WriteLine("\n***************************************");
            Engine.Singleton.Initialise();

            Engine.Singleton.CurrentLevel = new Level();
            Engine.Singleton.CurrentLevel.LoadLevel(MapName);

            Light light = Engine.Singleton.SceneManager.CreateLight();
            light.Type = Light.LightTypes.LT_DIRECTIONAL;
            light.Direction = new Vector3(1, -3, 1).NormalisedCopy;
            light.DiffuseColour = new ColourValue(0.2f, 0.2f, 0.2f);
			
            Engine.Singleton.SceneManager.ShadowTechnique = ShadowTechnique.SHADOWTYPE_STENCIL_MODULATIVE;
			//Engine.Singleton.SceneManager.ShadowUseLightClipPlanes = true;
			// Engine.Singleton.SceneManager.ShowDebugShadows = true;

            Engine.Singleton.HumanController.User = Engine.Singleton.User;
            Engine.Singleton.User.WalkSpeed = 0.1f;
            Engine.Singleton.User.Camera.Position = new Vector3(0, 0, 0);
            //Engine.Singleton.User.Camera.Angle = 45;

            Engine.Singleton.User.Mysz = Engine.Singleton.Mouse.MouseState;

            var Celownik = OverlayManager.Singleton.Create("TestOverlay");

            // Create a panel.
            var panelCelownik = (PanelOverlayElement)OverlayManager.Singleton.CreateOverlayElement("Panel", "PanelElement");
            // Set panel properties.
            panelCelownik.MaterialName = "Celownik";
            panelCelownik.MetricsMode = GuiMetricsMode.GMM_PIXELS;
            panelCelownik.Top = (Engine.Singleton.Root.AutoCreatedWindow.Height / 2) - 2;
            panelCelownik.Left = (Engine.Singleton.Root.AutoCreatedWindow.Width / 2) - 2;
            panelCelownik.Width = 4;
            panelCelownik.Height = 4;

            // Add the panel to the overlay.
            Celownik.Add2D(panelCelownik);

            // Make the overlay visible.
            Celownik.Show();

            while (true)
            {
                Engine.Singleton.Update();

                if (Engine.Singleton.Keyboard.IsKeyDown(MOIS.KeyCode.KC_ESCAPE))
                    break;

                if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_F1))
                    Engine.Singleton.Save();

                if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_F2))
                    Engine.Singleton.Load();

				if (Engine.Singleton.IsKeyTyped(MOIS.KeyCode.KC_F3))
                    Kibel = !Kibel;

                if (Kibel)
                    Engine.Singleton.NewtonDebugger.ShowDebugInformation();
                else
                    Engine.Singleton.NewtonDebugger.HideDebugInformation();
            }

            Engine.Singleton.Root.Dispose();
        }
    }
}
