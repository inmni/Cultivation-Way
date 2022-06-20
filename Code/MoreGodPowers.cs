using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using System;
using Cultivation_Way.Utils;
using UnityEngine;
namespace Cultivation_Way
{
    internal class MoreGodPowers
    {
        internal void init()
        {
            #region 更多生物
            GodPower power0 = new GodPower();
            power0.id = "spawnEasternHuman";
            power0.name = "spawnEasternHuman";
            power0.rank = PowerRank.Rank0_free;
            power0.actorStatsId = "unit_EasternHuman";
            power0.actorSpawnHeight = 3f;
            power0.unselectWhenWindow = true;
            power0.ignoreFastSpawn = false;
            power0.showSpawnEffect = "spawn";
            power0.spawnSound = "spawnHuman";
            power0.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
            AssetManager.powers.add(power0);

            GodPower power = new GodPower();
            power.id = "spawnTian";
            power.name = "spawnTian";
            power.rank = PowerRank.Rank0_free;
            power.actorStatsId = "unit_Tian";
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
            power1.actorStatsId = "unit_Ming";
            power1.actorSpawnHeight = 3f;
            power1.unselectWhenWindow = true;
            power1.ignoreFastSpawn = false;
            power1.showSpawnEffect = "spawn";
            power1.spawnSound = "spawnHuman";
            power1.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return ((Func<PowerLibrary, WorldTile, string, bool>)AssetManager.powers.GetFastMethod("spawnUnit"))(AssetManager.powers, pTile, pPower);
            });
            AssetManager.powers.add(power1);

            GodPower power2 = new GodPower();
            power2.id = "spawnYao";
            power2.name = "spawnYao";
            power2.rank = PowerRank.Rank0_free;
            power2.actorStatsId = "unit_Yao";
            power2.actorSpawnHeight = 3f;
            power2.unselectWhenWindow = true;
            power2.ignoreFastSpawn = false;
            power2.showSpawnEffect = "spawn";
            power2.spawnSound = "spawnOrc";
            power2.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return PowerActionLibrary.spawnYao(pTile, pPower);
            });
            AssetManager.powers.add(power2);

            GodPower Wu = new GodPower();
            Wu.id = "spawnWu";
            Wu.name = "spawnWu";
            Wu.rank = PowerRank.Rank0_free;
            Wu.actorStatsId = "unit_Wu";
            Wu.actorSpawnHeight = 3f;
            Wu.unselectWhenWindow = true;
            Wu.ignoreFastSpawn = false;
            Wu.showSpawnEffect = "spawn";
            Wu.spawnSound = "spawnOrc";
            Wu.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
            AssetManager.powers.add(Wu);

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
                Vector3 end = MapBox.instance.GetTile(1, 1).posV3;
                Vector3 start = pTile.posV3 + end;
                int count = 0;
                NewSpriteAnimation e = NewEffectManager.spawnOn("LXST", pTile, Vector3.one);
                e.setMove(end,30,false, new Action(() => 
                { 
                    if (count++ > 100) 
                    { 
                        e.stop(true); 
                        return; 
                    } 
                    e.setMove(new Vector3(Toolbox.randomFloat(end.x, start.x), Toolbox.randomFloat(end.y, start.y)), 
                        Toolbox.randomFloat(e.moveSpeed*0.9f,e.moveSpeed*1.1f), true); 
                }));
                e.loop = true;
                e.frameActions = new Action[e.frames.Length];
                e.setFrameAction(1, new Action(()=> 
                { 
                    MapBox.spawnLightning(
                        MapBox.instance.GetTile((int)e.m_gameobject.transform.position.x, 
                        (int)e.m_gameobject.transform.position.y),e.moveSpeed); 
                }));
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
            AssetManager.powers.add(fairyFox);

            GodPower fuRen = new GodPower();
            fuRen.id = "spawnFuRen";
            fuRen.name = "spawnFuRen";
            fuRen.rank = PowerRank.Rank0_free;
            fuRen.actorStatsId = "FuRen";
            fuRen.actorSpawnHeight = 3f;
            fuRen.unselectWhenWindow = true;
            fuRen.ignoreFastSpawn = true;
            fuRen.showSpawnEffect = "spawn";
            fuRen.spawnSound = "spawnHuman";
            fuRen.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                            =>
            {
                if (ExtendedWorldData.instance.creatureLimit[fuRen.actorStatsId] <= 0)
                {
                    return false;
                }
                ExtendedWorldData.instance.creatureLimit[fuRen.actorStatsId]--;
                ExtendedActor FuRen = (ExtendedActor)MapBox.instance.spawnNewUnit("FuRen", pTile, "", 3f);
                FuRen.easyData.level = 11;
                FuRen.easyData.health = int.MaxValue >> 2;
                FuRen.setStatsDirty();
                return true;
            });
            AssetManager.powers.add(fuRen);

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
                if (ExtendedWorldData.instance.creatureLimit[easterDragon.actorStatsId] <= 0)
                {
                    return false;
                }
                ExtendedWorldData.instance.creatureLimit[easterDragon.actorStatsId]--;
                ExtendedActor actor = (ExtendedActor)MapBox.instance.spawnNewUnit("EasternDragon", pTile, "", 3f);
                actor.easyData.firstName = "龙王";
                actor.easyData.level = 11;
                actor.easyData.health = int.MaxValue >> 2;
                actor.extendedCurStats.element.baseElementContainer = new int[5] { 20, 20, 20, 20, 20 };
                actor.extendedCurStats.element.setType();
                return true;
            });
            AssetManager.powers.add(easterDragon);
            #region 妖圣
            GodPower MonkeySheng1 = AssetManager.powers.clone("spawnMonkeySheng1", "spawnEasternDragon");
            MonkeySheng1.name = MonkeySheng1.id;
            MonkeySheng1.actorStatsId = MonkeySheng1.id.Remove(0, 5);
            MonkeySheng1.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return PowerActionLibrary.spawnSheng(pTile, pPower);
            });
            GodPower MonkeySheng2 = AssetManager.powers.clone("spawnMonkeySheng2", "spawnEasternDragon");
            MonkeySheng2.name = MonkeySheng2.id;
            MonkeySheng2.actorStatsId = MonkeySheng2.id.Remove(0, 5);
            MonkeySheng2.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return PowerActionLibrary.spawnSheng(pTile, pPower);
            });
            GodPower CatSheng = AssetManager.powers.clone("spawnCatSheng", "spawnEasternDragon");
            CatSheng.name = CatSheng.id;
            CatSheng.actorStatsId = CatSheng.id.Remove(0, 5);
            CatSheng.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return PowerActionLibrary.spawnSheng(pTile, pPower);
            });
            GodPower ChickenSheng = AssetManager.powers.clone("spawnChickenSheng", "spawnEasternDragon");
            ChickenSheng.name = ChickenSheng.id;
            ChickenSheng.actorStatsId = ChickenSheng.id.Remove(0, 5);
            ChickenSheng.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return PowerActionLibrary.spawnSheng(pTile, pPower);
            });
            GodPower WolfSheng = AssetManager.powers.clone("spawnWolfSheng", "spawnEasternDragon");
            WolfSheng.name = WolfSheng.id;
            WolfSheng.actorStatsId = WolfSheng.id.Remove(0, 5);
            WolfSheng.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return PowerActionLibrary.spawnSheng(pTile, pPower);
            });
            GodPower CowSheng = AssetManager.powers.clone("spawnCowSheng", "spawnEasternDragon");
            CowSheng.name = CowSheng.id;
            CowSheng.actorStatsId = CowSheng.id.Remove(0, 5);
            CowSheng.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return PowerActionLibrary.spawnSheng(pTile, pPower);
            });
            GodPower SnakeSheng = AssetManager.powers.clone("spawnSnakeSheng", "spawnEasternDragon");
            SnakeSheng.name = SnakeSheng.id;
            SnakeSheng.actorStatsId = SnakeSheng.id.Remove(0, 5);
            SnakeSheng.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return PowerActionLibrary.spawnSheng(pTile, pPower);
            });
            #endregion
            #endregion

            #region BOSS
            GodPower JiaoDragon = new GodPower();
            JiaoDragon.id = "spawnJiaoDragon";
            JiaoDragon.name = "spawnJiaoDragon";
            JiaoDragon.rank = PowerRank.Rank0_free;
            JiaoDragon.actorStatsId = "JiaoDragon";
            JiaoDragon.actorSpawnHeight = 30f;
            JiaoDragon.unselectWhenWindow = true;
            JiaoDragon.showSpawnEffect = "spawn";
            JiaoDragon.spawnSound = "spawnDragon";
            JiaoDragon.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
            AssetManager.powers.add(JiaoDragon);

            GodPower XieDragon = new GodPower();
            XieDragon.id = "spawnXieDragon";
            XieDragon.name = "spawnXieDragon";
            XieDragon.rank = PowerRank.Rank0_free;
            XieDragon.actorStatsId = "XieDragon";
            XieDragon.actorSpawnHeight = 30f;
            XieDragon.unselectWhenWindow = true;
            XieDragon.showSpawnEffect = "spawn";
            XieDragon.spawnSound = "spawnHuman";
            XieDragon.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                return (bool)AssetManager.powers.CallMethod("spawnUnit", pTile, pPower);
            });
            AssetManager.powers.add(XieDragon);

            GodPower Nian = new GodPower();
            Nian.id = "spawnNian";
            Nian.name = "spawnNian";
            Nian.rank = PowerRank.Rank0_free;
            Nian.actorStatsId = "Nian";
            Nian.unselectWhenWindow = true;
            Nian.showSpawnEffect = "spawn";
            Nian.spawnSound = "spawnDragon";
            Nian.click_action = new PowerActionWithID((WorldTile pTile, string pPower)
                                =>
            {
                if (ExtendedWorldData.instance.creatureLimit[Nian.actorStatsId] <= 0)
                {
                    return false;
                }
                ExtendedWorldData.instance.creatureLimit[Nian.actorStatsId]--;
                ExtendedActor actor = (ExtendedActor)MapBox.instance.spawnNewUnit("Nian", pTile, "", 3f);
                actor.easyData.firstName = "夕";
                actor.extendedCurStats.element.baseElementContainer = new int[5] { 20, 20, 20, 20, 20 };
                actor.extendedCurStats.element.setType();
                int level = MapBox.instance.mapStats.year / 2022 * 10 + 1;
                if (level > 110)
                {
                    level = 110;
                }
                actor.easyData.level = level;
                actor.easyData.health = int.MaxValue >> 2;
                WorldTools.logSomething($"<color={Toolbox.colorToHex(Toolbox.color_log_warning, true)}>年兽入侵！</color>", "iconKingslayer", pTile);
                return true;
            });
            AssetManager.powers.add(Nian);
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
            checkElement.toggle_action = (PowerToggleAction)Delegate.Combine(checkElement.toggle_action, new PowerToggleAction(toggleOption));
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
                disableAllOtherMapModes(pPower);
                Main.instance.addMapMode = godPower.toggle_name;
            }
            else
            {
                Main.instance.addMapMode = string.Empty;
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
        [HarmonyPrefix]
        [HarmonyPatch(typeof(PowerLibrary), "disableAllOtherMapModes")]
        public static bool disableAllOtherMapModes_Prefix(string pMainPower)
        {
            Main.instance.addMapMode = string.Empty;
            return true;
        }
    }
}
