using CultivationWay;
using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.CompilerServices;
using System.Reflection;

namespace Cultivation_Way
{
    static class ActorTools
    {
        /// <summary>
        /// 返回data(ActorStatus)
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static ActorStatus GetData(this Actor actor)
        {
            if (actor == null)
            {
                return null;
            }
            return ((ExtendedActor)actor).easyData;
        }
        /// <summary>
        /// 返回curStats(BaseStats)
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static BaseStats GetCurStats(this Actor actor)
        {
            if (actor == null)
            {
                return null;
            }
            return ((ExtendedActor)actor).easyCurStats;
        }
        /// <summary>
        /// 返回actor的体质
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static SpecialBody GetSpecialBody(this Actor actor)
        {
            SpecialBodyLibrary specialBodyLibrary = AddAssetManager.specialBodyLibrary;
            return specialBodyLibrary.get(((ExtendedActor)actor).extendedData.status.specialBody);
        }
        /// <summary>
        /// 返回actor的家族
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static Family GetFamily(this Actor actor)
        {
            return Main.instance.familys[((ExtendedActor)actor).extendedData.status.familyID];
        }
        public static bool canCastSpell(this ExtendedActor actor,ExtensionSpell spell)
        {
            return actor.extendedData.status.magic > spell.cost && spell.leftCool == 0;
        }
        //"haveOppsiteTrait"完全来自游戏内部，拷贝目的是为了减少反射带来额外开销
        public static bool haveOppositeTrait(this ActorStatus data, ActorTrait pTraitMain)
        {
            if (pTraitMain == null)
            {
                return false;
            }
            if (pTraitMain.oppositeArr == null)
            {
                return false;
            }
            for (int i = 0; i < pTraitMain.oppositeArr.Length; i++)
            {
                string item = pTraitMain.oppositeArr[i];
                if (data.traits.Contains(item))
                {
                    return true;
                }
            }
            return false;
        }
        //"inheritTraits"完全来自于游戏内部，拷贝目的是为了减少反射带来额外开销
        public static void inheritTraits(this ActorStatus data, List<string> pTraits)
        {
            for (int i = 0; i < pTraits.Count; i++)
            {
                string pID = pTraits[i];
                ActorTrait actorTrait = AssetManager.traits.get(pID);
                if (actorTrait != null && actorTrait.inherit != 0f)
                {
                    float num = Toolbox.randomFloat(0f, 100f);
                    if (actorTrait.inherit >= num && !data.traits.Contains(actorTrait.id) && !data.haveOppositeTrait(actorTrait))
                    {
                        data.addTrait(actorTrait.id);
                    }
                }
            }
        }
        /// <summary>
        /// 生成新的体质
        /// </summary>
        /// <param name="actor"></param>
        public static void generateNewBody(this Actor actor)
        {
            SpecialBody newBody = new SpecialBody();
            if (((ExtendedActor)actor).extendedData.status.specialBody == null || ((ExtendedActor)actor).extendedData.status.specialBody == string.Empty)
            {
                ((ExtendedActor)actor).extendedData.status.specialBody = AddAssetManager.specialBodyLibrary.list.GetRandom().id;
            }
            newBody.id = actor.GetData().actorID;
            newBody.madeBy = actor.GetData().firstName;
            newBody.origin = actor.GetSpecialBody().origin;
            newBody.rank = Toolbox.randomInt(1, 7);
            if (newBody.rank > actor.GetSpecialBody().rank)
            {
                ((ExtendedActor)actor).extendedData.status.specialBody = newBody.id;
            }
            newBody.name = ChineseNameGenerator.getName("specialBody_name"+newBody.rank)+ChineseNameAsset.rankName1[newBody.rank-1];
            newBody.mod_damage = Toolbox.randomInt(0, 20 * newBody.rank);
            newBody.mod_health = Toolbox.randomInt(0, 20 * newBody.rank);
            newBody.mod_attack_speed = Toolbox.randomInt(-10, 10 * newBody.rank);
            newBody.mod_speed = Toolbox.randomInt(-10, 5 * newBody.rank);
            newBody.spellRelief = Toolbox.randomInt(0, 5 * newBody.rank);
            AddAssetManager.specialBodyLibrary.add(newBody);
        }
        public static void learnNewSpell(this Actor actor)
        {
            ExtendedActor extendedActor = (ExtendedActor)actor;
            ExtensionSpell[] spells= Main.instance.familys[extendedActor.extendedData.status.familyID].cultivationBook.spells;
            for(int i = 0; i < spells.Length; i++)
            {
                if (spells[i] == null)
                {
                    continue;
                }
                extendedActor.extendedCurStats.addSpell(spells[i]);
            }
        }
        /// <summary>
        /// 返回ID为id的动物化形后的statID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static string getTranformID(string id)
        {
            return Utils.OthersHelper.UpperFirst(id + "Yao");
        }
        /// <summary>
        /// 尝试化形
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static bool tryTransform(this Actor actor)
        {
            if (!Main.instance.MoreActors.protoAndYao.GetFirsts().Contains(actor.stats.id))
            {
                return false;
            }
            if (actor.stats.id.EndsWith("Yao"))
            {
                return false;
            }
            if (!PowerActionLibrary.lightningPunishment(actor))
            {
                return false;
            }
            int level = actor.GetData().level;
            string originRace = actor.stats.race;
            string targetRace = Main.instance.MoreActors.protoAndYao.GetByFirst(originRace);
            //两个特判
            if (originRace == "chicken" && Toolbox.randomBool())
            {
                targetRace = "CockYao";
            }
            if (originRace == "crab" && Toolbox.randomBool())
            {
                targetRace = "LobsterYao";
            }
            if (Toolbox.randomChance(0.1f))
            {
                targetRace = "unit_Yao";
                if (Toolbox.randomChance(0.1f) && Main.instance.MoreActors.protoAndShengs[0].GetFirsts().Contains(originRace))
                {
                    int index = 0;
                    //又一个特判
                    if (originRace == "monkey" && Toolbox.randomChance(0.5f))
                    {
                        index = 1;
                    }
                    string tmp = Main.instance.MoreActors.protoAndShengs[index].GetByFirst(originRace);
                    if (Main.instance.creatureLimit[tmp] > 0)
                    {
                        targetRace = tmp;
                        Main.instance.creatureLimit[tmp]--;
                        level = 50;
                    }
                }
            }
            Actor transformTo = MapBox.instance.spawnNewUnit(targetRace, actor.currentTile, pSpawnHeight: 0f);
            copyActor(actor, transformTo);
            transformTo.GetData().level = level;
            if (level == 50)
            {
                transformTo.tryToUnite();
            }
            actor.killHimself(false, AttackType.GrowUp, false, false);
            return true;
        }
        /// <summary>
        /// 尝试统一，目前仅限妖族
        /// </summary>
        /// <param name="actor"></param>
        public static void tryToUnite(this Actor actor)
        {
            if (actor.stats.race == "Yao")
            {
                if (actor.city == null || actor.kingdom == null)
                {
                    return;
                }
                bool unite = true;
                List<Kingdom> unionKingdoms = new List<Kingdom>();

                foreach (Kingdom kingdom in MapBox.instance.kingdoms.list)
                {
                    if (!kingdom.isCiv() || kingdom.raceID != "Yao")
                    {
                        continue;
                    }

                    if (kingdom.king != null && kingdom.king != actor && kingdom.king.GetData().level >= actor.GetData().level)
                    {
                        unite = false;
                        break;
                    }
                    if (kingdom.getPopulationTotal() > 0)
                    {
                        Actor tmp = kingdom.getMaxLevelActor();

                        if (tmp != actor && tmp.GetData().level >= actor.GetData().level)
                        {
                            unite = false;
                            break;
                        }
                    }
                    if (actor.kingdom != kingdom)
                    {
                        unionKingdoms.Add(kingdom);
                    }
                }
                if (unite)
                {
                    List<City> temp = new List<City>();
                    if (!actor.city.isCapitalCity())
                    {
                        actor.city.CallMethod("makeOwnKingdom");
                    }
                    actor.kingdom.setKing(actor);
                    foreach (Kingdom kingdom in unionKingdoms)
                    {
                        foreach (City city in kingdom.cities)
                        {
                            temp.Add(city);
                        }
                    }
                    foreach (City city in temp)
                    {
                        city.joinAnotherKingdom(actor.kingdom, true);
                    }
                    WorldTools.logUnite(actor.kingdom);
                }
            }
        }
        /// <summary>
        /// 比较完全的复制个体，不复制等级，包括mod相关，actorID与InstanceID不复制
        /// </summary>
        /// <param name="from">拷贝来源</param>
        /// <param name="to">拷贝去向</param>
        public static void copyActor(Actor from, Actor to)
        {
            ai.ActorTool.copyUnitToOtherUnit(from, to);
            copyMore(from, to);
        }
        /// <summary>
        /// 复制moreStats和moreActorData
        /// </summary>
        /// <param name="from"></param>
        /// <param name="to"></param>
        public static void copyMore(Actor from, Actor to,bool compositionCopied = false)
        {
            ExtendedActor fromActor = (ExtendedActor)from;
            ExtendedActor toActor = (ExtendedActor)to;
            toActor.extendedCurStats.element = new ChineseElement(fromActor.extendedCurStats.element.baseElementContainer);
            toActor.extendedData.status.bonusStats = new MoreStats().addAnotherStats(fromActor.extendedData.status.bonusStats);
            toActor.extendedData.status.familyID = fromActor.extendedData.status.familyID;
            toActor.extendedData.status.familyName = fromActor.extendedData.status.familyName;
            toActor.extendedData.status.magic = fromActor.extendedData.status.magic;
            toActor.extendedData.status.specialBody = fromActor.extendedData.status.specialBody;
            toActor.extendedData.status.cultisystem = fromActor.extendedData.status.cultisystem;
            toActor.extendedData.status.canCultivate = fromActor.extendedData.status.canCultivate;
            if (compositionCopied)
            {
                toActor.extendedData.status.compositionSetting = fromActor.extendedData.status.compositionSetting;
                foreach(BaseSimObject baseSimObject in fromActor.compositions)
                {
                    ComposeTools.composeTwo(toActor, baseSimObject);
                }
            }
            //MonoBehaviour.print("successfully copy " + to.GetData().actorID + "'s more...");
        }
        public static int getRealm(this Actor actor)
        {
            int realm = actor.GetData().level;
            if (realm > 10)
            {
                realm = (realm + 9) / 10 + 9;
                if (realm > 20)
                {
                    realm = 20;
                }
            }
            return realm;
        }
        public static string getRealmName(this Actor actor)
        {
            int realm = actor.getRealm();
            return LocalizedTextManager.getText(((ExtendedActor)actor).extendedData.status.cultisystem + realm);
        }
        #region updateStats所用
        /// <summary>
        /// 处理更新人物属性时的前部分
        /// </summary>
        /// <param name="actor"></param>
        public static void dealStatsHelper1(Actor actor)
        {
            ExtendedActor actor1 = (ExtendedActor)actor;
            MoreStats morestats = actor1.extendedCurStats;
            MoreStatus moredata = actor1.extendedData.status;
            morestats.clear();
            int realm = actor.getRealm();
            morestats.addAnotherStats(AddAssetManager.cultisystemLibrary.get(moredata.cultisystem).moreStats[realm-1]);
            morestats.addAnotherStats(Main.instance.familys[moredata.familyID].cultivationBook.stats[realm-1]);
            morestats.addAnotherStats(moredata.bonusStats);
        }
        /// <summary>
        /// 数据最后处理
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static void dealStatsHelper2(Actor actor)
        {
            int maxArmor = 80 + actor.GetData().level / 6;
            ExtendedActor actor1 = (ExtendedActor)actor;
            MoreStatus moredata = actor1.extendedData.status;
            if (moredata.cultisystem == "bodying")
            {
                maxArmor += 5;
            }
            if (maxArmor > 99)
            {
                maxArmor = 99;
            }
            if (actor.GetCurStats().armor > maxArmor)
            {
                actor.GetCurStats().armor = maxArmor;
            }
            if (moredata.magic > actor1.extendedCurStats.magic)
            {
                moredata.magic = actor1.extendedCurStats.magic;
            }
            return;
        }
        public static City GetCity(Actor actor)
        {
            return actor.city;
        }
        #endregion
        #region Egg.update所用
        public static Actor getActor(Egg egg)
        {
            return egg.GetComponent<Actor>();
        }
        #endregion
        public static float getCombat(this Actor actor)
        {
            ExtendedActor actor1 = (ExtendedActor)actor;
            float result = 1f;
            float element = actor1.extendedCurStats.element.getImPurity();
            float spellCount = actor1.extendedCurStats.spells.Count;
            float specialBody = actor.GetSpecialBody().rank;
            float baseStats = (actor.GetCurStats().health >> 7) * (actor.GetCurStats().damage >> 5) / (100-actor.GetCurStats().armor)*actor.GetCurStats().attackSpeed;
            result = element * spellCount * specialBody * baseStats / 4;
            return result;
        }
        public static float getPotience(this Actor actor)
        {
            return 0f;
        }
    }
}
