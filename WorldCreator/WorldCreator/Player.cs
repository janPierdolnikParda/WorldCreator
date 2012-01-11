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

        public Player()
        {
            Camera = new GameCamera();

            Mysz = new MOIS.MouseState_NativePtr();
            InventoryItem = null;
        }

        public void Update()
        {
            Camera.Update();
            Mysz.height = Engine.Singleton.Camera.Viewport.ActualHeight;
            Mysz.width = Engine.Singleton.Camera.Viewport.ActualWidth;

            AimPosition = new Vector3();
            AimPosition.x = (float) System.Math.Sin((double)-Camera.getY().ValueRadians) * 5.0f;
            AimPosition.x = (float)System.Math.Cos((double)Camera.getX().ValueRadians) * AimPosition.x + Camera.Position.x;
            AimPosition.y = (float) System.Math.Sin((double)Camera.getX().ValueRadians) * 5.0f + Camera.Position.y;
            AimPosition.z = (float) System.Math.Cos((double)Camera.getY().ValueRadians) * -5.0f;
            AimPosition.z = (float)System.Math.Cos((double)Camera.getX().ValueRadians) * AimPosition.z + Camera.Position.z;

            if (Engine.Singleton.HumanController.State == HumanController.HumanControllerState.FREE)
            {
                FocusedObject = null;

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
        }

        public void AddItem(bool Left)
        {
            Described newItem = new Described(InventoryItem);
            if (Left)
                newItem.Position = AimPosition;
            else
                newItem.Position = Camera.Position;

			if (!Engine.Singleton.HumanController.Gravity)
				newItem.Body.SetMassMatrix(0, Vector3.ZERO);

            Engine.Singleton.ObjectManager.Add(newItem);
        }
    }
}
