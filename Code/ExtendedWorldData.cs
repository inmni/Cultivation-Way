using System.Collections.Generic;

namespace Cultivation_Way
{
    internal class ExtendedWorldData
    {
        public static ExtendedWorldData instance;
        public float worldLevel = 0f;
        public int levelLimit = 110;

        public List<BonusStatsManager> bonusStatsManagers = new List<BonusStatsManager>();

        public Dictionary<string, int> creatureLimit = new Dictionary<string, int>();//生物限制
        public Dictionary<string, string> godList = new Dictionary<string, string>();//神明
        public Dictionary<string, MoreData> tempMoreData = new Dictionary<string, MoreData>();//未生成单位的属性

        public Dictionary<string, Family> familys = new Dictionary<string, Family>();//家族

        public List<MapChunk> chunks = new List<MapChunk>();//方便获取区块
        public Dictionary<int, ChineseElement> chunkToElement = new Dictionary<int, ChineseElement>();//区块与元素映射词典

        public Dictionary<string, ExtendedKingdomStats> kingdomStats = new Dictionary<string, ExtendedKingdomStats>();
        public Dictionary<string, List<ExtendedActor>> kingdomBindActors = new Dictionary<string, List<ExtendedActor>>();//国家id与其绑定的生物

        public ExtendedWorldData()
        {
            instance = this;
        }
    }
}
