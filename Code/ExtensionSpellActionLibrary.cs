using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using UnityEngine;
using Cultivation_Way.Utils;
using ReflectionUtility;
using CultivationWay;
using System.Diagnostics;
using HarmonyLib;

namespace Cultivation_Way
{
    class ExtensionSpellActionLibrary
    {
        //StackSpellEffects
        //样例
        public static bool exampleSpell(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            Utils.ResourcesHelper.playSpell(spell.spellAssetID, pTarget.currentPosition, pTarget.currentPosition, 1f);
            float oriDamage = OthersHelper.getSpellDamage(spell, pUser, pTarget);
            //再进行伤害处理
            if (pTarget.objectType == MapObjectType.Actor)
            {
                ((Actor)pTarget).CallMethod("getHit", oriDamage, true, AttackType.Other, pUser, true);
                ((Actor)pTarget).startColorEffect("red");
            }
            else
            {
                ((Building)pTarget).CallMethod("getHit", oriDamage, true, AttackType.Other, pUser, true);
            }

            return true;
        }
        //雷法
        public static bool lightningSpell(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            OthersHelper.hitEnemiesInRange(pUser, pTarget, 6f, 0f,spell);
            if (Toolbox.randomChance(spell.might / 1000))
            {
                MapBox.instance.startShake();
            }
            Vector2 target = new Vector2(pTarget.currentPosition.x+0.55f, pTarget.currentPosition.y - 7f);
            Utils.ResourcesHelper.playSpell(spell.spellAssetID, target, target, 3f);
            return true;
        }
        public static bool lightning1Spell(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            Vector2 target = new Vector2(pTarget.currentPosition.x, pTarget.currentPosition.y+2.2f);
            Utils.ResourcesHelper.playSpell(spell.spellAssetID, target, target, ((Actor)pUser).GetData().level/20f);
            float oriDamage = OthersHelper.getSpellDamage(spell, pUser, pTarget);
            //再进行伤害处理
            if (pTarget.objectType == MapObjectType.Actor)
            {
                ((Actor)pTarget).CallMethod("getHit", oriDamage, true, AttackType.Other, pUser, true);
            }
            else
            {
                ((Building)pTarget).CallMethod("getHit", oriDamage, true, AttackType.Other, pUser, true);
            }
            pTarget.currentTile.setBurned();
            return true;
        }
        //基础火
        public static bool defaultFire(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            OthersHelper.hitEnemiesInRange(pUser, pTarget, 2f, 0f,spell);
            Vector2 target = new Vector2(pTarget.currentPosition.x, pTarget.currentPosition.y-1.5f);
            Utils.ResourcesHelper.playSpell(spell.spellAssetID, target, target, 1f);
            return true;
        }
        //剑阵
        public static bool swordsArray(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            OthersHelper.hitEnemiesInRange(pUser, pTarget, 4f, 0f,spell);
            Vector2 target = new Vector2(pTarget.currentPosition.x, pTarget.currentPosition.y - 5f);
            ResourcesHelper.playSpell(spell.spellAssetID, target, target, 2f);
            return true;
        }
        //激光，目前只用于蛟龙
        public static bool laserSpell(ExtensionSpell spell,BaseSimObject pUser =null,BaseSimObject pTarget = null)
        {
            if (pUser == pTarget||pUser==null||pTarget==null)
            {
                return false;
            }
            OthersHelper.hitEnemiesInRange(pUser, pTarget, 10f, 0f,spell);
            Vector3 start = new Vector3(pUser.currentTile.posV3.x, pUser.currentTile.posV3.y + 2.5f);
            Vector3 end = new Vector3(pTarget.currentTile.posV3.x, pTarget.currentTile.posV3.y);

            float xScale = OthersHelper.getDistance(start, end);
            float angle = OthersHelper.getAngle(start, end);
            if (angle > 0 && angle < 180)
            {
                xScale *= Mathf.Abs(angle - 90) * Mathf.PI / 180f;
            }

            BaseSpellEffectController baseEffectController = Main.instance.spellEffects.get(spell.spellAssetID);
            BaseSpellEffect baseEffect = ((baseEffectController != null) ? baseEffectController.spawnAt(new Vector3(end.x, end.y+ 2.8f), new Vector3(xScale * 0.02f, 0.2f, 0), new Vector3(0f, end.y - start.y, angle)) : null);

            return true;
        }
        //基础召唤
        public static bool summonSpell(ExtensionSpell spell,BaseSimObject pUser = null,BaseSimObject pTarget = null)
        {
            if (pUser == null||((Actor)pUser).stats.race.Contains("summon"))
            {
                return false;
            }
            int num = (int)spell.might;//威力取整作为召唤生物的个数

            for(int i = 0; i < num; i++)
            {
                WorldTile tile = pUser.currentTile.neighboursAll.GetRandom();
                Utils.ResourcesHelper.playSpell(spell.spellAssetID, tile.posV3, tile.posV3, 1f);
                Actor summoned = MapBox.instance.createNewUnit(spell.spellAssetID, tile);

                ((AiSystemActor)Reflection.GetField(typeof(Actor), summoned, "ai")).setJob("attacker");
                ((AiSystemActor)Reflection.GetField(typeof(Actor), summoned, "ai")).setTask("warrior_army_follow_leader");
                summoned.GetData().profession = UnitProfession.Warrior;
                
                summoned.kingdom = pUser.kingdom;
                if (pUser.city != null)
                {
                    pUser.city.addNewUnit(summoned);
                }
                if (((Actor)pUser).GetData().level > 10)
                {
                    summoned.GetData().level = ((Actor)pUser).GetData().level - 10;
                }
                else
                {
                    summoned.GetData().level = 1;
                }
                summoned.GetData().firstName = ((Actor)pUser).GetData().firstName + "召唤物";
                summoned.GetData().health = ((Actor)pUser).GetCurStats().health / 10;
                summoned.setStatsDirty();
            }
            return true;
        }
        //单独召唤
        public static bool summonTianSpell(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == null || ((Actor)pUser).stats.race.Contains("summon"))
            {
                return false;
            }
            int num = (int)spell.might;//威力取整作为召唤生物的等级因素
            //设置生成位置和动画位置
            WorldTile tile1 = pUser.currentTile;
            Vector2 pos = new Vector2(tile1.posV3.x+3f, tile1.posV3.y+12f);
            Actor summoned = MapBox.instance.createNewUnit(spell.spellAssetID, tile1);
            Utils.ResourcesHelper.playSpell(spell.spellAssetID, pos, pos, 15f);
            
            ((AiSystemActor)Reflection.GetField(typeof(Actor), summoned, "ai")).setJob("attacker");
                summoned.GetData().profession = UnitProfession.Warrior;
                summoned.kingdom = pUser.kingdom;
                if (pUser.city != null)
                {
                    pUser.city.addNewUnit(summoned);
                }
                if (((Actor)pUser).GetData().level > 10-num)
                {
                    summoned.GetData().level = ((Actor)pUser).GetData().level -10+ num;
                }
                else
                {
                    summoned.GetData().level = 1;
                }
                summoned.GetData().firstName = ((Actor)pUser).GetData().firstName + "召唤物";
                summoned.GetData().health = ((Actor)pUser).GetCurStats().health;
                summoned.setStatsDirty();
            return true;
        }
        public static bool summonTianSpell1(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == null || ((Actor)pUser).stats.race.Contains("summon")||Main.instance.summonTian1Limit<=0)
            {
                return false;
            }
            Main.instance.summonTian1Limit--;
            //设置生成位置和动画位置
            WorldTile tile1 = pUser.currentTile;
            Vector2 pos = new Vector2(tile1.posV3.x, tile1.posV3.y);
            Actor summoned = MapBox.instance.createNewUnit(spell.spellAssetID, tile1);
            ResourcesHelper.playSpell("explosion", pos, pos, 15f);
            summoned.kingdom = pUser.kingdom;
            if (pUser.city != null)
            {
                pUser.city.addNewUnit(summoned);
            }
            if (((Actor)pUser).kingdom.king == ((Actor)pUser))
            {
                ((Actor)pUser).kingdom.king = summoned;
            }
            else if (((Actor)pUser).city.leader == ((Actor)pUser))
            {
                ((Actor)pUser).city.leader = summoned;
            }
            else
            {
                ((AiSystemActor)Reflection.GetField(typeof(Actor), summoned, "ai")).setJob("defender");
                summoned.GetData().profession = UnitProfession.Warrior;
            }
            summoned.GetData().level = 110;
            summoned.GetData().health = int.MaxValue >> 2;
            #region 复制
            summoned.GetData().actorID = ((Actor)pUser).GetData().actorID;
            ai.ActorTool.copyUnitToOtherUnit((Actor)pUser, summoned);
            Reflection.SetField(summoned, "s_personality", (PersonalityAsset)Reflection.GetField(typeof(Actor), (Actor)pUser, "s_personality"));
            MoreActorData copyData = new MoreActorData();
            MoreActorData originData = ((Actor)pUser).GetMoreData();
            copyData.bonusStats = originData.bonusStats;
            copyData.coolDown = originData.coolDown;
            copyData.cultisystem = originData.cultisystem;
            copyData.element = originData.element;
            copyData.familyID = originData.familyID;
            copyData.magic = originData.magic;
            copyData.specialBody = originData.specialBody;
            MoreStats copyStats = new MoreStats();
            copyStats.element = copyData.element;
            copyStats.cultisystem = copyData.cultisystem;
            copyStats.family = Main.instance.familys[copyData.familyID];
            //ItemGenerator.generateItem(AssetManager.items.get("summonTian1"), "adamantine", summoned.equipment.weapon,
            //                           0, "天族帝国", null, 10);
            ((Actor)pUser).killHimself(true, AttackType.GrowUp, false, false);
            Main.instance.actorToMoreData[summoned.GetData().actorID] = copyData;
            Main.instance.actorToMoreStats[summoned.GetData().actorID] = copyStats;
            #endregion
            summoned.setStatsDirty();
            summoned.CallMethod("updateStats");
            return true;
        }
        //法相天地
        public static bool Shengtixianhua(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {

            if(pUser==null||((Actor)pUser).GetData().health< ((Actor)pUser).GetCurStats().health*3/4)
            {
                return false;
            }
            Vector3 start = new Vector3(pUser.currentTile.posV3.x, pUser.currentTile.posV3.y + 2f);
            int time = 2;
            SpecialBody body = ((Actor)pUser).GetSpecialBody();
            if (body.rank > 1)
            {
                BaseSpellEffectController baseEffectController = Main.instance.spellEffects.get(body.origin);
                BaseSpellEffect baseEffect = ((baseEffectController != null) ? baseEffectController.spawnAt(start, 0.1f, true, (Actor)pUser, time, 0, 2f) : null);
            }

            MoreActorData moredata = Main.instance.actorToMoreData[((Actor)pUser).GetData().actorID];
            MoreStats bonusStats = new MoreStats();
            bonusStats.magic = 0;
            bonusStats.baseStats.mod_damage += body.mod_damage;
            bonusStats.baseStats.mod_attackSpeed += body.mod_attack_speed;
            bonusStats.baseStats.mod_speed += body.mod_speed;
            bonusStats.baseStats.mod_health += body.mod_health;
            moredata.bonusStats.addAnotherStats(bonusStats);

            MoreStats punishStats = -0.3f * bonusStats;
            new BonusStatsManager((Actor)pUser, bonusStats, time, punishStats);
            pUser.setStatsDirty();
            
            return true;
        }



        #region 通过游戏原版的委托进行的非交互法术
        public static bool bLightningSpell(BaseSimObject pUser,WorldTile pTile=null)
        {//目前只适用建筑

            if (pUser == null)
            {
                return false;
            }
            if (pTile == null)
            {
                pTile = pUser.currentTile;
            }
            Vector2 pos = new Vector2(pTile.posV3.x, pTile.posV3.y-5f);
            Utils.ResourcesHelper.playSpell("lightningPunishment", pos, pos, 3f);
            List<Actor> targets = OthersHelper.getEnemyObjectInRange(pUser, pTile, 6f);
            foreach(Actor target in targets)
            {
                if (target.GetData().alive)
                {
                    target.CallMethod("getHit", 300f, true, AttackType.Age, null, true);
                }
            }
            return true;
        }

        #endregion
    }
}
