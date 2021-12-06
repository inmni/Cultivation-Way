using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
        public string[] spells;     //法术
		public float spellRange;	//法术距离
		public int magic;			//蓝量
		public float vampire;       //吸血
		public float antiInjury;	//反伤
		public float spellRelief;   //法伤减免
		public string cultisystem;  //修炼体系
		public int talent;			//天赋


		public MoreStats(BaseSimObject pObject)
        {
			spells = new string[] { };
			spellRange = 5f;
			magic = 10;
			vampire = 0f;
			antiInjury = 0f;
			spellRelief = 0f;
			cultisystem = "";
			talent = 0;

            if (pObject.objectType == MapObjectType.Actor)
            {
				talent = 1;
				cultisystem = "Xian";
            }
        }
    }
}
