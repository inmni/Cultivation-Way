using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    class ExtensionSpellLibrary:AssetLibrary<ExtensionSpellAsset>
    {
        public override void init()
        {
            base.init();
            this.add(new ExtensionSpellAsset
            {
                id = "default",
                name = "测试",
                rarity = 1,
                bannedRace = new List<string> { "orc" },
                bannedCultisystem = new List<string>(),
                might = 5f,
                type = new ExtensionSpellType(),//主动，可重复，必定触发，作用于单位，远程，单体，矢量
                direction = DirectionType.toTarget,
                chineseElement = new ChineseElement(new int[] { 50, 20, 10, 10, 10 })
            }) ;
        }
    }
}
