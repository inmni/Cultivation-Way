using System.Collections.Generic;
namespace Cultivation_Way
{
    internal class ExtendedActor : Actor
    {
        public MoreStats extendedCurStats = new MoreStats();
        public MoreData extendedData = new MoreData();
        public List<BaseSimObject> compositions = new List<BaseSimObject>();
        public ActorStatus easyData;
        public BaseStats easyCurStats;
    }
}
