using Cultivation_Way.Utils;
using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using System.Collections.Generic;

namespace Cultivation_Way
{
    internal class ExtendedBuilding : Building
    {
        public MoreStats extendedCurStats = new MoreStats();
        public ExtendedBuildingData extendedData = new ExtendedBuildingData();
        public BuildingData easyData;
        public BaseStats easyCurStats;
        public BuildingAsset easyStats; 
        public ExtendedBuildingStats extendedStats;
        public List<BaseSimObject> compositions = new List<BaseSimObject>();
        #region harmony
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Building),"getHit")]
        public static bool getHit_Prefix(Building __instance)
        {
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Building), "startDestroyBuilding")]
        public static bool actionBeforeDestroyBuilding(Building __instance, bool pRemove)
        {
            if (pRemove || __instance.getBuildingData().underConstruction)
            {
                return true;
            }
            WorldAction destroy_action = ExtendedBuildingStats.GetWorldAction(((ExtendedBuilding)__instance).easyStats.id);
            if (destroy_action != null)
            {
                destroy_action(__instance);
            }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BuildingTower), "checkEnemies")]
        public static bool checkEnemies_Prefix(BuildingTower __instance)
        {
            Building b = Reflection.GetField(typeof(BuildingTower), __instance, "building") as Building;
            if (b.kingdom.id == "abandoned")
            {
                return false;
            }

            BuildingAsset ba = Reflection.GetField(typeof(Building), b, "stats") as BuildingAsset;
            if (ba.baseStats.range < 20f)
            {
                return true;
            }

            List<Actor> targets = new List<Actor>();

            BrushData brushData = Brush.get((int)ba.baseStats.range, "circ_");
            WorldTile worldTile = b.currentTile;
            for (int i = 0; i < brushData.pos.Count; i++)
            {
                BrushPixelData brushPixelData = brushData.pos[i];
                int num = worldTile.pos.x + brushPixelData.x;
                int num2 = worldTile.pos.y + brushPixelData.y;
                if (num >= 0 && num < MapBox.width && num2 >= 0 && num2 < MapBox.height)
                {
                    WorldTile tileSimple = MapBox.instance.GetTileSimple(num, num2);

                    if (tileSimple.units.Count > 0)
                    {
                        for (int k = 0; k < tileSimple.units.Count; k++)
                        {
                            Actor actor = tileSimple.units[k];
                            if (actor.kingdom == null || (b.kingdom.enemies.ContainsKey(actor.kingdom) && b.kingdom.enemies[actor.kingdom] == true))
                            {
                                targets.Add(actor);
                            }
                        }
                    }
                }
            }
            if (targets.Count == 0)
            {

                return false;
            }
            BaseSimObject baseSimObject = targets.GetRandom();
            b.startShake(0.1f);

            Reflection.SetField(__instance, "_shootingActive", true);
            Reflection.SetField(__instance, "_shootingTarget", baseSimObject);
            Reflection.SetField(__instance, "_shootingAmount", ba.tower_projectile_amount);
            return false;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(UnitSpawner), "setUnitFromHere")]
        public static void setUnitFromHere(UnitSpawner __instance, Actor pActor)
        {
            Building building = Reflection.GetField(typeof(UnitSpawner), __instance, "building") as Building;
            if (building.city != null)
            {
                building.city.addNewUnit(pActor, true);
                pActor.CallMethod("setProffesion", UnitProfession.Warrior);
                int level = building.city.getArmyMaxCity() / 10;
                CityData data = Reflection.GetField(typeof(City), building.city, "data") as CityData;
                if (data.storage.get("gold") < level * 30)
                {
                    level = data.storage.get("gold") / 30;
                }
                if (level < 1)
                {
                    level = 1;
                }
                data.storage.change("gold", level * 30);
                ExtendedActor actor = (ExtendedActor)pActor;
                actor.easyData.level = level;
                actor.easyData.health = int.MaxValue >> 2;
                pActor.setStatsDirty();
            }
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "addBuilding")]
        public static bool addBuilding_Prefix(ref Building __result, string pID, WorldTile pTile, BuildingData pData = null, bool pCheckForBuild = false, bool pSfx = false, BuildPlacingType pType = BuildPlacingType.New)
        {
            BuildingAsset buildingAsset = AssetManager.buildings.get(pID);
            
            if (pCheckForBuild && !FastReflection.mapbox_canBuildFrom(MapBox.instance,pTile, buildingAsset, null, pType))
            {
                __result = null;
                return false;
            }
            ExtendedBuilding building =Instantiate(Main.instance.prefabs.ExtendedBuildingPrefab).GetComponent<ExtendedBuilding>();
            building.gameObject.SetActive(true);
            FastReflection.building_create(building);
            FastReflection.building_setBuilding(building, pTile, buildingAsset, pData);

            if (pData != null)
            {
                building.loadBuilding(pData);
                building.easyData = pData;
            }
            else
            {
                building.easyData = building.GetValue<BuildingData>("data", Types.t_ExtendedBuilding);
            }
            building.easyStats = buildingAsset;
            building.easyCurStats = building.GetValue<BaseStats>("curStats", Types.t_ExtendedBuilding);
            building.transform.parent = Main.instance.transformBuildings;
            if (buildingAsset.buildingType == BuildingType.Tree)
            {
                building.transform.parent = Main.instance.transformTrees;
            }
            building.resetShadow();
            MapBox.instance.buildings.Add(building);
            if (pSfx && buildingAsset.sfx != "none")
            {
                Sfx.play(buildingAsset.sfx, true, -1f, -1f);
            }
            if (Config.timeScale > 10f)
            {
                FastReflection.building_finishScaleTween(building);
            }
            __result = building;

            return false;
        }
        #endregion
    }
}
