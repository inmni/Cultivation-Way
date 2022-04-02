using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    public enum SpecialActorState
    {
        Stop = 0,//停止
        Move = 1,//移动
        Attack = 2,//短程攻击
        Spell = 3,//远程施法
        Death = 4
    }
}
