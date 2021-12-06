using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cultivation_Way
{
    class ChineseElementLibrary:AssetLibrary<ChineseElementAsset>
    {
        public override void init()
        {
            base.init();
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
            //混沌
            this.add(new ChineseElementAsset
            {
                id = "All",
                name = "混沌",
                content = new int[5] { 20, 20, 20, 20, 20 },
                oppositeElementId = new string[] { "*" },//星号表示所有，指在实现时
                promoteElementId = new string[] { "*"}
            }) ;
        }
    }
}
