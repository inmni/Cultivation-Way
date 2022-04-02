using ai.behaviours;
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
            #region 天族
            //主要国家
            KingdomAsset addKingdom1 = AssetManager.kingdoms.clone("Tian", "human");
            addKingdom1.tags.Clear();
            addKingdom1.addTag("civ");
            addKingdom1.addTag("Tian");
            addKingdom1.addFriendlyTag("Tian");
            addKingdom1.addFriendlyTag("neutral");
            addKingdom1.addFriendlyTag("good");
            addKingdom1.addEnemyTag("undead");
            addKingdom1.addEnemyTag("bandits");
            addKingdom1.civ = true;
            AssetManager.kingdoms.get("good").addFriendlyTag("Tian");
            this.newHiddenKingdom(addKingdom1);
            //临时用的国家
            KingdomAsset addKingdom2 = AssetManager.kingdoms.clone("nomads_Tian", "nomads_human");
            addKingdom2.tags.Clear();
            addKingdom2.addTag("civ");
            addKingdom2.addTag("Tian");
            addKingdom2.addFriendlyTag("Tian");
            addKingdom2.addFriendlyTag("neutral");
            addKingdom2.addFriendlyTag("good");
            addKingdom2.addEnemyTag("undead");
            addKingdom2.addEnemyTag("bandits");
            addKingdom2.mobs = true;
            addKingdom2.nomads = true;
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
            //主要国家
            KingdomAsset addKingdom3 = AssetManager.kingdoms.clone("Ming", "human");
            addKingdom3.tags.Clear();
            addKingdom3.addTag("civ");
            addKingdom3.addTag("Ming");
            addKingdom3.addFriendlyTag("undead");
            addKingdom3.addFriendlyTag("Ming");
            addKingdom3.addFriendlyTag("neutral");
            addKingdom3.addFriendlyTag("good");
            addKingdom3.addEnemyTag("bandits");
            addKingdom3.civ = true;
            addKingdom3.nomads = true;
            this.newHiddenKingdom(addKingdom3);
            //临时用的国家
            KingdomAsset addKingdom4 = AssetManager.kingdoms.clone("nomads_Ming", "nomads_human");
            addKingdom4.tags.Clear();
            addKingdom4.addTag("civ");
            addKingdom4.addTag("Ming");
            addKingdom4.addFriendlyTag("Ming");
            addKingdom3.addFriendlyTag("undead");
            addKingdom4.addFriendlyTag("neutral");
            addKingdom4.addFriendlyTag("good");
            addKingdom4.addEnemyTag("bandits");
            addKingdom4.mobs = true;
            addKingdom4.nomads = true;
            this.newHiddenKingdom(addKingdom4);
            #endregion
            #region 妖族
            //主要国家
            KingdomAsset addKingdom5 = AssetManager.kingdoms.clone("Yao", "human");
            addKingdom5.tags.Clear();
            addKingdom5.addTag("civ");
            addKingdom5.addTag("Yao");
            addKingdom5.addTag("nature_creature");
            addKingdom5.addFriendlyTag("nature_creature");
            addKingdom5.addFriendlyTag("Yao");
            addKingdom5.addFriendlyTag("neutral");
            addKingdom5.addFriendlyTag("good");
            addKingdom5.addEnemyTag("bandits");
            addKingdom5.civ = true;
            AssetManager.kingdoms.get("snakes").enemy_tags.Remove("civ");
            this.newHiddenKingdom(addKingdom5);
            //临时用的国家
            KingdomAsset addKingdom6 = AssetManager.kingdoms.clone("nomads_Yao", "nomads_human");
            addKingdom6.tags.Clear();
            addKingdom6.addTag("civ");
            addKingdom6.addTag("Yao");
            addKingdom6.addTag("nature_creature");
            addKingdom6.addFriendlyTag("nature_creature");
            addKingdom6.addFriendlyTag("Yao");
            addKingdom6.addFriendlyTag("neutral");
            addKingdom6.addFriendlyTag("good");
            addKingdom6.addEnemyTag("bandits");
            addKingdom6.mobs = true;
            addKingdom6.nomads = true;
            this.newHiddenKingdom(addKingdom6);
            #endregion
            #region 东方人族
            //主要国家
            KingdomAsset addKingdom7 = AssetManager.kingdoms.clone("EasternHuman", "human");
            addKingdom7.tags.Clear();
            addKingdom7.addTag("civ");
            addKingdom7.addTag("EasternHuman");
            addKingdom7.addFriendlyTag("EasternHuman");
            addKingdom7.addFriendlyTag("neutral");
            addKingdom7.addFriendlyTag("good");
            addKingdom7.addEnemyTag("bandits");
            addKingdom7.civ = true;
            this.newHiddenKingdom(addKingdom7);
            //临时用的国家
            KingdomAsset addKingdom8 = AssetManager.kingdoms.clone("nomads_EasternHuman", "nomads_human");
            addKingdom8.tags.Clear();
            addKingdom8.addTag("civ");
            addKingdom8.addTag("EasternHuman");
            addKingdom8.addFriendlyTag("EasternHuman");
            addKingdom8.addFriendlyTag("neutral");
            addKingdom8.addFriendlyTag("good");
            addKingdom8.addEnemyTag("bandits");
            addKingdom8.mobs = true;
            addKingdom8.nomads = true;
            this.newHiddenKingdom(addKingdom8);
            #endregion
            #region BOSS
            KingdomAsset addKingdom0 = AssetManager.kingdoms.clone("boss", "Ming");
            addKingdom0.tags.Clear();
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
        
        //设置投降条件
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(City), "updateCapture")]
        public static IEnumerable<CodeInstruction> updateCapture_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo canBeCapturedSmoothly = AccessTools.Method(typeof(KingdomAndCityTools), "canBeCapturedSmoothly");
            Label label = new Label();
            codes[162].labels.Add(label);
            int offset = 0;
            codes.Insert(162 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(162 + offset, new CodeInstruction(OpCodes.Ldloc_0));
            offset++;
            codes.Insert(162 + offset, new CodeInstruction(OpCodes.Callvirt, canBeCapturedSmoothly));
            offset++;
            codes.Insert(162 + offset, new CodeInstruction(OpCodes.Brtrue, label));
            offset++;
            codes.Insert(162 + offset, new CodeInstruction(OpCodes.Ret));
            return codes.AsEnumerable();
        }
        //修改城市覆灭条件
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "destroyCity")]
        public static bool destroyCity_Prefix(City pCity)
        {
            if (pCity.getMaxLevelActor() != null)
            {
                if (pCity.getMaxLevelActor().GetData().level > 10)
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
        [HarmonyPatch(typeof(CityBehProduceUnit), "produceNewCitizen")]
        public static bool produceNewCitizen_Prefix(CityBehProduceUnit __instance, bool __result, Building pBuilding, City pCity)
        {
            Actor randomParent = pCity.getRandomParent(null);
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
            Actor randomParent2 = pCity.getRandomParent(randomParent);
            randomParent.GetData().children++;
            pCity.status.homesFree--;
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
            actorData.status.inheritTraits(randomParent.GetData().traits);
            actorData.status.hunger = stats.maxHunger / 2;
            if (randomParent2 != null)
            {
                actorData.status.inheritTraits(randomParent2.GetData().traits);
                randomParent2.GetData().children++;
            }
            actorData.status.skin = ai.ActorTool.getBabyColor(randomParent, randomParent2);
            actorData.status.skin_set = randomParent.GetData().skin_set;
            Culture babyCulture = (Culture)Reflection.CallStaticMethod(typeof(CityBehProduceUnit),"getBabyCulture", randomParent, randomParent2);
            if (babyCulture != null)
            {
                actorData.status.culture = babyCulture.id;
                actorData.status.level = babyCulture.getBornLevel();
            }
            #region 设置出生属性
            MoreActorData moreData = new MoreActorData();
            MoreStats moreStats = new MoreStats();
            actorData.status.actorID = MapBox.instance.mapStats.getNextId("unit");
            Main.instance.actorToMoreStats[actorData.status.actorID] = moreStats;
            Main.instance.actorToMoreData[actorData.status.actorID] = moreData;
            moreData.bonusStats = new MoreStats();
            moreData.coolDown = new Dictionary<string, int>();
            moreData.element = new ChineseElement();
            moreStats.maxAge = stats.maxAge;
            moreStats.element = moreData.element;
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
                    moreData.cultisystem = cultisystem.GetRandom().Remove(0, 6);
                }
            }
            //继承体质
            int bodyRank = randomParent.GetSpecialBody().rank;
            if (Toolbox.randomChance(1f / (bodyRank * (bodyRank + 1))))
            {
                moreData.specialBody = randomParent.GetSpecialBody().id;
            }
            if (Toolbox.randomChance(0.001f))
            {
                moreData.specialBody = AddAssetManager.specialBodyLibrary.list.GetRandom().id;
            }
            //继承家族
            moreData.familyID = randomParent.GetFamily().id;
            if (Toolbox.randomChance(0.05f))
            {
                if (stats.unit)
                {
                    moreData.familyID = AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_startList.GetRandom();
                }
                else
                {
                    moreData.familyID = AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_endList.GetRandom();
                }
            }
            Family family = Main.instance.familys[moreData.familyID];
            family.num++;
            //设置名字
            moreData.familyName = randomParent.GetMoreData().familyName;
            if (stats.unit)
            {
                if (Toolbox.randomChance(0.02f))
                {
                    moreData.familyName = AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_startList.GetRandom();
                }
                actorData.status.firstName = ChineseNameGenerator.getCreatureName(stats.nameTemplate, moreData.familyName, true);
            }
            else
            {
                if (Toolbox.randomChance(0.02f))
                {
                    moreData.familyName = AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_endList.GetRandom();
                }
                actorData.status.firstName = ChineseNameGenerator.getCreatureName(stats.nameTemplate, moreData.familyName, false);
            }
            #endregion
            cityData.popPoints.Add(actorData);
            __result = true;
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "spawnPopPoint")]
        public static bool spawnPopPoint(ActorData pData)
        {
            if (pData.status.actorID != "")
            {//一般情况
                MoreActorData tempMoreData = null;
                MoreStats tempMoreStats = null;
                if(!Main.instance.actorToMoreData.TryGetValue(pData.status.actorID,out tempMoreData))
                {
                    tempMoreData = new MoreActorData();
                    tempMoreData.bonusStats = new MoreStats();
                    tempMoreData.coolDown = new Dictionary<string, int>();
                    tempMoreData.element = new ChineseElement();
                    ActorStats stats = AssetManager.unitStats.get(pData.status.statsID);
                    if (stats.unit)
                    {
                        tempMoreData.familyID = AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_startList.GetRandom();
                    }
                    else
                    {
                        tempMoreData.familyID = AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_endList.GetRandom();
                    }
                    tempMoreData.familyName = tempMoreData.familyID;
                }
                if(!Main.instance.actorToMoreStats.TryGetValue(pData.status.actorID,out tempMoreStats))
                {
                    tempMoreStats = new MoreStats();
                    tempMoreStats.element = tempMoreData.element;
                }
                string nextID = "u_" + MapBox.instance.mapStats.id_unit;

                Main.instance.actorToMoreStats.Remove(pData.status.actorID);
                Main.instance.actorToMoreData.Remove(pData.status.actorID);

                Main.instance.actorToMoreData[nextID] = tempMoreData;
                Main.instance.actorToMoreStats[nextID] = tempMoreStats;
            }
            else
            {//特殊情况
                MoreStats moreStats = new MoreStats();
                MoreActorData moreData = new MoreActorData();
                string nextID = "u_" + MapBox.instance.mapStats.id_unit;
                Main.instance.actorToMoreData.Add(nextID, moreData);
                Main.instance.actorToMoreStats.Add(nextID, moreStats);
                string name = pData.status.firstName;
                ActorStats stats = AssetManager.unitStats.get(pData.status.statsID);
                foreach (string fn in ChineseNameAsset.familyNameTotal)
                {
                    if (stats.unit && name.StartsWith(fn))
                    {
                        moreData.familyID = fn;
                        break;
                    }
                    else if (!stats.unit && name.EndsWith(fn))
                    {
                        moreData.familyID = fn;
                    }
                }
                moreData.cultisystem = "default";
                moreData.specialBody = "FT";
                moreData.element = new ChineseElement();
                moreData.bonusStats = new MoreStats();
                moreData.coolDown = new Dictionary<string, int>();
                moreData.canCultivate = true;
                moreStats.element = moreData.element;
            }
            return true;
        }
        //国家创建时事件
        [HarmonyPostfix]
        [HarmonyPatch(typeof(KingdomManager),"makeNewCivKingdom")]
        public static void makeNewCivKingdom_Postfix(Kingdom __result)
        {
            Main.instance.kingdomBindActors[__result.id] = new List<Actor>();
        }
        //国家灭亡事件
        [HarmonyPostfix]
        [HarmonyPatch(typeof(KingdomManager),"destroyKingdom")]
        public static void destroyKingdom_Postfix(Kingdom pKingdom)
        {
            List<Actor> actors = null;
            if(!Main.instance.kingdomBindActors.TryGetValue(pKingdom.id,out actors))
            {
                return;
            }
            for(int i=0;i<actors.Count;i++)
            {
                Actor actor = actors[i];
                string id = actor.GetData().actorID;
                actors.RemoveAt(i);
                i--;
                actor.killHimself(true, AttackType.Other, true, true);
                if (actor == null)
                {
                    MonoBehaviour.print("成功摧毁");
                }
                if (MapBox.instance.getActorByID(id)!=null)
                {
                    MonoBehaviour.print("[MoreKingdoms.destroyKingdom_Postfix]未成功删除对象");
                }
            }
            Main.instance.kingdomBindActors.Remove(pKingdom.id);
        }
        //妖族建立城市修改
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(BehCheckBuildCity),"execute")]
        public static IEnumerable<CodeInstruction> execute_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            Label next = new Label();
            codes[39].labels.Add(next);
            MethodInfo isAllowedBuildCity = AccessTools.Method(typeof(KingdomAndCityTools), "isAllowedBuildCity");
            int offset = 0;
            codes.Insert(39+offset, new CodeInstruction(OpCodes.Ldarg_1));
            offset++;
            codes.Insert(39 + offset, new CodeInstruction(OpCodes.Callvirt, isAllowedBuildCity));
            offset++;
            codes.Insert(39+offset, new CodeInstruction(OpCodes.Brtrue_S, next));
            offset++;
            codes.Insert(39 + offset, new CodeInstruction(OpCodes.Ldc_I4_1));
            offset++;
            codes.Insert(39 + offset, new CodeInstruction(OpCodes.Ret));
            return codes.AsEnumerable();
        }
        #region 神明加成（未完工）
        //月老Hymen提高生育
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(CityBehProduceUnit),"execute")]
        public static IEnumerable<CodeInstruction> produceMoreUnit(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo moreProduceNum = AccessTools.Method(typeof(KingdomAndCityTools), "moreProduceMin");
            codes[28] = new CodeInstruction(OpCodes.Ldarg_1);
            codes.Insert(29, new CodeInstruction(OpCodes.Callvirt, moreProduceNum));
            return codes.AsEnumerable();
        }
        //钟馗ZhongKui提高对冥族的伤害
        //在getHit中已经完成

        //二合一
        //土地EarthGod提高作物产出
        //山神MountainGod加快树木生长和矿物获取
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Building),"extractResources")]
        public static IEnumerable<CodeInstruction> produceMoreResources(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo moreGained = AccessTools.Method(typeof(KingdomAndCityTools), "produceMore");
            int offset = 0;
            codes.Insert(70 + offset, new CodeInstruction(OpCodes.Ldarg_1));
            offset++;
            codes.Insert(70 + offset, new CodeInstruction(OpCodes.Ldloc_2));
            offset++;
            codes.Insert(70 + offset, new CodeInstruction(OpCodes.Callvirt, moreGained));
            offset++;
            codes.Insert(70 + offset, new CodeInstruction(OpCodes.Add));
            return codes.AsEnumerable();
        }
        //财神Momman增加黄金
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(BehBoatMakeTrade),"execute")]
        public static IEnumerable<CodeInstruction> gainMoreGold(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo moreGold = AccessTools.Method(typeof(KingdomAndCityTools), "gainMoreFromTrade");
            codes[3] = new CodeInstruction(OpCodes.Ldarg_1);
            codes.Insert(4, new CodeInstruction(OpCodes.Callvirt, moreGold));
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
        #endregion
    }
}
