using System.Collections.Generic;

namespace Cultivation_Way
{
    internal class ExtendedActorStatus
    {
        /// <summary>
        /// 修炼体系
        /// </summary>
        public string cultisystem;
        /// <summary>
        /// 家族id
        /// </summary>
        public string familyID;
        /// <summary>
        /// 姓氏
        /// </summary>
        public string familyName;
        /// <summary>
        /// 体质id
        /// </summary>
        public string specialBody;
        /// <summary>
        /// 容量提升
        /// </summary>
        public int mod_maxExp;
        /// <summary>
        /// 剩余护盾
        /// </summary>
        public int leftShied = 0;
        /// <summary>
        /// 是否可以修炼
        /// </summary>
        public bool canCultivate = false;
        /// <summary>
        /// 修炼功法
        /// </summary>
        public CultivationBook cultiBook;
        /// <summary>
        /// 灵根
        /// </summary>
        public ChineseElement chineseElement = new ChineseElement();
        /// <summary>
        /// 额外加成
        /// </summary>
        public MoreStats bonusStats = new MoreStats();
        public List<ExtendedSpell> spells = new List<ExtendedSpell>();
        /// <summary>
        /// 复合设定
        /// </summary>
        public CompositionSetting compositionSetting = new CompositionSetting();

        
    }
}
