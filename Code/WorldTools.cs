using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionUtility;
using CultivationWay;
using UnityEngine;
namespace Cultivation_Way
{
    static class WorldTools
    {
        public static bool showElementZones(this MapBox world)
        {
            return Main.instance.addMapMode == "map_reki_zones";
        }
    }
}
