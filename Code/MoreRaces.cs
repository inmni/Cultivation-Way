
using Cultivation_Way.Utils;
using CultivationWay;
using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace Cultivation_Way
{
    [Serializable]
    internal class MoreRaces
    {
        private static Race tRace;
        internal void init()
        {

            #region 东方人族
            Main.instance.addRaces.Add("EasternHuman");
            Race EasternHuman = AssetManager.raceLibrary.clone("EasternHuman", "human");
            tRace = EasternHuman;
            EasternHuman.build_order_id = "EasternHuman";
            EasternHuman.path_icon = "ui/Icons/iconEasternHuman";
            EasternHuman.nameLocale = "EasternHumans";
            EasternHuman.production = new string[] { "bread", "pie", "tea" };
            EasternHuman.skin_citizen_male = List.Of<string>(new string[] { "unit_male_1" });
            EasternHuman.skin_citizen_female = List.Of<string>(new string[] { "unit_female_1" });
            EasternHuman.skin_warrior = List.Of<string>(new string[] { "unit_warrior_1" });
            setPreferredStatPool("diplomacy#1,warfare#1,stewardship#1,intelligence#1");
            setPreferredFoodPool("bread#1,fish#1,tea#1");
            addPreferredWeapon("bow", 1);
            addPreferredWeapon("sword", 1);
            #endregion
            #region 巫族
            Main.instance.addRaces.Add("Wu");
            Race Wu = AssetManager.raceLibrary.clone("Wu", "human");
            tRace = Wu;
            Wu.build_order_id = "Wu";
            Wu.path_icon = "ui/Icons/iconWu";
            Wu.nameLocale = "Wus";
            Wu.name_template_city = "Wu_city";
            Wu.name_template_kingdom = "Wu_kingdom";
            Wu.name_template_culture = "Wu_culture";
            Wu.skin_citizen_male = List.Of<string>(new string[] { "unit_male_1" });
            Wu.skin_citizen_female = List.Of<string>(new string[] { "unit_female_1" });
            Wu.skin_warrior = List.Of<string>(new string[] { "unit_warrior_1" });
            setPreferredStatPool("diplomacy#1,warfare#1,stewardship#1,intelligence#1");
            setPreferredFoodPool("meat#5,bread#1,fish#5,tea#1");
            addPreferredWeapon("bow", 1);
            addPreferredWeapon("sword", 1);
            #endregion
            #region 天族
            Main.instance.addRaces.Add("Tian");
            Race Tian = AssetManager.raceLibrary.clone("Tian", "human");
            tRace = Tian;
            Tian.culture_rate_tech_limit = 8;
            Tian.culture_knowledge_gain_per_intelligence = 1.2f;
            Tian.color = Toolbox.makeColor("#000000");
            Tian.path_icon = "ui/Icons/iconTian";
            Tian.nameLocale = "Tians";
            Tian.build_order_id = "Tian";
            Tian.banner_id = "human";
            Tian.hateRaces = "orc,dwarf";
            Tian.preferred_weapons.Clear();
            Tian.production = new string[] { "bread", "jam", "sushi", "cider" };
            Tian.skin_citizen_male = List.Of<string>(new string[] { "unit_male_1" });
            Tian.skin_citizen_female = List.Of<string>(new string[] { "unit_female_1" });
            Tian.skin_warrior = List.Of<string>(new string[] { "unit_warrior_1" });
            Tian.name_template_city = "Tian_city";
            Tian.name_template_kingdom = "Tian_kingdom";
            Tian.name_template_culture = "Tian_culture";
            Tian.culture_forbidden_tech.Add("building_roads");
            Tian.culture_forbidden_tech.Add("Circumvallation_1");
            setPreferredStatPool("diplomacy#4,warfare#2,stewardship#2,intelligence#8");
            setPreferredFoodPool("berries#5,bread#5,fish#1,sushi#10,cider#10,tea#1");
            addPreferredWeapon("knife1", 1);
            addPreferredWeapon("knife2", 1);
            addPreferredWeapon("minigun1", 6);
            addPreferredWeapon("great_gun1", 6);
            addPreferredWeapon("lightning1", 10);
            #endregion

            #region 冥族
            Main.instance.addRaces.Add("Ming");
            Race Ming = AssetManager.raceLibrary.clone("Ming", "human");
            tRace = Ming;
            Ming.culture_rate_tech_limit = 8;
            Ming.culture_knowledge_gain_per_intelligence = 1.2f;
            Ming.color = Toolbox.makeColor("#FFFFFF");
            Ming.path_icon = "ui/Icons/iconMing";
            Ming.nameLocale = "Mings";
            Ming.build_order_id = "Ming";
            Ming.banner_id = "dwarf";
            Ming.hateRaces = "orc,dwarf,elf,human,Tian";
            Ming.preferred_weapons.Clear();
            Ming.production = new string[] { "bread", "jam", "sushi", "cider" };
            Ming.skin_citizen_male = List.Of<string>(new string[] { "unit_male_1" });
            Ming.skin_citizen_female = List.Of<string>(new string[] { "unit_female_1" });
            Ming.skin_warrior = List.Of<string>(new string[] { "unit_warrior_1" });
            Ming.name_template_city = "Ming_city";
            Ming.name_template_kingdom = "Ming_kingdom";
            Ming.name_template_culture = "Ming_culture";
            Ming.culture_forbidden_tech.Add("Circumvallation_1");
            setPreferredStatPool("diplomacy#4,warfare#2,stewardship#2,intelligence#8");
            setPreferredFoodPool("berries#5,bread#5,fish#1,sushi#10,cider#10,tea#1");
            addPreferredWeapon("bow", 1);
            addPreferredWeapon("sword", 1);
            #endregion

            #region 妖族
            Main.instance.addRaces.Add("Yao");
            Race Yao = AssetManager.raceLibrary.clone("Yao", "human");
            tRace = Yao;
            Yao.culture_rate_tech_limit = 8;
            Yao.culture_knowledge_gain_per_intelligence = 1.2f;
            Yao.color = Toolbox.makeColor("#000000");
            Yao.path_icon = "ui/Icons/iconYao";
            Yao.nameLocale = "Yaos";
            Yao.banner_id = "orc";
            Yao.build_order_id = "Yao";
            Yao.hateRaces = "elf,human";
            Yao.preferred_weapons.Clear();
            Yao.production = new string[] { "bread" };
            Yao.skin_citizen_male = List.Of<string>(new string[] { "unit_male_1" });
            Yao.skin_citizen_female = List.Of<string>(new string[] { "unit_female_1" });
            Yao.skin_warrior = List.Of<string>(new string[] { "unit_warrior_1" });
            Yao.name_template_city = "Yao_city";
            Yao.name_template_kingdom = "Yao_kingdom";
            Yao.name_template_culture = "Yao_culture";
            Yao.culture_forbidden_tech.Add("building_roads");
            Yao.culture_forbidden_tech.Add("Circumvallation_1");
            setPreferredStatPool("diplomacy#1,warfare#8,stewardship#1,intelligence#1");
            setPreferredFoodPool("meat#10,fish#5,bread#1,sushi#1,cider#1,tea#1");
            addPreferredWeapon("stick", 5);
            addPreferredWeapon("axe", 10);
            addPreferredWeapon("spear", 3);
            addPreferredWeapon("bow", 1);
            #endregion
            Race human = AssetManager.raceLibrary.get("human");
            human.culture_forbidden_tech.Add("Circumvallation_1");

            Race orc = AssetManager.raceLibrary.get("orc");
            orc.culture_forbidden_tech.Add("Circumvallation_1");

            Race elf = AssetManager.raceLibrary.get("elf");
            elf.culture_forbidden_tech.Add("Circumvallation_1");

            Race dwarf = AssetManager.raceLibrary.get("dwarf");
            dwarf.culture_forbidden_tech.Add("Circumvallation_1");
        }

        //国家颜色
        public static void kingdomColorsDataInit()
        {
            KingdomColorsData kingdomColorsData = JsonUtility.FromJson<KingdomColorsData>(ResourcesHelper.LoadTextAsset("colors/kingdom_colors.json"));
            Dictionary<string, KingdomColorContainer> dict = (Dictionary<string, KingdomColorContainer>)typeof(KingdomColors).GetField("dict", BindingFlags.Static | BindingFlags.NonPublic).GetValue(null);
            foreach (KingdomColorContainer kingdomColorContainer in kingdomColorsData.colors)
            {
                foreach (KingdomColor kingdomColor in kingdomColorContainer.list)
                {
                    kingdomColor.initColor();
                }
                dict.Add(kingdomColorContainer.race, kingdomColorContainer);
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
        //妖族行为
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ActorBase), "nextJobActor")]
        public static void nextJobActor_Postfix(ref string __result, ActorBase pActor)
        {
            if (pActor.stats.race == "Yao")
            {
                if (pActor.stats.baby)
                {
                    __result = "baby";
                }
                else if (pActor.city != null)
                {
                    __result = "citizen";
                }
            }
        }
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(Actor), "create")]
        public static IEnumerable<CodeInstruction> create_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo isCitizen = AccessTools.Method(typeof(MoreRaces), "isCitizen");
            int offset = 0;
            codes[20 + offset] = new CodeInstruction(OpCodes.Callvirt, isCitizen);
            codes[21 + offset] = new CodeInstruction(OpCodes.Nop);
            return codes.AsEnumerable();
        }
        public static bool isCitizen(Actor actor)
        {
            return (actor.stats.unit || Main.instance.moreActors.protoAndYao == null
                    || Main.instance.moreActors.protoAndShengs == null
                    || Main.instance.moreActors.protoAndShengs.Count < 2
                    || Main.instance.moreActors.protoAndYao.GetSeconds().Contains(actor.stats.id)
                    || Main.instance.moreActors.protoAndShengs[0].GetSeconds().Contains(actor.stats.id)
                    || Main.instance.moreActors.protoAndShengs[1].GetSeconds().Contains(actor.stats.id)
                    || actor.stats.id == "EasternDragon");
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
            foreach (string race in Main.instance.addRaces)
            {
                if (pTexturePath.Contains(race))
                {
                    pTexturePath = pTexturePath.Replace(race, "human");
                    return true;
                }
            }
            return true;
        }
        #endregion

    }

}
