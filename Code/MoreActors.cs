using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way
{
    class MoreActors
    {
        private static List<int> _color_sets = new List<int>();
        internal void init()
        {
            #region 智慧种族
            AssetManager.unitStats.get("unit_human").procreate = true;
            AssetManager.unitStats.get("unit_elf").procreate = true;
            AssetManager.unitStats.get("unit_dwarf").procreate = true;
            AssetManager.unitStats.get("unit_orc").procreate = true;
            ActorStats TianAsset = AssetManager.unitStats.clone("Tian", "unit_human");
            TianAsset.maxAge = 500;
            TianAsset.race = "Tian";
            TianAsset.unit = true;
            TianAsset.landCreature = true;
            TianAsset.oceanCreature = true;
            TianAsset.needFood = false;
            TianAsset.procreate = true;
            TianAsset.nameLocale = "Tians";
            TianAsset.nameTemplate = "Tian_name";
            TianAsset.shadowTexture = "unitShadow_5";
            TianAsset.setBaseStats(120, 17, 30, 4, 5, 100, 3);
            TianAsset.useSkinColors = false;
            TianAsset.texture_path = "t_Tian";
            TianAsset.texture_heads = "";
            TianAsset.heads = 2;
            TianAsset.icon = "Tian";
            TianAsset.animation_idle = "walk_0,walk_1,walk_2,walk_3";
            addActor(TianAsset);

            ActorStats MingAsset = AssetManager.unitStats.clone("Ming", "unit_human");
            MingAsset.maxAge = 500;
            MingAsset.race = "Ming";
            MingAsset.unit = true;
            MingAsset.needFood = false;
            MingAsset.procreate = true;
            MingAsset.traits.Add("cursed_immune");
            MingAsset.nameLocale = "Mings";
            MingAsset.nameTemplate = "Ming_name";
            MingAsset.shadowTexture = "unitShadow_5";
            MingAsset.setBaseStats(150, 20, 35, 4, 0, 90, 0);
            MingAsset.useSkinColors = false;
            MingAsset.texture_path = "t_Ming";
            MingAsset.texture_heads = "";
            MingAsset.heads = 2;
            MingAsset.animation_idle = "walk_0,walk_1,walk_2,walk_3";
            addActor(MingAsset);
            #endregion

            #region 其他生物
            //仙狐
            ActorStats FairyFoxAsset = AssetManager.unitStats.clone("FairyFox", "fox");
            FairyFoxAsset.maxAge = 300;
            FairyFoxAsset.race = "fox";
            FairyFoxAsset.needFood = false;
            FairyFoxAsset.nameLocale = "FairyFox";
            FairyFoxAsset.shadowTexture = "unitShadow_5";
            FairyFoxAsset.useSkinColors = false;
            FairyFoxAsset.texture_path = "FairyFox";
            FairyFoxAsset.texture_heads = "";
            FairyFoxAsset.icon = "iconFairyFox";
            FairyFoxAsset.defaultAttack = "snowball";
            FairyFoxAsset.damagedByOcean = false;
            addActor(FairyFoxAsset);
            //东方龙
            ActorStats EasternDragonAsset = AssetManager.unitStats.clone("EasternDragon", "fox");
            //EasternDragonAsset.race = "dragon";
            //EasternDragonAsset.kingdom = "dragons";
            EasternDragonAsset.flying = true;
            EasternDragonAsset.actorSize = ActorSize.S17_Dragon;
            EasternDragonAsset.shadowTexture = "unitShadow_23";
            EasternDragonAsset.texture_path = "EasternDragon";
            EasternDragonAsset.texture_heads = "";
            EasternDragonAsset.icon = "iconEasternDragon";
            EasternDragonAsset.disablePunchAttackAnimation = true;
            addActor(EasternDragonAsset);
            #endregion

            #region BOSS
            ActorStats JiaoDragonAsset = AssetManager.unitStats.clone("JiaoDragon", "wolf");
            JiaoDragonAsset.maxAge = 1000;
            JiaoDragonAsset.race = "JiaoDragon";
            JiaoDragonAsset.kingdom = "boss";
            JiaoDragonAsset.needFood = false;
            JiaoDragonAsset.nameLocale = "JiaoDragon";
            JiaoDragonAsset.nameTemplate = "default_name";
            JiaoDragonAsset.shadowTexture = "unitShadow_5";
            JiaoDragonAsset.useSkinColors = false;
            JiaoDragonAsset.texture_path = "JiaoDragon";
            JiaoDragonAsset.texture_heads = "";
            JiaoDragonAsset.icon = "iconJiaoDrangon";
            JiaoDragonAsset.setBaseStats(1000000, 10000, 0, 99, 100, 100);
            JiaoDragonAsset.defaultAttack = "snowball";
            JiaoDragonAsset.damagedByOcean = false;
            JiaoDragonAsset.actorSize = ActorSize.S17_Dragon;
            JiaoDragonAsset.baseStats.areaOfEffect = 5f;
            JiaoDragonAsset.baseStats.range = 30f;
            JiaoDragonAsset.baseStats.scale = 0.4f;
            JiaoDragonAsset.baseStats.size = 0.5f;
            JiaoDragonAsset.baseStats.projectiles = 20;
            JiaoDragonAsset.baseStats.knockback = 100f;
            JiaoDragonAsset.baseStats.knockbackReduction = 100f;
            JiaoDragonAsset.procreate = false;
            JiaoDragonAsset.disablePunchAttackAnimation = true;
            addActor(JiaoDragonAsset);

            ActorStats XieDragonAsset = AssetManager.unitStats.clone("XieDragon", "JiaoDragon");
            XieDragonAsset.nameLocale = "XieDragon";
            XieDragonAsset.nameTemplate = "default_name";
            XieDragonAsset.shadowTexture = "unitShadow_5";
            XieDragonAsset.texture_path = "XieDragon";
            XieDragonAsset.icon = "iconXieDrangon";
            XieDragonAsset.setBaseStats(1000000, 10000, 0, 99, 100, 100);
            XieDragonAsset.baseStats.scale = 0.20f;
            XieDragonAsset.baseStats.size = 0.5f;
            addActor(XieDragonAsset);
            #endregion

            #region 彩蛋
            ActorStats MengZhuAsset = AssetManager.unitStats.clone("MengZhu", "wolf");
            MengZhuAsset.maxAge = 1000;
            MengZhuAsset.race = "MengZhu";
            MengZhuAsset.kingdom = "good";
            MengZhuAsset.needFood = false;
            MengZhuAsset.nameLocale = "MengZhu";
            MengZhuAsset.nameTemplate = "MengZhu_name";
            MengZhuAsset.shadowTexture = "unitShadow_5";
            MengZhuAsset.useSkinColors = false;
            MengZhuAsset.inspect_home = false;
            MengZhuAsset.inspect_children = false;
            MengZhuAsset.ignoreTileSpeedMod = true;
            MengZhuAsset.needFood = false;
            MengZhuAsset.texture_path = "MengZhu";
            MengZhuAsset.texture_heads = "";
            MengZhuAsset.icon = "iconMengZhu";
            MengZhuAsset.traits = new List<string> { "immortal", "cursed_immune", "fire_proof", "freeze_proof", "poison_immune", "immune", "healing_aura" };
            MengZhuAsset.attack_spells = new List<string> { "bloodRain" };
            MengZhuAsset.setBaseStats(5000000, 10000, 15, 99, 100, 100);
            MengZhuAsset.defaultAttack = "snowball";
            MengZhuAsset.damagedByOcean = false;
            MengZhuAsset.procreate = false;
            MengZhuAsset.disablePunchAttackAnimation = true;
            MengZhuAsset.baseStats.areaOfEffect = 5f;
            MengZhuAsset.baseStats.range = 30f;
            MengZhuAsset.baseStats.scale = 0.02f;
            MengZhuAsset.actorSize = ActorSize.S13_Human;
            MengZhuAsset.baseStats.projectiles = 20;
            MengZhuAsset.baseStats.knockback = 100f;
            MengZhuAsset.baseStats.knockbackReduction = 100f;
            addActor(MengZhuAsset);
            #endregion

            #region 召唤物
            ActorStats summoned = AssetManager.unitStats.clone("summoned", "skeleton");
            summoned.canLevelUp = false;
            summoned.take_items_ignore_range_weapons = true;
            summoned.job = "attacker";
            summoned.race = "summoned";
            summoned.useSkinColors = false;
            ActorStats summon = AssetManager.unitStats.clone("summon", "summoned");
            summon.race = "undead";
            ActorStats summonTian = AssetManager.unitStats.clone("summonTian", "summoned");
            summonTian.use_items = false;
            summonTian.ignoreTileSpeedMod = true;
            summonTian.actorSize = ActorSize.S17_Dragon;
            summonTian.shadow = true;
            summonTian.shadowTexture = "unitShadow_23";
            summonTian.body_separate_part_head = false;
            summonTian.canReceiveTraits = false;
            summonTian.defaultWeapons = new List<string>();
            summonTian.baseStats.knockbackReduction = 100f;
            summonTian.baseStats.range = 15f;
            summonTian.baseStats.projectiles = 1;
            summonTian.baseStats.scale = 0.3f;
            summonTian.baseStats.speed = 1.5f;
            summonTian.traits.Add("giant");
            summonTian.disablePunchAttackAnimation = true;
            summonTian.texture_path = "summonTian";
            summonTian.flying = true;
            summonTian.procreate = false;
            summonTian.defaultAttack = "shotgun";
            addActor(summonTian);
            ActorStats summonTian1 = AssetManager.unitStats.clone("summonTian1", "summoned");
            summonTian1.ignoreTileSpeedMod = true;
            summonTian1.actorSize = ActorSize.S17_Dragon;
            summonTian1.shadow = true;
            summonTian1.shadowTexture = "unitShadow_9";
            summonTian1.baseStats.knockbackReduction = 100f;
            summonTian1.baseStats.range = 15f;
            summonTian1.baseStats.projectiles = 1;
            summonTian1.baseStats.scale = 0.42f;
            summonTian1.baseStats.speed = 1.5f;
            summonTian1.traits.Add("giant");
            summonTian1.disablePunchAttackAnimation = true;
            summonTian1.texture_path = "summonTian1";
            summonTian1.body_separate_part_head = false;
            summonTian1.swimToIsland = false;
            summonTian1.procreate = false;
            summonTian1.defaultAttack = "summonTian1";
            summonTian1.defaultWeapons = new List<string>();
            summonTian1.use_items = false;
            addActor(summonTian1);
            #endregion
        }

        private void addActor(ActorStats pStats,string from = "#FFC984",string to = "#543E2C")
        {
            Main.instance.moreActors.Add(pStats.id);
            if (pStats.color_sets == null)
            {
                pStats.color_sets = new List<ColorSet>();
            }
            ColorSet colorSet = new ColorSet();
            colorSet.id = "default";
            pStats.color_sets.Add(colorSet);
            Color pFrom = Toolbox.makeColor(from);
            Color pTo = Toolbox.makeColor(to);
            int num = 5;
            float num2 = 1f / (float)(num - 1);
            for (int i = 0; i < num; i++)
            {
                float num3 = 1f - (float)i * num2;
                if (num3 > 1f)
                {
                    num3 = 1f;
                }
                Color c = Toolbox.blendColor(pFrom, pTo, num3);
                colorSet.colors.Add(c);
            }
            if (pStats.shadow)
            {
                Reflection.CallMethod(AssetManager.unitStats, "loadShadow", pStats);
            }
            /*
			 * 用于自动添加命名，复制人类的命名
			((ChineseNameLibrary)AssetManager.instance.dict["chineseNameGenerator"]).clone(pStats.nameTemplate, "human_name");
			*/
        }


        #region 原版函数
        public static void setSkinSet(Actor pActor, string pForceUnitSet)
        {
            if (string.IsNullOrEmpty(pForceUnitSet) || !pActor.stats.useSkinColors)
            {
                return;
            }
            if (pActor.stats.color_sets.Count == 0)
            {
                return;
            }
            _color_sets.Clear();
            _color_sets = new List<int>();
            int num = 0;
            for (int i = 0; i < pActor.stats.color_sets.Count; i++)
            {
                if (pActor.stats.color_sets[i].id.Contains(pForceUnitSet))
                {
                    _color_sets.Add(num);
                }
                num++;
            }
            if (_color_sets.Count > 0)
            {
                int random = _color_sets.GetRandom();

                pActor.GetData().skin_set = random;
            }

        }

        #endregion

        #region 拦截
        //释放法术
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "applyAttack")]
        public static bool useSpell(BaseSimObject pAttacker, BaseSimObject pTarget)
        {
            if (pAttacker.objectType != MapObjectType.Actor)
            {
                return true;
            }
            ActorStatus dataA = ((Actor)pAttacker).GetData();
            MoreActorData moreData = Main.instance.actorToMoreData[dataA.actorID];
            MoreStats morestats = Main.instance.actorToMoreStats[dataA.actorID];
            if (morestats.spells.Count != 0)
            {
                int index = Toolbox.randomInt(0, Main.instance.actorToMoreStats[dataA.actorID].spells.Count);
                ExtensionSpell spell = morestats.spells[index];
                if (moreData.coolDown.ContainsKey(spell.spellAssetID))
                {
                    //进行蓝耗和冷却检查
                    if (moreData.coolDown[spell.spellAssetID] == 1 && moreData.magic >= spell.cost
                        && AddAssetManager.extensionSpellLibrary.get(spell.spellAssetID).type.attacking)
                    {
                        if (spell.castSpell(pAttacker, pTarget))
                        {
                            moreData.coolDown[spell.spellAssetID] = spell.cooldown;
                            moreData.magic -= spell.cost;
                            return false;
                        }
                    }
                }
                else
                {
                    MonoBehaviour.print(spell.spellAssetID);
                }
            }
            return true;
        }
        //经验条修改
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "getExpToLevelup")]
        public static bool getExpToLevelUp(Actor __instance, ref int __result)
        {
            ActorStatus data = __instance.GetData();
            __result = (int)((100 + (data.level - 1) * (data.level - 1) * 50) * Main.instance.actorToMoreStats[data.actorID].element.getPurity());
            return false;
        }
        //升级修改
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "addExperience")]
        public static bool addExperiece_Prefix(Actor __instance, int pValue)
        {
            ActorStatus data = __instance.GetData();
            int num = 110;
            data.experience += pValue;
            MoreStats moreStats = __instance.GetMoreStats();
            MoreActorData moreData = __instance.GetMoreData();
            if (data.experience >= __instance.getExpToLevelup())
            {
                //回蓝，回冷却
                moreData.magic = moreStats.magic;//待调整与元素相关
                foreach (ExtensionSpell spell in moreStats.spells)
                {
                    if (!moreData.coolDown.ContainsKey(spell.spellAssetID))
                    {
                        moreData.coolDown[spell.spellAssetID] = spell.cooldown;
                    }
                    moreData.coolDown[spell.spellAssetID] = 1;
                }
                __instance.setStatsDirty();
            }
            else
            {
                return false;
            }
            if (data.level >= num || !__instance.stats.canLevelUp)
            {
                while (data.experience >= __instance.getExpToLevelup())
                {
                    data.experience -= __instance.getExpToLevelup();
                    ChineseElement element1 = new ChineseElement();
                    if (element1.getPurity() < moreStats.element.getPurity())
                    {
                        moreStats.element = element1;
                    }
                }
            }
            while (data.experience >= __instance.getExpToLevelup() && data.level < num)
            {
                //缺少其他升级条件
                data.experience -= __instance.getExpToLevelup();
                data.level++;
                //准备雷劫

                #region 升级福利
                data.health = int.MaxValue >> 4;
                int realm = data.level;
                if (realm > 10)
                {
                    if (data.level == 110)
                    {
                        __instance.generateNewBody();
                    }
                    realm = (realm + 9) / 10 + 9;
                }
                //家族升级

                if (data.level > moreStats.family.maxLevel)
                {
                    int count = 0;
                    while (count < moreStats.family.maxLevel / 10)
                    {
                        ChineseElement element1 = new ChineseElement();
                        if (element1.getPurity() < moreStats.element.getPurity())
                        {
                            moreStats.element = element1;
                            break;
                        }
                        count++;
                    }
                    moreStats.family.levelUp(data.firstName);
                }
                else if (data.level == moreStats.family.maxLevel)
                {
                    ChineseElement element1 = new ChineseElement();
                    if (element1.getPurity() < moreStats.element.getPurity())
                    {
                        moreStats.element = element1;
                    }
                }
                //国家和城市领导人变化
                if (__instance.kingdom == null)
                {
                    continue;
                }
                if (__instance.kingdom.king != null)
                {
                    if (data.level > __instance.kingdom.king.GetData().level)
                    {
                        Actor lastKing = __instance.kingdom.king;
                        __instance.kingdom.setKing(__instance);
                        foreach (City city in __instance.kingdom.cities)
                        {
                            if (city.leader == null)
                            {
                                City.makeLeader(lastKing, city);
                                break;
                            }
                            else
                            {
                                if (city.leader.GetData().level <= lastKing.GetData().level)
                                {
                                    City.makeLeader(lastKing, city);
                                    break;
                                }
                            }
                        }
                    }
                }
            }

            //法术释放
            string actorID = data.actorID;
            int level = data.level;
            foreach (ExtensionSpell spell in Main.instance.actorToMoreStats[actorID].spells)
            {
                if (__instance != null && data != null && data.alive &&
                    AddAssetManager.extensionSpellLibrary.get(spell.spellAssetID).type.levelUp
                    && AddAssetManager.extensionSpellLibrary.get(spell.spellAssetID).type.requiredLevel <= level)
                {
                    spell.castSpell(__instance, __instance);
                    break;
                }
            }
            #endregion


            return false;
        }
        //境界压制
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "getHit")]
        public static bool getHit_Prefix(Actor __instance, ref float pDamage,AttackType pType = AttackType.None, BaseSimObject pAttacker = null)
        {
            ActorStatus data1 = __instance.GetData();
            
            if (__instance.haveTrait("asylum"))
            {
                return false;
            }
            if(pType == AttackType.Age)
            {
                if (__instance.GetCurStats().armor != 0)
                {
                    pDamage /= 1 - __instance.GetCurStats().armor/100f;
                }
                return true;
            }
            if (pAttacker == null)//采用年龄伤害作为真伤判断
            {
                pDamage *= 1f - data1.level * 0.1f;
                return true;
            }
            if (__instance == pAttacker)
            {
                return false;
            }

            //人对人伤害减免
            if (pAttacker.objectType == MapObjectType.Actor)
            {
                ActorStatus data2 = ((Actor)pAttacker).GetData();
                if (data2.level <= data1.level - 10)
                {
                    return false;
                }
                if (data2.level < data1.level)
                {
                    pDamage *= 1 - (data1.level - data2.level + 1) * data1.level / 100f;
                }
            }
            //人对塔伤害减免暂时去除
            //else
            //{
            //    pDamage *= 1f - data1.level * 0.005f;
            //}
            return true;
        }
        //每年修炼经验修改
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "updateAge")]
        public static bool updateAge(Actor __instance)
        {
            ActorStatus data = __instance.GetData();
            MoreStats moreStats = __instance.GetMoreStats();
            MoreActorData moreData = __instance.GetMoreData();
            if (!updateAge(AssetManager.raceLibrary.get(__instance.stats.race), data, __instance) && !__instance.haveTrait("immortal"))
            {
                __instance.killHimself(false, AttackType.Age, true, true);
                return false;
            }
            //回蓝，回冷却
            moreData.magic += data.level;//待调整与元素相关
            if (moreData.magic > moreStats.magic + 1)
            {
                moreData.magic = moreStats.magic + 1;
            }
            foreach (ExtensionSpell spell in moreStats.spells)
            {
                if (!moreData.coolDown.ContainsKey(spell.spellAssetID))
                {
                    moreData.coolDown[spell.spellAssetID] = spell.cooldown;
                }
                if (moreData.coolDown[spell.spellAssetID] > 1)
                {
                    moreData.coolDown[spell.spellAssetID]--;
                }
            }

            ChineseElement chunkElement = Main.instance.chunkToElement[__instance.currentTile.chunk.id];
            ChineseElement actorElement = moreStats.element;
            //MonoBehaviour.print(__instance + "准备");
            //MonoBehaviour.print(Main.instance.actorToID.ContainsValue(data.actorID));
            //ChineseElement actorElement = new ChineseElement();
            //try
            //{
            //    actorElement = Main.instance.actorToMoreStats[data.actorID].element;
            //}
            //catch (Exception e)
            //{
            //    WorldTip.showNow("清理bug生物: "+data.firstName,false,"top");
            //    __instance.killHimself();
            //    return false;
            //}

            float exp = 5;
            for (int i = 0; i < 5; i++)
            {
                int temp = chunkElement.baseElementContainer[i] % 100;
                exp *= temp / (float)(actorElement.baseElementContainer[i] + 1);
            }
            addExperiece_Prefix(__instance, (int)exp);

            if (data.age > 100 && Toolbox.randomChance(0.03f))
            {
                __instance.addTrait("wise");
            }
            //人人平等
            //if (__instance.city != null)
            //{
            //    if (__instance.kingdom.king == __instance)
            //    {
            //        addExperiece_Prefix(__instance, 2);
            //    }
            //    if (__instance.city.leader == __instance)
            //    {
            //        addExperiece_Prefix(__instance, 1);
            //    }
            //}
            return false;
        }
        //属性添加
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorBase), "updateStats")]
        public static bool updateStats_Prefix(Actor __instance)
        {
            BaseStats curStats = __instance.GetCurStats();
            ActorStatus data = __instance.GetData();
            #region 数据索引不存在排错（也用于新生物生成
            if (!Main.instance.actorToMoreStats.ContainsKey(data.actorID))
            {
                MoreStats moreStats = new MoreStats();
                MoreActorData moreData = new MoreActorData();
                moreStats.spells = Main.instance.raceFeatures[__instance.stats.race].raceSpells;
                moreData.cultisystem = moreStats.cultisystem;
                moreData.element = moreStats.element;
                moreData.magic = moreStats.magic;
                if (Toolbox.randomChance(0.001f))
                {
                    moreData.specialBody = AddAssetManager.specialBodyLibrary.list.GetRandom().id;
                }
                else
                {
                    moreData.specialBody = "FT";
                }
                Main.instance.actorToMoreStats.Add(data.actorID, moreStats);

                Main.instance.actorToMoreData.Add(data.actorID, moreData);
                string name = data.firstName;
                foreach (string fn in ChineseNameAsset.familyNameTotal)
                {
                    if (name.StartsWith(fn))
                    {
                        moreStats.family = Main.instance.familys[fn];
                        moreData.familyID = moreStats.family.id;
                        break;
                    }
                }
                if (moreStats.family == null)
                {
                    moreStats.family = Main.instance.familys["甲"];
                    moreData.familyID = moreStats.family.id;
                }
            }
            #endregion
            MoreStats morestats = __instance.GetMoreStats();
            MoreActorData moredata = __instance.GetMoreData();
            #region 修炼体系
            if (__instance.getCulture() != null && moredata.cultisystem == "default")
            {
                List<string> cultisystem = new List<string>();
                foreach (string tech in __instance.getCulture().list_tech_ids)
                {
                    if (tech.StartsWith("culti_"))
                    {
                        cultisystem.Add(tech);
                    }
                }
                if (cultisystem.Count > 0)
                {
                    morestats.cultisystem = cultisystem.GetRandom().Remove(0, 6);
                    moredata.cultisystem = morestats.cultisystem;
                }
            }
            #endregion
            #region 原版其他
            Reflection.SetField(__instance, "statsDirty", false);
            if (__instance.stats.useSkinColors && data.skin_set == -1 && __instance.stats.color_sets != null)
            {
                //typeof(ActorBase).GetMethod("setSkinSet", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(__instance, null);
                setSkinSet(__instance, "default");
            }
            if (__instance.stats.useSkinColors && data.skin == -1)
            {
                data.skin = Toolbox.randomInt(0, __instance.stats.color_sets[data.skin_set].colors.Count);
            }
            if (string.IsNullOrEmpty(data.mood))
            {
                data.mood = "normal";
            }
            MoodAsset moodAsset = AssetManager.moods.get(data.mood);
            #endregion
            #region 种族法术与各类属性
            morestats.clear();
            morestats.spells.AddRange(Main.instance.raceFeatures[__instance.stats.race].raceSpells);
            int realm = data.level;
            if (realm > 10)
            {
                realm = (realm + 9) / 10 + 9;
                if (realm > 19)
                {
                    realm = 19;
                }
            }
            if (!data.alive)
            {
                return false;
            }
            //每个境界属性均加上
            for (int i = 0; i < realm; i++)
            {
                morestats.addAnotherStats(AddAssetManager.cultisystemLibrary.get(morestats.cultisystem).moreStats[i]);
                morestats.addAnotherStats(morestats.family.cultivationBook.stats[i]);
            }
            #endregion
            #region 法术去重
            List<ExtensionSpell> fixedSpells = new List<ExtensionSpell>();
            for (int i = 0; i < morestats.spells.Count; i++)
            {
                ExtensionSpell spell = morestats.spells[i];
                ExtensionSpellAsset spellAsset = AddAssetManager.extensionSpellLibrary.get(spell.spellAssetID);
                if (!spellAsset.bannedCultisystem.Contains(morestats.cultisystem) && __instance.kingdom != null && !spellAsset.bannedRace.Contains(__instance.kingdom.raceID))
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
                        if (spellAsset1.chineseElement.getPurity() < spellAsset2.chineseElement.getPurity())
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
            if (moredata.coolDown == null)
            {
                moredata.coolDown = new Dictionary<string, int>();
            }
            if (moredata.bonusStats == null)
            {
                moredata.bonusStats = new MoreStats();
            }
            if (morestats.spells.Count > 0 && moredata.coolDown.Count < morestats.spells.Count)
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
            #region 添加属性
            morestats.addAnotherStats(moredata.bonusStats);
            curStats.CallMethod("clear");
            curStats.CallMethod("addStats", __instance.stats.baseStats);
            curStats.CallMethod("addStats", moodAsset.baseStats);
            curStats.CallMethod("addStats", morestats.baseStats);

            curStats.diplomacy += data.diplomacy;
            curStats.stewardship += data.stewardship;
            curStats.intelligence += data.intelligence;
            curStats.warfare += data.warfare;
            #endregion
            #region 原版的一堆东西（注意一个攻击方式，装备贴图，装备属性
            if (Reflection.GetField(typeof(Actor), __instance, "activeStatusEffects") != null)
            {
                for (int i = 0; i < ((List<ActiveStatusEffect>)Reflection.GetField(typeof(Actor), __instance, "activeStatusEffects")).Count; i++)
                {
                    ActiveStatusEffect activeStatusEffect = ((List<ActiveStatusEffect>)Reflection.GetField(typeof(Actor), __instance, "activeStatusEffects"))[i];
                    curStats.CallMethod("addStats", activeStatusEffect.asset.baseStats);
                }
            }
            ItemAsset itemAsset = AssetManager.items.get(__instance.stats.defaultAttack);
            if (itemAsset != null)
            {
                curStats.CallMethod("addStats", itemAsset.baseStats);
            }
            Reflection.SetField(__instance, "s_attackType", ((ItemAsset)__instance.CallMethod("getWeaponAsset")).attackType);
            Reflection.SetField(__instance, "s_slashType", ((ItemAsset)__instance.CallMethod("getWeaponAsset")).slash);
            Reflection.SetField(__instance, "item_sprite_dirty", true);
            if (__instance.stats.use_items && !__instance.equipment.weapon.isEmpty())
            {
                Reflection.SetField(__instance, "s_weapon_texture", "w_" + __instance.equipment.weapon.data.id + "_" + __instance.equipment.weapon.data.material);
            }
            else
            {
                Reflection.SetField(__instance, "s_weapon_texture", string.Empty);
            }
            typeof(ActorBase).GetMethod("findHeadSprite", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(__instance, null);
            for (int j = 0; j < data.traits.Count; j++)
            {
                string pID = data.traits[j];
                if (pID == "eyepatch" || pID == "crippled")
                {
                    data.traits.Remove(pID);
                    continue;
                }
                ActorTrait actorTrait = AssetManager.traits.get(pID);
                if (actorTrait != null)
                {
                    curStats.CallMethod("addStats", actorTrait.baseStats);
                }
            }
            if (__instance.stats.unit)
            {
                Reflection.SetField(__instance, "s_personality", (PersonalityAsset)null);
                if ((__instance.kingdom != null && __instance.kingdom.isCiv() && __instance.kingdom.king == __instance) || (__instance.city != null && __instance.city.leader == __instance))
                {
                    string pID2 = "balanced";
                    int num = curStats.diplomacy;
                    if (curStats.diplomacy > curStats.stewardship)
                    {
                        pID2 = "diplomat";
                        num = curStats.diplomacy;
                    }
                    else if (curStats.diplomacy < curStats.stewardship)
                    {
                        pID2 = "administrator";
                        num = curStats.stewardship;
                    }
                    if (curStats.warfare > num)
                    {
                        pID2 = "militarist";
                    }
                    Reflection.SetField(__instance, "s_personality", AssetManager.personalities.get(pID2));
                    curStats.CallMethod("addStats", ((PersonalityAsset)Reflection.GetField(typeof(Actor), __instance, "s_personality")).baseStats);
                }
            }
            Reflection.SetField(__instance, "_trait_weightless", __instance.haveTrait("weightless"));
            Reflection.SetField(__instance, "_status_frozen", __instance.CallMethod("haveStatus", "frozen"));
            Reflection.SetField(__instance, "_trait_weightless", __instance.haveTrait("weightless"));
            ((HashSet<string>)Reflection.GetField(typeof(Actor), __instance, "s_traits_ids")).Clear();
            List<ActorTrait> list = (List<ActorTrait>)Reflection.GetField(typeof(Actor), __instance, "s_special_effect_traits");
            if (list != null)
            {
                list.Clear();
            }
            for (int k = 0; k < data.traits.Count; k++)
            {
                string text = data.traits[k];
                ((HashSet<string>)Reflection.GetField(typeof(Actor), __instance, "s_traits_ids")).Add(text);
                ActorTrait actorTrait2 = AssetManager.traits.get(text);
                if (actorTrait2 != null && actorTrait2.action_special_effect != null)
                {
                    if ((List<ActorTrait>)Reflection.GetField(typeof(Actor), __instance, "s_special_effect_traits") == null)
                    {
                        Reflection.SetField(__instance, "s_special_effect_traits", new List<ActorTrait>());
                    }
                    ((List<ActorTrait>)Reflection.GetField(typeof(Actor), __instance, "s_special_effect_traits")).Add(actorTrait2);
                }
            }
            List<ActorTrait> list2 = (List<ActorTrait>)Reflection.GetField(typeof(Actor), __instance, "s_special_effect_traits");
            if (list2 != null && list2.Count == 0)
            {
                Reflection.SetField(__instance, "s_special_effect_traits", (List<ActorTrait>)null);
            }
            typeof(ActorBase).GetMethod("checkMadness", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(__instance, null);
            Reflection.SetField(__instance, "_trait_peaceful", __instance.haveTrait("peaceful"));
            Reflection.SetField(__instance, "_trait_fire_resistant", __instance.haveTrait("fire_proof"));
            if (__instance.stats.use_items)
            {
                List<ActorEquipmentSlot> list3 = ActorEquipment.getList(__instance.equipment, false);
                for (int l = 0; l < list3.Count; l++)
                {
                    ActorEquipmentSlot actorEquipmentSlot = list3[l];
                    if (actorEquipmentSlot.data != null)
                    {
                        ItemTools.calcItemValues(actorEquipmentSlot.data);
                        curStats.CallMethod("addStats", ItemTools.s_stats);
                    }
                }
            }
            #endregion
            #region 属性倍乘
            curStats.health += (int)((float)curStats.health * (curStats.mod_health / 100f));
            curStats.damage += (int)((float)curStats.damage * (curStats.mod_damage / 100f));
            curStats.armor += (int)((float)curStats.armor * (curStats.mod_armor / 100f));
            curStats.crit += (float)((int)(curStats.crit * (curStats.mod_crit / 100f)));
            curStats.diplomacy += (int)((float)curStats.diplomacy * (curStats.mod_diplomacy / 100f));
            curStats.speed += (float)((int)(curStats.speed * (curStats.mod_speed / 100f)));
            curStats.attackSpeed += (float)((int)(curStats.attackSpeed * (curStats.mod_attackSpeed / 100f)));
            #endregion
            #region 满血
            if ((bool)Reflection.GetField(typeof(Actor), __instance, "event_full_heal") == true)
            {
                Reflection.SetField(__instance, "event_full_heal", false);
                data.health = curStats.health + (int)((float)curStats.health * (curStats.mod_health / 100f));
            }
            #endregion
            #region 属性格式化
            curStats.normalize();
            Culture culture = __instance.getCulture();
            if (culture != null)
            {
                curStats.damage = (int)((float)curStats.damage + (float)curStats.damage * culture.stats.bonus_damage.value);
                curStats.armor = (int)((float)curStats.armor + (float)curStats.armor * culture.stats.bonus_armor.value);
            }
            if (curStats.health < 1)
            {
                curStats.health = 1;
            }
            if (data.health > curStats.health)
            {
                data.health = curStats.health;
            }
            if (data.health < 1)
            {
                data.health = 1;
            }
            if (curStats.damage < 1)
            {
                curStats.damage = 1;
            }
            if (curStats.speed < 1f)
            {
                curStats.speed = 1f;
            }
            if (curStats.armor < 0)
            {
                curStats.armor = 0;
            }
            int maxArmor = 80 + data.level / 6;
            if (morestats.cultisystem == "bodying")
            {
                maxArmor += 5;
            }
            if (maxArmor > 99)
            {
                maxArmor = 99;
            }
            if (curStats.armor > maxArmor)
            {
                curStats.armor = maxArmor;
            }
            if (curStats.diplomacy < 0)
            {
                curStats.diplomacy = 0;
            }
            if (curStats.dodge < 0f)
            {
                curStats.dodge = 0f;
            }
            if (curStats.accuracy < 10f)
            {
                curStats.accuracy = 10f;
            }
            if (curStats.crit < 0f)
            {
                curStats.crit = 0f;
            }
            if (curStats.attackSpeed < 0f)
            {
                curStats.attackSpeed = 1f;
            }
            if (curStats.attackSpeed >= 300f)
            {
                curStats.attackSpeed = 300f;
            }
            if (moredata.magic > morestats.magic)
            {
                moredata.magic = morestats.magic;
            }
            Reflection.SetField(__instance, "s_attackSpeed_seconds", (300f - curStats.attackSpeed) / (100f + curStats.attackSpeed));
            curStats.s_crit_chance = curStats.crit / 100f;
            curStats.zones = (curStats.stewardship + 1) * 2;
            curStats.cities = curStats.stewardship / 5 + 1;
            curStats.army = curStats.warfare + 5;
            curStats.bonus_towers = curStats.warfare / 10;
            if (curStats.bonus_towers > 2)
            {
                curStats.bonus_towers = 2;
            }
            if (curStats.army < 0)
            {
                curStats.army = 5;
            }
            #endregion
            #region 设置攻击时间与人物大小
            Reflection.SetField(__instance, "attackTimer", 0f);
            typeof(ActorBase).GetMethod("updateTargetScale", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(__instance, null);
            curStats.normalize();
            __instance.currentScale.x = curStats.scale;
            __instance.currentScale.y = curStats.scale;
            __instance.currentScale.z = curStats.scale;
            #endregion
            return false;
        }
        //寿命实现
        public static bool updateAge(Race pRace, ActorStatus pData, Actor pActor)
        {
            pData.age++;
            ActorStats actorStats = AssetManager.unitStats.get(pData.statsID);
            pData.CallMethod("updateAttributes", actorStats, pRace, false);
            if (!MapBox.instance.worldLaws.world_law_old_age.boolVal)
            {
                return true;
            }
            int num = actorStats.maxAge;
            Culture culture = MapBox.instance.cultures.get(pData.culture);
            if (culture != null)
            {
                num += culture.getMaxAgeBonus();
            }
            MoreStats morestats = pActor.GetMoreStats();
            if (morestats.maxAge == 0 && pData.level > 1)
            {
                pActor.setStatsDirty();
            }
            num += morestats.maxAge;
            return actorStats.maxAge == 0 || num > pData.age || (morestats.maxAge == 0 && pData.level > 1) || !Toolbox.randomChance(0.15f) || pActor.haveTrait("asylum");
        }
        //人物窗口
        [HarmonyPrefix]
        [HarmonyPatch(typeof(WindowCreatureInfo), "OnEnable")]
        public static bool windowCreatureInfo(WindowCreatureInfo __instance)
        {
            if (Config.selectedUnit == null)
            {
                return false;
            }
            Actor selectedUnit = Config.selectedUnit;
            BaseStats curStats = selectedUnit.GetCurStats();
            ActorStatus data = selectedUnit.GetData();
            WindowCreatureInfoHelper helper = new WindowCreatureInfoHelper();
            helper.instance = __instance;
            __instance.nameInput.setText(data.firstName);
            __instance.health.setBar((float)data.health, (float)curStats.health, data.health.ToString() + "/" + curStats.health.ToString());
            if (selectedUnit.stats.needFood || selectedUnit.stats.unit)
            {
                __instance.hunger.gameObject.SetActive(true);
                int num = (int)((float)data.hunger / (float)selectedUnit.stats.maxHunger * 100f);
                __instance.hunger.setBar((float)num, 100f, num.ToString() + "%");
            }
            else
            {
                __instance.hunger.gameObject.SetActive(false);
            }
            __instance.damage.gameObject.SetActive(true);
            __instance.armor.gameObject.SetActive(true);
            __instance.speed.gameObject.SetActive(true);
            __instance.attackSpeed.gameObject.SetActive(true);
            __instance.crit.gameObject.SetActive(true);
            __instance.diplomacy.gameObject.SetActive(true);
            __instance.warfare.gameObject.SetActive(true);
            __instance.stewardship.gameObject.SetActive(true);
            __instance.intelligence.gameObject.SetActive(true);
            if (!selectedUnit.stats.unit)
            {
                __instance.diplomacy.gameObject.SetActive(false);
                __instance.warfare.gameObject.SetActive(false);
                __instance.stewardship.gameObject.SetActive(false);
                __instance.intelligence.gameObject.SetActive(false);
            }
            if (!selectedUnit.stats.inspect_stats)
            {
                __instance.damage.gameObject.SetActive(false);
                __instance.armor.gameObject.SetActive(false);
                __instance.speed.gameObject.SetActive(false);
                __instance.diplomacy.gameObject.SetActive(false);
                __instance.attackSpeed.gameObject.SetActive(false);
                __instance.crit.gameObject.SetActive(false);
            }
            __instance.damage.text.text = curStats.damage.ToString() ?? "";
            __instance.armor.text.text = curStats.armor.ToString() + "%";
            __instance.speed.text.text = curStats.speed.ToString() ?? "";
            __instance.crit.text.text = curStats.crit.ToString() + "%";
            __instance.attackSpeed.text.text = curStats.attackSpeed.ToString() ?? "";
            helper.showAttribute(__instance.diplomacy.text, curStats.diplomacy);
            helper.showAttribute(__instance.stewardship.text, curStats.stewardship);
            helper.showAttribute(__instance.intelligence.text, curStats.intelligence);
            helper.showAttribute(__instance.warfare.text, curStats.warfare);
            Sprite sprite = (Sprite)Resources.Load("ui/Icons/" + selectedUnit.stats.icon, typeof(Sprite));
            __instance.icon.sprite = sprite;
            __instance.avatarLoader.load(selectedUnit);
            if (selectedUnit.stats.hideFavoriteIcon)
            {
                __instance.iconFavorite.transform.parent.gameObject.SetActive(false);
            }
            else
            {
                __instance.iconFavorite.transform.parent.gameObject.SetActive(true);
            }
            __instance.text_description.text = "";
            __instance.text_values.text = "";
            Color32 colorAge = Color.green;
            if (data.age * 5 >= (Main.instance.actorToMoreStats[data.actorID].maxAge + AssetManager.unitStats.get(data.statsID).maxAge) << 2)
            {
                colorAge = Color.red;
            }
            helper.showStat("creature_statistics_age", data.age + "/" + (Main.instance.actorToMoreStats[data.actorID].maxAge + AssetManager.unitStats.get(data.statsID).maxAge), colorAge);
            if (selectedUnit.stats.inspect_kills)
            {
                helper.showStat("creature_statistics_kills", data.kills);
            }
            if (selectedUnit.stats.inspect_experience)
            {
                helper.showStat("creature_statistics_character_experience", data.experience.ToString() + "/" + Config.selectedUnit.getExpToLevelup().ToString());
            }
            if (selectedUnit.stats.inspect_experience)
            {
                helper.showStat("creature_statistics_character_level", data.level);
            }
            if (selectedUnit.stats.inspect_children)
            {
                helper.showStat("creature_statistics_children", data.children);
            }
            __instance.moodBG.gameObject.SetActive(false);
            __instance.favoriteFoodBg.gameObject.SetActive(false);
            __instance.favoriteFoodSprite.gameObject.SetActive(false);
            if (selectedUnit.stats.unit && !selectedUnit.stats.baby)
            {
                string pValue = "??";
                if (!string.IsNullOrEmpty(data.favoriteFood))
                {
                    pValue = LocalizedTextManager.getText(data.favoriteFood, null);
                    __instance.favoriteFoodBg.gameObject.SetActive(true);
                    __instance.favoriteFoodSprite.gameObject.SetActive(true);
                    __instance.favoriteFoodSprite.sprite = AssetManager.resources.get(data.favoriteFood).getSprite();
                }
                helper.showStat("creature_statistics_favorite_food", pValue);
            }
            if (selectedUnit.stats.unit)
            {
                __instance.moodBG.gameObject.SetActive(true);
                helper.showStat("creature_statistics_mood", LocalizedTextManager.getText("mood_" + data.mood, null));
                MoodAsset moodAsset = AssetManager.moods.get(data.mood);
                __instance.moodSprite.sprite = moodAsset.getSprite();
                if ((PersonalityAsset)Reflection.GetField(typeof(Actor), selectedUnit, "s_personality") != null)
                {
                    helper.showStat("creature_statistics_personality", LocalizedTextManager.getText("personality_" + ((PersonalityAsset)Reflection.GetField(typeof(Actor), selectedUnit, "s_personality")).id, null));
                }
            }
            Text text = __instance.text_description;
            text.text += "\n";
            Text text2 = __instance.text_values;
            text2.text += "\n";
            if (selectedUnit.stats.inspect_home)
            {
                string pID = "creature_statistics_homeVillage";
                object pValue2 = ((Config.selectedUnit.city != null) ? ((CityData)Reflection.GetField(typeof(City), Config.selectedUnit.city, "data")).cityName : "??");
                Kingdom kingdom = Config.selectedUnit.kingdom;
                Color? color;
                if (kingdom == null)
                {
                    color = null;
                }
                else
                {
                    KingdomColor kingdomColor = (KingdomColor)Reflection.GetField(typeof(Kingdom), kingdom, "kingdomColor");
                    color = ((kingdomColor != null) ? new Color?(kingdomColor.colorBorderOut) : null);
                }
                Color? color2 = color;
                helper.showStat(pID, pValue2, (color2 != null) ? new Color32?(color2.GetValueOrDefault()) : null);
            }
            if (Config.selectedUnit.kingdom != null && Config.selectedUnit.kingdom.isCiv())
            {
                string pID2 = "kingdom";
                object name = Config.selectedUnit.kingdom.name;
                Kingdom kingdom2 = Config.selectedUnit.kingdom;
                Color? color3;
                if (kingdom2 == null)
                {
                    color3 = null;
                }
                else
                {
                    KingdomColor kingdomColor2 = (KingdomColor)Reflection.GetField(typeof(Kingdom), kingdom2, "kingdomColor");
                    color3 = ((kingdomColor2 != null) ? new Color?(kingdomColor2.colorBorderOut) : null);
                }
                Color? color2 = color3;
                helper.showStat(pID2, name, (color2 != null) ? new Color32?(color2.GetValueOrDefault()) : null);
            }
            Culture culture = MapBox.instance.cultures.get(data.culture);
            if (culture != null)
            {
                string text3 = "";
                text3 += culture.name;
                text3 = text3 + "[" + culture.followers.ToString() + "]";
                text3 = Toolbox.coloredString(text3, new Color32?(culture.color32_text));
                helper.showStat("culture", text3);
                __instance.buttonCultures.SetActive(true);
            }
            else
            {
                __instance.buttonCultures.SetActive(false);
            }
            if (Config.selectedUnit.stats.isBoat)
            {
                Boat component = Config.selectedUnit.GetComponent<Boat>();
                helper.showStat("passengers", ((HashSet<Actor>)Reflection.GetField(typeof(Boat), component, "unitsInside")).Count);
                if ((bool)component.CallMethod("isState", BoatState.TransportDoLoading))
                {
                    helper.showStat("status", LocalizedTextManager.getText("status_waiting_for_passengers", null));
                }
            }
            __instance.text_description.GetComponent<LocalizedText>().CallMethod("checkTextFont");
            __instance.text_values.GetComponent<LocalizedText>().CallMethod("checkTextFont");
            __instance.text_description.GetComponent<LocalizedText>().CallMethod("checkSpecialLanguages");
            __instance.text_values.GetComponent<LocalizedText>().CallMethod("checkSpecialLanguages");
            if (LocalizedTextManager.isRTLLang())
            {
                __instance.text_description.alignment = TextAnchor.UpperRight;
                __instance.text_values.alignment = TextAnchor.UpperLeft;
            }
            else
            {
                __instance.text_description.alignment = TextAnchor.UpperLeft;
                __instance.text_values.alignment = TextAnchor.UpperRight;
            }
            if (selectedUnit.city == null)
            {
                __instance.buttonCity.SetActive(false);
            }
            else
            {
                __instance.buttonCity.SetActive(true);
            }
            if (selectedUnit.kingdom == null || !selectedUnit.kingdom.isCiv())
            {
                __instance.buttonKingdom.SetActive(false);
            }
            else
            {
                __instance.buttonKingdom.SetActive(true);
            }
            __instance.backgroundCiv.SetActive(__instance.buttonCity.activeSelf || __instance.buttonKingdom.activeSelf);
            helper.updateFavoriteIconFor(selectedUnit);
            helper.clearPrevButtons();
            __instance.CallMethod("loadTraits");
            helper.loadEquipment();
            return false;
        }
        //修改死亡机制
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "killHimself")]
        public static bool killHimself_Prefix(Actor __instance)
        {
            if (__instance.haveTrait("asylum"))
            {
                return false;
            }
            if (__instance.stats.id == "summonTian1")
            {
                Main.instance.summonTian1Limit++;
            }
            return true;
        }
        //待补充
        #endregion
    }
}
