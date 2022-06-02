using System.Collections.Generic;

namespace Cultivation_Way
{
    class MoreStatus
    {
        /// <summary>
        /// 修炼体系
        /// </summary>
        public string cultisystem = "default";
        /// <summary>
        /// 家族id
        /// </summary>
        public string familyID = "甲";
        /// <summary>
        /// 姓氏
        /// </summary>
        public string familyName = "甲";
        /// <summary>
        /// 体质id
        /// </summary>
        public string specialBody = "FT";
        /// <summary>
        /// 当前蓝量
        /// </summary>
        public int magic = 0;
        /// <summary>
        /// 是否可以修炼
        /// </summary>
        public bool canCultivate = true;
        /// <summary>
        /// buff效果
        /// </summary>
        public MoreStats bonusStats = new MoreStats();
        /// <summary>
        /// 复合设定
        /// </summary>
        public CompositionSetting compositionSetting = new CompositionSetting();

        //以下两项仅在存读档时使用
        /// <summary>
        /// 复合对象
        /// </summary>
        public string[] compositionsID;
        /// <summary>
        /// 元素
        /// </summary>
        public int[] element = { 0, 0, 0, 0, 0 };
        

    }
}
