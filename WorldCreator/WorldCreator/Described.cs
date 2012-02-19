using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreNewt;

namespace WorldCreator
{
    public class Described : SelectableObject
    {
        public DescribedProfile Profile;
        public String Activator;
		
        Entity Entity;
        SceneNode Node;
        public Body Body;

		public Vector3 Inertia;

        public Described(DescribedProfile profile)
        {
            Profile = profile.Clone();
            Activator = "";
            Entity = Engine.Singleton.SceneManager.CreateEntity(Profile.MeshName);
            Node = Engine.Singleton.SceneManager.RootSceneNode.CreateChildSceneNode();

			Entity.CastShadows = true;
			Node.AttachObject(Entity);



            ConvexCollision coll = new MogreNewt.CollisionPrimitives.ConvexHull(Engine.Singleton.NewtonWorld, 
                Node, 
                Quaternion.IDENTITY,
                0.1f, 
                Engine.Singleton.GetUniqueBodyId());
          
            Vector3 inertia = new Vector3(1,1,1), offset;
            coll.CalculateInertialMatrix(out inertia, out offset);

			Inertia = inertia;
            Body = new Body(Engine.Singleton.NewtonWorld, coll, true);
            Body.AttachNode(Node);
            Body.SetMassMatrix(Profile.Mass, Profile.Mass * inertia);

            Body.MaterialGroupID = Engine.Singleton.MaterialManager.DescribedMaterialID;
            Body.UserData = this;

            coll.Dispose();

			Body.ForceCallback += BodyForceCallback;
        }

        public bool IsPickable
        {
            get { return Profile.IsPickable; }
        }

		public void BodyForceCallback(Body body, float timeStep, int threadIndex)
		{

		}

        public void TurnTo(Vector3 point)
        {
            Orientation = Vector3.UNIT_Z.GetRotationTo((point - Position) * new Vector3(1, 0, 1));
        }

        void BodyTransformCallback(Body sender, Quaternion orientation, Vector3 position, int threadIndex)
        {
            Node.Position = position;
            Node.Orientation = Orientation;
        }

        public override void Update()
        {
        }

        public override Vector3 Position
        {
            get { return Body.Position; }
            set { Body.SetPositionOrientation(value, Orientation); }
        }
        public override Quaternion Orientation
        {
            get { return Body.Orientation; }
            set { Body.SetPositionOrientation(Body.Position, value); }
        }
        public override string DisplayName
        {
            get { return Profile.DisplayName; }
            set { Profile.DisplayName = value; }
        }
        public override string Description
        {
            get { return Profile.Description; }
            set { Profile.Description = value; }
        }
        public override Vector3 DisplayNameOffset
        {
            get { return Profile.DisplayNameOffset; }
            set { Profile.DisplayNameOffset = value; }
        }

        public Radian getZ()
        {
            Matrix3 orientMatrix;
            orientMatrix = Orientation.ToRotationMatrix();

            Radian yRad, pRad, rRad;
            orientMatrix.ToEulerAnglesYXZ(out yRad, out pRad, out rRad);

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

        public override void Destroy()
        {
            Node.DetachAllObjects();
            Engine.Singleton.SceneManager.DestroySceneNode(Node);
            Engine.Singleton.SceneManager.DestroyEntity(Entity);
            Body.Dispose();
            Body = null;

            base.Destroy();
        }
    }
}
