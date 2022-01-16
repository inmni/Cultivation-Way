using System.Collections.Generic;

namespace Cultivation_Way
{
    class MoreActorData
    {
        public ChineseElement element;          //元素

        public string cultisystem;              //修炼体系

        public string familyID;                 //家族姓氏

        public string specialBody;              //体质

        public Dictionary<string,int> coolDown;              //冷却

        public int magic;                       //蓝量

        public MoreStats bonusStats;            //buff作用
    }
}
