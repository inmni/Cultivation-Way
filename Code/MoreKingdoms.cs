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
            //主要国家
            KingdomAsset addKingdom1 = AssetManager.kingdoms.clone("Tian", "human");
            addKingdom1.addFriendlyTag("Tian");
            addKingdom1.civ = true;
            addKingdom1.nomads = true;
            this.newHiddenKingdom(addKingdom1);
            //临时用的国家
            KingdomAsset addKingdom2 = AssetManager.kingdoms.clone("nomads_Tian", "Tian");
            addKingdom2.civ = true;
            addKingdom2.nomads = true;
            this.newHiddenKingdom(addKingdom2);
            #endregion
            #region 冥族
            //主要国家
            KingdomAsset addKingdom3 = AssetManager.kingdoms.clone("Ming", "human");
            addKingdom3.addFriendlyTag("Ming");
            addKingdom3.civ = true;
            addKingdom3.nomads = true;
            this.newHiddenKingdom(addKingdom3);
            //临时用的国家
            KingdomAsset addKingdom4 = AssetManager.kingdoms.clone("nomads_Ming", "Ming");
            addKingdom4.civ = true;
            addKingdom4.nomads = true;
            this.newHiddenKingdom(addKingdom4);
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
