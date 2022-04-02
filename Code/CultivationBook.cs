namespace Cultivation_Way
{
    class CultivationBook
    {

        public string bookName;

        public int rank;//品级，10级一阶

        public MoreStats[] stats;//各个境界加成，包括法术

        public void levelUp()
        {
            rank++;
            for (int i = 0; i < 20; i++)
            {
                stats[i].setBasicStats(i * rank, rank, 0, rank / 20, i);
            }
            if (rank % 10 == 0 || Toolbox.randomChance(1 - rank / 110f))
            {
                ExtensionSpell gainSpell = new ExtensionSpell(((ExtensionSpellLibrary)AssetManager.instance.dict["extensionSpell"]).spellList.GetRandom());
                bool isRepeated = false;
                foreach (ExtensionSpell spell in stats[19].spells)
                {
                    if (spell.spellAssetID == gainSpell.spellAssetID)
                    {
                        spell.might += gainSpell.might / 100f;
                        isRepeated = true;
                    }
                }
                //各个境界的法术待设置
                //目前采用统一处理
                for (int i = gainSpell.GetSpellAsset().type.requiredLevel-1; i < 20 && !isRepeated; i++)
                {
                    stats[i].spells.Add(gainSpell);
                }
            }
            bookName = bookName.Remove(bookName.Length - 4, 4) + "(" + ChineseNameAsset.rankName[(rank + 9) / 10] + "阶)";
        }
        public CultivationBook()
        {
            bookName = ChineseNameGenerator.getName("book_name") + "(凡阶)";

            rank = 1;

            stats = new MoreStats[20];

            for (int i = 0; i < 20; i++)
            {
                stats[i] = new MoreStats();
            }
        }
    }
}
