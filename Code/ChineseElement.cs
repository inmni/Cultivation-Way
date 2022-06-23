using System;
using UnityEngine;

namespace Cultivation_Way
{
    internal class ChineseElement
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
            for (int i = 0; i < 5; i++)
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
                id = "AllElement";
                return;
            }
            id = "All";
            int maxDistance = 0;
            int corrRarity = 1;
            int distance;
            for (int j = 0; j < elementLibrary.list.Count; j++)
            {
                //rarity作为量度，rarity越大，要求的membership越小
                distance=0;
                for (int i = 0; i < 5; i++)
                {
                    distance += baseElementContainer[i] * elementLibrary.list[j].content[i];
                }
                if ((distance == maxDistance && elementLibrary.list[j].rarity>corrRarity)
                    ||(distance/ elementLibrary.list[j].rarity> maxDistance/corrRarity))
                {
                    id = elementLibrary.list[j].id;
                    maxDistance = distance;
                    corrRarity = elementLibrary.list[j].rarity;
                }
            }
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
        /// 偏移
        /// </summary>
        /// <param name="content"></param>
        public void deflectTo(int[] content,float scale = 0.3f)
        {
            for(int i = 0; i < 5; i++)
            {
                baseElementContainer[i] -= (int)((baseElementContainer[i]-content[i])*scale);
            }
            normalize();
        }
        public void setContent(int[] content)
        {
            for (int i = 0; i < 5; i++)
            {
                baseElementContainer[i] = content[i];
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
            int length = content.Length;
            baseElementContainer = new int[length];
            for(int i = 0; i < length; i++)
            {
                baseElementContainer[i] = content[i];
            }
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
        public static int getMatchDegree(ChineseElement e1, ChineseElement e2,bool mutiply = false)
        {
            int res = 0;
            if (mutiply) 
            {
                for (int i = 0; i < 5; i++)
                {
                    res += (e1.baseElementContainer[i] - e2.baseElementContainer[i])* (e1.baseElementContainer[i] - e2.baseElementContainer[i]);
                }
                return res;
            }
            for (int i = 0; i < 5; i++)
            {
                res += Mathf.Abs(e1.baseElementContainer[i] - e2.baseElementContainer[i]);
            }
            return res;
        }
        public static int getMatchDegree(int[] e1, int[] e2, bool mutiply = false)
        {
            int res = 0;
            if (mutiply)
            {
                for (int i = 0; i < 5; i++)
                {
                    res += (e1[i] - e2[i]) * (e1[i] - e2[i]);
                }
                return res;
            }
            for (int i = 0; i < 5; i++)
            {
                res += Mathf.Abs(e1[i] - e2[i]);
            }
            return res;
        }
        public static bool isMatch(ChineseElement e1, ChineseElement e2)
        {
            return getMatchDegree(e1, e2,true) <=8000 ;
        }
    }
}