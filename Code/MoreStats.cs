using ReflectionUtility;
using System.Collections.Generic;

namespace Cultivation_Way
{
    class MoreStats
    {
        #region BaseStats中已有属性
        //public float accuracy;		命中率
        //public float areaOfEffect;	范围效果
        //public float attackSpeed;		攻速
        //public float crit;			暴击率
        //public float damageCritMod;	暴击伤害
        //public float dodge;			格挡率
        //public float knockback;		击退
        //public float knockbackReduction;抗性
        //public float mod_armor;		防御增幅
        //public float mod_attackSpeed;	攻速增幅
        //public float mod_crit;		暴击率增幅
        //public float mod_damage;		伤害增幅
        //public float mod_diplomacy;	外交增幅
        //public float mod_health;		生命增幅
        //public float mod_speed;		移速增幅
        //public float mod_supply_timer;？？
        //public float personality_administration;
        //public float personality_aggression;
        //public float personality_diplomatic;
        //public float personality_rationality;
        //public float range;			攻击范围
        //public float s_crit_chance;	总暴击率
        //public float scale;			大小
        //public float size;			尺寸
        //public float speed;			移速
        //public int armor;				防御
        //public int army;				军队规模
        //public int bonus_towers;		箭塔增额
        //public int cities;			城市限制
        //public int damage;			伤害
        //public int diplomacy;			外交
        //public int health;			生命
        //public int intelligence;		智力
        //public int loyalty_mood;		情绪影响忠诚度
        //public int loyalty_traits;	特质影响忠诚度
        //public int opinion;			态度，观念
        //public int projectiles;		投掷物数量
        //public int stewardship;		管理能力
        //public int targets;			攻击目标数量
        //public int warfare;			军事
        //public int zones;				土地限制
        #endregion
        public BaseStats baseStats;
        /// <summary>
        /// 法术
        /// </summary>
        public List<ExtensionSpell> spells;
        /// <summary>
        /// 施法距离
        /// </summary>
        public float spellRange;
        /// <summary>
        /// 蓝量
        /// </summary>
        public int magic;
        /// <summary>
        /// 吸血
        /// </summary>
        public float vampire;
        /// <summary>
        /// 反伤
        /// </summary>
        public float antiInjury;
        /// <summary>
        /// 法术豁免
        /// </summary>
        public float spellRelief;
        /// <summary>
        /// 寿命
        /// </summary>
        public int maxAge;
        /// <summary>
        /// 天赋
        /// </summary>
        public int talent;
        /// <summary>
        /// 元神
        /// </summary>
        public int soul;
        /// <summary>
        /// 元素
        /// </summary>
        public ChineseElement element;
        /// <summary>
        /// 随机元素
        /// </summary>
        /// <returns></returns>
        private ChineseElement GetElementRandomly()
        {
            return new ChineseElement().getRandom();
        }
        /// <summary>
        /// 加法
        /// </summary>
        /// <param name="another"></param>
        public MoreStats addAnotherStats(MoreStats another)
        {
            baseStats.CallMethod("addStats", another.baseStats);

            this.spells.AddRange(another.spells);//待修改，减少重复？目前是存在重复的
            this.spellRange += another.spellRange;
            this.magic += another.magic;
            this.vampire += another.vampire;
            this.antiInjury += another.antiInjury;
            this.spellRelief += another.spellRelief;
            this.talent += another.talent;
            this.soul += another.soul;
            this.maxAge += another.maxAge;
            return this;
        }
        /// <summary>
        /// 减法
        /// </summary>
        /// <param name="another"></param>
        public void minusAnotherStats(MoreStats another)
        {
            this.spellRange -= another.spellRange;
            this.magic -= another.magic;
            this.vampire -= another.vampire;
            this.antiInjury -= another.antiInjury;
            this.spellRelief -= another.spellRelief;
            this.talent -= another.talent;
            this.soul -= another.soul;
            this.maxAge -= another.maxAge;
            baseStats.mod_attackSpeed -= another.baseStats.mod_attackSpeed;
            baseStats.mod_damage -= another.baseStats.mod_damage;
            baseStats.mod_health -= another.baseStats.mod_health;
            baseStats.mod_speed -= another.baseStats.mod_speed;
            //其他属性待补充
        }
        /// <summary>
        /// 设置基础属性
        /// </summary>
        /// <param name="health"></param>
        /// <param name="damage"></param>
        /// <param name="speed"></param>
        /// <param name="armor"></param>
        /// <param name="magic"></param>
        public void setBasicStats(int health, int damage, int speed, int armor, int magic = 0)
        {
            this.baseStats.health = health;
            this.baseStats.damage = damage;
            this.baseStats.speed = speed;
            this.baseStats.armor = armor;
            this.magic = magic;
        }
        /// <summary>
        /// 返回BaseStats
        /// </summary>
        /// <returns></returns>
        public BaseStats GetBaseStats()
        {
            return this.baseStats;
        }
        /// <summary>
        /// 设置特殊属性
        /// </summary>
        /// <param name="maxAge"></param>
        /// <param name="soul"></param>
        /// <param name="talent"></param>
        public void setSpecialStats(int maxAge, int soul, int talent)
        {
            this.maxAge = maxAge;
            this.soul = soul;
            this.talent = talent;
        }
        public void addSpell(ExtensionSpell spell)
        {
            for(int i = 0; i < this.spells.Count; i++)
            {
                if (this.spells[i].spellAssetID == spell.spellAssetID)
                {
                    this.spells[i].might = this.spells[i].might>spell.might?this.spells[i].might:spell.might;
                    return;
                }
            }
            this.spells.Add(new ExtensionSpell(spell));
        }
        public ExtensionSpell addSpell(string spellID)
        {
            for (int i = 0; i < this.spells.Count; i++)
            {
                if (this.spells[i].spellAssetID == spellID)
                {
                    return this.spells[i];
                }
            }
            ExtensionSpell spell = new ExtensionSpell(spellID);
            this.spells.Add(spell);
            return spell;
        }
        /// <summary>
        /// 清空
        /// </summary>
        public void clear()
        {
            baseStats = new BaseStats();
            spellRange = 1f;
            magic = 5;
            vampire = 0f;
            antiInjury = 0f;
            spellRelief = 0f;
            soul = 0;
            talent = 0;
            maxAge = 0;
        }
        /// <summary>
        /// 数乘
        /// </summary>
        /// <param name="num"></param>
        /// <param name="moreStats"></param>
        /// <returns></returns>
        public static MoreStats operator *(float num, MoreStats moreStats)
        {
            MoreStats result = new MoreStats();
            result.vampire = moreStats.vampire * num;
            result.antiInjury = moreStats.antiInjury * num;
            //result.spellRange = moreStats.spellRange * num;
            result.spellRelief = moreStats.spellRelief * num;
            result.magic = (int)(moreStats.magic * num);
            result.soul = (int)(moreStats.soul * num);
            result.talent = (int)(moreStats.talent * num);
            return result;
        }
        public MoreStats()
        {
            baseStats = new BaseStats();
            spells = new List<ExtensionSpell>();
            spellRange = 1f;
            magic = 5;
            vampire = 0f;
            antiInjury = 0f;
            spellRelief = 0f;
            soul = 0;
            element = GetElementRandomly();
            talent = 0;
            maxAge = 0;
        }
    }
}
