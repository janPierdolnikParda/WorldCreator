using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WorldCreator
{
    public class Player
    {
        public GameCamera Camera;

        public float WalkSpeed;

        public GameObject FocusedObject;

        public MOIS.MouseState_NativePtr Mysz;

        public Player()
        {
            Camera = new GameCamera();

            Mysz = new MOIS.MouseState_NativePtr();
        }

        public void Update()
        {
            Camera.Update();
            Mysz.height = Engine.Singleton.Camera.Viewport.ActualHeight;
            Mysz.width = Engine.Singleton.Camera.Viewport.ActualWidth;
        }
    }
}
