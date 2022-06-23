using System.Collections.Generic;

namespace Cultivation_Way
{
    class DefaultSetting
    {
        public static CultivationBook fireBase;
        public static CultivationBook waterBase;
        public static CultivationBook woodBase;
        public static CultivationBook metalBase;
        public static CultivationBook earthBase;
        public static CultivationBook allBase;
        internal static void init()
        {
            fireBase = new CultivationBook(CultivationBookType.CULTIVATE, "炼气术（火）")
            {
                element = new ChineseElement(new int[] { 0, 0, 0, 100, 0 })
            }; 
            waterBase = new CultivationBook(CultivationBookType.CULTIVATE, "炼气术（水）")
            {
                element = new ChineseElement(new int[] { 0, 0, 100, 0, 0 })
            }; 
            woodBase = new CultivationBook(CultivationBookType.CULTIVATE, "炼气术（木）")
            {
                element = new ChineseElement(new int[] { 0, 100, 0, 0, 0 })
            }; 
            metalBase = new CultivationBook(CultivationBookType.CULTIVATE, "炼气术（金）")
            {
                element = new ChineseElement(new int[] { 100, 0, 0, 0, 0 })
            }; 
            earthBase = new CultivationBook(CultivationBookType.CULTIVATE, "炼气术（土）")
            {
                element = new ChineseElement(new int[] { 0, 0, 0, 0, 100 })
            }; 
            allBase = new CultivationBook(CultivationBookType.CULTIVATE, "元始炼气经")
            {
                element = new ChineseElement(new int[] { 20, 20, 20, 20, 20 })
            };
            ExtendedWorldData.instance.worldBookContainer.container.Add(DefaultSetting.fireBase);
            ExtendedWorldData.instance.worldBookContainer.container.Add(DefaultSetting.waterBase);
            ExtendedWorldData.instance.worldBookContainer.container.Add(DefaultSetting.woodBase);
            ExtendedWorldData.instance.worldBookContainer.container.Add(DefaultSetting.metalBase);
            ExtendedWorldData.instance.worldBookContainer.container.Add(DefaultSetting.earthBase);
            ExtendedWorldData.instance.worldBookContainer.container.Add(DefaultSetting.allBase);
        }
        internal static void setDefaultBooks()
        {
            
        }
    }
    internal class ExtendedWorldData
    {
        


        public float worldLevel = 0f;
        public int levelLimit = 110;
        public static ExtendedWorldData instance;
        public CultivationBookContainer worldBookContainer = new CultivationBookContainer();
        
        public List<MapChunk> chunks = new List<MapChunk>();//区块
        public List<BonusStatsManager> bonusStatsManagers = new List<BonusStatsManager>();

        public Dictionary<string, int> creatureLimit = new Dictionary<string, int>();//生物限制
        public Dictionary<string, string> godList = new Dictionary<string, string>();//神明
        public Dictionary<string, MoreData> tempMoreData = new Dictionary<string, MoreData>();//未生成单位的属性

        public Dictionary<string, Family> familys = new Dictionary<string, Family>();//家族
       
       
        public Dictionary<int, ChineseElement> chunkToElement = new Dictionary<int, ChineseElement>();//区块与元素映射词典

        public Dictionary<string, ExtendedKingdomStats> kingdomStats = new Dictionary<string, ExtendedKingdomStats>();
        public Dictionary<string, List<ExtendedActor>> kingdomBindActors = new Dictionary<string, List<ExtendedActor>>();//国家id与其绑定的生物

        public ExtendedWorldData()
        {
            instance = this;
            
        }
        public CultivationBook[] getDefaultCopies()
        {
            List<CultivationBook> simpleList = worldBookContainer.container;
            
            int count = simpleList.Count;
            CultivationBook[] res = new CultivationBook[count];
            for (int i = 0; i < count; i++)
            {
                res[i] = simpleList[i].getCopyOne();
            }
            return res;
        }
    }
}
