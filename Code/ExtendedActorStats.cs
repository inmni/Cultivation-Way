using CultivationWay;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    internal class ExtendedActorStats
    {
        public string raceID = string.Empty;

        public float cultivateVelo = 1.0f;

        public float cultivateChance = 0.2f;

        public int initialLevel = 1;

        public int[] preferedElement;//种族更倾向的元素

        public List<string> raceSpells;//种族自带法术

        public string defaultCultisystem = "default";











        internal static void init()
        {
            foreach (ActorStats stats in AssetManager.unitStats.list)
            {
                ExtendedActorStats feature = new ExtendedActorStats();
                feature.raceID = stats.race;
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
            ExtendedActorStats humanFeature = Main.instance.extendedActorStatsLibrary["unit_human"];

            ExtendedActorStats elfFeature = Main.instance.extendedActorStatsLibrary["unit_elf"];

            ExtendedActorStats dwarfFeature = Main.instance.extendedActorStatsLibrary["unit_dwarf"];

            ExtendedActorStats orcFeature = Main.instance.extendedActorStatsLibrary["unit_orc"];
            orcFeature.defaultCultisystem = "bodying";
            ExtendedActorStats TianFeature = Main.instance.extendedActorStatsLibrary["unit_Tian"];
            TianFeature.raceSpells.Add("summonTian");
            TianFeature.raceSpells.Add("summonTian1");
            TianFeature.cultivateChance = 0f;
            ExtendedActorStats MingFeature = Main.instance.extendedActorStatsLibrary["unit_Ming"];
            MingFeature.raceSpells.Add("summon");
            MingFeature.defaultCultisystem = "normal";
            ExtendedActorStats YaoFeature = Main.instance.extendedActorStatsLibrary["unit_Yao"];

            ExtendedActorStats EHFeature = Main.instance.extendedActorStatsLibrary["unit_EasternHuman"];
            EHFeature.defaultCultisystem = "normal";
            ExtendedActorStats WuFeature = Main.instance.extendedActorStatsLibrary["unit_Wu"];
            WuFeature.defaultCultisystem = "bodying";
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
            ExtendedActorStats JiaoDragonFeature = Main.instance.extendedActorStatsLibrary["JiaoDragon"];
            JiaoDragonFeature.raceSpells.Add("JiaoDragon_laser");

            ExtendedActorStats EasternDragonFeature = Main.instance.extendedActorStatsLibrary["EasternDragon"];
            EasternDragonFeature.raceSpells.Add("JiaoDragon_laser");

            ExtendedActorStats MengZhuFeature = Main.instance.extendedActorStatsLibrary["MengZhu"];
            MengZhuFeature.raceSpells.Add("lightning");
            MengZhuFeature.raceSpells.Add("summonTian");

            ExtendedActorStats MonkeySheng1 = Main.instance.extendedActorStatsLibrary["MonkeySheng1"];
            MonkeySheng1.raceSpells.Add("goldBar");
            MonkeySheng1.raceSpells.Add("goldBarDown");

        }
    }
}
