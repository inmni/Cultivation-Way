namespace Cultivation_Way
{
    class Family
    {
        /// <summary>
        /// 家族名
        /// </summary>
        public string id;//家族姓氏
        /// <summary>
        /// 家族人数
        /// </summary>
        public int num;
        /// <summary>
        /// 家族内最大等级
        /// </summary>
        public int maxLevel;//家族最大等级
        /// <summary>
        /// 家族历史最强者名字
        /// </summary>
        public string honorary;//家族历史最强者名字
        /// <summary>
        /// 家族功法
        /// </summary>
        public CultivationBook cultivationBook;//家族功法

        public void levelUp(string name)
        {
            honorary = name;
            maxLevel++;
            if (Toolbox.randomChance(maxLevel / 110f))
            {
                cultivationBook.levelUp();
            }
        }
        public Family(string id)
        {
            this.id = id;
            maxLevel = 0;
            num = 0;
            honorary = "";
            cultivationBook = new CultivationBook();
        }
        public Family()
        {
            id = "甲";
            maxLevel = 0;
            num = 0;
            honorary = "";
            cultivationBook = new CultivationBook();
        }
    }
}
