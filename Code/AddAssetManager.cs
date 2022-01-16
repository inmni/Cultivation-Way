
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
            Main.instance.MoreItem.init();
            Main.instance.MoreTraits.init();
            Main.instance.MoreGodPowers.init();
            Main.instance.MoreActors.init();
            Main.instance.MoreRaces.init();
            Main.instance.MoreKingdoms.init();
            Main.instance.MoreBuildings.init();
            Main.instance.MoreDrops.init();
            Main.instance.MoreProjectiles.init();

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
