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
        public static void set(this BuildingFundament fundament,int left,int right,int top,int bottom)
        {
            fundament.left = left;
            fundament.right = right;
            fundament.top = top;
            fundament.bottom = bottom;
        }
    }
}
