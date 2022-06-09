using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CultivationWay;
namespace Cultivation_Way
{
    class ExtendedKingdomStats
    {
        internal string kingdom_id;

        internal KingdomStats curStatus;
        public ExtendedKingdomStats(string kingdomID, KingdomStats loadStats = null)
        {
            kingdom_id = kingdomID;
            if (loadStats == null)
            {
                this.curStatus = new KingdomStats();
            }
            else
            {
                this.curStatus = loadStats;
            }
        }
        public Kingdom getKingdom()
        {
            return MapBox.instance.kingdoms.getKingdomByID(this.kingdom_id);
        }
        public static float getStatus(string kingdomID,string id,float defaultVal = 0f)
        {
            ExtendedKingdomStats stats;
            if(ExtendedWorldData.instance.kingdomStats.TryGetValue(kingdomID,out stats))
            {
                return stats.getStatus(id, defaultVal);
            }
            ExtendedWorldData.instance.kingdomStats[kingdomID] = new ExtendedKingdomStats(kingdomID);
            return defaultVal;
        }
        public void setStatus(string id, float val)
        {
            KingdomStatVal statVal = new KingdomStatVal(id);
            statVal.value = val;
            curStatus.dict[id] = statVal;
        }
        public float getStatus(string id,float defaultVal=0f)
        {
            KingdomStatVal val;
            if (curStatus.dict.TryGetValue(id, out val))
            {
                return val.value;
            }
            else
            {
                return defaultVal;
            }
        }
    }
}
