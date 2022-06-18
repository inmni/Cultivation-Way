namespace Cultivation_Way
{
    internal class MoreStatus
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
        /// 剩余护盾
        /// </summary>
        public int leftShied = 0;
        /// <summary>
        /// 是否可以修炼
        /// </summary>
        public bool canCultivate = false;
        /// <summary>
        /// 灵根
        /// </summary>
        public ChineseElement chineseElement = new ChineseElement();
        /// <summary>
        /// 额外加成
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
