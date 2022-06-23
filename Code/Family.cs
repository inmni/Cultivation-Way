using System.Collections.Generic;

namespace Cultivation_Way
{
    internal class Family
    {
        /// <summary>
        /// 家族名
        /// </summary>
        public string id;
        /// <summary>
        /// 家族功法阁
        /// </summary>
        public CultivationBookContainer[] lofts;
        public CultivationBook getbook(ExtendedActor pActor,CultivationBookType bookType = CultivationBookType.NONE)
        {
            return getbook(pActor.extendedData.status, bookType);
        }
        public CultivationBook getbook(ExtendedActorStatus pStatus, CultivationBookType bookType = CultivationBookType.NONE)
        {
            switch (bookType)
            {
                case CultivationBookType.NONE:
                    return lofts[0].getBestOne(pStatus, CultivationBookType.NONE);
                case CultivationBookType.CULTIVATE:
                    return lofts[1].getBestOne(pStatus, CultivationBookType.CULTIVATE);
                case CultivationBookType.ATTACK:
                    return lofts[2].getBestOne(pStatus, CultivationBookType.ATTACK);
                case CultivationBookType.DEFEND:
                    return lofts[3].getBestOne(pStatus, CultivationBookType.DEFEND);
                case CultivationBookType.MOVE:
                    return lofts[4].getBestOne(pStatus, CultivationBookType.MOVE);
                case CultivationBookType.OTHER:
                    return lofts[5].getBestOne(pStatus, CultivationBookType.OTHER);
                default:
                    return null;
            }
        }
        private void addCultibook(CultivationBook book)
        {
            int i = (int)book.BookType;
            lofts[i].container.Add(book);
        }
        /// <summary>
        /// 将book存入，若与已有相似，则设置已有功法内容为book（未修改引用），否则添加book
        /// </summary>
        /// <param name="book"></param>
        public int storeCultibook(CultivationBook book)
        {
            int i = (int)book.BookType;
            return lofts[i].storeNewOne(book);
        }
        public Family(string id="甲")
        {
            this.id = id;
            lofts = new CultivationBookContainer[6];
            lofts[0] = new CultivationBookContainer(CultivationBookType.NONE);
            lofts[1] = new CultivationBookContainer(CultivationBookType.CULTIVATE);
            lofts[2] = new CultivationBookContainer(CultivationBookType.ATTACK);
            lofts[3] = new CultivationBookContainer(CultivationBookType.DEFEND);
            lofts[4] = new CultivationBookContainer(CultivationBookType.MOVE);
            lofts[5] = new CultivationBookContainer(CultivationBookType.OTHER);
            CultivationBook[] copies = ExtendedWorldData.instance.getDefaultCopies();
            int count = copies.Length;
            for(int i = 0; i < count; i++)
            {
                this.addCultibook(copies[i]);
            }

        }
    }
}
