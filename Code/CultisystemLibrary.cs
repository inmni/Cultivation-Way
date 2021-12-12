namespace Cultivation_Way
{
    class CultisystemLibrary : AssetLibrary<CultisystemAsset>
    {
        public override void init()
        {
            base.init();
            this.add(new CultisystemAsset
            {
                id = "normal",
                name = "仙路",
                realms = new string[20] { "练气境","筑基境","金丹境","元婴境","化神境","炼虚境","合体境","大乘境","渡劫境","登仙境",
                                        "地仙","真仙","天仙","玄仙","太乙金仙","大罗金仙","混元大罗金仙","仙王","仙尊","仙帝"},
                moreStats = new MoreStats[20]
            });
            for (int i = 0; i < 20; i++)
            {
                this.t.moreStats[i] = new MoreStats(new BaseSimObject());
                this.t.moreStats[i].baseStats.knockbackReduction = (i / 10) * 5;
                this.t.moreStats[i].setBasicStats(50 * (i + 1), 5 * (i + 1), (int)(0.1 * (i + 1)), i + 1);
                this.t.moreStats[i].setSpecialStats(150 * (i + 1), 2 * (i + 1), 0);
            }
            this.add(new CultisystemAsset
            {
                id = "default",
                name = "仙路",
                realms = new string[20] { "练气境","筑基境","金丹境","元婴境","化神境","炼虚境","合体境","大乘境","渡劫境","登仙境",
                                        "地仙","真仙","天仙","玄仙","太乙金仙","大罗金仙","混元大罗金仙","仙王","仙尊","仙帝"},
                moreStats = new MoreStats[20]
            });
            for (int i = 0; i < 20; i++)
            {
                this.t.moreStats[i] = new MoreStats(new BaseSimObject());
                this.t.moreStats[i].baseStats.knockbackReduction = (i / 10) * 5;
                this.t.moreStats[i].setBasicStats(50 * (i + 1), 5 * (i + 1), (int)(0.1 * (i + 1)), i + 1);
                this.t.moreStats[i].setSpecialStats(150 * (i + 1), 2 * (i + 1), 0);
            }

            this.add(new CultisystemAsset
            {
                id = "bodying",
                name = "锻体",
                realms = new string[20] { "固元期","凝血期","淬骨期","洗髓期","通脉境","易经期","磐石","金身境","超凡","千山境",
                                        "万象","神力境","神勇境","天人","涅槃","纳界","破虚","开天","灵城","帝疆"},
                moreStats = new MoreStats[20]
            });
            for (int i = 0; i < 20; i++)
            {
                this.t.moreStats[i] = new MoreStats(new BaseSimObject());
                this.t.moreStats[i].baseStats.knockbackReduction = (i / 10)*9;
                this.t.moreStats[i].setBasicStats(20 * (i + 1) * (i + 1), 2*(i + 1), (int)(0.05 * (i + 1)), 3 * (i + 1));
                this.t.moreStats[i].setSpecialStats(80 * (i + 1) * (i + 1), i, 0);
            }
        }
    }
}
