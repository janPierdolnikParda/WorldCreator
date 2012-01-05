using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;

namespace WorldCreator
{
    public class GameCamera
    {
        // Odległość w jakiej kamera trzyma się od głowy postaci
        //public float Distance;
        // Kąt, pod jakim kamera spogląda na głowę postaci
        public Degree Angle;

        public Vector3 Position;

        public Vector3 Velocity;

        public Quaternion Orientation;

        public float TurnY;
        public float TurnX;

        public GameCamera()
        {
            Orientation = Quaternion.IDENTITY;
            Velocity = new Vector3(0, 0, 0);
        }

		public Radian fixRoll()
		{
			

		   Matrix3 orientMatrix;
		   orientMatrix = Orientation.ToRotationMatrix();

		   Radian yRad, pRad, rRad;
		   orientMatrix.ToEulerAnglesYXZ( out yRad, out pRad, out rRad);

		   return rRad;

		}

        public void Update()
        {
            /*Vector3 offset =
            Character.Node.Orientation * (-Vector3.UNIT_Z +
              (Vector3.UNIT_Y * (float)System.Math.Tan(Angle.ValueRadians))
              ).NormalisedCopy * Distance;

            Vector3 head = Character.Node.Position + Character.Profile.HeadOffset;
            Vector3 desiredPosition = head + offset;

            InterPosition += (desiredPosition - InterPosition) * 0.1f;

            PredicateRaycast raycast = new PredicateRaycast((b => !(b.UserData is TriggerVolume)));
            raycast.Go(Engine.Singleton.NewtonWorld, head, InterPosition);
            if (raycast.Contacts.Count != 0)
            {
                raycast.SortContacts();
                Engine.Singleton.Camera.Position = head
                  + (InterPosition - head) * raycast.Contacts[0].Distance
                  + raycast.Contacts[0].Normal * 0.15f;
            }
            else
                Engine.Singleton.Camera.Position = InterPosition;

            Engine.Singleton.Camera.LookAt(head); */

            if (TurnY != 0)
            {
                Quaternion rotation = Quaternion.IDENTITY;
                rotation.FromAngleAxis(new Degree(TurnY), Vector3.UNIT_Y);
                Orientation *= rotation;
                TurnY = 0;
            }

            if (TurnX != 0)
            {
                Quaternion rotation = Quaternion.IDENTITY;
                rotation.FromAngleAxis(new Degree(TurnX), Vector3.UNIT_X);
                Orientation *= rotation;
                TurnX = 0;
            }

            {
                Radian angle = fixRoll();
                //Orientation.ToAngleAxis(out angle, out Vector3.ZERO);
                //Console.WriteLine(angle.ValueDegrees);
                Quaternion rotation = Quaternion.IDENTITY;
                rotation.FromAngleAxis(0 - angle, Vector3.UNIT_Z);
                Orientation *= rotation;
            }

            Engine.Singleton.Camera.Position = Position;
            Engine.Singleton.Camera.Orientation = Orientation;
        }
    }
}
