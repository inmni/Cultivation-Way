using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CultivationWay;

namespace Cultivation_Way
{
    class ExtensionSpellAsset : Asset
    {
        public string name;//法术名

        public int rarity;//稀有度，影响获取概率

        public List<string> bannedRace;//禁用种族

        public List<string> bannedCultisystem;//禁用体系

        public float might;//基础效果，计量单位待定

        public int baseCost;//基础蓝耗

        public float coolDown;//冷却时间

        public ExtensionSpellType type;//法术类型

        public DirectionType direction = DirectionType.none;//动画方向，

        public ChineseElement chineseElement;//法术元素类型

        public SpellAction spellAction;//具体法术动画效果

        public ExtensionSpellAsset(){
            bannedRace = new List<string>();
            bannedCultisystem = new List<string>();
        }
    }
}
