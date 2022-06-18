using System.Collections.Generic;

namespace Cultivation_Way
{
    internal class ExtensionSpellAsset : Asset
    {
        public string name;//法术名

        public int rarity;//稀有度，影响获取概率，一般取值范围INT_MIN-9,10为不可能依靠随机获取

        public List<string> bannedRace;//禁用种族

        public List<string> bannedCultisystem;//禁用体系

        public float might;//基础效果，1f为基础攻击力，与基础攻击力相乘

        public int baseCost;//基础蓝耗，暂未实现

        public int coolDown;//冷却时间，可能遗弃

        public ExtensionSpellType type;//法术类型以及信息

        public DirectionType direction = DirectionType.none;//动画方向，

        public ChineseElement chineseElement;//法术元素类型

        public SpellAction spellAction;//具体法术效果

        public ExtensionSpellAsset()
        {
            bannedRace = new List<string>();
            bannedCultisystem = new List<string>();
        }
    }
}
