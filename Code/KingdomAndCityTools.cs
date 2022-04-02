using ReflectionUtility;
using System.Collections.Generic;
using System.Linq;
using CultivationWay;
namespace Cultivation_Way
{
    static class KingdomAndCityTools
    {
        public static Actor getMaxLevelActor(this Kingdom kingdom)
        {
            Actor res = null;
            int maxLevel = 0;
            foreach (Actor actor in kingdom.units)
            {
                if (actor.GetData().level > maxLevel)
                {
                    maxLevel = actor.GetData().level;
                    res = actor;
                }
            }
            return res;
        }
        public static bool isEnemy(this Kingdom kingdom, Kingdom pKingdom)
        {
            if (pKingdom == null)
            {
                return true;
            }
            if (kingdom.isCiv() && pKingdom.isCiv())
            {
                return pKingdom != kingdom && !kingdom.allies.ContainsKey(pKingdom);
            }
            return kingdom.asset.isFoe(pKingdom.asset);
        }
        public static bool isAllowedBuildCity(Actor actor)
        {
            if (actor == null)
            {
                return true;
            }
            return actor.stats.race != "Yao" || MapBox.instance.worldLaws.dict["YaoKingdom"].boolVal;
        }
        public static Actor getMaxLevelActor(this City city)
        {
            Actor res = null;
            int maxLevel = 0;
            if (city == null || city.units == null)
            {
                return null;
            }
            foreach (Actor actor in city.units)
            {
                if (actor.GetData().level > maxLevel)
                {
                    maxLevel = actor.GetData().level;
                    res = actor;
                }
            }
            return res;
        }
        public static void joinAnotherKingdom(this City city, Kingdom pKingdom, bool force)
        {
            Kingdom pKingdom2 = city.units.ToList()[0].kingdom;
            city.CallMethod("removeFromCurrentKingdom");
            city.CallMethod("setKingdom", pKingdom);
            city.CallMethod("switchedKingdom");
        }
        public static Actor getRandomParent(this City city, Actor ignoreActor)
        {
            List<Actor> list = city.units.getSimpleList();
            list.Shuffle();
            for (int i = 0; i < list.Count; i++)
            {
                Actor actor = list[i];
                if (actor.GetData().alive && !(actor == ignoreActor) && !actor.haveTrait("plague") && actor.GetData().age > 18 && actor.stats.procreate)
                {
                    return actor;
                }
            }
            return null;
        }
        public static bool canBeCapturedSmoothly(this City city,Kingdom kingdom)
        {
            if (kingdom.king == null)
            {
                return false;
            }
            if (city.leader == null)
            {
                return true;
            }
            if (city.leader.GetData().level < kingdom.king.GetData().level)
            {
                return true;
            }
            return false;
        }
        //神明加成
        public static int moreProduceMin(this City city)
        {
            if (city.leader != null && city.leader.kingdom.raceID == "EasternHuman")
            {
                foreach(Actor actor in Main.instance.kingdomBindActors[city.leader.kingdom.id])
                {
                    if (actor.stats.id == "Hymen")
                    {
                        return Toolbox.randomInt(1, city.status.population / 7+2);
                    }
                }
            }
            return 1;
        }
        public static int produceMore(Actor pActor,BuildingType buildingType)
        {
            if (pActor.kingdom == null)
            {
                return 0;
            }
            List<Actor> gods = null;
            if (!Main.instance.kingdomBindActors.TryGetValue(pActor.kingdom.id, out gods))
            {
                return 0;
            }
            if (gods.Exists(a => 
                  (a.stats.id == "MountainGod"&&(buildingType<=BuildingType.Gold||buildingType>=BuildingType.Tree))
                ||(a.stats.id=="EarthGod"&&buildingType==BuildingType.Wheat)))
            {
                return 1;
            }
            return 0;
        }
        public static int gainMoreFromTrade(Actor pActor)
        {
            if (pActor.kingdom == null)
            {
                return 5;
            }
            List<Actor> gods = null;
            if (!Main.instance.kingdomBindActors.TryGetValue(pActor.kingdom.id, out gods))
            {
                return 5;
            }
            if (gods.Exists(actor => actor.stats.id == "Mammon"))
            {
                return 8;
            }
            return 5;
        }
        public static bool checkAchelous(ActorBase pActor)
        {
            if (pActor.kingdom == null)
            {
                return false;
            }
            if (pActor.city == null)
            {
                return false;
            }
            List<Actor> gods = null;
            if (!Main.instance.kingdomBindActors.TryGetValue(pActor.kingdom.id, out gods))
            {
                return false;
            }
            return gods.Exists(actor => actor.stats.id == "Achelous");
        }
        public static bool checkZhongKui(Actor pActor)
        {
            if (pActor.kingdom == null)
            {
                return false;
            }
            if (pActor.city == null)
            {
                return false;
            }
            List<Actor> gods = null;
            if (!Main.instance.kingdomBindActors.TryGetValue(pActor.kingdom.id, out gods))
            {
                return false;
            }
            return gods.Exists(actor => actor.stats.id == "ZhongKui");
        }
    }
}
