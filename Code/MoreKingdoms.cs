
using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
namespace Cultivation_Way
{
    class MoreKingdoms
    {
        internal void init()
        {
            AssetManager.kingdoms.add(new KingdomAsset
            {
                id="empty",
                civ=true
            });
            AssetManager.kingdoms.add(new KingdomAsset
            {
                id = "nomads_empty",
                nomads = true,
                mobs = true
            });
            #region 天族
            //主要国家
            KingdomAsset addKingdom1 = AssetManager.kingdoms.clone("Tian", "empty");
            addKingdom1.addTag("civ");
            addKingdom1.addTag("Tian");
            addKingdom1.addFriendlyTag("Tian");
            addKingdom1.addFriendlyTag("neutral");
            addKingdom1.addFriendlyTag("good");
            addKingdom1.addEnemyTag("undead");
            addKingdom1.addEnemyTag("bandits");
            AssetManager.kingdoms.get("good").addFriendlyTag("Tian");
            this.newHiddenKingdom(addKingdom1);
            //临时用的国家
            KingdomAsset addKingdom2 = AssetManager.kingdoms.clone("nomads_Tian", "nomads_empty");
            addKingdom2.addTag("civ");
            addKingdom2.addTag("Tian");
            addKingdom2.addFriendlyTag("Tian");
            addKingdom2.addFriendlyTag("neutral");
            addKingdom2.addFriendlyTag("good");
            addKingdom2.addEnemyTag("undead");
            addKingdom2.addEnemyTag("bandits");
            this.newHiddenKingdom(addKingdom2);
            #endregion
            #region 冥族
            KingdomAsset undead = AssetManager.kingdoms.get("undead");
            undead.addTag("undead");
            undead.addFriendlyTag("undead");
            undead.addFriendlyTag("Ming");
            undead.addEnemyTag("human");
            undead.addEnemyTag("elf");
            undead.addEnemyTag("orc");
            undead.addEnemyTag("dwarf");
            undead.addEnemyTag("Yao");
            undead.addEnemyTag("Tian");
            undead.addEnemyTag("EasternHuman");
            //主要国家
            KingdomAsset addKingdom3 = AssetManager.kingdoms.clone("Ming", "empty");
            addKingdom3.addTag("civ");
            addKingdom3.addTag("Ming");
            addKingdom3.addFriendlyTag("undead");
            addKingdom3.addFriendlyTag("Ming");
            addKingdom3.addFriendlyTag("neutral");
            addKingdom3.addFriendlyTag("good");
            addKingdom3.addEnemyTag("bandits");
            this.newHiddenKingdom(addKingdom3);
            //临时用的国家
            KingdomAsset addKingdom4 = AssetManager.kingdoms.clone("nomads_Ming", "nomads_empty");
            addKingdom4.addTag("civ");
            addKingdom4.addTag("Ming");
            addKingdom4.addFriendlyTag("Ming");
            addKingdom3.addFriendlyTag("undead");
            addKingdom4.addFriendlyTag("neutral");
            addKingdom4.addFriendlyTag("good");
            addKingdom4.addEnemyTag("bandits");
            this.newHiddenKingdom(addKingdom4);
            #endregion
            #region 妖族
            //主要国家
            KingdomAsset addKingdom5 = AssetManager.kingdoms.clone("Yao", "empty");
            addKingdom5.addTag("civ");
            addKingdom5.addTag("Yao");
            addKingdom5.addTag("nature_creature");
            addKingdom5.addFriendlyTag("nature_creature");
            addKingdom5.addFriendlyTag("Yao");
            addKingdom5.addFriendlyTag("neutral");
            addKingdom5.addFriendlyTag("good");
            addKingdom5.addEnemyTag("bandits");
            AssetManager.kingdoms.get("snakes").addFriendlyTag("civ");
            this.newHiddenKingdom(addKingdom5);
            //临时用的国家
            KingdomAsset addKingdom6 = AssetManager.kingdoms.clone("nomads_Yao", "nomads_empty");
            addKingdom6.addTag("civ");
            addKingdom6.addTag("Yao");
            addKingdom6.addTag("nature_creature");
            addKingdom6.addFriendlyTag("nature_creature");
            addKingdom6.addFriendlyTag("Yao");
            addKingdom6.addFriendlyTag("neutral");
            addKingdom6.addFriendlyTag("good");
            addKingdom6.addEnemyTag("bandits");
            this.newHiddenKingdom(addKingdom6);
            #endregion
            #region 东方人族
            //主要国家
            KingdomAsset addKingdom7 = AssetManager.kingdoms.clone("EasternHuman", "empty");
            addKingdom7.addTag("civ");
            addKingdom7.addTag("EasternHuman");
            addKingdom7.addFriendlyTag("EasternHuman");
            addKingdom7.addFriendlyTag("neutral");
            addKingdom7.addFriendlyTag("good");
            addKingdom7.addEnemyTag("bandits");
            this.newHiddenKingdom(addKingdom7);
            //临时用的国家
            KingdomAsset addKingdom8 = AssetManager.kingdoms.clone("nomads_EasternHuman", "nomads_empty");
            addKingdom8.addTag("civ");
            addKingdom8.addTag("EasternHuman");
            addKingdom8.addFriendlyTag("EasternHuman");
            addKingdom8.addFriendlyTag("neutral");
            addKingdom8.addFriendlyTag("good");
            addKingdom8.addEnemyTag("bandits");
            this.newHiddenKingdom(addKingdom8);
            #endregion
            #region BOSS
            KingdomAsset addKingdom0 = AssetManager.kingdoms.clone("boss", "Ming");
            addKingdom0.addTag("boss");
            addKingdom0.addEnemyTag("civ");
            addKingdom0.addEnemyTag("boss");
            addKingdom0.addEnemyTag("undead");
            addKingdom0.addEnemyTag("neutral");
            addKingdom0.addEnemyTag("good");
            addKingdom0.addEnemyTag("bandits");
            addKingdom0.nomads = false;
            addKingdom0.mad = true;
            addKingdom0.nature = false;
            addKingdom0.attack_each_other = true;
            this.newHiddenKingdom(addKingdom0);
            #endregion


            //BannerGenerator.loadBanners($"Mods/EmbededResources/banners");
        }
        private void newHiddenKingdom(KingdomAsset pAsset)
        {
            Kingdom kingdom = new Kingdom();
            kingdom.asset = pAsset;
            kingdom.createHidden();
            kingdom.id = pAsset.id;
            kingdom.name = pAsset.id;
            KingdomManager kingdomManager = MapBox.instance.kingdoms;
            kingdomManager.addKingdom(kingdom, false);
        }
        //城市给予物品修复
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "giveItem")]
        public static bool giveItem_Prefix(ref bool __result, Actor pActor, List<ItemData> pItems, City pCity)
        {
            if (pActor == null || pActor.equipment == null || pItems == null || pCity == null)
            {
                __result = false;
                return false;
            }
            return true;
        }

        //城市获取死亡人口的装备修复
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "takeAllItemsFromActor")]
        public static bool takeAllItemsFromActor_Prefix(Actor pActor)
        {
            if (pActor.stats.use_items)
            {
                return true;
            }
            return false;
        }
        //设置投降条件
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(City), "updateCapture")]
        public static IEnumerable<CodeInstruction> updateCapture_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo canBeCapturedSmoothly = AccessTools.Method(typeof(KingdomAndCityTools), "canBeCapturedSmoothly");
            Label label = new Label();
            codes[118].labels.Add(label);
            int offset = 0;
            codes.Insert(118 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(118 + offset, new CodeInstruction(OpCodes.Ldloc_0));
            offset++;
            codes.Insert(118 + offset, new CodeInstruction(OpCodes.Callvirt, canBeCapturedSmoothly));
            offset++;
            codes.Insert(118 + offset, new CodeInstruction(OpCodes.Brtrue, label));
            offset++;
            codes.Insert(118 + offset, new CodeInstruction(OpCodes.Ret));
            return codes.AsEnumerable();
        }
        //修改城市覆灭条件
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "destroyCity")]
        public static bool destroyCity_Prefix(City pCity)
        {
            if (pCity.getMaxLevelActor() != null)
            {
                if (pCity.getMaxLevelActor().easyData.level > 10)
                {
                    return false;
                }
            }
            return true;
        }
        //修复制造士兵bug
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "tryToMakeWarrior")]
        public static bool tryToMakeWarrior_Prefix(City __instance, Actor pActor)
        {
            if (pActor.equipment != null)
            {
                return true;
            }
            
            pActor.CallMethod("setProfession", UnitProfession.Warrior);
            __instance.status.warriorCurrent++;
            //强制征兵，不做冷却限制，如果该单位没有装备栏
            return false;
        }
        //彻底修改生育
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ai.behaviours.CityBehProduceUnit), "produceNewCitizen")]
        public static bool produceNewCitizen_Prefix(ai.behaviours.CityBehProduceUnit __instance, bool __result, Building pBuilding, City pCity)
        {
            ExtendedActor randomParent = (ExtendedActor)pCity.getRandomParent(null);
            if (randomParent == null)
            {
                __result = false;
                return false;
            }
            if (randomParent.haveTrait("infected") && Toolbox.randomBool())
            {
                __result = false;
                return false;
            }
            ExtendedActor randomParent2 = (ExtendedActor)pCity.getRandomParent(randomParent);
            randomParent.easyData.children++;
            pCity.status.housingFree--;
            CityData cityData = Reflection.GetField(typeof(City), pCity, "data") as CityData;
            if (randomParent.kingdom != null)
            {
                randomParent.kingdom.born++;
            }
            ActorStats stats = randomParent.stats;
            ActorData actorData = new ActorData();


            actorData.cityID = cityData.cityID;
            actorData.status = new ActorStatus();
            actorData.status.statsID = stats.id;
            ActorBase.generateCivUnit(stats, actorData.status, AssetManager.raceLibrary.get(stats.race));
            actorData.status.generateTraits(stats, AssetManager.raceLibrary.get(stats.race));
            actorData.status.inheritTraits(randomParent.easyData.traits);
            actorData.status.hunger = stats.maxHunger / 2;
            if (randomParent2 != null)
            {
                actorData.status.inheritTraits(randomParent2.easyData.traits);
                randomParent2.easyData.children++;
            }
            actorData.status.skin = ai.ActorTool.getBabyColor(randomParent, randomParent2);
            actorData.status.skin_set = randomParent.easyData.skin_set;
            Culture babyCulture = (Culture)Reflection.CallStaticMethod(typeof(ai.behaviours.CityBehProduceUnit), "getBabyCulture", randomParent, randomParent2);
            if (babyCulture != null)
            {
                actorData.status.culture = babyCulture.id;
                actorData.status.level = babyCulture.getBornLevel();
            }
            #region 设置出生属性
            MoreData moreData = new MoreData();
            actorData.status.actorID = MapBox.instance.mapStats.getNextId("unit");
            ExtendedWorldData.instance.tempMoreData[actorData.status.actorID] = moreData;
            moreData.status.element = new ChineseElement().baseElementContainer;
            //获取修炼体系
            if (babyCulture != null)
            {
                List<string> cultisystem = new List<string>();
                foreach (string tech in babyCulture.list_tech_ids)
                {
                    if (tech.StartsWith("culti_"))
                    {
                        cultisystem.Add(tech);
                    }
                }
                if (cultisystem.Count > 0)
                {
                    moreData.status.cultisystem = cultisystem.GetRandom().Remove(0, 6);
                }
            }
            //继承体质
            int bodyRank = randomParent.GetSpecialBody().rank;
            if (Toolbox.randomChance(1f / (bodyRank * (bodyRank + 1))))
            {
                moreData.status.specialBody = randomParent.GetSpecialBody().id;
            }
            if (Toolbox.randomChance(0.001f))
            {
                moreData.status.specialBody = AddAssetManager.specialBodyLibrary.list.GetRandom().id;
            }
            //继承家族
            moreData.status.familyID = randomParent.GetFamily().id;
            if (Toolbox.randomChance(0.05f))
            {
                moreData.status.familyID = stats.unit ? AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_startList.GetRandom() : AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_endList.GetRandom();
            }
            Family family = ExtendedWorldData.instance.familys[moreData.status.familyID];
            family.num++;
            //设置名字
            moreData.status.familyName = randomParent.extendedData.status.familyName;
            if (Toolbox.randomChance(0.02f))
            {
                moreData.status.familyName = stats.unit ? AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_startList.GetRandom() : AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_endList.GetRandom();
            }
            actorData.status.firstName = stats.unit ? ChineseNameGenerator.getCreatureName(stats.nameTemplate, moreData.status.familyName, true) : ChineseNameGenerator.getCreatureName(stats.nameTemplate, moreData.status.familyName, false);
            #endregion
            cityData.popPoints.Add(actorData);
            __result = true;
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "spawnPopPoint")]
        public static bool spawnPopPoint(ActorData pData, WorldTile pTile)
        {

            if (pData.status.age <= 8)
            {
                pData.status.statsID = pData.status.statsID.Replace("unit_", "baby_");
                pData.status.profession = UnitProfession.Baby;
            }
            ActorStats actorStats = AssetManager.unitStats.get(pData.status.statsID);
            for (int i = 0; i < actorStats.traits.Count; i++)
            {
                string item = actorStats.traits[i];
                if (!pData.status.traits.Contains(item))
                {
                    pData.status.traits.Add(item);
                }
            }
            ExtendedActor actor = (ExtendedActor)MapBox.instance.spawnAndLoadUnit(pData.status.statsID, pData, pTile);
            actor.setStatsDirty();
            if (actor.stats.baby)
            {
                actor.GetComponent<Baby>().timerGrow = (float)actor.stats.timeToGrow;
            }

            actor.extendedData = ExtendedWorldData.instance.tempMoreData[pData.status.actorID];
            actor.extendedCurStats.element = new ChineseElement(actor.extendedData.status.element);

            ExtendedWorldData.instance.tempMoreData.Remove(pData.status.actorID);
            pData.status.actorID = MapBox.instance.mapStats.getNextId("unit");
            return false;
        }
        //国家创建时事件
        [HarmonyPostfix]
        [HarmonyPatch(typeof(KingdomManager), "makeNewCivKingdom")]
        public static void makeNewCivKingdom_Postfix(Kingdom __result)
        {
            Main.instance.kingdomBindActors[__result.id] = new List<Actor>();
        }
        //国家灭亡事件
        [HarmonyPostfix]
        [HarmonyPatch(typeof(KingdomManager), "destroyKingdom")]
        public static void destroyKingdom_Postfix(Kingdom pKingdom)
        {
            List<ExtendedActor> actors;
            if (!ExtendedWorldData.instance.kingdomBindActors.TryGetValue(pKingdom.id, out actors))
            {
                return;
            }
            for (int i = 0; i < actors.Count; i++)
            {
                ExtendedActor actor = actors[i];
                string id = actor.easyData.actorID;
                actors.RemoveAt(i);
                i--;
                actor.killHimself(true, AttackType.Other, true, true);
            }
            Main.instance.kingdomBindActors.Remove(pKingdom.id);
        }
        //妖族建立城市修改
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(ai.behaviours.BehCheckBuildCity), "execute")]
        public static IEnumerable<CodeInstruction> execute_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            Label next = new Label();
            codes[39].labels.Add(next);
            MethodInfo isAllowedBuildCity = AccessTools.Method(typeof(KingdomAndCityTools), "isAllowedBuildCity");
            int offset = 0;
            codes.Insert(39 + offset, new CodeInstruction(OpCodes.Ldarg_1));
            offset++;
            codes.Insert(39 + offset, new CodeInstruction(OpCodes.Callvirt, isAllowedBuildCity));
            offset++;
            codes.Insert(39 + offset, new CodeInstruction(OpCodes.Brtrue_S, next));
            offset++;
            codes.Insert(39 + offset, new CodeInstruction(OpCodes.Ldc_I4_1));
            offset++;
            codes.Insert(39 + offset, new CodeInstruction(OpCodes.Ret));
            return codes.AsEnumerable();
        }
        //#region 神明加成（未完工）
        //月老Hymen提高生育
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(ai.behaviours.CityBehProduceUnit), "execute")]
        public static IEnumerable<CodeInstruction> produceMoreUnit(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo moreProduceNum = AccessTools.Method(typeof(KingdomAndCityTools), "moreProduceMin");
            codes[41] = new CodeInstruction(OpCodes.Ldarg_1);
            codes.Insert(42, new CodeInstruction(OpCodes.Callvirt, moreProduceNum));
            return codes.AsEnumerable();
        }
        //钟馗ZhongKui提高对冥族的伤害
        //在getHit中已经完成

        //二合一
        //土地EarthGod提高作物产出
        //山神MountainGod加快树木生长和矿物获取
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(ai.behaviours.BehExtractResourcesFromBuilding), "execute")]
        public static IEnumerable<CodeInstruction> produceMoreResources(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo moreGained = AccessTools.Method(typeof(KingdomAndCityTools), "produceMore");
            FieldInfo beh_building_target = AccessTools.Field(typeof(ActorBase), "beh_building_target");
            FieldInfo stats = AccessTools.Field(typeof(Building), "stats");
            FieldInfo buildingType = AccessTools.Field(typeof(BuildingAsset), "buildingType");
            int offset = 0;
            codes.Insert(30 + offset, new CodeInstruction(OpCodes.Ldloc_S,4));
            offset++;
            codes.Insert(30 + offset, new CodeInstruction(OpCodes.Ldarg_1));
            offset++;
            codes.Insert(30 + offset, new CodeInstruction(OpCodes.Ldarg_1));
            offset++;
            codes.Insert(30 + offset, new CodeInstruction(OpCodes.Ldfld,beh_building_target));
            offset++;
            codes.Insert(30 + offset, new CodeInstruction(OpCodes.Ldfld,stats));
            offset++;
            codes.Insert(30 + offset, new CodeInstruction(OpCodes.Ldfld, buildingType));
            offset++;
            codes.Insert(30 + offset, new CodeInstruction(OpCodes.Callvirt, moreGained));
            offset++;
            codes.Insert(30 + offset, new CodeInstruction(OpCodes.Add));
            return codes.AsEnumerable();
        }
        //财神Momman增加黄金
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(ai.behaviours.BehBoatMakeTrade), "execute")]
        public static IEnumerable<CodeInstruction> gainMoreGold(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo moreGold = AccessTools.Method(typeof(KingdomAndCityTools), "gainMoreFromTrade");
            codes[2] = new CodeInstruction(OpCodes.Ldarg_1);
            codes.Insert(3, new CodeInstruction(OpCodes.Callvirt, moreGold));
            return codes.AsEnumerable();
        }
        //河伯Achelous无视浅海，未完工
        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(ActorBase),"goTo")]
        //public static bool walkIgnoreWater(ActorBase __instance,ref bool pPathOnWater)
        //{
        //    pPathOnWater = KingdomAndCityTools.checkAchelous(__instance);
        //    return true;
        //}
        //[HarmonyTranspiler]
        //[HarmonyPatch(typeof(Actor),"checkCurTileAction")]
        //public static bool 
        // #endregion
    }
}
