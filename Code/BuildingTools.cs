using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionUtility;
namespace Cultivation_Way
{
    static class BuildingTools
    {
        public static BuildingData GetData(this Building building)
        {
            return Reflection.GetField(typeof(Building), building, "data") as BuildingData;
        }
    }
}
