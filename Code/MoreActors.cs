using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Cultivation_Way
{
    class MoreActors
    {
        private static List<int> _color_sets = new List<int>();
        internal void init()
        {
            ActorStats TianAsset = AssetManager.unitStats.clone("Tian", "unit_human");
            TianAsset.maxAge = 500;
            TianAsset.race = "Tian";
            TianAsset.unit = true;
            TianAsset.needFood = false;
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
            #region BOSS
            ActorStats JiaoDragonAsset = AssetManager.unitStats.clone("JiaoDragon", "wolf");
            JiaoDragonAsset.maxAge = 1000;
            JiaoDragonAsset.race = "boss";
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
            JiaoDragonAsset.attack_spells = new List<string> { "fire", "lightning", "lightning", "lightning", "fire" };
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
            addActor(JiaoDragonAsset);

            ActorStats XieDragonAsset = AssetManager.unitStats.clone("XieDragon", "JiaoDragon");
            XieDragonAsset.nameLocale = "XieDragon";
            XieDragonAsset.nameTemplate = "default_name";
            XieDragonAsset.shadowTexture = "unitShadow_5";
            XieDragonAsset.texture_path = "XieDragon";
            XieDragonAsset.icon = "iconXieDrangon";
            XieDragonAsset.setBaseStats(1000000, 10000, 0, 99, 100, 100);
            XieDragonAsset.attack_spells = new List<string> { "fire", "lightning", "lightning", "lightning", "fire" };
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
            MengZhuAsset.nameLocale = "盟主";
            MengZhuAsset.nameTemplate = "MengZhu_name";
            MengZhuAsset.shadowTexture = "unitShadow_5";
            MengZhuAsset.useSkinColors = false;
            MengZhuAsset.inspect_home = false;
            MengZhuAsset.inspect_children = false;
            MengZhuAsset.ignoreTileSpeedMod = true;
            MengZhuAsset.needFood = false;
            MengZhuAsset.texture_path = "盟主";
            MengZhuAsset.texture_heads = "";
            MengZhuAsset.icon = "icon盟主";
            MengZhuAsset.traits = new List<string>{ "immortal", "cursed_immune", "fire_proof", "freeze_proof", "poison_immune", "immune", "healing_aura" };
            MengZhuAsset.attack_spells = new List<string> { "bloodRain", "lightning",};
            MengZhuAsset.setBaseStats(5000000, 10000, 15, 99, 100, 100);
            MengZhuAsset.defaultAttack = "snowball";
            MengZhuAsset.damagedByOcean = false;
            MengZhuAsset.baseStats.areaOfEffect = 5f;
            MengZhuAsset.baseStats.range = 30f;
            MengZhuAsset.baseStats.scale = 0.02f;
            MengZhuAsset.actorSize = ActorSize.S13_Human;
            MengZhuAsset.baseStats.projectiles = 20;
            MengZhuAsset.baseStats.knockback = 100f;
            MengZhuAsset.baseStats.knockbackReduction = 100f;
            addActor(MengZhuAsset);
            #endregion
        }

        private void addActor(ActorStats pStats)
        {
            Main.instance.moreActors.Add(pStats.id);
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
                int random = _color_sets.GetRandom<int>();
                ((ActorStatus)Reflection.GetField(typeof(Actor), pActor, "data")).skin_set = random;
            }
        }

        #endregion

        #region 拦截
        //经验条修改
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "getExpToLevelup")]
        public static bool getExpToLevelUp(Actor __instance, ref int __result)
        {
             ActorStatus data = Reflection.GetField(typeof(Actor), __instance, "data") as ActorStatus;
            __result= (int)((100 + (data.level - 1) * (data.level - 1) * 50) * Main.instance.actorToMoreStats[((ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data")).actorID].element.getPurity());
            return false;
        }
        //升级修改
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "addExperience")]
        public static bool addExperiece_Prefix(Actor __instance, int pValue)
        {
            ActorStatus data = Reflection.GetField(typeof(Actor), __instance, "data") as ActorStatus;
            int num = 110;
            if (data.level >= num)
            {
                return false;
            }
            data.experience += pValue;
            while (data.experience >= __instance.getExpToLevelup())
            {
                //缺少其他升级条件
                data.experience -= __instance.getExpToLevelup();
                data.level++;
                //准备雷劫

                //修改属性
                MoreStats stats = Main.instance.actorToMoreStats[((ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data")).actorID];
                int realm = data.level;
                if (realm > 10)
                {
                    realm = (realm + 9) / 10;
                }
                if (data.level > Main.instance.actorToMoreStats[((ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data")).actorID].family.maxLevel)
                {
                    Main.instance.actorToMoreStats[((ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data")).actorID].family.levelUp(data.firstName);
                }

                stats.addAnotherStats(((CultisystemLibrary)AssetManager.instance.dict["cultisystem"]).get(stats.cultisystem).moreStats[realm - 1]);
                stats.addAnotherStats(Main.instance.actorToMoreStats[((ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data")).actorID].family.cultivationBook.stats[realm - 1]);
                __instance.setStatsDirty();
                Reflection.SetField(__instance, "event_full_heal", true);
            }
            return false;
        }
        //境界压制
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor),"getHit")]
        public static bool getHit_Prefix(Actor __instance,float pDamage,BaseSimObject pAttacker = null)
        {
            if (pAttacker == null)
            {
                return true;
            }
            ActorStatus data1 = Reflection.GetField(typeof(Actor), __instance, "data") as ActorStatus;
            //人对人伤害减免
            if (pAttacker.objectType == MapObjectType.Actor)
            {
                ActorStatus data2 = Reflection.GetField(typeof(Actor), pAttacker, "data") as ActorStatus;
                if (data2.level < data1.level)
                {
                    pDamage *= 1-(data1.level - data2.level+1)*data1.level/500f;
                }
            }
            //人对其他伤害减免
            else if(data1.level>=10)
            {
                pDamage *= 1f - data1.level * 0.009f;
            }
            return true;
        }
        //每年修炼经验修改
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "updateAge")]
        public static bool updateAge(Actor __instance)
        {
            ActorStatus data = Reflection.GetField(typeof(Actor), __instance, "data") as ActorStatus;

            if (!updateAge(AssetManager.raceLibrary.get(__instance.stats.race), data, __instance) && !__instance.haveTrait("immortal"))
            {
                __instance.killHimself(false, AttackType.Age, true, true);
                return false;
            }
            ChineseElement chunkElement = Main.instance.chunkToElement[__instance.currentTile.chunk.id];
            ChineseElement actorElement = Main.instance.actorToMoreStats[((ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data")).actorID].element;
            float exp = 5;
            for (int i = 0; i < 5; i++)
            {
                int temp = chunkElement.baseElementContainer[i] % 100;
                exp *= temp / (float)(actorElement.baseElementContainer[i] + 1);
            }
            addExperiece_Prefix(__instance, (int)exp);
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
            if (data.age > 40 && Toolbox.randomChance(0.3f))
            {
                __instance.addTrait("wise");
            }
            return false;
        }
        //属性添加
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorBase), "updateStats")]
        public static bool updateStats_Prefix(Actor __instance)
        {
            if (!Main.instance.actorToMoreStats.Keys.Contains(((ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data")).actorID))
            {
                MoreStats moreStats = new MoreStats(__instance);
                Main.instance.actorToMoreStats.Add(((ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data")).actorID, moreStats);
                string name = ((ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data")).firstName;
                foreach (string fn in ChineseNameAsset.familyNameTotal)
                {
                    if (name.StartsWith(fn))
                    {
                        moreStats.family = Main.instance.familys[fn];
                        break;
                    }
                }

                ChineseElementLibrary elementLibrary= (ChineseElementLibrary)AssetManager.instance.dict["element"];
                if (Main.dsa.ContainsKey(elementLibrary.dict[moreStats.element.element.id].name))
                {
                    Main.dsa[elementLibrary.dict[moreStats.element.element.id].name]++;
                }
                else
                {
                    Main.dsa.Add(elementLibrary.dict[moreStats.element.element.id].name, 1);
                }
              
            }
            if (__instance.getCulture() != null && Main.instance.actorToMoreStats[((ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data")).actorID].cultisystem == "default")
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
                    Main.instance.actorToMoreStats[((ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data")).actorID].cultisystem = cultisystem.GetRandom().Remove(0,6);
                }
            }
            BaseStats curStats = (BaseStats)Reflection.GetField(typeof(Actor), __instance, "curStats");
            ActorStatus data = (ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data");
            Reflection.SetField(__instance, "statsDirty", false);
            if (!data.alive)
            {
                return false;
            }
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
            curStats.CallMethod("clear");
            curStats.CallMethod("addStats", __instance.stats.baseStats);
            curStats.CallMethod("addStats", moodAsset.baseStats);
            curStats.CallMethod("addStats", Main.instance.actorToMoreStats[((ActorStatus)Reflection.GetField(typeof(Actor), __instance, "data")).actorID].baseStats);
            curStats.diplomacy += data.diplomacy;
            curStats.stewardship += data.stewardship;
            curStats.intelligence += data.intelligence;
            curStats.warfare += data.warfare;
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
            curStats.normalize();
            if ((bool)Reflection.GetField(typeof(Actor), __instance, "event_full_heal"))
            {
                Reflection.SetField(__instance, "event_full_heal", false);
                data.health = curStats.health;
            }
            curStats.health += (int)((float)curStats.health * (curStats.mod_health / 100f));
            curStats.damage += (int)((float)curStats.damage * (curStats.mod_damage / 100f));
            curStats.armor += (int)((float)curStats.armor * (curStats.mod_armor / 100f));
            curStats.crit += (float)((int)(curStats.crit * (curStats.mod_crit / 100f)));
            curStats.diplomacy += (int)((float)curStats.diplomacy * (curStats.mod_diplomacy / 100f));
            curStats.speed += (float)((int)(curStats.speed * (curStats.mod_speed / 100f)));
            curStats.attackSpeed += (float)((int)(curStats.attackSpeed * (curStats.mod_attackSpeed / 100f)));
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
            if (curStats.armor > 80+data.level/6)
            {
                curStats.armor = 80 + data.level / 6;
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
            Reflection.SetField(__instance, "attackTimer", 0f);
            typeof(ActorBase).GetMethod("updateTargetScale", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance).Invoke(__instance, null);
            curStats.normalize();
            __instance.currentScale.x = curStats.scale;
            __instance.currentScale.y = curStats.scale;
            __instance.currentScale.z = curStats.scale;
            return false;
        }
        //寿命实现
        public static bool updateAge(Race pRace,ActorStatus pData,Actor pActor)
        {
            pData.age++;
            ActorStats actorStats = AssetManager.unitStats.get(pData.statsID);
            pData.CallMethod("updateAttributes",actorStats, pRace, false);
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
            num += Main.instance.actorToMoreStats[((ActorStatus)Reflection.GetField(typeof(Actor), pActor, "data")).actorID].maxAge;
            return  actorStats.maxAge == 0 || num > pData.age || !Toolbox.randomChance(0.15f);
        }
        //人物窗口
        //待补充
        #endregion
    }
}
