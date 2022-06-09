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
        public static BuildingData getBuildingData(this Building building)
        {
            return ((ExtendedBuilding)building).easyData;
        }
        public static string getCircumType(Building building)
        {
            return ((ExtendedBuilding)building).easyStats.id.Split('_')[1];
        }
    }
}
