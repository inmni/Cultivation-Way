using ReflectionUtility;
using HarmonyLib;
using UnityEngine;
using System.Collections.Generic;
using System.Reflection;

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
            addKingdom2.addEnemyTag("bandits");
            addKingdom2.mobs = true;
            addKingdom2.nomads = true;
            this.newHiddenKingdom(addKingdom2);
            #endregion
            #region 冥族
            KingdomAsset undead = AssetManager.kingdoms.get("undead");
            undead.addTag("undead");
            undead.addFriendlyTag("Ming");
            KingdomAsset neutral = AssetManager.kingdoms.get("neutral_animals");
            neutral.addEnemyTag("Ming");
            //主要国家
            KingdomAsset addKingdom3 = AssetManager.kingdoms.clone("Ming", "human");
            addKingdom3.tags.Clear();
            addKingdom3.addTag("civ");
            addKingdom3.addTag("Ming");
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
            addKingdom4.addFriendlyTag("neutral");
            addKingdom4.addFriendlyTag("good");
            addKingdom4.addEnemyTag("bandits");
            addKingdom4.mobs = true;
            addKingdom4.nomads = true;
            this.newHiddenKingdom(addKingdom4);
            #endregion

            #region BOSS
            KingdomAsset addKingdom0 = AssetManager.kingdoms.clone("boss", "Ming");
            addKingdom0.tags.Clear();
            addKingdom0.addTag("boss");
            addKingdom0.nomads = false;
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
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "joinAnotherKingdom")]
        public static bool joinAnotherKingdom_Prefix(Kingdom pKingdom,City __instance)
        {
            Kingdom kingdom = Reflection.GetField(typeof(City), __instance, "kingdom") as Kingdom;
            if (kingdom.getMaxLevelActor().GetData().level >= pKingdom.getMaxLevelActor().GetData().level)
            {
                return false;
            }
            return true;
        }
        //修改城市覆灭条件
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox),"destroyCity")]
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
        //修改生育
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ai.behaviours.CityBehProduceUnit),"getRandomParent")]
        public static bool getRandomParent(ref Actor __result, ai.behaviours.CityBehProduceUnit __instance,List<Actor> pList,Actor pActorIgnore)
        {
            FieldInfo parent_index = typeof(ai.behaviours.CityBehProduceUnit).GetField("parent_index", BindingFlags.Static | BindingFlags.NonPublic);
            int value = (int)parent_index.GetValue(__instance);
            if ( value>= pList.Count)
            {
                Reflection.SetField( __instance, "parent_index",0);
            }
            for (int i = value; i < pList.Count; i++)
            {
                pList.ShuffleOne(i);
                Actor actor = pList[i];
                if (actor.GetData().alive && !(actor == pActorIgnore) && !actor.haveTrait("plague") && actor.GetData().age > 18&&actor.stats.procreate)
                {
                    parent_index.SetValue(new ai.behaviours.CityBehProduceUnit(), i);
                    __result = actor;
                    return false;
                }
            }
            
            return false;
        }
    }
}
