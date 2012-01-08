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

        public Radian getY()
        {
            Matrix3 orientMatrix;
            orientMatrix = Orientation.ToRotationMatrix();

            Radian yRad, pRad, rRad;
            orientMatrix.ToEulerAnglesYXZ(out yRad, out pRad, out rRad);

            return yRad;
        }

        public Radian getX()
        {
            Matrix3 orientMatrix;
            orientMatrix = Orientation.ToRotationMatrix();

            Radian yRad, pRad, rRad;
            orientMatrix.ToEulerAnglesYXZ(out yRad, out pRad, out rRad);

            return pRad;
        }

        public void Update()
        {
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
