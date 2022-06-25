using Cultivation_Way.Utils;
using CultivationWay;
using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    internal class ExtendedWorldActions
    {
        //StackSpellEffects
        //样例
        public static bool exampleSpell(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            NewEffectManager.spawnOn(spell.spellAssetID, pTarget.currentPosition, 0.01f);
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
        public static bool baseSpell(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            ExtendedActor user = (ExtendedActor)pUser;
            string realSpell = OthersHelper.getRealBaseSpell(user.extendedCurStats.element);
            Projectile p = Utils.OthersHelper.startProjectile(realSpell, pUser, pTarget);
            Reflection.SetField(p, "byWho", user);
            p.setStats(user.easyCurStats);
            p.targetObject = pTarget;
            return true;
        }
        //雷法
        public static bool lightningSpell(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
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
            NewEffectManager.spawnOn(spell.spellAssetID, pTarget.currentPosition, 0.03f);
            return true;
        }
        public static bool lightning1Spell(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            NewEffectManager.spawnOn(spell.spellAssetID, pTarget.currentPosition, ((ExtendedActor)pUser).easyData.level/2000f);
            float oriDamage = OthersHelper.getSpellDamage(spell, pUser, pTarget);
            //再进行伤害处理
            if (pTarget.objectType == MapObjectType.Actor)
            {
                ((ExtendedActor)pTarget).CallMethod("getHit", oriDamage, true, AttackType.Other, pUser, true);
            }
            else
            {
                ((ExtendedBuilding)pTarget).CallMethod("getHit", oriDamage, true, AttackType.Other, pUser, true);
            }
            pTarget.currentTile.setBurned();
            return true;
        }
        //基础火
        public static bool defaultFire(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            OthersHelper.hitEnemiesInRange(pUser, pTarget.currentTile, 2f, 0f, spell);
            NewEffectManager.spawnOn(spell.spellAssetID, pTarget.currentPosition, 0.01f);
            return true;
        }
        //剑阵
        public static bool swordsArray(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            OthersHelper.hitEnemiesInRange(pUser, pTarget.currentTile, 4f, 0f, spell);
            NewEffectManager.spawnOn(spell.spellAssetID, pTarget.currentPosition, 0.02f);
            return true;
        }
        //激光，目前只用于蛟龙
        public static bool laserSpell(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
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
            NewSpriteAnimation anim = NewEffectManager.spawnOn(spell.spellAssetID, new Vector3(end.x, end.y + 2.8f), new Vector3(xScale * 0.02f, 0.2f, 0));
            anim.m_gameobject.transform.localEulerAngles = new Vector3(0f, end.y - start.y, angle);//待修改
            

            return true;
        }
        public static bool barSpell(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            OthersHelper.hitEnemiesInRange(pUser, pTarget.currentTile, 10f, 0f, spell);
            Vector3 start = new Vector3(pUser.currentTile.posV3.x, pUser.currentTile.posV3.y - 0.5f);
            Vector3 end = new Vector3(pTarget.currentTile.posV3.x, pTarget.currentTile.posV3.y - 0.5f);
            float deltaY = pTarget.currentPosition.y - pUser.currentPosition.y;
            NewEffectManager.spawnOn(spell.spellAssetID, pTarget.currentPosition, new Vector3(-0.06f, -0.06f, 0))
                .m_gameobject.transform.localEulerAngles = new Vector3(0f, deltaY, 135);
            NewEffectManager.spawnOn(spell.spellAssetID, pTarget.currentPosition, new Vector3(0.06f, -0.06f, 0))
               .m_gameobject.transform.localEulerAngles = new Vector3(0f, deltaY, -135);
            NewEffectManager.spawnOn(spell.spellAssetID, pTarget.currentPosition, new Vector3(0.06f, 0.06f, 0))
               .m_gameobject.transform.localEulerAngles = new Vector3(0f, deltaY, 45);
            NewEffectManager.spawnOn(spell.spellAssetID, pTarget.currentPosition, new Vector3(-0.06f, 0.06f, 0))
               .m_gameobject.transform.localEulerAngles = new Vector3(0f, deltaY, -45);

            return true;
        }
        public static bool barDownSpell(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == pTarget || pUser == null || pTarget == null)
            {
                return false;
            }
            float rad = 15f;
            OthersHelper.hitEnemiesInRange(pUser, pUser.currentTile, rad, 0f, spell);
            Vector3 end1 = new Vector3(pUser.currentTile.posV3.x + 7.2f, pUser.currentTile.posV3.y);
            Vector3 end2 = new Vector3(pUser.currentTile.posV3.x, pUser.currentTile.posV3.y + 6f);
            Vector3 end3 = new Vector3(pUser.currentTile.posV3.x - 7.2f, pUser.currentTile.posV3.y);
            Vector3 end4 = new Vector3(pUser.currentTile.posV3.x, pUser.currentTile.posV3.y - 6f);
            NewEffectManager.spawnOn(spell.spellAssetID, end1, 0.015f);
            NewEffectManager.spawnOn(spell.spellAssetID, end2, 0.015f);
            NewEffectManager.spawnOn(spell.spellAssetID, end3, 0.015f);
            NewEffectManager.spawnOn(spell.spellAssetID, end4, 0.015f);
            return true;
        }
        //骷髅召唤
        public static bool summonSpell(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            ExtendedActor user = (ExtendedActor)pUser;
            if (pUser == null || user.stats.race.Contains("summon"))
            {
                return false;
            }
            int level = user.easyData.level;
            int num = (int)spell.might + level / 30;//威力取整作为召唤生物的个数

            for (int i = 0; i < num; i++)
            {
                WorldTile tile = pUser.currentTile.neighboursAll.GetRandom();
                NewEffectManager.spawnOn(spell.spellAssetID, tile.posV3, 0.01f);
                ExtendedActor summoned = (ExtendedActor)MapBox.instance.createNewUnit(spell.spellAssetID, tile);

                ((AiSystemActor)Reflection.GetField(typeof(Actor), summoned, "ai")).setJob("attacker");
                ((AiSystemActor)Reflection.GetField(typeof(Actor), summoned, "ai")).setTask("warrior_army_follow_leader");
                summoned.easyData.profession = UnitProfession.Warrior;

                summoned.kingdom = pUser.kingdom;
                if (pUser.city != null)
                {
                    pUser.city.addNewUnit(summoned);
                }
                if (user.easyData.level > 10 - level / 11)
                {
                    summoned.easyData.level = user.easyData.level - 10 + level / 11;
                }
                else
                {
                    summoned.easyData.level = 1;
                }
                summoned.easyData.firstName = user.easyData.firstName + "召唤物";
                summoned.easyData.health = user.easyCurStats.health / 3;
                summoned.setStatsDirty();
            }
            return true;
        }
        //单独召唤
        public static bool summonTianSpell(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            ExtendedActor user = (ExtendedActor)pUser;
            if (pUser == null || user.stats.race.Contains("summon"))
            {
                return false;
            }
            int num = (int)spell.might;//威力取整作为召唤生物的等级因素
            //设置生成位置和动画位置
            WorldTile tile1 = user.currentTile;
            Vector2 pos = new Vector2(tile1.posV3.x + 3f, tile1.posV3.y + 12f);

            ExtendedActor summoned = (ExtendedActor)MapBox.instance.createNewUnit(spell.spellAssetID, tile1);
            Reflection.SetField(summoned, "hitboxZ", 10f);
            NewEffectManager.spawnOn(spell.spellAssetID, pos, 0.15f);
            ((AiSystemActor)Reflection.GetField(typeof(Actor), summoned, "ai")).setJob("attacker");
            summoned.easyData.profession = UnitProfession.Warrior;
            summoned.kingdom = pUser.kingdom;
            if (user.city != null)
            {
                user.city.addNewUnit(summoned);
            }
            if (user.easyData.level > 10 - num)
            {
                summoned.easyData.level = user.easyData.level - 10 + num;
            }
            else
            {
                summoned.easyData.level = 1;
            }
            summoned.easyData.firstName = user.easyData.firstName + "召唤物";
            summoned.easyData.health = user.easyCurStats.health;
            summoned.setStatsDirty();
            return true;
        }
        public static bool summonTianSpell1(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            ExtendedActor user = (ExtendedActor)pUser;
            if (pUser == null || user.stats.race.Contains("summon") || ExtendedWorldData.instance.creatureLimit[spell.spellAssetID] <= 0)
            {
                return false;
            }
            ExtendedWorldData.instance.creatureLimit[spell.spellAssetID]--;
            //设置生成位置和动画位置
            WorldTile tile1 = pUser.currentTile;
            Vector2 pos = new Vector2(tile1.posV3.x, tile1.posV3.y);
            ExtendedActor summoned = (ExtendedActor)MapBox.instance.createNewUnit(spell.spellAssetID, tile1);
            NewEffectManager.spawnOn("explosion", pos, 0.01f);
            summoned.kingdom = pUser.kingdom;
            if (user.city != null)
            {
                user.city.addNewUnit(summoned);
            }
            user.kingdom.setKing(summoned);
            int level = MapBox.instance.mapStats.year / 50 + 1;
            if (level > 110)
            {
                level = 110;
            }
            summoned.easyData.level = level;
            summoned.easyData.health = int.MaxValue >> 2;
            #region 复制
            Reflection.SetField(summoned, "s_personality", (PersonalityAsset)Reflection.GetField(typeof(Actor), user, "s_personality"));
            ActorTools.copyActor(user, summoned);
            user.killHimself(false, AttackType.GrowUp, false, false);
            #endregion
            summoned.setStatsDirty();
            return true;
        }
        //法相天地
        public static bool Shengtixianhua(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            ExtendedActor user = (ExtendedActor)pUser;
            if (pUser == null || user.easyData.health < user.easyCurStats.health * 3 / 4)
            {
                return false;
            }
            Vector3 start = new Vector3(pUser.currentTile.posV3.x, pUser.currentTile.posV3.y + 2f);
            int time = 2;
            SpecialBody body = user.GetSpecialBody();
            if (body.rank > 1)
            {
                NewSpriteAnimation anim = NewEffectManager.spawnOn(Utils.OthersHelper.getOriginBodyID(body), pUser, 0.02f);
                new WorldTimer(time, new System.Action(() =>
                 {
                     anim.stop();
                 }));
            }

            ExtendedActorStatus moredata = ((ExtendedActor)pUser).extendedData.status;
            MoreStats bonusStats = new MoreStats();
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
        //发射类
        public static bool projectileLike(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == null || pUser == pTarget || pTarget == null
                ||!pUser.base_data.alive||!pTarget.base_data.alive)
            {
                return false;
            }
            ExtendedActor user = (ExtendedActor)pUser;
            Projectile p = Utils.OthersHelper.startProjectile(spell.spellAssetID, pUser, pTarget);
            Reflection.SetField(p, "byWho", user);
            p.setStats(user.easyCurStats);
            p.targetObject = pTarget;
            return true;
        }
        #region 十二祖巫
        public static bool RuShou(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == null||!pUser.base_data.alive)
            {
                return false;
            }
            //添加防护罩
            ((Actor)pUser).CallMethod("addStatusEffect", "shield", 30f);
            return true;
        }
        public static bool XuanMing1(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == null || !pUser.base_data.alive)
            {
                return false;
            }
            //召唤雨云（待强化）
            return true;
        }
        public static bool HouTu1(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == null || !pUser.base_data.alive)
            {
                return false;
            }
            //地刺
            NewSpriteAnimation anim = NewEffectManager.spawnOn("sunkens", pUser.currentTile, 0.5f);
            WorldTile lastTile = pUser.currentTile;
            WorldTile currTile;
            float damage;
            anim.setFrameAction(-1, new System.Action(() =>
             {
                 currTile = anim.getCurrentTile();
                 if (currTile == null || currTile == lastTile)
                 {
                     return;
                 }
                 lastTile = currTile;
                 FastReflection.mapbox_getObjectsInChunks(MapBox.instance, lastTile, 1, MapObjectType.All);
                 foreach (BaseSimObject o in Main.instance.temp_map_objects)
                 {
                     if (o.kingdom != null && pUser.kingdom != null && o.kingdom.isEnemy(pUser.kingdom))
                     {
                         continue;
                     }
                     damage = OthersHelper.getSpellDamage(spell, pUser, o);
                     if (o.objectType == MapObjectType.Actor)
                     {
                         FastReflection.actor_getHit((ExtendedActor)o, damage, true, AttackType.Other, pUser, false);
                     }
                     else if (o.objectType == MapObjectType.Building)
                     {
                         FastReflection.building_getHit((ExtendedBuilding)o, damage, true, AttackType.Other, pUser, false);
                     }
                 }
             }));
            return true;
        }
        public static bool HouTu2(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == null || !pUser.base_data.alive)
            {
                return false;
            }
            //召唤亡灵

            return true;
        }
        public static bool HouTu3(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == null || !pUser.base_data.alive)
            {
                return false;
            }
            //召唤刑天
            ExtendedActor user = (ExtendedActor)pUser;
            WorldTile tile = pUser.currentTile.neighboursAll.GetRandom();
            NewEffectManager.spawnOn("XingTian", tile.posV3, 0.01f);
            ExtendedActor summoned = (ExtendedActor)MapBox.instance.createNewUnit(spell.spellAssetID, tile);

            ((AiSystemActor)Reflection.GetField(typeof(Actor), summoned, "ai")).setJob("attacker");
            ((AiSystemActor)Reflection.GetField(typeof(Actor), summoned, "ai")).setTask("warrior_army_follow_leader");
            summoned.easyData.profession = UnitProfession.Warrior;

            summoned.kingdom = pUser.kingdom;
            if (pUser.city != null)
            {
                pUser.city.addNewUnit(summoned);

            }
            if (user.easyData.level > 5)
            {
                summoned.easyData.level = user.easyData.level - 5;
            }
            else
            {
                summoned.easyData.level = 1;
            }
            summoned.easyData.health = user.easyCurStats.health / 3;
            summoned.setStatsDirty();
            return true;
        }
        public static bool ZhuJiuYin(ExtendedSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null)
        {
            if (pUser == null || !pUser.base_data.alive)
            {
                return false;
            }
            //时间暂停
            ExtendedWorldData.instance.chunkTimeStop = true;
            MapChunk centerChunk = pUser.currentTile.chunk;
            ExtendedWorldData.instance.stoppedZone.x1 = centerChunk.x - 1;
            ExtendedWorldData.instance.stoppedZone.x2 = centerChunk.x + 1;
            ExtendedWorldData.instance.stoppedZone.y1 = centerChunk.y + 1;
            ExtendedWorldData.instance.stoppedZone.y2 = centerChunk.y - 1;
            new WorldTimer(9f, new System.Action(()=>
            {
                ExtendedWorldData.instance.chunkTimeStop = false;
            }));
            return true;
        }
        public static bool ZhuRong(BaseSimObject pTarget,WorldTile pTile = null)
        {
            BaseSimObject attackTarget = pTarget.GetValue<BaseSimObject>("attackTarget", Types.t_BaseSimObject);
            if (attackTarget == null)
            {
                return false;
            }
            System.Action<WorldTile, bool> worldTile_setFire = (System.Action<WorldTile, bool>)pTarget.currentTile.GetFastMethod("setFire", Types.t_WorldTile);
            MapBox.instance.loopWithBrush(pTarget.currentTile, Brush.get(4, "circ_"), new PowerActionWithID(ActionLibrary.burningFeetEffectTileDraw), "burningFeetEffect");
            if (!pTarget.currentTile.Type.liquid)
            {
                worldTile_setFire(pTarget.currentTile, true);
            }
            for (int i = 0; i < pTarget.currentTile.neighbours.Count; i++)
            {
                WorldTile worldTile = pTarget.currentTile.neighbours[i];
                worldTile_setFire(worldTile, false);
                worldTile.setBurned(-1);
            }
            return true;
        }
        #endregion

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
            NewEffectManager.spawnOn("lightningPunishment", pos, 0.08f);
            List<Actor> targets = OthersHelper.getEnemyObjectInRange(pUser, pTile, 6f);
            foreach (ExtendedActor target in targets)
            {
                if (target.easyData.alive)
                {
                    target.CallMethod("getHit", 300f, true, AttackType.None, null, true);
                }
            }
            return true;
        }
        public static bool aTransformToGod(BaseSimObject pUser, WorldTile pTile = null)
        {
            ExtendedActor user = (ExtendedActor)pUser;
            if (user.easyData.level <= 10 || Toolbox.randomChance(0.15f))
            {
                return false;
            }
            string godID = string.Empty;
            foreach (string key in ExtendedWorldData.instance.godList.Keys)
            {
                if (ExtendedWorldData.instance.creatureLimit[key] > 0)
                {
                    godID = key;
                    break;
                }
            }
            if (godID == string.Empty)
            {
                return false;
            }
            ExtendedWorldData.instance.creatureLimit[godID]--;

            ExtendedActor god = (ExtendedActor)MapBox.instance.createNewUnit(godID, pTile);
            ActorTools.copyActor(user, god);
            god.kingdom = pUser.kingdom;
            if (pUser.city != null)
            {
                pUser.city.addNewUnit(god,true);
            }
            ExtendedKingdomStats.setStatus(god.kingdom.id, godID, 1f);
            if (ExtendedWorldData.instance.kingdomBindActors.ContainsKey(god.kingdom.id))
            {
                ExtendedWorldData.instance.kingdomBindActors[god.kingdom.id].Add(god);
            }
            god.easyData.health = int.MaxValue >> 2;
            god.easyData.level = user.easyData.level;
            return true;
        }
        public static bool aTransformToXingTian(BaseSimObject pUser, WorldTile pTile = null)
        {
            ExtendedActor user = (ExtendedActor)pUser;
            if (!FastReflection.actor_isProfession(user,UnitProfession.Warrior)||Toolbox.randomChance(0.9f))
            {
                return false;
            }

            ExtendedActor xt = (ExtendedActor)MapBox.instance.createNewUnit("XingTian", pTile);
            ActorTools.copyActor(user, xt);
            xt.kingdom = pUser.kingdom;
            if (pUser.city != null)
            {
                pUser.city.addNewUnit(xt, true, false);
            }
            ((AiSystemActor)Reflection.GetField(typeof(Actor), xt, "ai")).setJob("attacker");
            ((AiSystemActor)Reflection.GetField(typeof(Actor), xt, "ai")).setTask("warrior_army_follow_leader");
            xt.easyData.profession = UnitProfession.Warrior;
            xt.easyData.health = int.MaxValue >> 2;
            xt.easyData.level = user.easyData.level;
            return true;
        }
        public static bool aSimpleRangeDamage(BaseSimObject pUser, WorldTile pTile = null)
        {
            if (pUser == null)
            {
                return true;
            }
            if (pTile == null)
            {
                pTile = pUser.currentTile;
            }
            List<Actor> targets = OthersHelper.getEnemyObjectInRange(pUser, pTile, 3f);
            ExtendedActor user = (ExtendedActor)pUser;
            float damage = user.easyCurStats.damage;
            foreach (ExtendedActor target in targets)
            {
                if (target.easyData.alive)
                {
                    target.CallMethod("getHit", damage, true, AttackType.Other, null, true);
                }
            }
            return true;
        }
        public static bool aFireworkDamage(BaseSimObject pUser, WorldTile pTile = null)
        {
            if (pUser == null)
            {
                return false;
            }
            List<WorldTile> tiles = OthersHelper.getTilesInRange(pTile, 20f);
            bool hasFound = false;
            foreach (WorldTile tile in tiles)
            {
                foreach (Actor actor in tile.units)
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
            NewEffectManager.spawnOn("firework", pTile.posV3, 0.2f);
            return true;
        }
        public static bool aNianDie(BaseSimObject pUser, WorldTile pTile = null)
        {
            if (pUser == null)
            {
                return false;
            }
            pTile = pUser.currentTile;
            NewEffectManager.spawnOn("happySpringFestival", pTile.posV3, 0.2f);
            return true;
        }
        public static bool dizz(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget == null)
            {
                return false;
            }

            return true;
        }
        //public static bool teleportRandom(BaseSimObject pTarget, WorldTile pTile = null)
        //{
        //    WorldTile worldTile;
            
        //    if (pTile.region == null||pTile.region.island==null)
        //    {
        //        IslandsCalculator islandsCalculator = Reflection.GetField(typeof(MapBox), MapBox.instance, "islandsCalculator") as IslandsCalculator;
        //        worldTile = islandsCalculator.getRandomGround();
        //    }
        //    else
        //    {
        //        MapRegion random = pTile.region.island.regions.GetRandom();
        //        worldTile = ((random != null) ? random.tiles.GetRandom<WorldTile>() : null);
        //    }
        //    WorldTile worldTile2 = worldTile;
        //    if (worldTile2 == null)
        //    {
        //        return false;
        //    }
        //    if (worldTile2.Type.block)
        //    {
        //        return false;
        //    }
        //    if (!worldTile2.Type.ground)
        //    {
        //        return false;
        //    }
        //    Actor actor = (Actor)pTarget;
        //    string text = actor.stats.effect_teleport;
        //    if (text == string.Empty)
        //    {
        //        text = "fx_teleport_blue";
        //    }
        //    QualityChanger qualityChanger = Reflection.GetField(typeof(MapBox), MapBox.instance, "qualityChanger") as QualityChanger;
        //    if (!qualityChanger.isFullLowRes())
        //    {
        //        BaseStats curStats = actor.GetCurStats();
        //        BaseEffectController controller = MapBox.instance.stackEffects.CallMethod("get", text) as BaseEffectController;
        //        controller.CallMethod("spawnAt",pTarget.currentPosition, curStats.scale);
        //        BaseEffect baseEffect = controller.CallMethod("spawnAt",worldTile2.posV3, curStats.scale) as BaseEffect;
        //        if (baseEffect != null)
        //        {
        //            baseEffect.GetComponent<SpriteAnimation>().setFrameIndex(9);
        //        }
        //    }
        //    actor.cancelAllBeh(null);
        //    actor.CallMethod("spawnOn",worldTile2, 0f);
        //    return true;
        //}
        #endregion
    }
}
