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

        public Player()
        {
            Camera = new GameCamera();
        }

        public void Update()
        {
            Camera.Update();
        }
    }
}
