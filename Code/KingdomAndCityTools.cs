using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    static class KingdomAndCityTools
    {
        public static Actor getMaxLevelActor(this Kingdom kingdom)
        {
            Actor res = null;
            int maxLevel = 0;
            foreach(Actor actor in kingdom.units)
            {
                if (actor.GetData().level > maxLevel)
                {
                    maxLevel = actor.GetData().level;
                    res = actor;
                }
            }
            return res;
        }
        public static Actor getMaxLevelActor(this City city)
        {
            Actor res = null;
            int maxLevel = 0;
            foreach(Actor actor in city.units)
            {
                if (actor.GetData().level > maxLevel)
                {
                    maxLevel = actor.GetData().level;
                    res = actor;
                }
            }
            return res;
        }
    }
}
