using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    class ExtensionSpellType
    {
        public bool active = true;//是否为主动
        public bool reusable = true;//可否重复使用
        public bool byChance = false;//是否概率触发
        public bool action = true;//是否作用于单位
        public bool remote =true;//是否为远程
        public bool range =false;//是否为范围
        public bool isDirective = true;//具有释放方向
        public void set(bool active,bool reusable,bool byChance,bool action,bool remote,bool range,bool isDirective)
        {
            this.active = active;
            this.reusable = reusable;
            this.byChance = byChance;
            this.active = action;
            this.remote = remote;
            this.range = range;
            this.isDirective = isDirective;
        }
        public ExtensionSpellType()
        {
        }

    }
}
