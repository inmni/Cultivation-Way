using ReflectionUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cultivation_Way
{
    class MoreGodPowers
    {
        internal void init()
        {
            #region 更多生物
            GodPower power = new GodPower();
            power.id = "spawnTian";
            power.name = "spawnTian";
            power.rank = PowerRank.Rank0_free;
            power.actorStatsId = "Tian";
            power.actorSpawnHeight = 3f;
            power.unselectWhenWindow = true;
            power.showSpawnEffect = "spawn";
            power.spawnSound = "spawnHuman";
            power.click_action = new PowerActionWithID((WorldTile pTile, string pPower) 
                                => { 
                                        return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower); 
                                    });
            AssetManager.powers.add(power);

            GodPower power1 = new GodPower();
            power1.id = "spawnMing";
            power1.name = "spawnMing";
            power1.rank = PowerRank.Rank0_free;
            power1.actorStatsId = "Ming";
            power1.actorSpawnHeight = 3f;
            power1.unselectWhenWindow = true;
            power1.showSpawnEffect = "spawn";
            power1.spawnSound = "spawnHuman";
            power1.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                => {
                                    return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
                                });
            AssetManager.powers.add(power1);
            #endregion
        }
    }
}
