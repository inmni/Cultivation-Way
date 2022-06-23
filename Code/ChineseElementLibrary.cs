namespace Cultivation_Way
{
    internal class ChineseElementLibrary : AssetLibrary<ChineseElementAsset>
    {
        public override void init()
        {
            base.init();

            #region 单属性 五行
            //金
            add(new ChineseElementAsset
            {
                id = "Gold",
                name = "金",
                content = new int[5] { 100, 0, 0, 0, 0 },
                oppositeElementId = new string[] { "Wood" },
                promoteElementId = new string[] { "Water" },
                rarity = 3
            });
            //木
            add(new ChineseElementAsset
            {
                id = "Wood",
                name = "木",
                content = new int[5] { 0, 100, 0, 0, 0 },
                oppositeElementId = new string[] { "Ground" },
                promoteElementId = new string[] { "Fire" },
                rarity = 3
            });
            //水
            add(new ChineseElementAsset
            {
                id = "Water",
                name = "水",
                content = new int[5] { 0, 0, 100, 0, 0 },
                oppositeElementId = new string[] { "Fire" },
                promoteElementId = new string[] { "Wood" },
                rarity = 3
            });
            //火
            add(new ChineseElementAsset
            {
                id = "Fire",
                name = "火",
                content = new int[5] { 0, 0, 0, 100, 0 },
                oppositeElementId = new string[] { "Gold" },
                promoteElementId = new string[] { "Ground" },
                rarity = 3
            });
            //土
            add(new ChineseElementAsset
            {
                id = "Ground",
                name = "土",
                content = new int[5] { 0, 0, 0, 0, 100 },
                oppositeElementId = new string[] { "Water" },
                promoteElementId = new string[] { "Gold" },
                rarity = 3
            });
            #endregion

            #region 双属性
            add(new ChineseElementAsset
            {
                id = "GoWo",
                name = "金木",
                content = new int[5] { 50, 50, 0, 0, 0 },
                rarity = 2
            });
            add(new ChineseElementAsset
            {
                id = "GoWa",
                name = "金水",
                content = new int[5] { 50, 0, 50, 0, 0 },
                rarity = 2
            });
            add(new ChineseElementAsset
            {
                id = "GoF",
                name = "金火",
                content = new int[5] { 50, 0, 0, 50, 0 },
                rarity = 2
            });
            add(new ChineseElementAsset
            {
                id = "GoGr",
                name = "金土",
                content = new int[5] { 50, 0, 0, 0, 50 },
                rarity = 2
            });
            add(new ChineseElementAsset
            {
                id = "WoWa",
                name = "木水",
                content = new int[5] { 0, 50, 50, 0, 0 },
                rarity = 2
            });
            add(new ChineseElementAsset
            {
                id = "WoF",
                name = "木火",
                content = new int[5] { 0, 50, 0, 50, 0 },
                rarity = 2
            });
            add(new ChineseElementAsset
            {
                id = "WoGr",
                name = "木土",
                content = new int[5] { 0, 50, 0, 0, 50 },
                rarity = 2
            });
            add(new ChineseElementAsset
            {
                id = "WaF",
                name = "水火",
                content = new int[5] { 0, 0, 50, 50, 0 },
                rarity = 2
            });
            add(new ChineseElementAsset
            {
                id = "WaWr",
                name = "水土",
                content = new int[5] { 0, 0, 50, 0, 50 },
                rarity = 2
            });
            add(new ChineseElementAsset
            {
                id = "FGr",
                name = "火土",
                content = new int[5] { 0, 0, 0, 50, 50 },
                rarity = 2
            });
            #endregion

            #region 三属性

            #endregion

            #region 四属性

            #endregion
            #region 五属性
            //混沌
            add(new ChineseElementAsset
            {
                id = "AllElement",
                name = "混沌",
                content = new int[5] { 20, 20, 20, 20, 20 },
                oppositeElementId = new string[] { "*" },//星号表示所有，指在实现时
                promoteElementId = new string[] { },//无
                rarity = 20
            });
            add(new ChineseElementAsset
            {
                id = "All",
                name = "杂",
                content = new int[5] { 20, 20, 20, 20, 20 },
                oppositeElementId = new string[] { "*" },//星号表示所有，指在实现时
                promoteElementId = new string[] { },//无
                rarity = 1
            });
            #endregion

        }

        public void reset()
        {
            dict.Clear();
            list.Clear();
            init();
        }
    }
}
