using ReflectionUtility;

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
                                =>
            {
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
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
            AssetManager.powers.add(power1);
            #endregion

            #region BOSS
            GodPower power2 = new GodPower();
            power2.id = "spawnJiaoDragon";
            power2.name = "spawnJiaoDragon";
            power2.rank = PowerRank.Rank0_free;
            power2.actorStatsId = "JiaoDragon";
            power2.actorSpawnHeight = 30f;
            power2.unselectWhenWindow = true;
            power2.showSpawnEffect = "spawn";
            power2.spawnSound = "spawnHuman";
            power2.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
            AssetManager.powers.add(power2);

            GodPower power3 = new GodPower();
            power3.id = "spawnXieDragon";
            power3.name = "spawnXieDragon";
            power3.rank = PowerRank.Rank0_free;
            power3.actorStatsId = "XieDragon";
            power3.actorSpawnHeight = 30f;
            power3.unselectWhenWindow = true;
            power3.showSpawnEffect = "spawn";
            power3.spawnSound = "spawnHuman";
            power3.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
            AssetManager.powers.add(power3);
            #endregion

            #region 彩蛋
            GodPower power5 = new GodPower();
            power5.id = "spawnMengZhu";
            power5.name = "spawnMengZhu";
            power5.rank = PowerRank.Rank0_free;
            power5.actorStatsId = "MengZhu";
            power5.actorSpawnHeight = 3f;
            power5.unselectWhenWindow = true;
            power5.showSpawnEffect = "spawn";
            power5.spawnSound = "spawnHuman";
            power5.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
            AssetManager.powers.add(power5);

            #endregion

            GodPower power4 = new GodPower();
            power4.id = "exp";
            power4.name = "exp";
            power4.dropID = "exp";
            power4.showToolSizes = true;
            power4.fallingChance = 0.02f;
            power4.unselectWhenWindow = true;
            power4.holdAction = true;
            power4.click_power_action = new PowerAction((WorldTile pTile, GodPower pPower)
                                     =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnDrops", pTile, pPower);
            });
            power4.click_power_brush_action = new PowerAction((WorldTile pTile, GodPower pPower)
                                    =>
            {
                return (bool)AssetManager.powers.CallMethod("loopWithCurrentBrushPower", pTile, pPower);
            });
            AssetManager.powers.add(power4);
        }
    }
}
