using System.Collections.Generic;

namespace Cultivation_Way
{
    internal class CultivationBookContainer
    {
        internal List<CultivationBook> container = new List<CultivationBook>();
        internal CultivationBookType containerType = CultivationBookType.NONE;
        internal int maxStoreCount = 30;
        /// <summary>
        /// 获取随机一本匹配类型的功法
        /// </summary>
        /// <param name="pActor"></param>
        /// <param name="requireType"></param>
        /// <returns></returns>
        public CultivationBook getRandomOne(ExtendedActor pActor, CultivationBookType requireType = CultivationBookType.NONE)
        {
            List<CultivationBook> possibleBooks = new List<CultivationBook>();
            getPossibleBooks(pActor, requireType, possibleBooks);
            if (possibleBooks.Count == 0)
            {
                return null;
            }
            return possibleBooks.GetRandom();
        }
        /// <summary>
        /// 获取最合适一本匹配类型的功法
        /// </summary>
        /// <param name="pActor"></param>
        /// <param name="requireType"></param>
        /// <returns></returns>
        public CultivationBook getBestOne(ExtendedActorStatus pStatus, CultivationBookType requireType = CultivationBookType.NONE)
        {
            CultivationBook bestOne = null;
            int count = container.Count;
            float bestMatchDegree = 10000000f;
            float temp;

            for (int i = 0; i < count; i++)
            {
                if ((requireType == CultivationBookType.NONE || containerType == CultivationBookType.NONE || container[i].BookType == requireType)
                        && container[i].allowActor(pStatus))
                {
                    temp = container[i].getMatchDegree(pStatus);
                    if (temp < bestMatchDegree)
                    {
                        bestMatchDegree = temp;
                        bestOne = container[i];
                    }
                }
            }
            return bestOne;
        }
        public int storeNewOne(CultivationBook book)
        {
            if (containerType == CultivationBookType.NONE || book.BookType == containerType)
            {
                int count = container.Count;
                if (count >= maxStoreCount)
                {
                    List<CultivationBook> possibleBooks = new List<CultivationBook>();
                    for (int i = 0; i < count; i++)
                    {
                        if (book.rank > container[i].rank)
                        {
                            possibleBooks.Add(container[i]);
                        }
                    }
                    int minDistance=999999;
                    int temp;
                    CultivationBook toReplace=null;
                    foreach(CultivationBook book1 in possibleBooks)
                    {
                        temp = ChineseElement.getMatchDegree(book.element, book1.element,true);
                        if (temp < minDistance)
                        {
                            minDistance = temp;
                            toReplace = book1;
                        }
                    }
                    int res = -1;
                    if (toReplace != null)
                    {
                        res = container.IndexOf(toReplace);
                        container[res].setContent(book);
                    }
                    return res;
                }
                for (int i = 0; i < count; i++)
                {
                    if (book.isSimilar(container[i]))
                    {
                        if (book.rank > container[i].rank)
                        {
                            container[i].setContent(book);
                            return i;
                        }
                        else
                        {
                            return -1;
                        }
                    }

                }
                container.Add(book);
                return container.Count-1;
            }
            return -1;
        }
        private void getPossibleBooks(ExtendedActor pActor, CultivationBookType requireType, List<CultivationBook> possibleBooks)
        {
            int count = container.Count;
            for (int i = 0; i < count; i++)
            {
                if ((requireType == CultivationBookType.NONE || containerType == CultivationBookType.NONE || container[i].BookType == requireType)
                    && container[i].allowActor(pActor))
                {
                    possibleBooks.Add(container[i]);
                }
            }
        }
        
        public CultivationBookContainer(CultivationBookType containerType = CultivationBookType.NONE)
        {
            this.containerType = containerType;
        }
    }
}
