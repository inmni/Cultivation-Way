
using CultivationWay;

namespace Cultivation_Way
{
    class AddAssetManager
    {
        ChineseNameLibrary chineseNameGenerator;

        ChineseElementLibrary chineseElementLibrary;

        public static void addAsset()
        {
            Main.instance.MoreTraits.init();
            Main.instance.MoreGodPowers.init();
            Main.instance.MoreActors.init();
            Main.instance.MoreRaces.init();
            Main.instance.MoreKingdoms.init();
            Main.instance.MoreBuildings.init();
            Main.instance.MoreDrops.init();
            Main.instance.MoreProjectiles.init();

            ChineseNameLibrary chineseNameGenerator = new ChineseNameLibrary();
            ChineseElementLibrary chineseElementLibrary = new ChineseElementLibrary();
            CultisystemLibrary cultisystemLibrary = new CultisystemLibrary();
            add(chineseNameGenerator, "chineseNameGenerator");
            add(chineseElementLibrary, "element");
            add(cultisystemLibrary, "cultisystem");

            Main.instance.MoreCultureTech.init();//顺序不可调换

        }

        private static void add(BaseAssetLibrary pLibrary, string pID)
        {
            pLibrary.init();
            pLibrary.id = pID;
            AssetManager.instance.dict.Add(pID, pLibrary);
            AssetManager.instance.list.Add(pLibrary);
        }
    }
}
