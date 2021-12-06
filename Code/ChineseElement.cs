using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cultivation_Way
{
    class ChineseElement
    {
        //该
        public int[] baseElementContainer;

        public ChineseElementAsset element;

        //元素变化
        public void change() { }

        //确定元素衍生类型
        public void setType(int gold, int wood, int water, int fire, int ground) {
            /*
             * 进行一定的计算
             * 
             */
            element = ((ChineseElementLibrary)AssetManager.instance.dict["element"]).dict["待添入"];
        }

        public ChineseElement(int gold, int wood, int water, int fire, int ground)
        {
            setType(gold,wood,water,fire,ground);
            baseElementContainer = new int[] { gold, wood, water, fire, ground };
        }
    }
}
