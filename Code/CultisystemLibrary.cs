using ReflectionUtility;
using HarmonyLib;
namespace Cultivation_Way
{
    class CultisystemLibrary : AssetLibrary<CultisystemAsset>
    {
        public override void init()
        {
            base.init();
            this.add(new CultisystemAsset
            {
                id = "normal",
                name = "仙路",
                bannedRace = new string[] { "orc"},
                moreStats = new MoreStats[20]
            });
            this.t.moreStats[0] = new MoreStats();
            this.t.moreStats[0].setBasicStats(0, 0, 0, 0, 0);
            this.t.moreStats[0].setSpecialStats(0, 0, 0);
            for (int i = 1; i < 20; i++)
            {
                this.t.moreStats[i] = new MoreStats();
                this.t.moreStats[i].baseStats.knockbackReduction = i * 2;
                this.t.moreStats[i].setBasicStats(i * i << 4, (int)(0.9 * (i * i)), i + 1, (int)(0.1 * (i + 19)), i << 1);
                this.t.moreStats[i].setSpecialStats(30 * i * i, i >> 1, 0);
                this.t.moreStats[i].addAnotherStats(this.t.moreStats[i - 1]);
            }
            this.add(new CultisystemAsset
            {
                id = "default",
                name = "仙路",
                bannedRace = new string[] { },
                moreStats = new MoreStats[20]
            });
            this.t.moreStats[0] = new MoreStats();
            this.t.moreStats[0].setBasicStats(0, 0, 0, 0, 0);
            this.t.moreStats[0].setSpecialStats(0, 0, 0);
            for (int i = 1; i < 20; i++)
            {
                this.t.moreStats[i] = new MoreStats();
                this.t.moreStats[i].baseStats.knockbackReduction = i*2;
                this.t.moreStats[i].setBasicStats(i * i<<4, (int)(0.9 * (i * i)), i + 1, (int)(0.1 * (i + 19)),i<<1);
                this.t.moreStats[i].setSpecialStats(30*i*i, i>>1, 0);
                this.t.moreStats[i].addAnotherStats(this.t.moreStats[i - 1]);
            }
            this.add(new CultisystemAsset
            {
                id = "bodying",
                name = "炼体",
                bannedRace = new string[] { },
                moreStats = new MoreStats[20]
            });
            this.t.moreStats[0] = new MoreStats();
            this.t.moreStats[0].setBasicStats(0, 0, 0, 0, 0);
            this.t.moreStats[0].setSpecialStats(0, 0, 0);
            for (int i = 1; i < 20; i++)
            {
                this.t.moreStats[i] = new MoreStats();
                this.t.moreStats[i].baseStats.knockbackReduction = i*2;
                this.t.moreStats[i].setBasicStats(i * i * i*3, (int)(0.45 * (i * i)), i + 1, (int)(0.15 * (i + 19)));
                this.t.moreStats[i].setSpecialStats(100*i * i, i, 0);
                this.t.moreStats[i].addAnotherStats(this.t.moreStats[i - 1]);
            }

            this.add(new CultisystemAsset
            {
                id = "bushido",
                name = "武道",
                bannedRace = new string[] { "orc"},
                moreStats = new MoreStats[20]
            });
            this.t.moreStats[0] = new MoreStats();
            this.t.moreStats[0].setBasicStats(0, 0, 0, 0, 0);
            this.t.moreStats[0].setSpecialStats(0, 0, 0);
            for (int i = 1; i < 20; i++)
            {
                this.t.moreStats[i] = new MoreStats();
                this.t.moreStats[i].baseStats.knockbackReduction = i * 2;
                this.t.moreStats[i].setBasicStats(i * i * i<<1, (int)(0.65 * (i * i)), i + 1, (int)(0.12* (i + 19)));
                this.t.moreStats[i].setSpecialStats((int)(i*i * i*1.6), i, 0);
                this.t.moreStats[i].addAnotherStats(this.t.moreStats[i - 1]);
            }

            bannedCultisystemsForRace();
        }
        //对种族的修炼路线进行限制
        private void bannedCultisystemsForRace()
        {
            foreach(CultisystemAsset cultisystem in this.list)
            {
                foreach(string raceID in cultisystem.bannedRace)
                {
                    Race race = AssetManager.raceLibrary.get(raceID);
                    race.culture_forbidden_tech.Add("culti_" + cultisystem.id);
                }
            }
        }
    }
}
