using System.Collections.Generic;
using CultivationWay;
namespace Cultivation_Way
{
    class ExtendedActorStats
    {
        public string raceID = string.Empty;

        public float cultivateVelo = 1.0f;

        public float cultivateChance = 0.2f;

        public int[] preferedElement;//种族更倾向的元素

        public List<string> raceSpells;//种族自带法术













        internal static void init()
        {
            foreach (ActorStats stats in AssetManager.unitStats.list)
            {
                ExtendedActorStats feature = new ExtendedActorStats();
                feature.raceID = stats.race;
                feature.raceSpells = new List<string>();
                Main.instance.extendedActorStatsLibrary.Add(stats.id, feature);
            }
            setIntelligentRaceFeature();
            setOtherRaceFeature();
        }
        private static void setIntelligentRaceFeature()
        {
            ExtendedActorStats humanFeature = Main.instance.extendedActorStatsLibrary["unit_human"];

            ExtendedActorStats elfFeature = Main.instance.extendedActorStatsLibrary["unit_elf"];

            ExtendedActorStats dwarfFeature = Main.instance.extendedActorStatsLibrary["unit_dwarf"];

            ExtendedActorStats orcFeature = Main.instance.extendedActorStatsLibrary["unit_orc"];

            ExtendedActorStats TianFeature = Main.instance.extendedActorStatsLibrary["unit_Tian"];
            TianFeature.raceSpells.Add("summonTian");
            TianFeature.raceSpells.Add("summonTian1");
            ExtendedActorStats MingFeature = Main.instance.extendedActorStatsLibrary["unit_Ming"];
            MingFeature.raceSpells.Add("summon");
            ExtendedActorStats YaoFeature = Main.instance.extendedActorStatsLibrary["unit_Yao"];
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
