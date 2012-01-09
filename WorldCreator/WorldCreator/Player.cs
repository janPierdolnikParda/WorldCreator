using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace WorldCreator
{
    public class Player
    {
        public GameCamera Camera;

        public float WalkSpeed;

        public GameObject FocusedObject;

        public MOIS.MouseState_NativePtr Mysz;

        public DescribedProfile InventoryItem;

        public Vector3 AimPosition;

        //public Plane DolnyPlane;

        public Player()
        {
            Camera = new GameCamera();

            Mysz = new MOIS.MouseState_NativePtr();
            InventoryItem = null;

            /*DolnyPlane = new Plane(Vector3.UNIT_Y, 0);
            MeshManager.Singleton.CreatePlane("ground",
              ResourceGroupManager.DEFAULT_RESOURCE_GROUP_NAME, DolnyPlane,
              0.015f, 0.015f, 20, 20, true, 1, 5, 5, Vector3.UNIT_Z);

            Entity ent;

            ent = Engine.Singleton.SceneManager.CreateEntity("GroundEntity", "ground");
            Engine.Singleton.SceneManager.RootSceneNode.CreateChildSceneNode().AttachObject(ent);

            ent.SetMaterialName("Red");
            ent.CastShadows = false;*/
        }

        public void Update()
        {
            Camera.Update();
            Mysz.height = Engine.Singleton.Camera.Viewport.ActualHeight;
            Mysz.width = Engine.Singleton.Camera.Viewport.ActualWidth;

            FocusedObject = null;

            AimPosition = new Vector3();
            AimPosition.x = (float) System.Math.Sin((double)-Camera.getY().ValueRadians) * 5.0f;
            AimPosition.x = (float)System.Math.Cos((double)Camera.getX().ValueRadians) * AimPosition.x + Camera.Position.x;
            AimPosition.y = (float) System.Math.Sin((double)Camera.getX().ValueRadians) * 5.0f + Camera.Position.y;
            AimPosition.z = (float) System.Math.Cos((double)Camera.getY().ValueRadians) * -5.0f;
            AimPosition.z = (float)System.Math.Cos((double)Camera.getX().ValueRadians) * AimPosition.z + Camera.Position.z;
            
            /*Engine.Singleton.Camera.LookAt(new Vector3(5, 0, 0));
            Camera.Position = Engine.Singleton.Camera.Position;
            Camera.Orientation = Engine.Singleton.Camera.Orientation;*/

            //Vector3 desiredPosition = Camera.Position;

            PredicateRaycast raycast = new PredicateRaycast((b => !(b.UserData is TriggerVolume)));
            raycast.Go(Engine.Singleton.NewtonWorld, Camera.Position, AimPosition);
            if (raycast.Contacts.Count != 0)
            {
                raycast.SortContacts();
                AimPosition = Camera.Position
                  + (AimPosition - Camera.Position) * raycast.Contacts[0].Distance
                  + raycast.Contacts[0].Normal * 0.05f;

                if (raycast.Contacts[0].Body.UserData is GameObject)
                    FocusedObject = raycast.Contacts[0].Body.UserData as GameObject;
            }
        }

        public void AddItem(bool Left)
        {
            Described newItem = new Described(InventoryItem);
            if (Left)
                newItem.Position = AimPosition;
            else
                newItem.Position = Camera.Position;
            Engine.Singleton.ObjectManager.Add(newItem);
        }
    }
}
