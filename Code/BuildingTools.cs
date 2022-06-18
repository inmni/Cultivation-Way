namespace Cultivation_Way
{
    internal static class BuildingTools
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
