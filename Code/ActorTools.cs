using CultivationWay;
using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;
using System;
using Microsoft.CSharp.RuntimeBinder;
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
            ActorStatus data = null;
            Main.instance.actorToData.TryGetValue(actor.GetInstanceID(), out data);
            if (data != null)
            {
                return data;
            }
            data = Reflection.GetField(typeof(Actor), actor, "data") as ActorStatus;
            Main.instance.actorToData[actor.GetInstanceID()] = data;
            return data;
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
            if (Main.instance.actorToCurStats.ContainsKey(actor.GetInstanceID()))
            {
                return Main.instance.actorToCurStats[actor.GetInstanceID()];
            }
            else
            {
                BaseStats curStats = Reflection.GetField(typeof(Actor), actor, "curStats") as BaseStats;
                Main.instance.actorToCurStats.Add(actor.GetInstanceID(), curStats);
                return curStats;
            }

        }
        /// <summary>
        /// 返回MoreStats
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static MoreStats GetMoreStats(this Actor actor)
        {
            if (actor == null||actor.GetData()==null)
            {
                return null;
            }
            try
            {
                MoreStats moreStats = Main.instance.actorToMoreStats[actor.GetData().actorID];
                return moreStats;
            }
            catch (KeyNotFoundException e)
            {
                MonoBehaviour.print("["+e.StackTrace+"]");
                MonoBehaviour.print(actor.GetData().actorID + "(" + actor.stats.race + ")的MoreStats不存在");
                MonoBehaviour.print("name:" + actor.GetData().firstName);
                MonoBehaviour.print("age:" + actor.GetData().age);
                actor.GetData().favorite = true;
            }
            return null;
        }
        /// <summary>
        /// 返回MoreActorData
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static MoreActorData GetMoreData(this Actor actor)
        {
            if (actor == null||actor.GetData()==null)
            {
                return null;
            }
            try
            {
                MoreActorData moreData = Main.instance.actorToMoreData[actor.GetData().actorID];
                return moreData;
            }
            catch (KeyNotFoundException e)
            {
                MonoBehaviour.print("[" + e.StackTrace + "]");
                MonoBehaviour.print(actor.GetData().actorID + "(" + actor.stats.race + ")的MoreData不存在");
                MonoBehaviour.print("name:" + actor.GetData().firstName);
                MonoBehaviour.print("age:" + actor.GetData().age);
                actor.GetData().favorite = true;
            }
            return null;
        }
        /// <summary>
        /// 返回actor的体质
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static SpecialBody GetSpecialBody(this Actor actor)
        {
            SpecialBodyLibrary specialBodyLibrary = AddAssetManager.specialBodyLibrary;
            return specialBodyLibrary.get(actor.GetMoreData().specialBody);
        }
        /// <summary>
        /// 返回actor的家族
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static Family GetFamily(this Actor actor)
        {
            try
            {
                return Main.instance.familys[actor.GetMoreData().familyID];
            }
            catch (KeyNotFoundException e)
            {
                MonoBehaviour.print(actor.GetMoreData().familyID);
                MonoBehaviour.print(e.StackTrace);
                return Main.instance.familys["甲"];
            }
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
            if (actor.GetMoreData().specialBody == null || actor.GetMoreData().specialBody == string.Empty)
            {
                actor.GetMoreData().specialBody = AddAssetManager.specialBodyLibrary.list.GetRandom().id;
            }
            newBody.id = actor.GetData().actorID;
            newBody.madeBy = actor.GetData().firstName;
            newBody.origin = actor.GetSpecialBody().origin;
            newBody.rank = Toolbox.randomInt(1, 7);
            if (newBody.rank > actor.GetSpecialBody().rank)
            {
                actor.GetMoreData().specialBody = newBody.id;
            }
            newBody.name = ChineseNameGenerator.getName("specialBody_name"+newBody.rank)+ChineseNameAsset.rankName1[newBody.rank-1];
            newBody.mod_damage = Toolbox.randomInt(0, 20 * newBody.rank);
            newBody.mod_health = Toolbox.randomInt(0, 20 * newBody.rank);
            newBody.mod_attack_speed = Toolbox.randomInt(-10, 10 * newBody.rank);
            newBody.mod_speed = Toolbox.randomInt(-10, 5 * newBody.rank);
            newBody.spellRelief = Toolbox.randomInt(0, 5 * newBody.rank);
            AddAssetManager.specialBodyLibrary.add(newBody);
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
        public static void copyMore(Actor from, Actor to)
        {
            MoreActorData originMoreActorData = from.GetMoreData();
            MoreStats originMoreStats = from.GetMoreStats();

            MoreActorData newMoreActorData = new MoreActorData();
            MoreStats newMoreStats = new MoreStats();

            newMoreActorData.cultisystem = originMoreActorData.cultisystem;
            newMoreActorData.bonusStats = originMoreActorData.bonusStats;
            newMoreActorData.coolDown = originMoreActorData.coolDown;
            newMoreActorData.element = originMoreActorData.element;
            newMoreActorData.familyID = originMoreActorData.familyID;
            newMoreActorData.familyName = originMoreActorData.familyName;
            newMoreActorData.magic = originMoreActorData.magic;
            newMoreActorData.specialBody = originMoreActorData.specialBody;
            newMoreActorData.canCultivate = originMoreActorData.canCultivate;

            newMoreStats.baseStats = originMoreStats.baseStats;
            newMoreStats.magic = originMoreStats.magic;
            newMoreStats.element = originMoreStats.element;
            newMoreStats.spells = originMoreStats.spells;
            newMoreStats.talent = originMoreStats.talent;
            newMoreStats.maxAge = originMoreStats.maxAge;

            Main.instance.actorToMoreData[to.GetData().actorID] = newMoreActorData;
            Main.instance.actorToMoreStats[to.GetData().actorID] = newMoreStats;
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
            return LocalizedTextManager.getText(actor.GetMoreData().cultisystem + realm);
        }
        #region updateStats所用
        /// <summary>
        /// 处理更新人物属性时的前部分
        /// </summary>
        /// <param name="actor"></param>
        public static void dealStatsHelper1(Actor actor)
        {
            ActorStatus data = actor.GetData();
            MoreStats morestats = actor.GetMoreStats();
            MoreActorData moredata = actor.GetMoreData();
            #region 种族法术与各类属性
            morestats.clear();
                morestats.spells.AddRange(Main.instance.raceFeatures[actor.stats.id].raceSpells);
            int realm = actor.getRealm();
            //每个境界属性均加上
                for (int i = 0; i < realm; i++)
                {
                    morestats.addAnotherStats(AddAssetManager.cultisystemLibrary.get(moredata.cultisystem).moreStats[i]);
                    morestats.addAnotherStats(Main.instance.familys[moredata.familyID].cultivationBook.stats[i]);
                }
            #endregion
            #region 法术去重
            List<ExtensionSpell> fixedSpells = new List<ExtensionSpell>();
            for (int i = 0; i < morestats.spells.Count; i++)
            {
                ExtensionSpell spell = morestats.spells[i];
                if (spell.GetSpellAsset().type.requiredLevel > data.level&&spell.GetSpellAsset().rarity<10)
                {
                    continue;
                }
                ExtensionSpellAsset spellAsset = spell.GetSpellAsset();
                if (!spellAsset.bannedCultisystem.Contains(moredata.cultisystem) && actor.kingdom != null && !spellAsset.bannedRace.Contains(actor.kingdom.raceID))
                {
                    bool flag = false;
                    for (int j = 0; j < fixedSpells.Count; j++)
                    {
                        ExtensionSpell spell1 = fixedSpells[j];
                        if (spell1.spellAssetID == spellAsset.id)
                        {
                            flag = true;
                            if (spell1.might < spell.might)
                            {
                                fixedSpells[j] = spell;
                            }
                            break;
                        }

                    }
                    if (!flag)
                    {
                        fixedSpells.Add(spell);
                    }
                }
            }
            fixedSpells.Sort((ExtensionSpell spell1, ExtensionSpell spell2) =>
            {
                ExtensionSpellAsset spellAsset1 = spell1.GetSpellAsset();
                ExtensionSpellAsset spellAsset2 = spell2.GetSpellAsset();
                if (spellAsset1.rarity > spellAsset2.rarity)
                {
                    return -1;
                }
                else if (spellAsset1.rarity == spellAsset2.rarity)
                {
                    if (spellAsset1.might > spellAsset2.might)
                    {
                        return -1;
                    }
                    else if (spellAsset1.might == spellAsset2.might)
                    {
                        if (spellAsset1.chineseElement.getImPurity() < spellAsset2.chineseElement.getImPurity())
                        {
                            return -1;
                        }
                        else
                        {
                            return 1;
                        }
                    }
                    else
                    {
                        return 1;
                    }
                }
                else
                {
                    return 1;
                }
            });
            morestats.spells.Clear();
            morestats.spells = fixedSpells;
            #endregion
            #region 冷却实现
                if (morestats.spells.Count > 0)
                {
                    foreach (ExtensionSpell spell in morestats.spells)
                    {
                        if (moredata.coolDown.ContainsKey(spell.spellAssetID))
                        {
                            continue;
                        }
                        moredata.coolDown[spell.spellAssetID] = spell.cooldown;
                    }
                }
            #endregion
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
            MoreActorData moredata = actor.GetMoreData();
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
            if (moredata.magic > actor.GetMoreStats().magic)
            {
                moredata.magic = actor.GetMoreStats().magic;
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
            float result = 1f;
            float element = actor.GetMoreData().element.getImPurity();
            float spellCount = actor.GetMoreData().coolDown.Count;
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
