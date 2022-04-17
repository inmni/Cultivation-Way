using System.Collections.Generic;

namespace Cultivation_Way
{
    class MoreActorData
    {
        public ChineseElement element = new ChineseElement();          //元素
        /// <summary>
        /// 修炼体系
        /// </summary>
        public string cultisystem = "default";              //修炼体系
        /// <summary>
        /// 家族id
        /// </summary>
        public string familyID = "甲";                 //家族id
        /// <summary>
        /// 姓氏
        /// </summary>
        public string familyName = "甲";               //姓氏
        /// <summary>
        /// 体质id
        /// </summary>
        public string specialBody = "FT";              //体质
        /// <summary>
        /// 法术冷却
        /// </summary>
        public Dictionary<string, int> coolDown = new Dictionary<string, int>();              //冷却
        /// <summary>
        /// 当前蓝量
        /// </summary>
        public int magic = 0;                       //蓝量
        /// <summary>
        /// buff效果
        /// </summary>
        public MoreStats bonusStats;            //buff作用
        /// <summary>
        /// 是否可以修炼
        /// </summary>
        public bool canCultivate = true;               //可修炼
        public MoreStats currStats = new MoreStats();
    }
}
