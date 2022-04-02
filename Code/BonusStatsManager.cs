using CultivationWay;

namespace Cultivation_Way
{
    class BonusStatsManager
    {
        internal Actor actor;
        internal MoreStats bonusStats;
        internal MoreStats punishStats;
        //时间按年来算
        internal int leftTime;
        internal int punishTime;
        public BonusStatsManager(Actor actor, MoreStats bonusStats, int leftTime, MoreStats punishStats = null, int punishTime = 0)
        {
            this.actor = actor;
            this.bonusStats = bonusStats;
            this.punishStats = punishStats;
            this.leftTime = leftTime;
            this.punishTime = punishTime;
            Main.instance.bonusStatsManagers.Add(this);
        }
        public void update()
        {
            leftTime--;
            if (leftTime <= 0)
            {
                end();
            }
        }
        public void end()
        {
            if (actor != null)
            {
                Main.instance.actorToMoreData[actor.GetData().actorID].bonusStats.deleteAnotherStats(bonusStats);
                actor.setStatsDirty();

                if (punishStats != null)
                {
                    new BonusStatsManager(actor, punishStats, punishTime);
                }
            }
        }
    }
}
