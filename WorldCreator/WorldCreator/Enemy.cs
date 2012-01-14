using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreNewt;

namespace WorldCreator
{
    public class Enemy : SelectableObject
    {
        public Entity Entity;
        public SceneNode Node;
        public Body Body;

        public CharacterProfile Profile;

        public Vector3 Velocity;

        public Vector3 Displacement;

        public Vector3 Inertia;

        //public Body ObjectSensor;
        //public Node SensorNode;

        Quaternion _Orientation;

        //public List<GameObject> Contacts;
        
        //public Described PickingTarget;

        //public List<DescribedProfile> Inventory;
        //ItemSword _Sword;
        //Entity SwordEntity;
        //public CharacterAnimBlender AnimBlender;


        //public bool TalkPerm;
        //public bool InventoryPerm;

        //public bool PickItemOrder;
        //public bool MoveOrder;
        //public bool MoveOrderBack;

        //public bool GetSwordOrder;
        //public bool HideSwordOrder;
        //bool _RunOrder;

        /*public bool RunOrder
        {
            get
            {
                return _RunOrder;
            }
            set
            {
                if (_RunOrder == true && value == false)
                {
                    _RunOrder = false;
                    Profile.WalkSpeed -= 2.0f;
                }
                if (_RunOrder == false && value == true)
                {
                    _RunOrder = true;
                    Profile.WalkSpeed += 2.0f;
                }
            }
        }

        public float TurnDelta;

        public bool FollowPathOrder;
        public List<Vector3> WalkPath;
        public static DecTree.Enemies.e_Node Tree = new EnemyDecTree();


        //////////////////////////////////////////////
        //              Moje zmienne:
        //////////////////////////////////////////////

        Container Container;
        public bool isContainer;
        bool isSeen;
        bool isReachable;
        float ZasiegWzroku;
        float ZasiegOgolny;
        Prize DropPrize;
        public Statistics Statistics;*/

        public Enemy(CharacterProfile profile)
        {
            Profile = profile.Clone();

            _Orientation = Quaternion.IDENTITY;

            Entity = Engine.Singleton.SceneManager.CreateEntity(Profile.MeshName);
            Node = Engine.Singleton.SceneManager.RootSceneNode.CreateChildSceneNode();
            Node.AttachObject(Entity);

            // Vector3 scaledSize = Entity.BoundingBox.HalfSize * Profile.BodyScaleFactor;

            ConvexCollision collision = new MogreNewt.CollisionPrimitives.ConvexHull(Engine.Singleton.NewtonWorld,
                Node,
                Quaternion.IDENTITY,
                0.1f,
                Engine.Singleton.GetUniqueBodyId());

            Vector3 inertia, offset;
            collision.CalculateInertialMatrix(out inertia, out offset);

            Inertia = inertia;

            Body = new Body(Engine.Singleton.NewtonWorld, collision, true);
            Body.AttachNode(Node);
            Body.SetMassMatrix(Profile.BodyMass, inertia * Profile.BodyMass);
            //Body.AutoSleep = false;

            //Body.Transformed += BodyTransformCallback;
            Body.ForceCallback += BodyForceCallback;

            Body.UserData = this;
            Body.MaterialGroupID = Engine.Singleton.MaterialManager.CharacterMaterialID;

            //Joint upVector = new MogreNewt.BasicJoints.UpVector(
            // Engine.Singleton.NewtonWorld, Body, Vector3.UNIT_Y);

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

        public void TurnTo(Vector3 Vect)
        {
            Orientation = Vector3.UNIT_Z.GetRotationTo((Vect - Position) * new Vector3(1, 0, 1));
        }

        public void Attack()
        {
        }

        public override void Update()
        {
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
