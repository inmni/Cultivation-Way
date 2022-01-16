using CultivationWay;
using System;
using UnityEngine;

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
            baseElementContainer = new int[5] {20,20,20,20,20};
            for (int i = 0; i < 5; i++)
            {
                baseElementContainer[i] = Toolbox.randomInt(0, 101);
            }
            normalize();
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
            element = elementLibrary.get(maxMembership.Item1);
        }
        //计算元素不纯净度
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
            return 100f / baseElementContainer[maxPos];
        }
        //修改至总量为100
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

        public ChineseElement(int[] content)
        {
            baseElementContainer = content;
            setType();
        }
        public ChineseElement() {
            getRandom();
        }
    }
}