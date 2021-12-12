using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    [Serializable]
    public delegate bool SpellAction(BaseSimObject pUser=null, BaseSimObject pTarget = null, WorldTile pTile=null,float pRad = 0f,int limit = 1000);
}
