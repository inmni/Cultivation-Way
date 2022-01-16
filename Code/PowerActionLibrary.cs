using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CultivationWay;
namespace Cultivation_Way
{
    class PowerActionLibrary
    {
        public static bool inspectChunk(WorldTile pTile=null,string pPower = null)
        {
            if (pTile != null)
            {
                WindowChunkInfo.open(pTile.chunk);
            }
            return true;
        }
    }
}
