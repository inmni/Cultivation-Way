
using CultivationWay;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cultivation_Way
{
    class AddAssetManager
    {
        ChineseNameLibrary chineseNameGenerator;
        ChineseElementLibrary chineseElementLibrary;

        public static void addAsset()
        {
            Main.MoreGodPowers.init();
            Main.MoreActors.init();
            Main.MoreRaces.init();
            Main.MoreKingdoms.init();
            Main.MoreBuildings.init();

            ChineseNameLibrary chineseNameGenerator = new ChineseNameLibrary();
            ChineseElementLibrary chineseElementLibrary = new ChineseElementLibrary();
            add(chineseNameGenerator, "chineseNameGenerator");
            add(chineseElementLibrary, "chineseElementLibrary");

        }

        private static void add(BaseAssetLibrary pLibrary,string pID)
        {
            pLibrary.init();
            pLibrary.id = pID;
            AssetManager.instance.dict.Add(pID, pLibrary);
            AssetManager.instance.list.Add(pLibrary);
        }
    }
}
