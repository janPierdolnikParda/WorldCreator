using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Mogre;
using MogreNewt;

namespace WorldCreator
{
    class MaterialManager
    {
        public MaterialID LevelMaterialID;
        public MaterialID CharacterMaterialID;
        public MaterialID TriggerVolumeMaterialID;
        public MaterialID DescribedMaterialID;
        public MaterialID CharacterSensorMaterialID;
        public MaterialID EnemyMaterialID;

        MaterialPair TriggerVolumeCharacterPair;
        MaterialPair CharacterSensorPair;
        MaterialPair SensorLevelPair;
        MaterialPair SensorDescribedObjectPair;
        MaterialPair SensorTriggerVolumePair;
        MaterialPair DescribedTriggerVolumePair;
        MaterialPair EnemyTriggerVolumePair;
		MaterialPair EnemySensorPair;


        public void Initialise()
        {
            LevelMaterialID = new MaterialID(Engine.Singleton.NewtonWorld);
            CharacterMaterialID = new MaterialID(Engine.Singleton.NewtonWorld);
            TriggerVolumeMaterialID = new MaterialID(Engine.Singleton.NewtonWorld);
            DescribedMaterialID = new MaterialID(Engine.Singleton.NewtonWorld);
            CharacterSensorMaterialID = new MaterialID(Engine.Singleton.NewtonWorld);
            EnemyMaterialID = new MaterialID(Engine.Singleton.NewtonWorld);

            SensorLevelPair = new MaterialPair(
                Engine.Singleton.NewtonWorld,
                LevelMaterialID, CharacterSensorMaterialID);
            SensorLevelPair.SetContactCallback(new IgnoreCollisionCallback());

            SensorTriggerVolumePair = new MaterialPair(
                Engine.Singleton.NewtonWorld,
                TriggerVolumeMaterialID, CharacterSensorMaterialID);
            SensorTriggerVolumePair.SetContactCallback(new IgnoreCollisionCallback());

            DescribedTriggerVolumePair = new MaterialPair(
                Engine.Singleton.NewtonWorld,
                DescribedMaterialID, TriggerVolumeMaterialID);
            DescribedTriggerVolumePair.SetContactCallback(new IgnoreCollisionCallback());

            EnemyTriggerVolumePair = new MaterialPair(
                Engine.Singleton.NewtonWorld,
                EnemyMaterialID, TriggerVolumeMaterialID);
            EnemyTriggerVolumePair.SetContactCallback(new IgnoreCollisionCallback());

        }

        class IgnoreCollisionCallback : ContactCallback
        {
            public override int UserAABBOverlap(ContactMaterial material, Body body0, Body body1, int threadIndex)
            {
                return 0;
            }
        }
        
    }

}
