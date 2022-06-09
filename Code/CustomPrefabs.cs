using CultivationWay;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way
{
    class CustomPrefabs
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
            Main.instance.transformUnits = ReflectionUtility.Reflection.GetField(typeof(MapBox), MapBox.instance, "transformUnits") as Transform;
            Main.instance.transformCreatures = ReflectionUtility.Reflection.GetField(typeof(MapBox), MapBox.instance, "transformCreatures") as Transform;
        }
        private void generateActorPrefabs()
        {
            foreach(string actorPrefab in actorPrefabPath)
            {
                UnityEngine.Object.Instantiate<GameObject>(Resources.Load<GameObject>(actorPrefab)).SetActive(false);
            }
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
