namespace Cultivation_Way
{
    class ChineseElementAsset : Asset
    {
        /*
         * 组成灵根，
         * 确定衍生元素
         * 
         */
        public int[] content;//各个基本元素的含量

        public int rarity;//稀有度

        public string[] oppositeElementId;//克制元素的id

        public string[] promoteElementId;//加成元素的id

        public string name;//直接用于显示的名字

        public ChineseElementAsset()
        {
        }
    }
}
