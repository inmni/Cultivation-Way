using CultivationWay;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    internal class CustomPrefabs
    {
        private static string[] actorPrefabPath = new string[] { "actors/p_unit", "actors/p_dragon", "actors/p_tornado", "actors/p_ufo",
                                                        "actors/p_boat", "actors/p_boulder","actors/p_godFinger","actors/p_zombie_dragon",
                                                        "actors/p_crabzilla"};
        internal Dictionary<string, GameObject> extendPrefabDict = new Dictionary<string, GameObject>();
        internal GameObject ExtendedBuildingPrefab;
        internal void init()
        {
            transformActorPrefabs();
            transformBuildingPrefabs();
            generateActorPrefabs();
            Main.instance.transformUnits = ReflectionUtility.Reflection.GetField(typeof(MapBox), MapBox.instance, "transformUnits") as Transform;
            Main.instance.transformCreatures = ReflectionUtility.Reflection.GetField(typeof(MapBox), MapBox.instance, "transformCreatures") as Transform;
        }
        private void generateActorPrefabs()
        {
            GameObject easternDragon = UnityEngine.Object.Instantiate(extendPrefabDict["actors/p_unit"]);
            easternDragon.SetActive(false);
            easternDragon.name = "p_easternDragon";
            easternDragon.AddComponent<EasternDragon>();
            easternDragon.AddComponent<SpriteAnimation>();
            UnityEngine.Object.Destroy(easternDragon.GetComponent<Actor>());
            UnityEngine.Object.Destroy(easternDragon.GetComponent<UnitSpriteAnimation>());
            extendPrefabDict["actors/p_easternDragon"] = easternDragon;

            GameObject specialActor = UnityEngine.Object.Instantiate(extendPrefabDict["actors/p_unit"]);
            specialActor.SetActive(false);
            specialActor.name = "p_specialActor";
            specialActor.AddComponent<SpecialActor>();
            specialActor.AddComponent<SpriteAnimation>();
            UnityEngine.Object.Destroy(specialActor.GetComponent<Actor>());
            UnityEngine.Object.Destroy(specialActor.GetComponent<UnitSpriteAnimation>());
            extendPrefabDict["actors/p_specialActor"] = specialActor;
        }
        private void transformBuildingPrefabs()
        {
            ExtendedBuildingPrefab = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>("buildings/p_building"));
            ExtendedBuildingPrefab.SetActive(false);
            UnityEngine.Object.Destroy(ExtendedBuildingPrefab.GetComponent<Building>());
            ExtendedBuildingPrefab.AddComponent<ExtendedBuilding>();
        }
        private void transformActorPrefabs()
        {
            for (int i = 0; i < actorPrefabPath.Length; i++)
            {
                GameObject newPrefab = UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(actorPrefabPath[i]));
                newPrefab.SetActive(false);
                UnityEngine.Object.Destroy(newPrefab.GetComponent<Actor>());
                newPrefab.AddComponent<ExtendedActor>();
                extendPrefabDict[actorPrefabPath[i]] = newPrefab;
            }
        }
    }
}
