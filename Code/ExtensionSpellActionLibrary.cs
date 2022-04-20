using Cultivation_Way.Utils;
using CultivationWay;
using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;

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
        //基础术法
        public static bool baseSpell(ExtensionSpell spell,BaseSimObject pUser= null,BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            string realSpell = OthersHelper.getRealBaseSpell(((ExtendedActor)pUser).extendedCurStats.element);
            Projectile p = Utils.OthersHelper.startProjectile(realSpell, pUser, pTarget);
            Reflection.SetField(p, "byWho", (Actor)pUser);
            p.setStats(((Actor)pUser).GetCurStats());
            p.targetObject = pTarget;
            return true;
        }
        //雷法
        public static bool lightningSpell(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            OthersHelper.hitEnemiesInRange(pUser, pTarget.currentTile, 6f, 0f, spell);
            if (Toolbox.randomChance(spell.might / 1000))
            {
                MapBox.instance.startShake();
            }
            Vector2 target = new Vector2(pTarget.currentPosition.x + 0.55f, pTarget.currentPosition.y - 7f);
            Utils.ResourcesHelper.playSpell(spell.spellAssetID, target, target, 3f);
            return true;
        }
        public static bool lightning1Spell(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            Vector2 target = new Vector2(pTarget.currentPosition.x, pTarget.currentPosition.y + 2.2f);
            Utils.ResourcesHelper.playSpell(spell.spellAssetID, target, target, ((Actor)pUser).GetData().level / 20f);
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
            OthersHelper.hitEnemiesInRange(pUser, pTarget.currentTile, 2f, 0f, spell);
            Vector2 target = new Vector2(pTarget.currentPosition.x, pTarget.currentPosition.y - 1.5f);
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
            OthersHelper.hitEnemiesInRange(pUser, pTarget.currentTile, 4f, 0f, spell);
            Vector2 target = new Vector2(pTarget.currentPosition.x, pTarget.currentPosition.y - 5f);
            ResourcesHelper.playSpell(spell.spellAssetID, target, target, 2f);
            return true;
        }
        //激光，目前只用于蛟龙
        public static bool laserSpell(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            OthersHelper.hitEnemiesInRange(pUser, pTarget.currentTile, 10f, 0f, spell);
            Vector3 start = new Vector3(pUser.currentTile.posV3.x, pUser.currentTile.posV3.y + 2.5f);
            Vector3 end = new Vector3(pTarget.currentTile.posV3.x, pTarget.currentTile.posV3.y);

            float xScale = OthersHelper.getDistance(start, end);
            float angle = OthersHelper.getAngle(start, end);
            if (angle > 0 && angle < 180)
            {
                xScale *= Mathf.Abs(angle - 90) * Mathf.PI / 180f;
            }

            BaseSpellEffectController baseEffectController = Main.instance.spellEffects.get(spell.spellAssetID);
            BaseSpellEffect baseEffect = ((baseEffectController != null) ? baseEffectController.spawnAt(new Vector3(end.x, end.y + 2.8f), new Vector3(xScale * 0.02f, 0.2f, 0), new Vector3(0f, end.y - start.y, angle)) : null);

            return true;
        }
        public static bool barSpell(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            OthersHelper.hitEnemiesInRange(pUser, pTarget.currentTile, 10f, 0f, spell);
            Vector3 start = new Vector3(pUser.currentTile.posV3.x, pUser.currentTile.posV3.y - 0.5f);
            Vector3 end = new Vector3(pTarget.currentTile.posV3.x, pTarget.currentTile.posV3.y - 0.5f);
            float angle = OthersHelper.getAngle(start, end);
            BaseSpellEffectController baseEffectController1 = Main.instance.spellEffects.get(spell.spellAssetID);
            BaseSpellEffectController baseEffectController2 = Main.instance.spellEffects.get(spell.spellAssetID);
            BaseSpellEffectController baseEffectController3 = Main.instance.spellEffects.get(spell.spellAssetID);
            BaseSpellEffectController baseEffectController4 = Main.instance.spellEffects.get(spell.spellAssetID);

            BaseSpellEffect baseEffect1 = ((baseEffectController1 != null) ? baseEffectController1.spawnAt(end, new Vector3(-0.06f, -0.06f, 0), new Vector3(0f, end.y - start.y, 135)) : null);
            BaseSpellEffect baseEffect2 = ((baseEffectController2 != null) ? baseEffectController2.spawnAt(end, new Vector3(0.06f, -0.06f, 0), new Vector3(0f, end.y - start.y, -135)) : null);
            BaseSpellEffect baseEffect3 = ((baseEffectController3 != null) ? baseEffectController3.spawnAt(end, new Vector3(0.06f, 0.06f, 0), new Vector3(0f, end.y - start.y, 45)) : null);
            BaseSpellEffect baseEffect4 = ((baseEffectController4 != null) ? baseEffectController4.spawnAt(end, new Vector3(-0.06f, 0.06f, 0), new Vector3(0f, end.y - start.y, -45)) : null);

            return true;
        }
        public static bool barDownSpell(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            float rad = 15f;
            float t = 1.5f;
            OthersHelper.hitEnemiesInRange(pUser, pUser.currentTile, rad, 0f, spell);
            Vector3 end1 = new Vector3(pUser.currentTile.posV3.x + 7.2f, pUser.currentTile.posV3.y);
            Vector3 end2 = new Vector3(pUser.currentTile.posV3.x, pUser.currentTile.posV3.y + 6f);
            Vector3 end3 = new Vector3(pUser.currentTile.posV3.x - 7.2f, pUser.currentTile.posV3.y);
            Vector3 end4 = new Vector3(pUser.currentTile.posV3.x, pUser.currentTile.posV3.y - 6f);
            ResourcesHelper.playSpell(spell.spellAssetID, end1, end1, t);
            ResourcesHelper.playSpell(spell.spellAssetID, end1, end2, t);
            ResourcesHelper.playSpell(spell.spellAssetID, end1, end3, t);
            ResourcesHelper.playSpell(spell.spellAssetID, end1, end4, t);
            return true;
        }
        //骷髅召唤
        public static bool summonSpell(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == null || ((Actor)pUser).stats.race.Contains("summon"))
            {
                return false;
            }
            int level = ((Actor)pUser).GetData().level;
            int num = (int)spell.might + level / 30;//威力取整作为召唤生物的个数

            for (int i = 0; i < num; i++)
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
                if (((Actor)pUser).GetData().level > 10 - level / 11)
                {
                    summoned.GetData().level = ((Actor)pUser).GetData().level - 10 + level / 11;
                }
                else
                {
                    summoned.GetData().level = 1;
                }
                summoned.GetData().firstName = ((Actor)pUser).GetData().firstName + "召唤物";
                summoned.GetData().health = ((Actor)pUser).GetCurStats().health / 3;
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
            Vector2 pos = new Vector2(tile1.posV3.x + 3f, tile1.posV3.y + 12f);
            Actor summoned = MapBox.instance.createNewUnit(spell.spellAssetID, tile1);
            Reflection.SetField(summoned, "hitboxZ", 10f);
            Utils.ResourcesHelper.playSpell(spell.spellAssetID, pos, pos, 15f);

            ((AiSystemActor)Reflection.GetField(typeof(Actor), summoned, "ai")).setJob("attacker");
            summoned.GetData().profession = UnitProfession.Warrior;
            summoned.kingdom = pUser.kingdom;
            if (pUser.city != null)
            {
                pUser.city.addNewUnit(summoned);
            }
            if (((Actor)pUser).GetData().level > 10 - num)
            {
                summoned.GetData().level = ((Actor)pUser).GetData().level - 10 + num;
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
            if (pUser == null || ((Actor)pUser).stats.race.Contains("summon") || Main.instance.creatureLimit[spell.spellAssetID] <= 0)
            {
                return false;
            }
            Main.instance.creatureLimit[spell.spellAssetID]--;
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
            ((Actor)pUser).kingdom.setKing(summoned);
            int level = MapBox.instance.mapStats.year / 50 + 1;
            if (level > 110)
            {
                level = 110;
            }
            summoned.GetData().level = level;
            summoned.GetData().health = int.MaxValue >> 2;
            #region 复制
            Reflection.SetField(summoned, "s_personality", (PersonalityAsset)Reflection.GetField(typeof(Actor), (Actor)pUser, "s_personality"));
            ActorTools.copyActor((Actor)pUser, summoned);
            ((Actor)pUser).killHimself(false, AttackType.GrowUp, false, false);
            #endregion
            summoned.setStatsDirty();
            return true;
        }
        //法相天地
        public static bool Shengtixianhua(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {

            if (pUser == null || ((Actor)pUser).GetData().health < ((Actor)pUser).GetCurStats().health * 3 / 4)
            {
                return false;
            }
            Vector3 start = new Vector3(pUser.currentTile.posV3.x, pUser.currentTile.posV3.y + 2f);
            int time = 2;
            SpecialBody body = ((Actor)pUser).GetSpecialBody();
            if (body.rank > 1)
            {
                BaseSpellEffectController baseEffectController = Main.instance.spellEffects.get(Utils.OthersHelper.getOriginBodyID(body));
                BaseSpellEffect baseEffect = ((baseEffectController != null) ? baseEffectController.spawnAt(start, 0.1f, true, (Actor)pUser, time, 0, 2f) : null);
            }

            MoreStatus moredata = ((ExtendedActor)pUser).extendedData.status;
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
        public static bool bLightningSpell(BaseSimObject pUser, WorldTile pTile = null)
        {//目前只适用建筑

            if (pUser == null)
            {
                return false;
            }
            if (pTile == null)
            {
                pTile = pUser.currentTile;
            }
            Vector2 pos = new Vector2(pTile.posV3.x, pTile.posV3.y - 5f);
            Utils.ResourcesHelper.playSpell("lightningPunishment", pos, pos, 8f);
            List<Actor> targets = OthersHelper.getEnemyObjectInRange(pUser, pTile, 6f);
            foreach (Actor target in targets)
            {
                if (target.GetData().alive)
                {
                    target.CallMethod("getHit", 300f, true, AttackType.None, null, true);
                }
            }
            return true;
        }
        public static bool aTransformToGod(BaseSimObject pUser, WorldTile pTile = null)
        {
            if (((Actor)pUser).GetData().level <= 10 || Toolbox.randomChance(0.15f))
            {
                return false;
            }
            string godID = "";
            foreach(string key in Main.instance.godList.Keys)
            {
                if (Main.instance.creatureLimit[key] > 0)
                {
                    godID = key;
                    break;
                }
            }
            if (godID == string.Empty)
            {
                return false;
            }
            Main.instance.creatureLimit[godID]--;
            Actor god = MapBox.instance.createNewUnit(godID, pTile);
            ActorTools.copyActor((Actor)pUser, god);
            god.kingdom = pUser.kingdom;
            god.city = pUser.city;
            if (Main.instance.kingdomBindActors.ContainsKey(god.kingdom.id))
            {
                Main.instance.kingdomBindActors[god.kingdom.id].Add(god);
            }
            god.GetData().health = int.MaxValue >> 2;
            god.GetData().level = ((Actor)pUser).GetData().level;
            god.GetData().firstName = Main.instance.godList[godID];
            return true;
        }
        public static bool aWaterPoleDamage(BaseSimObject pUser, WorldTile pTile = null)
        {
            if (pUser == null)
            {
                return false;
            }
            if (pTile == null)
            {
                pTile = pUser.currentTile;
            }
            List<Actor> targets = OthersHelper.getEnemyObjectInRange(pUser, pTile, 3f);
            float damage = ((Actor)pUser).GetCurStats().damage * 5f;
            foreach (Actor target in targets)
            {
                if (target.GetData().alive)
                {
                    target.CallMethod("getHit", damage, true, AttackType.Other, null, true);
                }
            }
            return true;
        }
        public static bool aFireworkDamage(BaseSimObject pUser,WorldTile pTile = null)
        {
            if (pUser == null)
            {
                return false;
            }
            List<WorldTile> tiles = OthersHelper.getTilesInRange(pTile, 20f);
            bool hasFound = false;
            foreach(WorldTile tile in tiles)
            {
                foreach(Actor actor in tile.units)
                {
                    if (actor.stats.id == "Nian")
                    {
                        pTile = actor.currentTile;
                        actor.CallMethod("getHit", 5000, true, AttackType.None, pUser, true);
                        hasFound = true;
                        break;
                    }
                }
                if (hasFound)
                {
                    break;
                }
            }
            Vector2 pos = new Vector2();
            Utils.ResourcesHelper.playSpell("firework", pUser.currentTile.pos, pTile.pos, 20f);
            return true;
        }
        public static bool aNianDie(BaseSimObject pUser, WorldTile pTile = null)
        {
            if (pUser == null)
            {
                return false;
            }
            pTile = pUser.currentTile;
            Utils.ResourcesHelper.playSpell("happySpringFestival", pTile.pos, pTile.pos, 20f);
            return true;
        }
        #endregion
    }
}
