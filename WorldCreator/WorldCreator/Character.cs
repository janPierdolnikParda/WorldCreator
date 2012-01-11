using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreNewt;

namespace WorldCreator
{
    public class Character : SelectableObject
    {
        public Entity Entity;
        public SceneNode Node;
        public Body Body;

        public CharacterProfile Profile;

        public Vector3 Velocity;

        public Vector3 Displacement;

        Quaternion _Orientation;

		public Vector3 Inertia;

        public Character(CharacterProfile profile)
        {
            Profile = profile.Clone();

            _Orientation = Quaternion.IDENTITY;

            Entity = Engine.Singleton.SceneManager.CreateEntity(Profile.MeshName);
            Node = Engine.Singleton.SceneManager.RootSceneNode.CreateChildSceneNode();
            Node.AttachObject(Entity);

            Vector3 scaledSize = Entity.BoundingBox.HalfSize * Profile.BodyScaleFactor;

            ConvexCollision collision = new MogreNewt.CollisionPrimitives.Capsule(
                Engine.Singleton.NewtonWorld,
                
                System.Math.Min(scaledSize.x, scaledSize.z),
           
                scaledSize.y * 2,
                
                Vector3.UNIT_X.GetRotationTo(Vector3.UNIT_Y),
                Engine.Singleton.GetUniqueBodyId());




            Vector3 inertia, offset;
            collision.CalculateInertialMatrix(out inertia, out offset);

           

			Inertia = inertia;

            Body = new Body(Engine.Singleton.NewtonWorld, collision, true);
            Body.AttachNode(Node);
            Body.SetMassMatrix(Profile.BodyMass, inertia * Profile.BodyMass);
            Body.AutoSleep = false;

            Body.Transformed += BodyTransformCallback;
            Body.ForceCallback += BodyForceCallback;

            Body.UserData = this;
            Body.MaterialGroupID = Engine.Singleton.MaterialManager.CharacterMaterialID;

            Joint upVector = new MogreNewt.BasicJoints.UpVector(
            Engine.Singleton.NewtonWorld, Body, Vector3.UNIT_Y);

            collision.Dispose();

        }

        void BodyTransformCallback(Body sender, Quaternion orientation,
            Vector3 position, int threadIndex)
        {
            Node.Position = position;
            Node.Orientation = Orientation;
        }

        public void BodyForceCallback(Body body, float timeStep, int threadIndex)
        {
            Vector3 force = (Velocity - Body.Velocity * new Vector3(1, 0, 1))
                * Profile.BodyMass * Engine.FixedFPS;
            Body.Velocity = Velocity * new Vector3(1, 0, 1) + Body.Velocity * Vector3.UNIT_Y;
        }

        public override void Update()
        {
            
        }

        public void TurnTo(Vector3 point)
        {
            Orientation = Vector3.UNIT_Z.GetRotationTo((point - Position) * new Vector3(1, 0, 1));
        }

        public override Vector3 Position
        {
            get
            {
                return Body.Position;
            }
            set
            {
                Body.SetPositionOrientation(value, Orientation);
            }
        }

        public override Quaternion Orientation
        {
            get
            {
                return _Orientation;
            }
            set
            {
                _Orientation = value;
            }
        }

        public override string DisplayName
        {
            get
            {
                return Profile.DisplayName;
            }
            set
            {
                Profile.DisplayName = value;
            }
        }

        public override string Description
        {
            get
            {
                return Profile.Description;
            }
            set
            {
                Profile.Description = value;
            }
        }

        public override Vector3 DisplayNameOffset
        {
            get
            {
                return Profile.DisplayNameOffset;
            }
            set
            {
                Profile.DisplayNameOffset = value;
            }
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
