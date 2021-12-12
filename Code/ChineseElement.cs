using CultivationWay;
using System;

namespace Cultivation_Way
{
    class ChineseElement
    {
        //该元素的基本元素含量
        public int[] baseElementContainer;
        //衍生元素类型
        public ChineseElementAsset element;

        //元素变化
        public void change() { }
        //随机设置元素
        public ChineseElement getRandom()
        {
            int sum = 0;
            int[] content = new int[5] { 0, 0, 0, 0, 0 };
            for (int i = 0; i < 5; i++)
            {
                content[i] = Toolbox.randomInt(0, 101);
                sum += content[i];
            }
            for (int i = 0; i < 5; i++)
            {
                content[i] = content[i] * 100 / sum;
            }
            baseElementContainer = content;
            setType();
            return this;
        }
        //确定元素衍生类型
        public void setType()
        {

            ChineseElementLibrary elementLibrary = (ChineseElementLibrary)AssetManager.instance.dict["element"];

            Tuple<string, int> maxMembership = new Tuple<string, int>("All", 100000);


            foreach (string id in elementLibrary.dict.Keys)
            {
                int membership = 1;
                for (int i = 0; i < 5; i++)
                {
                    membership *= Math.Abs(baseElementContainer[i] - elementLibrary.dict[id].content[i]) + 1;
                }

                if (membership < maxMembership.Item2)
                {
                    maxMembership = new Tuple<string, int>(id, membership);
                }
            }
            element = elementLibrary.dict[maxMembership.Item1];
            
        }
        //计算元素纯净度
        public float getPurity()
        {
            int maxPos = 0;
            for (int i = 1; i < 5; i++)
            {
                if (baseElementContainer[i] > baseElementContainer[maxPos])
                {
                    maxPos = i;
                }
            }
            float purity = 0;
            for (int i = 0; i < 5; i++)
            {
                purity += baseElementContainer[i] / (float)baseElementContainer[maxPos];
            }
            return purity;
        }

        public ChineseElement(int[] content)
        {
            baseElementContainer = content;
            setType();
        }
        public ChineseElement() { }
    }
}