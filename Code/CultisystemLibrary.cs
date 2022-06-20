namespace Cultivation_Way
{
    internal class CultisystemLibrary : AssetLibrary<CultisystemAsset>
    {
        public override void init()
        {
            base.init(); 
            add(new CultisystemAsset
            {
                id = "default",
                name = "无",
                flag = 0 << 0,
                addExperience = nothing,
                bannedRace = new string[] { },
                moreStats = new MoreStats[20]
            });
            t.moreStats[0] = new MoreStats();
            t.moreStats[0].setBasicStats(0, 0, 0, 0, 0);
            t.moreStats[0].setSpecialStats(0, 0, 0);
            for (int i = 1; i < 20; i++)
            {
                t.moreStats[i] = new MoreStats();
                t.moreStats[i].baseStats.knockbackReduction = i * 2;
                t.moreStats[i].setBasicStats(i * i << 4, (int)(0.9 * (i * i)), i + 1, (int)(0.1 * (i + 19)), i << 1);
                t.moreStats[i].setSpecialStats(30 * i * i, i >> 1, 0);
                t.moreStats[i].addAnotherStats(t.moreStats[i - 1]);
            }
            add(new CultisystemAsset
            {
                id = "normal",
                name = "仙路",
                flag = 0 << 1,
                addExperience = normal,
                bannedRace = new string[] { "orc","Wu","Tian" },
                moreStats = new MoreStats[20]
            });
            t.moreStats[0] = new MoreStats();
            t.moreStats[0].setBasicStats(0, 0, 0, 0, 0);
            t.moreStats[0].setSpecialStats(0, 0, 0);
            for (int i = 1; i < 20; i++)
            {
                t.moreStats[i] = new MoreStats();
                t.moreStats[i].baseStats.knockbackReduction = i * 2;
                t.moreStats[i].setBasicStats(i * i << 4, (int)(0.9 * (i * i)), i + 1, (int)(0.1 * (i + 19)), i << 1);
                t.moreStats[i].setSpecialStats(30 * i * i, i >> 1, 0);
                t.moreStats[i].addAnotherStats(t.moreStats[i - 1]);
            }
            add(new CultisystemAsset
            {
                id = "bodying",
                name = "炼体",
                flag = 0 << 2,
                addExperience = normal,
                bannedRace = new string[] { "Tian","Ming"},
                moreStats = new MoreStats[20]
            });
            t.moreStats[0] = new MoreStats();
            t.moreStats[0].setBasicStats(0, 0, 0, 0, 0);
            t.moreStats[0].setSpecialStats(0, 0, 0);
            for (int i = 1; i < 20; i++)
            {
                t.moreStats[i] = new MoreStats();
                t.moreStats[i].baseStats.knockbackReduction = i * 2;
                t.moreStats[i].setBasicStats(i * i * i * 3, (int)(0.45 * (i * i)), i + 1, (int)(0.15 * (i + 19)));
                t.moreStats[i].setSpecialStats(100 * i * i, i, 0);
                t.moreStats[i].addAnotherStats(t.moreStats[i - 1]);
            }

            add(new CultisystemAsset
            {
                id = "bushido",
                name = "武道",
                flag = 0 << 3,
                addExperience = normal,
                bannedRace = new string[] { "orc","Tian" },
                moreStats = new MoreStats[20]
            });
            t.moreStats[0] = new MoreStats();
            t.moreStats[0].setBasicStats(0, 0, 0, 0, 0);
            t.moreStats[0].setSpecialStats(0, 0, 0);
            for (int i = 1; i < 20; i++)
            {
                t.moreStats[i] = new MoreStats();
                t.moreStats[i].baseStats.knockbackReduction = i * 2;
                t.moreStats[i].setBasicStats(i * i * i << 1, (int)(0.65 * (i * i)), i + 1, (int)(0.12 * (i + 19)));
                t.moreStats[i].setSpecialStats((int)(i * i * i * 1.6), i, 0);
                t.moreStats[i].addAnotherStats(t.moreStats[i - 1]);
            }

            bannedCultisystemsForRace();
        }
        //对种族的修炼路线进行限制
        private void bannedCultisystemsForRace()
        {
            foreach (CultisystemAsset cultisystem in list)
            {
                foreach (string raceID in cultisystem.bannedRace)
                {
                    Race race = AssetManager.raceLibrary.get(raceID);
                    race.culture_forbidden_tech.Add("culti_" + cultisystem.id);
                }
            }
        }
        private static bool nothing(ExtendedActor pActor,int pValue)
        {
            return false;
        }
        private static bool normal(ExtendedActor pActor, int pValue)
        {
            ActorStatus data;
            if (!canLevelUp(pActor, pValue, out data))
            {
                return false;
            }
            while (data.experience >= pActor.getExpToLevelup() && data.level < ExtendedWorldData.instance.levelLimit)
            {
                data.experience -= pActor.getExpToLevelup();
                data.level++;
                //准备雷劫
                if ((data.level - 1) % 10 == 0)
                {
                    if (!PowerActionLibrary.lightningPunishment(pActor))
                    {
                        return false;
                    }
                    data.health = int.MaxValue >> 4;
                    if (data.level > 10)
                    {
                        if (data.level == 110)
                        {
                            pActor.generateNewBody();
                        }
                    }
                    if (Toolbox.randomChance(data.level * (pActor.extendedCurStats.talent + 1) / 110f))
                    {
                        pActor.learnNewSpell();
                    }
                }
                //法术释放
                foreach (ExtensionSpell spell in pActor.extendedCurStats.spells)
                {
                    if (spell.GetSpellAsset().type==ExtensionSpellType.LEVELUP)
                    {
                        spell.castSpell(pActor, pActor);
                        break;
                    }
                }
            }
            return true;
        }

        private static bool canLevelUp(ExtendedActor pActor, int pValue, out ActorStatus data)
        {
            if (pActor == null)
            {
                data = null;
                return false;
            }
            data = pActor.easyData;
            if (!data.alive || !pActor.extendedData.status.canCultivate || !pActor.stats.canLevelUp)
            {
                return false;
            }
            int exp = pActor.getExpToLevelup();
            if ((data.experience += pValue) < exp)
            {
                return false;
            }
            //回蓝，回冷却
            pActor.extendedData.status.magic = pActor.extendedCurStats.magic;//待调整与元素相关
            foreach (ExtensionSpell spell in pActor.extendedCurStats.spells)
            {
                spell.leftCool -= spell.leftCool > 0 ? 1 : 0;
            }
            pActor.setStatsDirty();
            //如果等级达到上限，或者该生物不能升级，则优化灵根
            if (data.level >= ExtendedWorldData.instance.levelLimit)
            {
                return false;
            }
            return true;
        }
    }
}
