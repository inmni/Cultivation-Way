namespace Cultivation_Way
{
    class ChineseElementLibrary : AssetLibrary<ChineseElementAsset>
    {
        public override void init()
        {
            base.init();
            #region 单属性 五行
            //金
            this.add(new ChineseElementAsset
            {
                id = "Gold",
                name = "金",
                content = new int[5] { 100, 0, 0, 0, 0 },
                oppositeElementId = new string[] { "Wood" },
                promoteElementId = new string[] { "Water" }
            });
            //木
            this.add(new ChineseElementAsset
            {
                id = "Wood",
                name = "木",
                content = new int[5] { 0, 100, 0, 0, 0 },
                oppositeElementId = new string[] { "Ground" },
                promoteElementId = new string[] { "Fire" }
            });
            //水
            this.add(new ChineseElementAsset
            {
                id = "Water",
                name = "水",
                content = new int[5] { 0, 0, 100, 0, 0 },
                oppositeElementId = new string[] { "Fire" },
                promoteElementId = new string[] { "Wood" }
            });
            //火
            this.add(new ChineseElementAsset
            {
                id = "Fire",
                name = "火",
                content = new int[5] { 0, 0, 0, 100, 0 },
                oppositeElementId = new string[] { "Gold" },
                promoteElementId = new string[] { "Ground" }
            });
            //土
            this.add(new ChineseElementAsset
            {
                id = "Ground",
                name = "土",
                content = new int[5] { 0, 0, 0, 0, 100 },
                oppositeElementId = new string[] { "Water" },
                promoteElementId = new string[] { "Gold" }
            });
            #endregion

            #region 双属性
            this.add(new ChineseElementAsset
            {
                id = "GoWo",
                name = "金木",
                content = new int[5] { 50, 50, 0, 0, 0 }
            });
            this.add(new ChineseElementAsset
            {
                id = "GoWa",
                name = "金水",
                content = new int[5] { 50, 0, 50, 0, 0 }
            });
            this.add(new ChineseElementAsset
            {
                id = "GoF",
                name = "金火",
                content = new int[5] { 50, 0, 0, 50, 0 }
            });
            this.add(new ChineseElementAsset
            {
                id = "GoGr",
                name = "金土",
                content = new int[5] { 50, 0, 0, 0, 50 }
            });
            this.add(new ChineseElementAsset
            {
                id = "WoWa",
                name = "木水",
                content = new int[5] { 0, 50, 50, 0, 0 }
            });
            this.add(new ChineseElementAsset
            {
                id = "WoF",
                name = "木火",
                content = new int[5] { 0, 50, 0, 50, 0 }
            });
            this.add(new ChineseElementAsset
            {
                id = "WoGr",
                name = "木土",
                content = new int[5] { 0, 50, 0, 0, 50 }
            });
            this.add(new ChineseElementAsset
            {
                id = "WaF",
                name = "水火",
                content = new int[5] { 0, 0, 50, 50, 0 }
            });
            this.add(new ChineseElementAsset
            {
                id = "WaWr",
                name = "水土",
                content = new int[5] { 0, 0, 50, 0, 50 }
            });
            this.add(new ChineseElementAsset
            {
                id = "FGr",
                name = "火土",
                content = new int[5] { 0, 0, 0, 50, 50 }
            });
            #endregion

            #region 三属性

            #endregion

            #region 四属性

            #endregion

            #region 五属性
            //混沌
            this.add(new ChineseElementAsset
            {
                id = "All",
                name = "杂",
                content = new int[5] { 20, 20, 20, 20, 20 },
                oppositeElementId = new string[] { "*" },//星号表示所有，指在实现时
                promoteElementId = new string[] { }//无
            });
            #endregion
        }
    }
}
