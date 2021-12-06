using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cultivation_Way
{
    class MoreKingdoms
    {
        internal void init()
        {
            #region 天族
            KingdomAsset addKingdom1 = AssetManager.kingdoms.clone("Tian", "human");
            addKingdom1.addFriendlyTag("Tian");


            addKingdom1 = AssetManager.kingdoms.clone("nomads_Tian", "Tian");
            this.newHiddenKingdom(addKingdom1);
            #endregion
            #region 冥族
            KingdomAsset addKingdom2 = AssetManager.kingdoms.clone("Ming", "human");
            addKingdom2.addFriendlyTag("Ming");


            addKingdom2 = AssetManager.kingdoms.clone("nomads_Ming", "Ming");
            this.newHiddenKingdom(addKingdom2);
            #endregion

            //BannerGenerator.loadBanners($"Mods/EmbededResources/banners");
        }
        private void newHiddenKingdom(KingdomAsset pAsset)
        {
            Kingdom kingdom = new Kingdom();
            kingdom.asset = pAsset;
            kingdom.createHidden();
            kingdom.id = pAsset.id;
            kingdom.name = pAsset.id;
            KingdomManager kingdomManager = MapBox.instance.kingdoms;
            kingdomManager.addKingdom(kingdom, false);
        }
    }
}
