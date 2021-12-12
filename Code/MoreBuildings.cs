using Cultivation_Way.Utils;
using CultivationWay;
using System.Collections.Generic;
using UnityEngine;
using ReflectionUtility;
using HarmonyLib;
namespace Cultivation_Way
{
    class MoreBuildings
    {
        List<BuildingAsset> humanBuildings = new List<BuildingAsset>();
        internal void init()
        {
            foreach (BuildingAsset humanBuilding in AssetManager.buildings.list)
            {
                if (humanBuilding.race == "human")
                {
                    humanBuildings.Add(humanBuilding);
                }
            }
            addRaceNormalBuildings();
            addOthers();
            setSpecial();
        }
        private void addRaceNormalBuildings()
        {

            foreach (string race in Main.instance.moreRaces)
            {
                foreach (BuildingAsset humanBuilding in humanBuildings)
                {
                    BuildingAsset newBuilding = AssetManager.buildings.clone(humanBuilding.id.Replace("human", race), humanBuilding.id);
                    newBuilding.race = race;
                    newBuilding.shadow = false;
                    if (humanBuilding.canBeUpgraded)
                    {
                        newBuilding.upgradeTo = humanBuilding.upgradeTo.Replace("human", race);
                    }
                    loadSprites(newBuilding);
                }
            }
        }
        private void loadSprites(BuildingAsset pTemplate)
        {
            string folder = pTemplate.race;
            if (folder == string.Empty)
            {
                folder = "Others";
            }
            folder = folder + "/" + pTemplate.id.Replace("_" + pTemplate.race, "");
            Sprite[] array = ResourcesHelper.loadAllSprite("buildings/" + folder, 0.5f);

            pTemplate.sprites = new BuildingSprites();
            foreach (Sprite sprite in array)
            {
                string[] array2 = sprite.name.Split(new char[] { '_' });
                string text = array2[0];
                int num = int.Parse(array2[1]);

                if (array2.Length == 3)
                {
                    int.Parse(array2[2]);
                }
                while (pTemplate.sprites.animationData.Count < num + 1)
                {
                    pTemplate.sprites.animationData.Add(null);
                }
                if (pTemplate.sprites.animationData[num] == null)
                {
                    pTemplate.sprites.animationData[num] = new BuildingAnimationDataNew();
                }
                BuildingAnimationDataNew buildingAnimationDataNew = pTemplate.sprites.animationData[num];
                if (text.Equals("main"))
                {
                    buildingAnimationDataNew.list_main.Add(sprite);
                    if (buildingAnimationDataNew.list_main.Count > 1)
                    {
                        buildingAnimationDataNew.animated = true;
                    }
                }
                else if (text.Equals("ruin"))
                {
                    buildingAnimationDataNew.list_ruins.Add(sprite);
                }
                else if (text.Equals("shadow"))
                {
                    buildingAnimationDataNew.list_shadows.Add(sprite);
                }
                else if (text.Equals("special"))
                {
                    buildingAnimationDataNew.list_special.Add(sprite);
                }
                else if (text.Equals("mini"))
                {
                    pTemplate.sprites.mapIcon = new BuildingMapIcon(sprite);
                }
            }
            foreach (BuildingAnimationDataNew buildingAnimationDataNew2 in pTemplate.sprites.animationData)
            {
                buildingAnimationDataNew2.main = buildingAnimationDataNew2.list_main.ToArray();
                buildingAnimationDataNew2.ruins = buildingAnimationDataNew2.list_ruins.ToArray();
                buildingAnimationDataNew2.shadows = buildingAnimationDataNew2.list_shadows.ToArray();
                buildingAnimationDataNew2.special = buildingAnimationDataNew2.list_special.ToArray();
            }
        }

        private void addOthers()
        {

        }
        private void setSpecial()
        {
            BuildingAsset p = AssetManager.buildings.get("windmill_Tian");
            p.baseStats.health = 100;
            p.baseStats.targets = 10;
            p.baseStats.range = 38f;
            p.tower = true;
            p.tower_projectile = "lightning_orb";
            p.tower_projectile_amount = 1;
            p.tower_projectile_offset = 15f;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Building),"applyScale")]
        public static bool applyScale_Postfix(Building __instance)
        {
            if(((BuildingAsset)Reflection.GetField(typeof(Building),__instance,"stats")).id== "windmill_Tian")
            {
                __instance.currentScale =new Vector3(0.23f,0.23f,0.23f);
            }
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(BuildingTower),"checkEnemies")]
        public static bool checkEnemies_Prefix(BuildingTower __instance)
        {
            Building b = Reflection.GetField(typeof(BuildingTower), __instance, "building") as Building;
            if (b.kingdom.id=="abandoned")
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
                            if (actor.kingdom==null||(b.kingdom.enemies.ContainsKey(actor.kingdom)&&b.kingdom.enemies[actor.kingdom]==true))
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
            Reflection.SetField(__instance, "_shootingAmount",ba.tower_projectile_amount);
            return false;
        }
    }
}
