using System.Collections.Generic;
using HarmonyLib;
using Cultivation_Way.Utils;
using UnityEngine;
using System.Diagnostics;
using System.Reflection;
using System.Linq;
using System.Reflection.Emit;
using System;
using ReflectionUtility;
using UnityEngine.UI;
using CultivationWay;

namespace Cultivation_Way
{
    internal class ExtendedActor : Actor
    {
        public MoreStats extendedCurStats = new MoreStats();
        public ExtendedActorData extendedData;
        public List<BaseSimObject> compositions = new List<BaseSimObject>();
        public ActorStatus easyData;
        public BaseStats easyCurStats;
        public ExtendedActorStats extendedStats;


        #region 拦截

        //龙的攻击逻辑
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "canFight")]
        public static bool canFight_Prefix(Actor __instance, ref bool __result)
        {
            EasternDragon e = __instance.GetComponent<EasternDragon>();
            if (e != null)
            {
                if (__instance.GetComponent<EasternDragon>().getState().shape == EasternDragonState.Shape.Human)
                {
                    __result = true;
                    return false;
                }
            }
            return true;
        }
        //龙的生成，暂时采用此处拦截
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), "addChildren")]
        public static void addChildren_Postfix(Actor __instance)
        {
            if (__instance.GetComponent<EasternDragon>() != null)
            {
                EasternDragon e = __instance.GetComponent<EasternDragon>();
                List<BaseActorComponent> list = new List<BaseActorComponent>();
                __instance.SetValue("children_special", list);
                list.Add(e);
                e.create();
            }
            else if (__instance.GetComponent<SpecialActor>() != null)
            {
                SpecialActor e = __instance.GetComponent<SpecialActor>();
                List<BaseActorComponent> list = new List<BaseActorComponent>();
                __instance.SetValue("children_special", list);
                list.Add(e);
                e.create();
            }
        }
        //释放法术
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "applyAttack")]
        public static bool useSpell(BaseSimObject pAttacker, BaseSimObject pTarget)
        {
            if (pAttacker.objectType != MapObjectType.Actor)
            {
                return true;
            }
            ExtendedActor pa = (ExtendedActor)pAttacker;
            ActorStatus dataA = pa.easyData;

            if (pa.extendedData.status.spells.Count != 0)
            {
                int count = pa.extendedData.status.spells.Count;
                int max = dataA.level >> 5 + 1;
                int index;
                for (int i = 0, num = 0; i < count && num < max; i++)
                {
                    if (!pTarget.base_data.alive)
                    {
                        return true;
                    }
                    index = Toolbox.randomInt(i, count);
                    ExtendedSpell spell = pa.extendedData.status.spells[index];
                    //进行蓝耗和冷却检查
                    if (pa.easyData.experience >= spell.cost && spell.GetSpellAsset().type == ExtendedSpellType.ATTACK
                        && spell.GetSpellAsset().requiredLevel <= dataA.level)
                    {
                        if (spell.castSpell(pAttacker, pTarget))
                        {
                            pa.easyData.experience -= spell.cost;
                            num++;
                        }
                    }
                }

            }
            return true;
        }
        //攻击距离判定修改（并入法术距离判定
        [HarmonyPostfix]
        [HarmonyPatch(typeof(Actor), "isInAttackRange")]
        public static void isInAttackRange_Postfix(ref bool __result, Actor __instance, BaseSimObject pObject)
        {
            if (__result)
            {
                return;
            }
            ExtendedActor pActor = (ExtendedActor)__instance;
            float rangeLimit = Mathf.Max(pActor.easyCurStats.range, pActor.extendedCurStats.spellRange) + pObject.GetValue<BaseStats>("curStats").size;
            foreach (ExtendedSpell spell in pActor.extendedData.status.spells)
            {
                if (pActor.canCastSpell(spell) && spell.GetSpellAsset().type == ExtendedSpellType.ATTACK)
                {
                    if (Toolbox.DistVec3(__instance.currentPosition, pObject.currentPosition) < rangeLimit)
                    {
                        __result = true;
                        return;
                    }
                }
            }
        }
        //经验条修改
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "getExpToLevelup")]
        public static bool getExpToLevelUp(Actor __instance, ref int __result)
        {
            ExtendedActor pActor = (ExtendedActor)__instance;
            ActorStatus data = pActor.easyData;
            if (data == null)
            {
                __result = int.MaxValue;
                return false;
            }
            ChineseElement element = pActor.extendedData.status.chineseElement;
            __result = (int)((100 + (data.level - 1) * (data.level - 1) * 50) * element.getImPurity() / element.GetAsset().rarity);
            return false;
        }
        //升级修改
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "addExperience")]
        public static bool addExperiece_Prefix(Actor __instance, int pValue)
        {
            ExtendedActor pActor = (ExtendedActor)__instance;
            //限制经验加成来源
            StackTrace st = new StackTrace();
            for (int i = 2; i < 5; i++)
            {
                if (st.GetFrame(i).GetMethod().Name.StartsWith("ap"))
                {
                    return false;
                }
            }
            AddAssetManager.cultisystemLibrary.get(pActor.extendedData.status.cultisystem).addExperience(pActor, pValue);
            return false;
        }
        //境界压制
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Actor), "getHit")]
        public static bool getHit_Prefix(Actor __instance, ref float pDamage, AttackType pType, BaseSimObject pAttacker, ref bool pSkipIfShake)
        {
            pSkipIfShake = false;
            if (__instance == null)
            {
                return true;
            }
            ExtendedActor pActor = (ExtendedActor)__instance;
            ActorStatus data1 = pActor.easyData;
            if (!data1.alive)
            {
                return true;
            }
            if (__instance.haveTrait("asylum"))
            {
                return false;
            }
            if (pType == AttackType.None)
            {
                if (pActor.easyCurStats.armor != 0)
                {
                    pDamage /= 1 - pActor.easyCurStats.armor / 100f;
                }
                return true;
            }//采用无类型伤害作为真伤判断
            if (pAttacker == null)
            {
                pDamage *= 1f - data1.level * 0.1f;
                float damageReduce = 1 - pActor.easyCurStats.armor / 100f;
                int damageResult = (int)(pDamage * damageReduce);
                if (damageResult <= 0)
                {
                    return true;
                }
                if (damageResult > pActor.extendedData.status.leftShied)
                {
                    pDamage += pActor.extendedData.status.leftShied / damageReduce;
                    pActor.extendedData.status.leftShied = 0;
                }
                return true;
            }
            if (__instance == pAttacker)
            {
                return false;
            }
            //人对人伤害增益
            if (pAttacker.objectType == MapObjectType.Actor)
            {
                ExtendedActor attacker = (ExtendedActor)pAttacker;
                ActorStatus data2 = attacker.easyData;
                if (data2.level <= data1.level - 10)
                {
                    return false;
                }
                if (data2.level < data1.level)
                {
                    pDamage *= 1 - (data1.level - data2.level + 1) * data1.level / 100f;
                }
                if (pAttacker.kingdom != null && pAttacker.kingdom.raceID == "EasternHuman" && __instance.kingdom != null)
                {
                    switch (__instance.kingdom.raceID)
                    {
                        case "Ming":
                            if (KingdomAndCityTools.checkZhongKui(attacker))
                                pDamage *= 1.5f;
                            break;
                        case "Yao":
                            if (KingdomAndCityTools.checkZhongKui(attacker))
                                pDamage *= 1.2f;
                            break;
                        case "Tian":
                            if (KingdomAndCityTools.checkZhongKui(attacker))
                                pDamage *= 1.1f;
                            break;
                    }
                }
                float damageReduce = 1 - pActor.easyCurStats.armor / 100f;
                int damageResult = (int)(pDamage * damageReduce);
                if (damageResult <= 0)
                {
                    return true;
                }
                if (damageResult > pActor.extendedData.status.leftShied)
                {
                    pDamage += pActor.extendedData.status.leftShied / damageReduce;
                    pActor.extendedData.status.leftShied = 0;
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
        public static bool updateAge_Prefix(Actor __instance)
        {
            if (__instance == null)
            {
                return true;
            }
            ExtendedActor actor = (ExtendedActor)__instance;
            ActorStatus data = actor.easyData;

            if (!new_updateAge(AssetManager.raceLibrary.get(__instance.stats.race), data, actor) && !__instance.haveTrait("immortal"))
            {
                __instance.killHimself(false, AttackType.Age, true, true);
                return false;
            }
            ChineseElement chunkElement = ExtendedWorldData.instance.chunkToElement[__instance.currentTile.chunk.id];
            ChineseElement actorElement = actor.extendedCurStats.element;
            float exp = (5 + actor.extendedData.status.cultiBook.rank) * actor.extendedData.status.cultiBook.cultiVelco[actor.getRealm() - 1];
            float mod = 0f;
            //待调整为与功法相关
            for (int i = 0; i < 5; i++)
            {
                int temp = chunkElement.baseElementContainer[i] % 100;
                mod += temp * (actorElement.baseElementContainer[i] + 1) / 1000f;
            }
            exp *= mod * actor.GetSpecialBody().rank;
            addExperiece_Prefix(__instance, (int)exp);

            if (data.age > 100 && Toolbox.randomChance(0.03f))
            {
                __instance.addTrait("wise");
            }
            actor.modifyCultivationBook();
            return false;
        }
        //属性实现
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(ActorBase), "updateStats")]
        public static IEnumerable<CodeInstruction> updateStats_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            #region 绑定函数
            MethodInfo getCity = AccessTools.Method(typeof(ActorTools), "GetCity");
            MethodInfo getBsFromMoreStats = AccessTools.Method(typeof(MoreStats), "GetBaseStats");
            MethodInfo part1 = AccessTools.Method(typeof(ActorTools), "dealStatsHelper1");
            MethodInfo part2 = AccessTools.Method(typeof(ActorTools), "dealStatsHelper2");
            MethodInfo addStats = AccessTools.Method(typeof(BaseStats), "addStats", new System.Type[] { typeof(BaseStats) });

            FieldInfo CurStats = AccessTools.Field(typeof(ExtendedActor), "curStats");
            FieldInfo extendedCurStats = AccessTools.Field(typeof(ExtendedActor), "extendedCurStats");
            //MethodInfo addStats = typeof(BaseStats).GetMethod("addStats", BindingFlags.Instance | BindingFlags.NonPublic);
            #endregion
            #region 属性添加处理
            int offset = 0;
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Callvirt, part1));
            offset++;//执行part1函数(done)
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Ldfld, CurStats));
            offset++;//获取并将CurStats压入栈
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Ldfld, extendedCurStats));
            offset++;
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Callvirt, getBsFromMoreStats));
            offset++;//获取MoreStats的BaseStats并压入栈
            codes.Insert(64 + offset, new CodeInstruction(OpCodes.Callvirt, addStats));
            offset++;//两者相加
            #endregion
            #region 性格设置（为妖族
            codes[211 + offset] = new CodeInstruction(OpCodes.Callvirt, getCity);
            codes[212 + offset] = new CodeInstruction(OpCodes.Nop);
            #endregion
            #region 属性规格化处理
            codes.Insert(728 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(728 + offset, new CodeInstruction(OpCodes.Callvirt, part2));
            offset++;
            #endregion
            return codes;
        }
        //寿命实现
        public static bool new_updateAge(Race pRace, ActorStatus pData, ExtendedActor pActor)
        {
            pData.age++;
            ((Action<ActorStatus, ActorStats, Race, bool>)pData.GetFastMethod("updateAttributes"))(pData, pActor.stats, pRace, false);
            if (!MapBox.instance.worldLaws.world_law_old_age.boolVal)
            {
                return pData.age < pActor.extendedStats.forceDeathAge;
            }
            int num = pActor.stats.maxAge;
            MoreStats morestats = pActor.extendedCurStats;
            if (!pActor.stats.id.StartsWith("summon"))
            {
                num += morestats.maxAge;
                Culture culture = MapBox.instance.cultures.get(pData.culture);
                if (culture != null)
                {
                    num += culture.getMaxAgeBonus();
                }
            }
            return (pActor.stats.maxAge == 0 || num > pData.age || !Toolbox.randomChance(0.15f) || pActor.haveTrait("asylum")) && (pData.age < pActor.extendedStats.forceDeathAge);
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
            ExtendedActor selectedUnit = (ExtendedActor)Config.selectedUnit;
            BaseStats curStats = selectedUnit.easyCurStats;
            ActorStatus data = selectedUnit.easyData;
            WindowCreatureInfoHelper helper = new WindowCreatureInfoHelper();
            helper.instance = __instance;
            __instance.nameInput.setText(data.firstName);
            __instance.health.setBar((float)data.health, (float)curStats.health, "/" + curStats.health.ToString(), true, false);
            if (selectedUnit.stats.needFood || selectedUnit.stats.unit)
            {
                __instance.hunger.gameObject.SetActive(true);
                int num = (int)((float)data.hunger / (float)selectedUnit.stats.maxHunger * 100f);
                __instance.hunger.setBar((float)num, 100f, "%");
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
            int maxAge = AssetManager.unitStats.get(data.statsID).maxAge;
            if (!data.statsID.StartsWith("summon"))
            {
                maxAge += ((ExtendedActor)selectedUnit).extendedCurStats.maxAge;
            }
            if (data.age * 5 >= maxAge << 2)
            {
                colorAge = Color.red;
            }
            helper.showStat("creature_statistics_age", data.age + "/" + maxAge, colorAge);
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
            if (__instance == null)
            {
                return true;
            }
            ExtendedActor pActor = (ExtendedActor)__instance;
            if (!pActor.easyData.alive)
            {
                return true;
            }
            if (pActor.haveTrait("asylum") && !pActor.stats.baby)
            {
                return false;
            }
            if (pActor.kingdom != null)
            {
                List<ExtendedActor> actors;
                if (ExtendedWorldData.instance.kingdomBindActors.TryGetValue(__instance.kingdom.id, out actors))
                {
                    if (actors.Remove(pActor))
                        MonoBehaviour.print("[MoreActors.killHimself_Prefix]" + pActor.easyData.statsID + ":" + pActor.easyData.actorID);
                }
            }
            return true;
        }
        //小孩问题修复
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Baby), "update")]
        public static bool update_Prefix(Baby __instance, float pElapsed)
        {
            if (__instance.timerGrow > pElapsed)
            {
                return true;
            }
            ExtendedActor actorF = __instance.GetComponent<ExtendedActor>();
            ActorStatus data = actorF.easyData;
            if (!data.alive)
            {
                return false;
            }
            if ((bool)MapBox.instance.CallMethod("isPaused"))
            {
                return false;
            }

            ExtendedActor actor = (ExtendedActor)MapBox.instance.createNewUnit(actorF.stats.growIntoID, actorF.currentTile, null, 0f, null);
            ActorStatus data1 = actor.easyData;
            actor.startBabymakingTimeout();
            data1.hunger = actor.stats.maxHunger / 2;
            Main.instance.gameStatsData.creaturesBorn--;
            if (actorF.city != null)
            {
                actorF.city.addNewUnit(actor, true, true);
            }
            actor.CallMethod("setKingdom", actorF.kingdom);
            data1.diplomacy = data.diplomacy;
            data1.intelligence = data.intelligence;
            data1.stewardship = data.stewardship;
            data1.warfare = data.warfare;
            data1.culture = data.culture;
            data1.experience = data.experience;
            data1.level = data.level;
            data1.firstName = data.firstName;
            if (data.skin != -1)
            {
                data1.skin = data.skin;
            }
            if (data.skin_set != -1)
            {
                data1.skin_set = data.skin_set;
            }
            data1.age = data.age;
            data1.bornTime = data.bornTime;
            data1.health = data.health;
            data1.gender = data.gender;
            data1.kills = data.kills;
            foreach (string text in data.traits)
            {
                if (!(text == "peaceful"))
                {
                    actor.addTrait(text);
                }
            }
            if (data.favorite)
            {
                data1.favorite = true;
            }
            if (MoveCamera.inSpectatorMode() && MoveCamera.focusUnit == actorF)
            {
                MoveCamera.focusUnit = actor;
            }
            ActorTools.copyMore(actorF, actor);
            actorF.killHimself(true, AttackType.GrowUp, false, false);
            return false;
        }
        //蛋问题修复
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Egg), "update", typeof(float))]
        public static IEnumerable<CodeInstruction> update_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo copyMore = AccessTools.Method(typeof(ActorTools), "copyMore", new System.Type[] { typeof(Actor), typeof(Actor), typeof(bool) });
            MethodInfo getActor = AccessTools.Method(typeof(ActorTools), "getActor", new System.Type[] { typeof(Egg) });
            Label ret = new Label();

            int offset = 0;
            codes.Insert(80 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(80 + offset, new CodeInstruction(OpCodes.Callvirt, getActor));
            offset++;
            codes.Insert(80 + offset, new CodeInstruction(OpCodes.Ldloc_0));
            offset++;
            codes.Insert(80 + offset, new CodeInstruction(OpCodes.Ldc_I4_0));
            offset++;
            codes.Insert(80 + offset, new CodeInstruction(OpCodes.Call, copyMore));
            offset++;
            codes[87 + offset].labels.Add(ret);
            codes[17].operand = ret;
            return codes.AsEnumerable();
        }
        //处理新生物
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ActorBase), "generatePersonality")]
        public static void generatePersonality_Postfix(ActorBase __instance)
        {
            ExtendedActor actor = (ExtendedActor)__instance;
            actor.easyData = __instance.GetValue<ActorStatus>("data", Types.t_ActorBase);
            actor.extendedCurStats.maxAge = __instance.stats.maxAge;
            actor.generateExtendedData();
            return;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "createNewUnit")]
        public static bool createNewUnit_Prefix(ref Actor __result, string pStatsID, WorldTile pTile, string pJob = null, float pZHeight = 0f, ActorData pData = null)
        {
            ActorStats actorStats = AssetManager.unitStats.get(pStatsID);

            if (actorStats == null)
            {
                __result = null;
                return false;
            }

            ExtendedActor component;
            try
            {
                component = UnityEngine.Object.Instantiate(Main.instance.prefabs.extendPrefabDict["actors/" + actorStats.prefab]).gameObject.GetComponent<ExtendedActor>();
                //component = UnityEngine.Object.Instantiate(Main.instance.prefabs.ExtendedActorPrefab).GetComponent<ExtendedActor>();
            }
            catch (Exception)
            {
                UnityEngine.Debug.Log("Tried to create actor: " + actorStats.id);
                UnityEngine.Debug.Log("Failed to load prefab: " + actorStats.prefab);
                __result = null;
                return false;
            }
            component.transform.name = actorStats.id;
            component.new_creature = true;
            component.easyCurStats = component.GetValue<BaseStats>("curStats", Types.t_ExtendedActor);
            component.setWorld();
            component.loadStats(actorStats);
            component.extendedStats = Main.instance.extendedActorStatsLibrary[pStatsID];
            if (pData != null)
            {
                component.easyData = pData.status;
                component.new_creature = false;
                component.extendedData = MoreKingdoms.temp_data;
                component.SetValue("data", pData.status, Types.t_ExtendedActor);
                component.SetValue("professionAsset", AssetManager.professions.get(pData.status.profession), Types.t_ExtendedActor);
            }
            if (component.new_creature)
            {
                ((Action<ExtendedActor, int>)component.GetFastMethod("newCreature", Types.t_ExtendedActor))(component, (int)(Main.instance.gameStatsData.gameTime + (double)MapBox.instance.units.Count));
            }
            component.transform.position = pTile.posV3;
            ((Action<ExtendedActor, WorldTile, float>)component.GetFastMethod("spawnOn", Types.t_ExtendedActor))(component, pTile, pZHeight);
            ((Action<ExtendedActor>)component.GetFastMethod("create", Types.t_ExtendedActor))(component);
            if (component.stats.kingdom != "")
            {
                ((Action<ExtendedActor, Kingdom>)component.GetFastMethod("setKingdom", Types.t_ExtendedActor))(component, MapBox.instance.kingdoms.dict_hidden[component.stats.kingdom]);
            }
            component.transform.parent = component.stats.hideOnMinimap ? Main.instance.transformUnits : Main.instance.transformCreatures;

            MapBox.instance.units.Add(component);
            __result = component;
            return false;
        }
        //解除绑定
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "destroyActor", new Type[] { typeof(Actor) })]
        public static bool unBindActorToMoreStats(Actor pActor)
        {
            if (pActor == null)
            {
                return true;
            }
            ExtendedActor actor = (ExtendedActor)pActor;
            if (actor.easyData == null)
            {
                return true;
            }
            foreach (string key in ExtendedWorldData.instance.creatureLimit.Keys)
            {
                if (pActor.stats.id == key)
                {
                    ExtendedWorldData.instance.creatureLimit[key]++;
                    break;
                }
            }

            ExtendedActorStatus moreData = actor.extendedData.status;
            if (pActor.kingdom == null)
            {
                return true;
            }
            foreach (string key in ExtendedWorldData.instance.kingdomBindActors.Keys)
            {
                if (pActor.kingdom.id == key)
                {
                    ExtendedWorldData.instance.kingdomBindActors[pActor.kingdom.id].Remove(actor);
                    break;
                }
            }
            return true;
        }

        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Actor), "spawnParticle")]
        public static IEnumerable<CodeInstruction> spawnParticle_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo get_sprite = AccessTools.Method(typeof(SpriteRenderer), "get_sprite");
            MethodInfo op_Inequality = AccessTools.Method(typeof(UnityEngine.Object), "op_Inequality");
            FieldInfo spriteRenderer = AccessTools.Field(typeof(Actor), "spriteRenderer");

            Label label = new Label();
            int offset = 0;
            codes.Insert(19 + offset, new CodeInstruction(OpCodes.Ldarg_0));
            offset++;
            codes.Insert(19 + offset, new CodeInstruction(OpCodes.Ldfld, spriteRenderer));
            offset++;
            codes.Insert(19 + offset, new CodeInstruction(OpCodes.Callvirt, get_sprite));
            offset++;
            codes.Insert(19 + offset, new CodeInstruction(OpCodes.Ldnull));
            offset++;
            codes.Insert(19 + offset, new CodeInstruction(OpCodes.Call, op_Inequality));
            offset++;
            codes.Insert(19 + offset, new CodeInstruction(OpCodes.Brfalse_S, label));
            offset++;
            codes[40 + offset].labels.Add(label);
            return codes.AsEnumerable();
        }
        #endregion
    }
}
