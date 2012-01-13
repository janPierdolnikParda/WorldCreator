using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreNewt;

namespace WorldCreator
{
    class Level
    {
        public String Name;
        Entity GraphicsEntity;
        SceneNode GraphicsNode;

        Entity CollisionEntity;
        SceneNode CollisionNode;

        Body Body;

        void SetGraphicsMesh(String meshFile)
        {
            GraphicsNode = Engine.Singleton.SceneManager.RootSceneNode.CreateChildSceneNode();
            GraphicsEntity = Engine.Singleton.SceneManager.CreateEntity(meshFile);
            GraphicsNode.AttachObject(GraphicsEntity);
            GraphicsEntity.CastShadows = false;
        }

        void SetCollisionMesh(String meshFile)
        {
            CollisionNode = Engine.Singleton.SceneManager.RootSceneNode.CreateChildSceneNode();
            CollisionEntity = Engine.Singleton.SceneManager.CreateEntity(meshFile);
            CollisionNode.AttachObject(CollisionEntity);

            CollisionNode.SetVisible(false);

            MogreNewt.CollisionPrimitives.TreeCollisionSceneParser collision =
                new MogreNewt.CollisionPrimitives.TreeCollisionSceneParser(
               Engine.Singleton.NewtonWorld);
            collision.ParseScene(CollisionNode, true, 1);
            Body = new Body(Engine.Singleton.NewtonWorld, collision);
            collision.Dispose();
            Body.AttachNode(CollisionNode);
            Body.UserData = this;
            Body.MaterialGroupID = Engine.Singleton.MaterialManager.LevelMaterialID;
        }

        public void LoadLevel(String LevelName, bool isTheSame = false)  //////////@@@@@@@@@@@@@@@@@@ tu pewnie jeszcze navmesza
        {
            this.Name = LevelName;
                                                               // trza będzie walnąć, żeby wszystko ładnie się razem ładowało
            String Name = LevelName + ".mesh";
            //NavMeshName = "Media/nav/" + NavMeshName + ".obj";
            SetGraphicsMesh(Name);

            //navMesh.LoadFromOBJ(NavMeshName);

            if (isTheSame)
            {
                SetCollisionMesh(Name);
            }
            else
            {
                LevelName += "Col.mesh";
                SetCollisionMesh(LevelName);
            }

            Engine.Singleton.Load();
        }

        public void DeleteLevel()
        {
            GraphicsNode.DetachAllObjects();
            CollisionNode.DetachAllObjects();
            Body.Dispose();
            Engine.Singleton.SceneManager.RootSceneNode.RemoveChild(GraphicsNode);
            Engine.Singleton.SceneManager.RootSceneNode.RemoveChild(CollisionNode);
        }
    }
}
