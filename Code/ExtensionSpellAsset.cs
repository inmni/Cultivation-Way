using System.Collections.Generic;

namespace Cultivation_Way
{
    internal class ExtensionSpellAsset : Asset
    {
        public string name;//法术名

        public int rarity;//稀有度，影响获取概率，一般取值范围INT_MIN-9,10为不可能依靠随机获取

        public List<string> bannedRace;//禁用种族

        public List<string> bannedCultisystem;//禁用体系
        public uint allowedCultisystem;

        public float might;//基础效果，1f为基础攻击力，与基础攻击力相乘

        public int baseCost;//基础蓝耗

        public int coolDown;//冷却时间

        public float chance = 0.2f;//释放几率

        public ExtensionSpellType type = ExtensionSpellType.NONE;//法术类型以及信息
        public int requiredLevel = 1;

        public ChineseElement chineseElement;//法术元素类型

        public SpellAction spellAction;//具体法术效果

        public ExtensionSpellAsset()
        {
            bannedRace = new List<string>();
            bannedCultisystem = new List<string>();
        }
        internal bool allowCultisystem(uint cultiSystemID)
        {
            return (allowedCultisystem & cultiSystemID) > 0;
        }
    }
}
