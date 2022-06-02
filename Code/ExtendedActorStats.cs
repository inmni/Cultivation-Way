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
                Main.instance.raceFeatures.Add(stats.id, feature);
            }
            setIntelligentRaceFeature();
            setOtherRaceFeature();
        }
        private static void setIntelligentRaceFeature()
        {
            ExtendedActorStats humanFeature = Main.instance.raceFeatures["unit_human"];

            ExtendedActorStats elfFeature = Main.instance.raceFeatures["unit_elf"];

            ExtendedActorStats dwarfFeature = Main.instance.raceFeatures["unit_dwarf"];

            ExtendedActorStats orcFeature = Main.instance.raceFeatures["unit_orc"];

            ExtendedActorStats TianFeature = Main.instance.raceFeatures["unit_Tian"];
            TianFeature.raceSpells.Add("summonTian");
            TianFeature.raceSpells.Add("summonTian1");
            ExtendedActorStats MingFeature = Main.instance.raceFeatures["unit_Ming"];
            MingFeature.raceSpells.Add("summon");
            ExtendedActorStats YaoFeature = Main.instance.raceFeatures["unit_Yao"];
        }
        private static void setOtherRaceFeature()
        {
            ExtendedActorStats JiaoDragonFeature = Main.instance.raceFeatures["JiaoDragon"];
            JiaoDragonFeature.raceSpells.Add("JiaoDragon_laser");

            ExtendedActorStats EasternDragonFeature = Main.instance.raceFeatures["EasternDragon"];
            EasternDragonFeature.raceSpells.Add("JiaoDragon_laser");

            ExtendedActorStats MengZhuFeature = Main.instance.raceFeatures["MengZhu"];
            MengZhuFeature.raceSpells.Add("lightning");
            MengZhuFeature.raceSpells.Add("summonTian");

            ExtendedActorStats MonkeySheng1 = Main.instance.raceFeatures["MonkeySheng1"];
            MonkeySheng1.raceSpells.Add("goldBar");
            MonkeySheng1.raceSpells.Add("goldBarDown");

        }
    }
}
