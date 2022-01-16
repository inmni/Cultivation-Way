using Cultivation_Way.Utils;
using CultivationWay;
using HarmonyLib;
using NCMS.Utils;
using ReflectionUtility;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Config;

namespace Cultivation_Way
{
    [Serializable]
    class MoreRaces
    {
        private static Race tRace;
        internal void init()
        {
            #region 天族
            Main.instance.moreRaces.Add("Tian");
            Race Tian = AssetManager.raceLibrary.clone("Tian", "human");
            tRace = Tian;
            Tian.culture_rate_tech_limit = 8;
            Tian.culture_knowledge_gain_per_intelligence = 1.2f;
            Tian.civ_baseCities = 1;
            Tian.civ_baseArmy = 5;
            Tian.civ_baseZones = 16;
            Tian.color = Toolbox.makeColor("#000000");
            Tian.icon = "iconTian";
            Tian.nameLocale = "Tians";
            Tian.bannerId = "human";
            Tian.hateRaces = "orc,dwarf";
            Tian.preferred_weapons.Clear();
            Tian.production = new string[] { "metals", "bread", "jam", "sushi", "cider" };
            Tian.skin_citizen_male = List.Of<string>(new string[] { "unit_male_1" });
            Tian.skin_citizen_female = List.Of<string>(new string[] { "unit_female_1" });
            Tian.skin_warrior = List.Of<string>(new string[] { "unit_warrior_1" });
            Tian.name_template_city = "Tian_city";
            Tian.name_template_kingdom = "Tian_kingdom";
            Tian.name_template_culture = "Tian_culture";
            Tian.culture_forbidden_tech.Add("building_roads");
            setPreferredStatPool("diplomacy#4,warfare#2,stewardship#2,intelligence#8");
            setPreferredFoodPool("berries#5,bread#5,fish#1,sushi#10,cider#10,tea#1");
            addPreferredWeapon("bow", 1);
            addPreferredWeapon("sword", 1);
            #endregion

            #region 冥族
            Main.instance.moreRaces.Add("Ming");
            Race Ming = AssetManager.raceLibrary.clone("Ming", "human");
            tRace = Ming;
            Ming.culture_rate_tech_limit = 8;
            Ming.culture_knowledge_gain_per_intelligence = 1.2f;
            Ming.civ_baseCities = 1;
            Ming.civ_baseArmy = 5;
            Ming.civ_baseZones = 16;
            Ming.color = Toolbox.makeColor("#FFFFFF");
            Ming.icon = "iconMing";
            Ming.nameLocale = "Mings";
            Ming.bannerId = "dwarf";
            Ming.hateRaces = "orc,dwarf,elf,human,Tian";
            Ming.preferred_weapons.Clear();
            Ming.production = new string[] { "metals", "bread", "jam", "sushi", "cider" };
            Ming.skin_citizen_male = List.Of<string>(new string[] { "unit_male_1" });
            Ming.skin_citizen_female = List.Of<string>(new string[] { "unit_female_1" });
            Ming.skin_warrior = List.Of<string>(new string[] { "unit_warrior_1" });
            Ming.name_template_city = "Ming_city";
            Ming.name_template_kingdom = "Ming_kingdom";
            Ming.name_template_culture = "Ming_culture";
            Ming.culture_forbidden_tech.Add("building_roads");
            setPreferredStatPool("diplomacy#4,warfare#2,stewardship#2,intelligence#8");
            setPreferredFoodPool("berries#5,bread#5,fish#1,sushi#10,cider#10,tea#1");
            addPreferredWeapon("bow", 1);
            addPreferredWeapon("sword", 1);
            #endregion

        }
        internal void setIntelligentRaceFeature()
        {
            RaceFeature humanFeature = Main.instance.raceFeatures["human"];

            RaceFeature elfFeature = Main.instance.raceFeatures["elf"];

            RaceFeature dwarfFeature = Main.instance.raceFeatures["dwarf"];

            RaceFeature orcFeature = Main.instance.raceFeatures["orc"];

            RaceFeature TianFeature = Main.instance.raceFeatures["Tian"];
            TianFeature.raceSpells.Add(new ExtensionSpell("summonTian"));
            TianFeature.raceSpells.Add(new ExtensionSpell("summonTian1"));
            RaceFeature MingFeature = Main.instance.raceFeatures["Ming"];
            MingFeature.raceSpells.Add(new ExtensionSpell("summon") { might = 2f });
        }
        internal void setOtherRaceFeature()
        {
            RaceFeature JiaoDragonFeature = Main.instance.raceFeatures["JiaoDragon"];
            JiaoDragonFeature.raceSpells.Add(new ExtensionSpell("JiaoDragon_laser"));
            RaceFeature MengZhuFeature = Main.instance.raceFeatures["MengZhu"];
            MengZhuFeature.raceSpells.Add(new ExtensionSpell("lightning"));
            MengZhuFeature.raceSpells.Add(new ExtensionSpell("summonTian"));
        }

        //国家颜色
        public static void kingdomColorsDataInit()
        {
            KingdomColorsData kingdomColorsData = JsonUtility.FromJson<KingdomColorsData>(ResourcesHelper.LoadTextAsset("colors/kingdom_colors.json"));
            KingdomColors.dict = new Dictionary<string, KingdomColorContainer>();
            foreach (KingdomColorContainer kingdomColorContainer in kingdomColorsData.colors)
            {
                foreach (KingdomColor kingdomColor in kingdomColorContainer.list)
                {
                    kingdomColor.initColor();
                }
                KingdomColors.dict.Add(kingdomColorContainer.race, kingdomColorContainer);
            }
        }

        #region 原版的函数
        private static void setPreferredStatPool(string pString)
        {
            pString = pString.Replace(" ", string.Empty);
            string[] array = pString.Split(new char[] { ',' });
            for (int i = 0; i < array.Length; i++)
            {
                string[] array2 = array[i].Split(new char[] { '#' });
                int num = int.Parse(array2[1]);
                string item = array2[0];
                for (int j = 0; j < num; j++)
                {
                    tRace.preferred_attribute.Add(item);
                }
            }
        }

        private static void setPreferredFoodPool(string pString)
        {
            pString = pString.Replace(" ", string.Empty);
            string[] array = pString.Split(new char[] { ',' });
            for (int i = 0; i < array.Length; i++)
            {
                string[] array2 = array[i].Split(new char[] { '#' });
                int num = int.Parse(array2[1]);
                string item = array2[0];
                for (int j = 0; j < num; j++)
                {
                    tRace.preferred_food.Add(item);
                }
            }
        }

        private static void addPreferredWeapon(string pID, int pAmount)
        {
            for (int i = 0; i < pAmount; i++)
            {
                tRace.preferred_weapons.Add(pID);
            }
        }
        #endregion

        #region 拦截

        //城市界面
        [HarmonyPostfix]
        [HarmonyPatch(typeof(CityWindow), "showInfo")]
        public static void showInfo_Postfix(ref CityWindow __instance)
        {
            if (__instance == null || selectedCity == null || (Race)Reflection.GetField(typeof(City), selectedCity, "race") == null)
            {
                return;
            }
            if (Main.instance.moreRaces.Contains(((Race)Reflection.GetField(typeof(City), selectedCity, "race")).id))
            {
                __instance.icon.sprite = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/" + ((Race)Reflection.GetField(typeof(City), selectedCity, "race")).icon + ".png");
            }
        }
        //主贴图加载
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorAnimationLoader), "generateAnimation")]
        public static bool generateAnimation_Prefix(string pSheetPath, ActorStats pStats, ref AnimationDataUnit __result, ActorAnimationLoader __instance)
        {
            if (Main.instance.moreActors.Contains(pStats.id))
            {
                //根据原版的路径构建新路径
                AnimationDataUnit animationDataUnit = new AnimationDataUnit();
                //贴图覆盖
                Dictionary<string, Sprite> sprites = Reflection.GetField(animationDataUnit.GetType(), animationDataUnit, "sprites") as Dictionary<string, Sprite>;
                sprites = new Dictionary<string, Sprite>();
                string[] names = new string[8] { "swim_0", "swim_1", "swim_2", "swim_3", "walk_0", "walk_1", "walk_2", "walk_3" };
                foreach (string name in names)
                {
                    Sprite sprite = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/{pSheetPath}/{name}.png",ActorBase.spriteOffset.x);
                    sprite.name = name;
                    sprites.Add(name, sprite);
                }
                if (Main.instance.moreRaces.Contains(pStats.race))
                {
                    for(int i = 4; i < 8; i++)
                    {
                        float extraOffset = 0f;
                        if (i == 5 || i == 6)
                        {
                            extraOffset = 0.5f;
                        }
                        Sprite sprite = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/{pSheetPath}/{names[i]+"_item"}.png", ActorBase.spriteOffset.x, ActorBase.spriteOffset.y*(3.5f+extraOffset));
                        sprite.name = names[i]+"_item";
                        sprites.Add(names[i]+"_item", sprite);
                    }
                }

                //framedata重新生成

                Reflection.SetField(animationDataUnit, "frameData", new Dictionary<string, AnimationFrameData>());
                Reflection.SetField(animationDataUnit, "id", pSheetPath);
                Reflection.CallStaticMethod(typeof(ActorAnimationLoader), "generateFrameData", animationDataUnit, sprites, "walk_0,walk_1,walk_2,walk_3,swim_0,swim_1,swim_2,swim_3");

                Dictionary<string, AnimationDataUnit> _dict_units = Reflection.GetField(typeof(ActorAnimationLoader), __instance, "dict_units") as Dictionary<string, AnimationDataUnit>;
                _dict_units.Add(pSheetPath, animationDataUnit);

                if (pStats.animation_swim != string.Empty)
                {
                    ActorAnimation anim = (ActorAnimation)Reflection.CallStaticMethod(typeof(ActorAnimationLoader), "createAnim", 0, sprites, pStats.animation_swim, pStats.animation_swim_speed);
                    Reflection.SetField(animationDataUnit, "swimming", anim);
                }
                if (pStats.animation_walk != string.Empty)
                {
                    ActorAnimation anim = (ActorAnimation)Reflection.CallStaticMethod(typeof(ActorAnimationLoader), "createAnim", 1, sprites, pStats.animation_walk, pStats.animation_walk_speed);
                    Reflection.SetField(animationDataUnit, "walking", anim);
                }
                if (pStats.animation_idle != string.Empty)
                {
                    ActorAnimation anim = (ActorAnimation)Reflection.CallStaticMethod(typeof(ActorAnimationLoader), "createAnim", 2, sprites, pStats.animation_idle, pStats.animation_idle_speed);
                    Reflection.SetField(animationDataUnit, "idle", anim);
                }
                __result = animationDataUnit;
                return false;
            }
            return true;
        }
        //取消头部贴图
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorBase), "findHeadSprite")]
        public static bool findHeadSprite_Prefix(ActorBase __instance)
        {
            if (Main.instance.moreRaces.Contains(__instance.stats.race))
            {
                return false;
            }
            return true;
        }
        //船只贴图加载
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActorAnimationLoader), "loadAnimationBoat")]
        public static bool loadAnimationBoat_Prefix(ref string pTexturePath, ActorAnimationLoader __instance)
        {
            if (pTexturePath.EndsWith("_"))
            {
                pTexturePath = pTexturePath + "human";
                return true;
            }
            foreach (string race in Main.instance.moreRaces)
            {
                if (pTexturePath.Contains(race))
                {
                    pTexturePath = pTexturePath.Replace(race, "human");
                    return true;
                }
            }
            return true;
        }
        //音频加载
        [HarmonyPrefix]
        [HarmonyPatch(typeof(sfx.MusicMan), "clear")]
        public static bool sfx_MusicMan_clear_Prefix(sfx.MusicMan __instance)
        {
            if (sfx.MusicMan.races.Count == 0)
            {
                foreach (string race in Main.instance.moreRaces)
                {
                    sfx.MusicMan.races.Add(race, new sfx.MusicRaceContainer());
                }
            }
            return true;
        }
        #endregion
    }

}
