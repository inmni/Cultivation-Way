namespace Cultivation_Way
{
    internal enum CultivationBookType
    {
        NONE,
        GONG,
        JUE,
        SHU,
        FA
    }

    internal class CultivationBook
    {

        public string bookName;//名字

        public float[] cultiVelco;//各个境界修炼速度影响因子

        public int rank;//品级，10级一阶

        private uint allowedCultiSystem;//允许修炼的修炼体系（允许多体系

        private CultivationBookType bookType;//功法类型

        public ExtensionSpell[] spells;//法术（内置法术属性

        public MoreStats[] stats;//各个境界加成

        public ChineseElement element;//功法属性

        internal CultivationBookType BookType { get => bookType; }
        internal bool allowCultiSystem(uint cultiSystemID)
        {
            return (allowedCultiSystem & cultiSystemID) > 0;
        }
        internal bool allowActor(ExtendedActor pActor)
        {
            return allowCultiSystem(AddAssetManager.cultisystemLibrary.get(pActor.extendedData.status.cultisystem).flag)
                && ChineseElement.isMatch(pActor.extendedData.status.chineseElement, element);
        }
        internal float getMatchDegree(ExtendedActor pActor)
        {
            return 0f;
        }
        public void levelUp()
        {
            rank++;
            bookName = bookName.Remove(bookName.Length - 4, 4) + "(" + ChineseNameAsset.rankName[(rank + 9) / 10] + "阶)";

            if (rank % 10 == 0 || Toolbox.randomChance(1 - rank / 110f))
            {
                string gainSpellID = ((ExtensionSpellLibrary)AssetManager.instance.dict["extensionSpell"]).spellList.GetRandom();
                for (int i = 0; i < spells.Length; i++)
                {
                    if (spells[i] == null)
                    {
                        continue;
                    }
                    if (spells[i].spellAssetID == gainSpellID)
                    {
                        spells[i].might *= 1.2f;
                        return;
                    }
                }
                ExtensionSpell gainSpell = new ExtensionSpell(gainSpellID);
                for (int i = 0; i < spells.Length; i++)
                {
                    if (spells[i] == null)
                    {
                        spells[i] = gainSpell;
                        return;
                    }
                }
            }

        }
        public CultivationBook()
        {
            bookName = ChineseNameGenerator.getName("book_name") + "(凡阶)";

            rank = 1;

            spells = new ExtensionSpell[10];

            stats = new MoreStats[20];



            for (int i = 0; i < 20; i++)
            {
                stats[i] = new MoreStats();
            }
            stats[0].setBasicStats(0, 0, 0, 0, 0);
            for (int i = 1; i < 20; i++)
            {
                stats[i].setBasicStats(i * rank, rank, 0, rank / 20, i);
                stats[i].addAnotherStats(stats[i - 1]);
            }
        }
    }
}
