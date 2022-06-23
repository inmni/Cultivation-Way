using CultivationWay;
using ReflectionUtility;
using System.Collections.Generic;

namespace Cultivation_Way
{
    internal static class ActorTools
    {
        static List<string> temp_stringList = new List<string>(128);
        static List<int> temp_intList = new List<int>(128);
        /// <summary>
        /// 返回actor的体质
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static SpecialBody GetSpecialBody(this ExtendedActor actor)
        {
            SpecialBodyLibrary specialBodyLibrary = AddAssetManager.specialBodyLibrary;
            return specialBodyLibrary.get(actor.extendedData.status.specialBody);
        }
        public static Family GetFamily(this ExtendedActor actor)
        {
            Family family;

            if(!ExtendedWorldData.instance.familys.TryGetValue(actor.extendedData.status.familyID, out family))
            {
                family = new Family(actor.extendedData.status.familyName);
                actor.extendedData.status.familyID = family.id;
                ExtendedWorldData.instance.familys[actor.extendedData.status.familyID] = family;
            }
            return family;
        }
        public static Family GetFamily(this ExtendedActorStatus status)
        {
            Family family;

            if (!ExtendedWorldData.instance.familys.TryGetValue(status.familyID, out family))
            {
                family = new Family(status.familyName);
                status.familyID = family.id;
                ExtendedWorldData.instance.familys[status.familyID] = family;
            }
            return family;
        }
        public static void restoreAllHealth(this ExtendedActor pActor)
        {
            pActor.easyData.health = int.MaxValue >> 4;
        }
        public static void generateExtendedData(this ExtendedActor pActor,ExtendedActor inheritFrom=null)
        {
            Family family;
            MoreData res = new MoreData();
            ActorStats stats = pActor.stats;
            ExtendedActorStats eStats = Main.instance.extendedActorStatsLibrary[stats.id];
            pActor.extendedData = res;
            if (Toolbox.randomChance(eStats.cultivateChance))
            {
                res.status.canCultivate = true;
            }
            pActor.easyData.level = eStats.initialLevel;
            if (inheritFrom != null)
            {
                //设置修炼体系
                res.status.cultisystem = inheritFrom.extendedData.status.cultisystem;
                if (Toolbox.randomChance(0.2f) && pActor.easyData.culture != null)
                {
                    Culture culture = MapBox.instance.cultures.get(pActor.easyData.culture);
                    int count = culture.list_tech_ids.Count;
                    temp_intList.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        if (culture.list_tech_ids[i].StartsWith("culti_"))
                        {
                            temp_intList.Add(i);
                        }
                    }
                    if (temp_intList.Count == 0)
                    {
                        res.status.cultisystem = Main.instance.extendedActorStatsLibrary[pActor.easyData.statsID].defaultCultisystem;
                    }
                    else
                    {
                        res.status.cultisystem = culture.list_tech_ids[temp_intList.GetRandom()].Remove(0, 6);
                    }
                }
                //继承体质
                if (Toolbox.randomChance(inheritFrom.GetSpecialBody().inheritChance))
                {
                    res.status.specialBody = inheritFrom.extendedData.status.specialBody;
                }
                else
                {
                    if (Toolbox.randomChance(0.001f))
                    {
                        res.status.specialBody = AddAssetManager.specialBodyLibrary.list.GetRandom().id;
                    }
                    else
                    {
                        res.status.specialBody = "FT";
                    }
                }
                //继承灵根
                res.status.chineseElement = new ChineseElement(inheritFrom.extendedData.status.chineseElement.baseElementContainer);
                res.status.chineseElement.deflectTo(ExtendedWorldData.instance.chunkToElement[inheritFrom.currentTile.chunk.id].baseElementContainer);
                res.status.chineseElement.deflectTo(Main.instance.extendedActorStatsLibrary[pActor.easyData.statsID].preferedElement,
                                                eStats.preferedElementScale);
                //选择继承名字和家族
                if (Toolbox.randomChance(0.05f))
                {
                    res.status.familyName = stats.unit ?
                        AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_startList.GetRandom() :
                        AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_endList.GetRandom();
                    res.status.familyID = res.status.familyName;
                }
                else
                {
                    res.status.familyName = inheritFrom.extendedData.status.familyName;
                    res.status.familyID = inheritFrom.extendedData.status.familyID;
                }

            }
            else
            {
                //设置修炼体系
                if (Toolbox.randomChance(0.2f)&& pActor.easyData.culture != null&& pActor.easyData.culture!=string.Empty)
                {
                    Culture culture = MapBox.instance.cultures.get(pActor.easyData.culture);
                    int count = culture.list_tech_ids.Count;
                    temp_intList.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        if (culture.list_tech_ids[i].StartsWith("culti_"))
                        {
                            temp_intList.Add(i);
                        }
                    }
                    if (temp_intList.Count == 0)
                    {
                        res.status.cultisystem = eStats.defaultCultisystem;
                    }
                    else
                    {
                        res.status.cultisystem = culture.list_tech_ids[temp_intList.GetRandom()].Remove(0,6);
                    }
                }
                else
                {
                    res.status.cultisystem = eStats.defaultCultisystem;
                }
                //生成体质
                if (Toolbox.randomChance(0.001f))
                {
                    res.status.specialBody = AddAssetManager.specialBodyLibrary.list.GetRandom().id;
                }
                else
                {
                    res.status.specialBody = "FT";
                }
                //生成灵根
                res.status.chineseElement = new ChineseElement();
                res.status.chineseElement.deflectTo(eStats.preferedElement,
                                                eStats.preferedElementScale);
                //选择生成名字和家族
                res.status.familyName = stats.unit ?
                    AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_startList.GetRandom() :
                    AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_endList.GetRandom();
                res.status.familyID = res.status.familyName;
            }
            if (eStats.fixedName != null)
            {
                pActor.easyData.firstName = eStats.fixedName;
            }
            else
            {
                pActor.easyData.firstName = ChineseNameGenerator.getCreatureName(stats.nameTemplate, res.status.familyName, stats.unit);
            }
            family = pActor.GetFamily();
            //获取修炼功法
            res.status.cultiBook = family.getbook(res.status, CultivationBookType.CULTIVATE);
            if (res.status.cultiBook == null)
            {
                UnityEngine.Debug.Log("[ActorTools]:null cultiBook");
            }
        }
        public static MoreData generateExtendedStatus(this ActorData pData, ExtendedActor inheritFrom = null)
        {
            Family family;
            MoreData res = new MoreData();
            ActorStats stats = AssetManager.unitStats.get(pData.status.statsID);
            ExtendedActorStats eStats = Main.instance.extendedActorStatsLibrary[stats.id];
            if (Toolbox.randomChance(eStats.cultivateChance))
            {
                res.status.canCultivate = true;
            }
            pData.status.level = eStats.initialLevel;
            if (inheritFrom != null)
            {
                //设置修炼体系
                res.status.cultisystem = inheritFrom.extendedData.status.cultisystem;
                if (Toolbox.randomChance(0.2f) && pData.status.culture != null&& pData.status.culture != string.Empty)
                {
                    Culture culture = MapBox.instance.cultures.get(pData.status.culture);
                    int count = culture.list_tech_ids.Count;
                    temp_intList.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        if (culture.list_tech_ids[i].StartsWith("culti_"))
                        {
                            temp_intList.Add(i);
                        }
                    }
                    if (temp_intList.Count == 0)
                    {
                        res.status.cultisystem = Main.instance.extendedActorStatsLibrary[pData.status.statsID].defaultCultisystem;
                    }
                    else
                    {
                        res.status.cultisystem = culture.list_tech_ids[temp_intList.GetRandom()].Remove(0, 6);
                    }
                }
                //继承体质
                if (Toolbox.randomChance(inheritFrom.GetSpecialBody().inheritChance))
                {
                    res.status.specialBody = inheritFrom.extendedData.status.specialBody;
                }
                else
                {
                    if (Toolbox.randomChance(0.001f))
                    {
                        res.status.specialBody = AddAssetManager.specialBodyLibrary.list.GetRandom().id;
                    }
                    else
                    {
                        res.status.specialBody = "FT";
                    }
                }
                //继承灵根
                res.status.chineseElement = new ChineseElement(inheritFrom.extendedData.status.chineseElement.baseElementContainer);
                res.status.chineseElement.deflectTo(Main.instance.extendedActorStatsLibrary[pData.status.statsID].preferedElement,
                                                eStats.preferedElementScale);
                //选择继承名字和家族
                if (Toolbox.randomChance(0.05f))
                {
                    res.status.familyName = stats.unit ?
                        AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_startList.GetRandom() :
                        AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_endList.GetRandom();
                    res.status.familyID = res.status.familyName;
                }
                else
                {
                    res.status.familyName = inheritFrom.extendedData.status.familyName;
                    res.status.familyID = inheritFrom.extendedData.status.familyID;
                }
                
                
            }
            else
            {
                //设置修炼体系
                if (Toolbox.randomChance(0.2f)&& pData.status.culture != null)
                {
                    Culture culture = MapBox.instance.cultures.get(pData.status.culture);
                    int count = culture.list_tech_ids.Count;
                    temp_intList.Clear();
                    for (int i = 0; i < count; i++)
                    {
                        if (culture.list_tech_ids[i].StartsWith("culti_"))
                        {
                            temp_intList.Add(i);
                        }
                    }
                    if (temp_intList.Count == 0)
                    {
                        res.status.cultisystem = Main.instance.extendedActorStatsLibrary[pData.status.statsID].defaultCultisystem;
                    }
                    else
                    {
                        res.status.cultisystem = culture.list_tech_ids[temp_intList.GetRandom()].Remove(0, 6);
                    }
                }
                else
                {
                    res.status.cultisystem = Main.instance.extendedActorStatsLibrary[pData.status.statsID].defaultCultisystem;
                }
                //生成体质
                if (Toolbox.randomChance(0.001f))
                {
                    res.status.specialBody = AddAssetManager.specialBodyLibrary.list.GetRandom().id;
                }
                else
                {
                    res.status.specialBody = "FT";
                }
                //生成灵根
                res.status.chineseElement = new ChineseElement(inheritFrom.extendedData.status.chineseElement.baseElementContainer);
                res.status.chineseElement.deflectTo(Main.instance.extendedActorStatsLibrary[pData.status.statsID].preferedElement,
                                                eStats.preferedElementScale);
                //选择生成名字和家族
                res.status.familyName = stats.unit ?
                    AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_startList.GetRandom() :
                    AddAssetManager.chineseNameGenerator.get(stats.nameTemplate).addition_endList.GetRandom();
                res.status.familyID = res.status.familyName;
            }
            if (eStats.fixedName != null)
            {
                pData.status.firstName = eStats.fixedName;
            }
            else
            {
                pData.status.firstName = ChineseNameGenerator.getCreatureName(stats.nameTemplate, res.status.familyName, stats.unit);
            }
            family = res.status.GetFamily();
            //获取修炼功法
            res.status.cultiBook = family.getbook(res.status,CultivationBookType.CULTIVATE); 
            if (res.status.cultiBook == null)
            {
                UnityEngine.Debug.Log("[ActorTools]:null cultiBook");
            }
            return res;
        }
        public static bool canCastSpell(this ExtendedActor actor, ExtendedSpell spell)
        {
            return actor.easyData.experience > spell.cost;
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
        public static void generateNewBody(this ExtendedActor pActor)
        {
            SpecialBody newBody = new SpecialBody();
            if (pActor.extendedData.status.specialBody == null || pActor.extendedData.status.specialBody == string.Empty)
            {
                pActor.extendedData.status.specialBody = AddAssetManager.specialBodyLibrary.list.GetRandom().id;
            }
            newBody.id = pActor.easyData.actorID;
            newBody.madeBy = pActor.easyData.firstName;
            newBody.origin = pActor.GetSpecialBody().origin;
            newBody.rank = Toolbox.randomInt(1, 7);
            if (newBody.rank > pActor.GetSpecialBody().rank)
            {
                pActor.extendedData.status.specialBody = newBody.id;
            }
            newBody.name = ChineseNameGenerator.getName("specialBody_name" + newBody.rank) + ChineseNameAsset.rankName1[newBody.rank - 1];
            newBody.mod_damage = Toolbox.randomInt(0, 20 * newBody.rank);
            newBody.mod_health = Toolbox.randomInt(0, 20 * newBody.rank);
            newBody.mod_attack_speed = Toolbox.randomInt(-10, 10 * newBody.rank);
            newBody.mod_speed = Toolbox.randomInt(-10, 5 * newBody.rank);
            newBody.spellRelief = Toolbox.randomInt(0, 5 * newBody.rank);
            AddAssetManager.specialBodyLibrary.add(newBody);
        }
        public static bool canLearnSpell(this ExtendedActor pActor,string spellID)
        {
            ExtendedSpellAsset asset = AddAssetManager.extensionSpellLibrary.get(spellID);
            
            return asset.allowCultisystem(AddAssetManager.cultisystemLibrary.get(pActor.extendedData.status.cultisystem).flag)
                &&!asset.bannedRace.Contains(pActor.stats.race)&&asset.requiredLevel<=pActor.easyData.level
                &&ChineseElement.getMatchDegree(pActor.extendedData.status.chineseElement,asset.chineseElement,true)<=6400;
        }
        public static void tryLearnNewSpell(this ExtendedActor pActor)
        {
            if (Toolbox.randomChance(0.01f))
            {
                string spellID = AddAssetManager.extensionSpellLibrary.spellList.GetRandom();
                for (int j = 0; j < pActor.extendedData.status.spells.Count; j++)
                {
                    if (spellID == pActor.extendedData.status.spells[j].spellAssetID)
                    {
                        pActor.extendedData.status.spells[j].might *= 1.05f;
                        return;
                    }
                }
                //UnityEngine.Debug.Log(pActor.easyData.firstName + "_" + spellID);
                pActor.extendedData.status.spells.Add(new ExtendedSpell(spellID));
                return;
            }
            bool having = false;
            for(int i = 0; i < pActor.extendedData.status.cultiBook.spellCount; i++)
            {
                for (int j = 0; j < pActor.extendedData.status.spells.Count; j++) {
                    if (pActor.extendedData.status.cultiBook.spells[i].spellAssetID==pActor.extendedData.status.spells[j].spellAssetID)
                    {
                        having = true;
                        break;
                    }
                }
                if (!having&& pActor.canLearnSpell(pActor.extendedData.status.cultiBook.spells[i].spellAssetID))
                {
                    UnityEngine.Debug.Log(pActor.easyData.firstName + "_" + pActor.extendedData.status.cultiBook.spells[i].spellAssetID);
                    pActor.extendedData.status.spells.Add(new ExtendedSpell(pActor.extendedData.status.cultiBook.spells[i]));
                    return;
                }
                having = false;
            }
            if (pActor.extendedData.status.spells.Count != 0)
            {
                pActor.extendedData.status.spells[Toolbox.randomInt(0, pActor.extendedData.status.spells.Count)].might *= 1.05f;
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
        public static bool tryTransform(this ExtendedActor actor)
        {
            if (!Main.instance.moreActors.protoAndYao.GetFirsts().Contains(actor.stats.id))
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
            int level = actor.easyData.level;
            string originRace = actor.stats.race;
            string targetRace = Main.instance.moreActors.protoAndYao.GetByFirst(originRace);
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
                if (Toolbox.randomChance(0.1f) && Main.instance.moreActors.protoAndShengs[0].GetFirsts().Contains(originRace))
                {
                    int index = 0;
                    //又一个特判
                    if (originRace == "monkey" && Toolbox.randomChance(0.5f))
                    {
                        index = 1;
                    }
                    string tmp = Main.instance.moreActors.protoAndShengs[index].GetByFirst(originRace);
                    if (ExtendedWorldData.instance.creatureLimit[tmp] > 0)
                    {
                        targetRace = tmp;
                        ExtendedWorldData.instance.creatureLimit[tmp]--;
                        level = 50;
                    }
                }
            }
            ExtendedActor transformTo = (ExtendedActor)MapBox.instance.spawnNewUnit(targetRace, actor.currentTile, pSpawnHeight: 0f);
            copyActor(actor, transformTo);
            transformTo.easyData.level = level;
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
        public static void tryToUnite(this ExtendedActor actor)
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

                    if (kingdom.king != null && kingdom.king != actor && ((ExtendedActor)kingdom.king).easyData.level >= actor.easyData.level)
                    {
                        unite = false;
                        break;
                    }
                    if (kingdom.getPopulationTotal() > 0)
                    {
                        ExtendedActor tmp = kingdom.getMaxLevelActor();

                        if (tmp != actor && tmp.easyData.level >= actor.easyData.level)
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
        public static void copyMore(Actor from, Actor to, bool compositionCopied = false)
        {
            ExtendedActor fromActor = (ExtendedActor)from;
            ExtendedActor toActor = (ExtendedActor)to;
            toActor.extendedData.status.chineseElement = new ChineseElement(fromActor.extendedData.status.chineseElement.baseElementContainer);
            toActor.extendedData.status.bonusStats = new MoreStats().addAnotherStats(fromActor.extendedData.status.bonusStats);
            toActor.extendedData.status.familyID = fromActor.extendedData.status.familyID;
            toActor.extendedData.status.familyName = fromActor.extendedData.status.familyName;
            toActor.extendedData.status.specialBody = fromActor.extendedData.status.specialBody;
            toActor.extendedData.status.cultisystem = fromActor.extendedData.status.cultisystem;
            toActor.extendedData.status.canCultivate = fromActor.extendedData.status.canCultivate;
            toActor.extendedData.status.cultiBook = fromActor.extendedData.status.cultiBook;
            if (compositionCopied)
            {
                toActor.extendedData.status.compositionSetting = fromActor.extendedData.status.compositionSetting;
                foreach (BaseSimObject baseSimObject in fromActor.compositions)
                {
                    ComposeTools.composeTwo(toActor, baseSimObject);
                }
            }
            //MonoBehaviour.print("successfully copy " + to.easyData.actorID + "'s more...");
        }
        public static int getRealm(this ExtendedActor actor)
        {
            int realm = actor.easyData.level;
            if (realm > 10)
            {
                realm = (realm + 9) / 10 + 9;
                if (realm > 20)
                {
                    return 20;
                }
                return realm;
            }
            else
            {
                if (realm < 1)
                {
                    return 1;
                }
                return realm;
            }
        }
        public static string getRealmName(this ExtendedActor actor)
        {
            int realm = actor.getRealm();
            return LocalizedTextManager.getText(actor.extendedData.status.cultisystem + realm);
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
            ExtendedActorStatus moredata = actor1.extendedData.status;
            morestats.clear();
            int realm = actor1.getRealm();
            morestats.addAnotherStats(AddAssetManager.cultisystemLibrary.get(moredata.cultisystem).moreStats[realm - 1]);
            //morestats.addAnotherStats(ExtendedWorldData.instance.familys[moredata.familyID].cultivationBook.stats[realm - 1]);

            morestats.addAnotherStats(moredata.bonusStats);
        }
        /// <summary>
        /// 数据最后处理
        /// </summary>
        /// <param name="actor"></param>
        /// <returns></returns>
        public static void dealStatsHelper2(Actor actor)
        {
            ExtendedActor actor1 = (ExtendedActor)actor;
            ExtendedActorStatus moredata = actor1.extendedData.status;
            int maxArmor = 80 + actor1.easyData.level / 6;

            if (moredata.cultisystem == "bodying")
            {
                maxArmor += 5;
            }
            if (maxArmor > 99)
            {
                maxArmor = 99;
            }
            if (actor1.easyCurStats.armor > maxArmor)
            {
                actor1.easyCurStats.armor = maxArmor;
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
            float result;
            float element = actor1.extendedData.status.chineseElement.getImPurity();
            float spellCount = actor1.extendedData.status.spells.Count;
            float specialBody = actor1.GetSpecialBody().rank;
            float baseStats = (actor1.easyCurStats.health >> 7) * (actor1.easyCurStats.damage >> 5) / (100 - actor1.easyCurStats.armor) * actor1.easyCurStats.attackSpeed;
            result = element * spellCount * specialBody * baseStats / 4;
            return result;
        }
        public static float getPotience(this Actor actor)
        {
            return 0f;
        }
    }
}
