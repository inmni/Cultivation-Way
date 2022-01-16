using ReflectionUtility;
using System;
using CultivationWay;
using UnityEngine;
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
            power.ignoreFastSpawn = false;
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
            power1.ignoreFastSpawn = false;
            power1.showSpawnEffect = "spawn";
            power1.spawnSound = "spawnHuman";
            power1.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
            AssetManager.powers.add(power1);

            GodPower fairyFox = new GodPower();
            fairyFox.id = "spawnFairyFox";
            fairyFox.name = "spawnFairyFox";
            fairyFox.rank = PowerRank.Rank0_free;
            fairyFox.actorStatsId = "FairyFox";
            fairyFox.actorSpawnHeight = 3f;
            fairyFox.unselectWhenWindow = true;
            fairyFox.ignoreFastSpawn = false;
            fairyFox.showSpawnEffect = "spawn";
            fairyFox.spawnSound = "spawnHuman";
            fairyFox.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
            AssetManager.powers.add(fairyFox);

            GodPower easterDragon = new GodPower();
            easterDragon.id = "spawnEasternDragon";
            easterDragon.name = "spawnEasternDragon";
            easterDragon.rank = PowerRank.Rank0_free;
            easterDragon.actorStatsId = "EasternDragon";
            easterDragon.actorSpawnHeight = 0f;
            easterDragon.unselectWhenWindow = true;
            easterDragon.ignoreFastSpawn = true;
            easterDragon.spawnSound = "spawnDragon";
            easterDragon.showSpawnEffect = "spawn";
            easterDragon.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
            AssetManager.powers.add(easterDragon);

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


            #region 其他
            GodPower exp = new GodPower();
            exp.id = "exp";
            exp.name = "exp";
            exp.dropID = "exp";
            exp.showToolSizes = true;
            exp.fallingChance = 0.02f;
            exp.unselectWhenWindow = true;
            exp.holdAction = true;
            exp.click_power_action = new PowerAction((WorldTile pTile, GodPower pPower)
                                     =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnDrops", pTile, pPower);
            });
            exp.click_power_brush_action = new PowerAction((WorldTile pTile, GodPower pPower)
                                    =>
            {
                return (bool)AssetManager.powers.CallMethod("loopWithCurrentBrushPower", pTile, pPower);
            });
            AssetManager.powers.add(exp);

            GodPower checkElement = new GodPower();
            checkElement.id = "checkElement";
            checkElement.name = "checkElement";
            checkElement.unselectWhenWindow = true;
            checkElement.force_map_text = MapMode.None;
            checkElement.map_modes_switch = true;
            checkElement.toggle_name = "map_reki_zones";
            checkElement.toggle_action = (PowerToggleAction)Delegate.Combine(checkElement.toggle_action, new PowerToggleAction(this.toggleOption));
            AssetManager.powers.add(checkElement);
            #endregion
        }
        #region 原版函数
        private void toggleOption(string pPower)
        {//与原版不同
            GodPower godPower = AssetManager.powers.get(pPower);
            WorldTip.instance.showToolbarText(godPower);
            PlayerOptionData playerOptionData = PlayerConfig.dict[godPower.toggle_name];
            playerOptionData.boolVal = !playerOptionData.boolVal;
            if (playerOptionData.boolVal && godPower.map_modes_switch)
            {
                this.disableAllOtherMapModes(pPower);
                Main.instance.addMapMode = godPower.toggle_name;
            }
            else
            {
                Main.instance.addMapMode = "";
            }
            PlayerConfig.saveData();
        }
        private void disableAllOtherMapModes(string pMainPower)
        {
            for (int i = 0; i < AssetManager.powers.list.Count; i++)
            {
                GodPower godPower = AssetManager.powers.list[i];
                if (godPower.map_modes_switch && !(godPower.id == pMainPower))
                {
                    PlayerOptionData playerOptionData = PlayerConfig.dict[godPower.toggle_name];
                    if (playerOptionData.boolVal)
                    {
                        playerOptionData.boolVal = false;
                    }
                }
            }
        }
        #endregion
    }
}
