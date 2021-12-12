using System.Collections.Generic;

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
                stats[i].setBasicStats(i*rank,rank,0,rank/20,i);
            }
            bookName = bookName.Remove(bookName.Length - 4, 4) + "("+ChineseNameAsset.rankName[(rank + 9) / 10] + "阶)";
        }
        public CultivationBook()
        {
            bookName = NameGenerator.getName("book_name")+"(凡阶)";

            rank = 1;

            stats =new MoreStats[20];

            for(int i = 0; i < 20; i++)
            {
                stats[i] = new MoreStats(new BaseSimObject());
            }
        }
    }
}
