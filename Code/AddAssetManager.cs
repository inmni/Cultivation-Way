
using CultivationWay;

namespace Cultivation_Way
{
    class AddAssetManager
    {
        internal static ChineseNameLibrary chineseNameGenerator;

        internal static ChineseElementLibrary chineseElementLibrary;

        internal static CultisystemLibrary cultisystemLibrary;

        internal static ExtensionSpellLibrary extensionSpellLibrary;

        internal static SpecialBodyLibrary specialBodyLibrary;

        public static void addAsset()
        {
            //顺序尽量不要调整
            ChineseNameGenerator.init();
            Main.instance.moreItems.init();
            Main.instance.moreTraits.init();
            Main.instance.moreActors.init();
            Main.instance.moreGodPowers.init();
            Main.instance.moreRaces.init();
            Main.instance.moreKingdoms.init();
            Main.instance.moreBuildings.init();
            Main.instance.moreDrops.init();
            Main.instance.moreProjectiles.init();
            Main.instance.moreCityTasks.init();
            Main.instance.moreTopTileTypes.init();
            chineseNameGenerator = new ChineseNameLibrary();
            chineseElementLibrary = new ChineseElementLibrary();
            cultisystemLibrary = new CultisystemLibrary();
            extensionSpellLibrary = new ExtensionSpellLibrary();
            specialBodyLibrary = new SpecialBodyLibrary();
            add(chineseNameGenerator, "chineseNameGenerator");
            add(chineseElementLibrary, "element");
            add(cultisystemLibrary, "cultisystem");
            add(extensionSpellLibrary, "extensionSpell");
            add(specialBodyLibrary, "specialBody");
            Main.instance.moreCultureTechs.init();//顺序不可调换

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
