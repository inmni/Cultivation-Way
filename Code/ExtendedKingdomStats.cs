namespace Cultivation_Way
{
    internal class ExtendedKingdomStats
    {
        internal string kingdom_id;

        internal KingdomStats curStatus;
        public ExtendedKingdomStats(string kingdomID, KingdomStats loadStats = null)
        {
            kingdom_id = kingdomID;
            if (loadStats == null)
            {
                curStatus = new KingdomStats();
            }
            else
            {
                curStatus = loadStats;
            }
        }
        public Kingdom getKingdom()
        {
            return MapBox.instance.kingdoms.getKingdomByID(kingdom_id);
        }
        public static float getStatus(string kingdomID, string id, float defaultVal = 0f)
        {
            ExtendedKingdomStats stats;
            if (ExtendedWorldData.instance.kingdomStats.TryGetValue(kingdomID, out stats))
            {
                return stats.getStatus(id, defaultVal);
            }
            ExtendedWorldData.instance.kingdomStats[kingdomID] = new ExtendedKingdomStats(kingdomID);
            return defaultVal;
        }
        public static void setStatus(string kingdomID, string id, float val)
        {
            ExtendedKingdomStats stats;
            if (ExtendedWorldData.instance.kingdomStats.TryGetValue(kingdomID, out stats))
            {
                stats.setStatus(id, val);
                return;
            }
            stats = new ExtendedKingdomStats(kingdomID);
            stats.setStatus(id, val);
            ExtendedWorldData.instance.kingdomStats[kingdomID] = stats;
            return;
        }
        public void setStatus(string id, float val)
        {
            KingdomStatVal statVal = new KingdomStatVal(id);
            statVal.value = val;
            curStatus.dict[id] = statVal;
        }
        public float getStatus(string id, float defaultVal = 0f)
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
