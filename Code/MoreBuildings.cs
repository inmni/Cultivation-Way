using HarmonyLib;
using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;
using Cultivation_Way.Utils;
using System;
using CultivationWay;
namespace Cultivation_Way
{
    internal class MoreBuildings
    {
        private List<BuildingAsset> humanBuildings = new List<BuildingAsset>();
        private static float[] xOffsets = new float[] { 0.24f, 0, -0.24f, -0.24f, 0, 0, 0.08f, 0.00f };
        private static float[] yOffsets = new float[] { 0, 0, 0.00f, 0.00f, -0.04f, -0.04f, 0.24f, 0.24f };
        private static string[] CircumvallationParts = new string[] { "hori", "vert", "horiGate1", "horiGate2", "node1", "node2", "vertGate1", "vertGate2" };
        //                                                     横墙    竖墙      北门         南门      北节点   南节点     西门          东门
        internal void init()
        {
            foreach (BuildingAsset humanBuilding in AssetManager.buildings.list)
            {
                if (humanBuilding.race == "human")
                {
                    humanBuildings.Add(humanBuilding);
                }
            }
            AssetManager.race_build_orders.clone("Tian", "human").replace("human", "Tian");
            AssetManager.race_build_orders.clone("Ming", "human").replace("human", "Ming");
            AssetManager.race_build_orders.clone("EasternHuman", "human").replace("human", "EasternHuman");
            AssetManager.race_build_orders.clone("Yao", "human").replace("human", "Yao");
            AssetManager.race_build_orders.clone("Wu", "human").replace("human", "Wu");
            addRaceNormalBuildings("Tian");
            addRaceNormalBuildings("Ming");
            addRaceNormalBuildings("EasternHuman");
            addRaceNormalBuildings("Wu");
            clone("Yao", "orc");
            addOthers();
            setSpecial();
        }
        private void addRaceNormalBuildings(string race)
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
                loadSprites(newBuilding, string.Empty, humanBuilding.sprite_path);
            }
        }
        private void clone(string race, string from = "human")
        {
            List<BuildingAsset> fromList = new List<BuildingAsset>();
            foreach (BuildingAsset building in AssetManager.buildings.list)
            {
                if (building.race == from)
                {
                    fromList.Add(building);
                }
            }
            foreach (BuildingAsset building in fromList)
            {
                BuildingAsset newBuilding = AssetManager.buildings.clone(building.id.Replace(from, race), building.id);
                newBuilding.race = race;
                if (building.canBeUpgraded)
                {
                    newBuilding.upgradeTo = building.upgradeTo.Replace(from, race);
                }
                loadSprites(newBuilding, building.id, building.sprite_path);
            }

        }
        private void loadSprites(BuildingAsset pTemplate, string normalID, string sprite_path = "")
        {
            string path = sprite_path;
            if (sprite_path == string.Empty)
            {
                if (normalID == string.Empty)
                {
                    path = "buildings/" + pTemplate.id;
                }
                else
                {
                    path = "buildings/" + normalID;
                }
            }
            Sprite[] array = SpriteTextureLoader.getSpriteList(path);
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
                    //buildingAnimationDataNew.list_shadows.Add(sprite);
                }
                else if (text.Equals("construction"))
                {
                    pTemplate.sprites.construction = sprite;
                }
                else if (text.Equals("constructionShadow"))
                {
                    pTemplate.sprites.construction_shadow = sprite;
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
                //buildingAnimationDataNew2.shadows = buildingAnimationDataNew2.list_shadows.ToArray();
                buildingAnimationDataNew2.special = buildingAnimationDataNew2.list_special.ToArray();
            }
        }
        private void loadCircumvallationSprites(string race, int level)
        {
            string folder;
            string part;
            for (int i = 0; i < CircumvallationParts.Length; i++)
            {
                part = CircumvallationParts[i];
                BuildingAsset pTemplate = AssetManager.buildings.get($"{level}Circumvallation_{part}_{race}");
                folder = $"{level}Circumvallation_{race}/{part}";
                Sprite[] array = SpriteTextureLoader.getSpriteList("buildings/" + folder);

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
                        //buildingAnimationDataNew.sha.Add(sprite);
                    }
                    else if (text.Equals("special"))
                    {
                        buildingAnimationDataNew.list_special.Add(sprite);
                    }
                    else if (text.Equals("mini"))
                    {
                        pTemplate.sprites.mapIcon = new BuildingMapIcon(sprite);
                    }
                    else if (text.Equals("construction"))
                    {
                        pTemplate.sprites.construction = sprite;
                    }
                }
                foreach (BuildingAnimationDataNew buildingAnimationDataNew2 in pTemplate.sprites.animationData)
                {
                    buildingAnimationDataNew2.main = buildingAnimationDataNew2.list_main.ToArray();
                    buildingAnimationDataNew2.ruins = buildingAnimationDataNew2.list_ruins.ToArray();
                    //buildingAnimationDataNew2.shadows = buildingAnimationDataNew2.list_shadows.ToArray();
                    buildingAnimationDataNew2.special = buildingAnimationDataNew2.list_special.ToArray();
                }
            }
        }
        private void addOthers()
        {
            BuildingAsset Tian1 = AssetManager.buildings.clone("Barrack1_Tian", "!building");
            Tian1.baseStats.health = 100;
            Tian1.priority = 50;
            Tian1.tech = "building_barracks";
            Tian1.fundament = new BuildingFundament(3, 3, 4, 0);
            Tian1.cost = new ConstructionCost(20, 30, 50, 100);
            Tian1.type = "newBarracks";
            Tian1.race = "Tian";
            Tian1.canBePlacedOnLiquid = false;
            Tian1.ignoreBuildings = false;
            Tian1.checkForCloseBuilding = false;
            Tian1.canBeLivingHouse = false;
            Tian1.burnable = false;
            Tian1.spawnUnits = true;
            Tian1.spawnUnits_asset = "summonTian2";
            Tian1.shadow = false;
            loadSprites(Tian1, string.Empty);
            AssetManager.race_build_orders.get("Tian").addBuilding("Barrack1_Tian", 0, 2, 50, 10, false, false, 0);

            addCircumvallation();
        }
        private void addCircumvallation()
        {
            BuildingAsset b = AssetManager.buildings.add(new BuildingAsset
            {
                id = "1Circumvallation_vert_EasternHuman",
                tech = "Circumvallation_1",
                race = "EasternHuman",
                priority = 10000,
                buildingType = BuildingType.None,
                cost = new ConstructionCost(0, 0, 0, 0),
                fundament = new BuildingFundament(0, 0, 0, 0),
                cityBuilding = true,
                canBeAbandoned = false,
                canBeDamagedByTornado = false,
                canBeLivingHouse = false,
                canBeLivingPlant = false,
                canBePlacedOnBlocks = false,
                canBePlacedOnLiquid = false,
                canBeUpgraded = false,
                affectedByAcid = false,
                affectedByLava = false,
                ignoreBuildings = true,
                burnable = false,
                shadow = false
            });
            b.baseStats.health = 1000;
            b.baseStats.armor = 90;
            AssetManager.buildings.clone("1Circumvallation_hori_EasternHuman", "1Circumvallation_vert_EasternHuman");
            AssetManager.buildings.clone("1Circumvallation_node1_EasternHuman", "1Circumvallation_vert_EasternHuman").fundament = new BuildingFundament(0, 1, 1, 0);
            AssetManager.buildings.clone("1Circumvallation_node2_EasternHuman", "1Circumvallation_vert_EasternHuman").fundament = new BuildingFundament(0, 1, 1, 0);
            AssetManager.buildings.clone("1Circumvallation_vertGate1_EasternHuman", "1Circumvallation_vert_EasternHuman").fundament = new BuildingFundament(0, 1, 0, 3);
            AssetManager.buildings.clone("1Circumvallation_vertGate2_EasternHuman", "1Circumvallation_vertGate1_EasternHuman");
            AssetManager.buildings.clone("1Circumvallation_horiGate1_EasternHuman", "1Circumvallation_vertGate1_EasternHuman").fundament = new BuildingFundament(0, 3, 1, 0);
            AssetManager.buildings.clone("1Circumvallation_horiGate2_EasternHuman", "1Circumvallation_horiGate1_EasternHuman");
            loadCircumvallationSprites("EasternHuman", 1);
        }
        private void setSpecial()
        {
            BuildingAsset p = AssetManager.buildings.get("windmill_Tian");
            p.baseStats.health = 10000;
            p.baseStats.targets = 10;
            p.baseStats.range = 38f;
            p.tower = true;
            p.canBeUpgraded = false;
            p.tower_projectile = "lightning_orb";
            p.tower_projectile_amount = 1;
            p.tower_projectile_offset = 7f;
            BuildingAsset hall_EH = AssetManager.buildings.get("hall_EasternHuman");
            hall_EH.fundament = new BuildingFundament(5, 5, 8, 0);
            BuildingAsset hall1_EH = AssetManager.buildings.get("1hall_EasternHuman");
            hall1_EH.fundament = new BuildingFundament(5, 5, 10, 0);
            BuildingAsset hall2_EH = AssetManager.buildings.get("2hall_EasternHuman");
            hall2_EH.fundament = new BuildingFundament(7, 7, 14, 0);
            BuildingAsset house_EH = AssetManager.buildings.get("house_EasternHuman");
            house_EH.fundament = new BuildingFundament(2, 2, 5, 0);
            BuildingAsset house1_EH = AssetManager.buildings.get("1house_EasternHuman");
            house1_EH.fundament = new BuildingFundament(3, 3, 5, 0);
            BuildingAsset house2_EH = AssetManager.buildings.get("2house_EasternHuman");
            house2_EH.fundament = new BuildingFundament(3, 3, 5, 0);
            BuildingAsset house3_EH = AssetManager.buildings.get("3house_EasternHuman");
            house3_EH.fundament = new BuildingFundament(5, 5, 7, 0);
            BuildingAsset house4_EH = AssetManager.buildings.get("4house_EasternHuman");
            house4_EH.fundament = new BuildingFundament(6, 6, 10, 0);
            BuildingAsset house5_EH = AssetManager.buildings.get("5house_EasternHuman");
            house5_EH.fundament = new BuildingFundament(6, 6, 10, 0);
            get("windmill_EasternHuman").fundament.set(4, 4, 8, 0);
            get("1windmill_EasternHuman").fundament.set(4, 4, 8, 0);
            get("temple_EasternHuman").fundament.set(2, 3, 9, 0);
            get("barracks_EasternHuman").fundament.set(3, 3, 8, 0);
        }
        private BuildingAsset get(string pID)
        {
            return AssetManager.buildings.get(pID);
        }
        #region harmony
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
            if (pCheckForBuild && !((bool)MapBox.instance.CallMethod("canBuildFrom", pTile, buildingAsset, null, pType)))
            {
                __result = null;
                return false;
            }
            ExtendedBuilding building = UnityEngine.Object.Instantiate(Main.instance.prefabs.ExtendedBuildingPrefab).GetComponent<ExtendedBuilding>();
            building.gameObject.SetActive(true);
            ((Action<ExtendedBuilding>)building.GetFastMethod("create", Types.t_ExtendedBuilding))(building);
            ((Action<ExtendedBuilding, WorldTile, BuildingAsset,BuildingData>)building.GetFastMethod("setBuilding", Types.t_ExtendedBuilding))(building, pTile, buildingAsset, pData);

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
                ((Action<ExtendedBuilding>)building.GetFastMethod("finishScaleTween", Types.t_ExtendedBuilding))(building);
            }
            __result = building;

            return false;
        }
        #endregion
    }
}
