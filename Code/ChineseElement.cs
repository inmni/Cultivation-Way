using System;

namespace Cultivation_Way
{
    class ChineseElement
    {
        /// <summary>
        /// 五大元素含量，顺序为金木水火土
        /// </summary>
        public int[] baseElementContainer;
        /// <summary>
        /// 对应的id
        /// </summary>
        public string id;
        //元素变化
        public void change() { }
        /// <summary>
        /// 随机设置元素，并自动判断类型
        /// </summary>
        /// <returns></returns>
        public ChineseElement getRandom()
        {
            baseElementContainer = new int[5] { 20, 20, 20, 20, 20 };
            for (int i = 0; i < 5; i++)
            {
                baseElementContainer[i] = Toolbox.randomInt(0, 101);
            }
            normalize();
            setType();
            return this;
        }
        /// <summary>
        /// 判断类型
        /// </summary>
        public void setType()
        {
            bool isAll = true;
            for(int i = 0; i < 5; i++)
            {
                if (baseElementContainer[i] != 20)
                {
                    isAll = false;
                    break;
                }
            }
            
            ChineseElementLibrary elementLibrary = AddAssetManager.chineseElementLibrary;
            if (isAll)
            {
                ChineseElementAsset allElement = elementLibrary.get("AllElement");
                id = allElement.id;
            }
            Tuple<string, int> maxMembership = new Tuple<string, int>("All", 100000);


            foreach (string id in elementLibrary.dict.Keys)
            {
                //rarity作为量度，rarity越大，要求的membership越小
                int membership = 1;
                for (int i = 0; i < 5; i++)
                {
                    membership *= Math.Abs(baseElementContainer[i] - elementLibrary.dict[id].content[i]) + 1;
                }

                if (membership < maxMembership.Item2&&id!= "AllElement")
                {
                    maxMembership = new Tuple<string, int>(id, membership);
                }
            }
            ChineseElementAsset element = elementLibrary.get(maxMembership.Item1);
            id = element.id;
        }
        /// <summary>
        /// 返回不纯净度
        /// </summary>
        /// <returns></returns>
        public float getImPurity()
        {
            int maxPos = 0;
            for (int i = 1; i < 5; i++)
            {
                if (baseElementContainer[i] > baseElementContainer[maxPos])
                {
                    maxPos = i;
                }
            }
            return 100f / baseElementContainer[maxPos];
        }
        /// <summary>
        /// 规格化，调整总和为100
        /// </summary>
        public void normalize()
        {
            int sum = 0;
            for (int i = 0; i < 5; i++)
            {
                sum += baseElementContainer[i];
            }
            for (int i = 0; i < 5; i++)
            {
                baseElementContainer[i] = baseElementContainer[i] * 100 / sum;
            }
            sum = 0;
            for (int i = 0; i < 5; i++)
            {
                sum += baseElementContainer[i];
            }
            while (sum < 100)
            {
                int index = Toolbox.randomInt(0, 5);
                baseElementContainer[index]++;
                sum++;
            }
        }
        /// <summary>
        /// 获取对应的Asset
        /// </summary>
        /// <returns></returns>
        public ChineseElementAsset GetAsset()
        {
            return AddAssetManager.chineseElementLibrary.get(id);
        }
        /// <summary>
        /// 根据数组确定元素，顺序为金木水火土
        /// </summary>
        /// <param name="content"></param>
        public ChineseElement(int[] content)
        {
            baseElementContainer = content;
            normalize();
            setType();
        }
        /// <summary>
        /// 设定随机元素
        /// </summary>
        public ChineseElement()
        {
            getRandom();
        }

        public static bool isMatch(ChineseElement e1,ChineseElement e2)        
        {
            return true;
        }
    }
}