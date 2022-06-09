using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    class CultivationBookContainer:ObjectContainer<CultivationBook>
    {
        
        /// <summary>
        /// 获取随机一本匹配类型的功法
        /// </summary>
        /// <param name="pActor"></param>
        /// <param name="requireType"></param>
        /// <returns></returns>
        public CultivationBook getRandomOne(ExtendedActor pActor,CultivationBookType requireType=CultivationBookType.NONE)
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
        public CultivationBook getBestOne(ExtendedActor pActor, CultivationBookType requireType = CultivationBookType.NONE)
        {
            CultivationBook bestOne = null;
            float bestMatchDegree = 0f;
            float temp;
            using (IEnumerator<CultivationBook> enumerator = this.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if ((requireType == CultivationBookType.NONE || enumerator.Current.BookType == requireType)
                        && enumerator.Current.allowActor(pActor))
                    {
                        temp = enumerator.Current.getMatchDegree(pActor);
                        if (temp > bestMatchDegree)
                        {
                            bestMatchDegree = temp;
                            bestOne = enumerator.Current;
                        }
                    }
                }
            }
            return bestOne;
        }

        private void getPossibleBooks(ExtendedActor pActor, CultivationBookType requireType, List<CultivationBook> possibleBooks)
        {
            using (IEnumerator<CultivationBook> enumerator = this.GetEnumerator())
            {
                while (enumerator.MoveNext())
                {
                    if ((requireType == CultivationBookType.NONE || enumerator.Current.BookType == requireType)
                        && enumerator.Current.allowActor(pActor))
                    {
                        possibleBooks.Add(enumerator.Current);
                    }
                }
            }
        }
    }
}
