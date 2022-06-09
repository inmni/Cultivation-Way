using ReflectionUtility;
using System.Collections.Generic;
using System.Linq;
using CultivationWay;
namespace Cultivation_Way
{
    static class KingdomAndCityTools
    {
        public static ExtendedActor getMaxLevelActor(this Kingdom kingdom)
        {
            ExtendedActor res = null;
            int maxLevel = 0;
            foreach (ExtendedActor actor in kingdom.units)
            {
                if (actor.easyData.level > maxLevel)
                {
                    maxLevel = actor.easyData.level;
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
        public static ExtendedActor getMaxLevelActor(this City city)
        {
            ExtendedActor res = null;
            int maxLevel = 0;
            if (city == null || city.units == null)
            {
                return null;
            }
            foreach (ExtendedActor actor in city.units)
            {
                if (actor.easyData.level > maxLevel)
                {
                    maxLevel = actor.easyData.level;
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
        public static ExtendedActor getRandomParent(this City city, ExtendedActor ignoreActor)
        {
            List<Actor> list = city.units.getSimpleList();
            list.Shuffle();
            for (int i = 0; i < list.Count; i++)
            {
                ExtendedActor actor = (ExtendedActor)list[i];
                if (actor.easyData.alive && !(actor == ignoreActor) && !actor.haveTrait("plague") && actor.easyData.age > 18 && actor.stats.procreate)
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
            if (((ExtendedActor)city.leader).easyData.level < ((ExtendedActor)kingdom.king).easyData.level)
            {
                return true;
            }
            return false;
        }
        //神明加成
        public static int moreProduceMin(this City city)
        {
            if (city.leader == null || city.leader.kingdom==null|| ExtendedKingdomStats.getStatus(city.leader.kingdom.id,"Hymen")==0f)
            {
                return 1;
            }
            return Toolbox.randomInt(1, city.status.population / 7 + 2);
        }
        public static int produceMore(Actor pActor,BuildingType buildingType)
        {
            if (pActor.kingdom == null)
            {
                return 0;
            }
            if(buildingType <= BuildingType.Mineral || buildingType >= BuildingType.Tree)
            {
                return (int)ExtendedKingdomStats.getStatus(pActor.kingdom.id,"MountainGod");//1
            }
            else if(buildingType == BuildingType.Wheat)
            {
                return (int)ExtendedKingdomStats.getStatus(pActor.kingdom.id,"EarthGod");//1
            }
            return 0;
        }
        public static int gainMoreFromTrade(Actor pActor)
        {
            if (pActor.kingdom == null)
            {
                return 5;
            }
            return (int)ExtendedKingdomStats.getStatus(pActor.kingdom.id,"Mammon");//8
        }
        public static bool checkAchelous(ActorBase pActor)
        {
            if (pActor.kingdom == null)
            {
                return false;
            }
            return ExtendedKingdomStats.getStatus(pActor.kingdom.id,"Achelous")==0f;
        }
        public static bool checkZhongKui(Actor pActor)
        {
            if (pActor.kingdom == null)
            {
                return false;
            }
            return ExtendedKingdomStats.getStatus(pActor.kingdom.id,"ZhongKui")==0f;
        }
    }
}
