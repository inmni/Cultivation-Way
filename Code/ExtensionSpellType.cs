namespace Cultivation_Way
{
    /// <summary>
    /// 用于表示法术信息，而非类别
    /// </summary>
    internal class ExtensionSpellType
    {
        public bool attacking = true;
        public bool attacked = false;
        public bool levelUp = false;
        public bool byChance = false;
        /// <summary>
        /// 表示境界，并非等级
        /// </summary>
        public int requiredLevel = 10;
        public float chance = 0.2f;
        public ExtensionSpellType()
        {
        }

    }
}
