using CultivationWay;
namespace Cultivation_Way
{
    internal static class ExtendedItemTools
    {
        public static ExtendedItemStats GetExtendedStats(this ItemAsset itemAsset)
        {
            return Main.instance.extendedItemStatsLibrary[itemAsset.id];
        }
    }
}
