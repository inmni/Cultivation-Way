using CultivationWay;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    internal class ExtendedActorStats
    {
        public string raceID = string.Empty;

        public string statsID = string.Empty;

        public float cultivateVelo = 1.0f;//修炼速度

        public float cultivateChance = 0.2f;//可修炼几率

        public int initialLevel = 1;//初始等级
        
        public int[] preferedElement;//种族更倾向的元素

        public float preferedElementScale = 0f;//倾向程度

        public List<string> raceSpells;//种族自带法术

        public string defaultCultisystem = "default";//默认修炼体系

        public string fixedName = null;//预设名

        public bool ignoreTimeStop = false;//忽略时间暂停

        public int forceDeathAge = int.MaxValue;
        private static ExtendedActorStats t;
        public void addSpells(params object[] spells)
        {
            for(int i = 0; i < spells.Length; i++)
            {
                this.raceSpells.Add((string)spells[i]);
            }
        }

        internal static void init()
        {
            foreach (ActorStats stats in AssetManager.unitStats.list)
            {
                ExtendedActorStats feature = new ExtendedActorStats();
                feature.raceID = stats.race;
                feature.statsID = stats.id;
                feature.raceSpells = new List<string>();
                feature.preferedElement = new int[5] { 20, 20, 20, 20, 20 };
                Main.instance.extendedActorStatsLibrary.Add(stats.id, feature);
            }
            setIntelligentRaceFeature();
            setOtherRaceFeature();
        }
        private static void clone(string pTo, string pFrom)
        {
            ExtendedActorStats to = Main.instance.extendedActorStatsLibrary[pTo];
            ExtendedActorStats from = Main.instance.extendedActorStatsLibrary[pFrom];
            to.cultivateChance = from.cultivateChance;
            to.cultivateVelo = from.cultivateVelo;
            to.defaultCultisystem = from.defaultCultisystem;
            to.raceID = from.raceID;
            to.preferedElement = JsonUtility.FromJson<int[]>(JsonUtility.ToJson(from.preferedElement));
            to.raceSpells = JsonUtility.FromJson<List<string>>(JsonUtility.ToJson(from.raceSpells));
        }
        private static void setIntelligentRaceFeature()
        {
            #region unit
            t = Main.instance.extendedActorStatsLibrary["unit_human"];
            t.cultivateChance = 1.0f;
            t = Main.instance.extendedActorStatsLibrary["unit_elf"];
            t.cultivateChance = 1.0f;
            t = Main.instance.extendedActorStatsLibrary["unit_dwarf"];
            t.cultivateChance = 1.0f;
            t = Main.instance.extendedActorStatsLibrary["unit_orc"];
            t.cultivateChance = 1.0f;
            t.defaultCultisystem = "bodying";
            t = Main.instance.extendedActorStatsLibrary["unit_Tian"];
            t.raceSpells.Add("summonTian");
            t.raceSpells.Add("summonTian1");
            t.cultivateChance = 0f;
            t = Main.instance.extendedActorStatsLibrary["unit_Ming"];
            t.cultivateChance = 1.0f;
            t.raceSpells.Add("summon");
            t.defaultCultisystem = "normal";
            t = Main.instance.extendedActorStatsLibrary["unit_Yao"];
            t.cultivateChance = 1.0f;
            t = Main.instance.extendedActorStatsLibrary["unit_EasternHuman"];
            t.cultivateChance = 1.0f;
            t.defaultCultisystem = "normal";
            t = Main.instance.extendedActorStatsLibrary["unit_Wu"];
            t.cultivateChance = 1.0f;
            t.defaultCultisystem = "bodying";
            #endregion


            #region baby
            clone("baby_human", "unit_human");
            clone("baby_elf", "unit_elf");
            clone("baby_dwarf", "unit_dwarf");
            clone("baby_orc", "unit_orc");
            clone("baby_Tian", "unit_Tian");
            clone("baby_Ming", "unit_Ming");
            clone("baby_Yao", "unit_Yao");
            clone("baby_EasternHuman", "unit_EasternHuman");
            clone("baby_Wu", "unit_Wu");
            #endregion
        }
        private static void setOtherRaceFeature()
        {
            t = get("Achelous");
            t.fixedName = "河伯";
            t = get("EarthGod");
            t.fixedName = "土地";
            t = get("Mammon");
            t.fixedName = "财神";
            t = get("Hymen");
            t.fixedName = "月老";
            t = get("MountainGod");
            t.fixedName = "山神";
            t = get("ZhongKui");
            t.fixedName = "钟馗";

            t = get("DiJiang");
            t.fixedName = "帝江";
            t.ignoreTimeStop = true;
            t = get("GongGong");
            t.fixedName = "共工";
            t.addSpells("water_orb1");
            t.preferedElement = new int[] { 0, 0, 100, 0, 0 };
            t.preferedElementScale = 1.0f;
            t = get("HouTu");
            t.fixedName = "后土";
            t.addSpells("HouTuSummon1", "HouTuSummon2","sunkens");
            t.preferedElement = new int[] { 0, 0, 0, 0, 100 };
            t.preferedElementScale = 1.0f;
            t = get("GouMang");
            t.fixedName = "句芒";
            t.preferedElement = new int[] { 0, 100, 0, 0, 0 };
            t.preferedElementScale = 1.0f;
            t = get("QiangLiang");
            t.fixedName = "强良";
            t = get("ZhuJiuYin");
            t.fixedName = "烛九阴";
            t.addSpells("timeStop");
            t.ignoreTimeStop = true;
            t = get("RuShou");
            t.addSpells("shield");
            t.fixedName = "蓐收";
            t.preferedElement = new int[] { 100, 0, 0, 0, 0 };
            t.preferedElementScale = 1.0f;
            t = get("SheBiShi");
            t.addSpells("summonSnake");
            t.fixedName = "奢比尸";
            t = get("TianWu");
            t.addSpells("wind_blade");
            t.fixedName = "天吴";
            t = get("XingTian");
            t.fixedName = "刑天";
            t.forceDeathAge = 100;
            t = get("XiZi");
            t.fixedName = "翕兹";
            t = get("XuanMing");
            t.fixedName = "玄冥";
            t.addSpells("rainCloud", "ice_blade");
            t = get("ZhuRong");
            t.fixedName = "祝融";
            t.preferedElement = new int[] { 0, 0, 0, 100, 0 };
            t.preferedElementScale = 1.0f;

            t = Main.instance.extendedActorStatsLibrary["JiaoDragon"];
            t.raceSpells.Add("JiaoDragon_laser");

            t = Main.instance.extendedActorStatsLibrary["EasternDragon"];
            t.raceSpells.Add("JiaoDragon_laser");

            t = Main.instance.extendedActorStatsLibrary["MengZhu"];
            t.raceSpells.Add("lightning");
            t.raceSpells.Add("summonTian");

            t = Main.instance.extendedActorStatsLibrary["MonkeySheng1"];
            t.raceSpells.Add("goldBar");
            t.raceSpells.Add("goldBarDown");

        }
        public static ExtendedActorStats get(string id)
        {
            ExtendedActorStats res;
            if(!Main.instance.extendedActorStatsLibrary.TryGetValue(id,out res))
            {
                Debug.Log("[ExtendedActorStats]:"+id + " not found");
                return null;
            }
            return res;
        }
    }
}
