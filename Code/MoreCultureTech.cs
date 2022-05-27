using CultivationWay;
using HarmonyLib;
using NCMS.Utils;
using ReflectionUtility;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way
{
    class MoreCultureTech
    {
        internal void init()
        {
            foreach (CultisystemAsset cultisystem in ((CultisystemLibrary)AssetManager.instance.dict["cultisystem"]).list)
            {
                AssetManager.culture_tech.add(new CultureTechAsset
                {
                    id = "culti_" + cultisystem.id,
                    path_icon = cultisystem.id,
                    type = TechType.Rare,
                    enabled = true,
                });
            }
            AssetManager.culture_tech.get("culti_default").enabled = false;
            //城墙1级
            AssetManager.culture_tech.add(new CultureTechAsset
            {
                id = "Circumvallation_1",
                path_icon = "Circumvallation_1",
                type = TechType.Common,
                requirements = new System.Collections.Generic.List<string>() { "housing_3" },
                enabled = true,
            });
            //城墙2级
            AssetManager.culture_tech.add(new CultureTechAsset
            {
                id = "Circumvallation_2",
                path_icon = "Circumvallation_2",
                type = TechType.Common,
                requirements = new System.Collections.Generic.List<string>() { "housing_2", "Circumvallation_1" },
                enabled = false,
            });
            //城墙3级
            AssetManager.culture_tech.add(new CultureTechAsset
            {
                id = "Circumvallation_3",
                path_icon = "Circumvallation_3",
                type = TechType.Common,
                requirements = new System.Collections.Generic.List<string>() { "housing_3", "Circumvallation_2" },
                enabled = false,
            });
        }

        //贴图
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TechElement), "load", typeof(CultureTechAsset))]
        public static bool load_Prefix(CultureTechAsset pAsset, TechElement __instance)
        {
            if (pAsset.path_icon.StartsWith("tech")|| pAsset == null)
            {
                return true;
            }
            Reflection.SetField(__instance, "asset", pAsset);
            Sprite sprite = Utils.ResourcesHelper.loadSprite($"{Main.mainPath}/EmbededResources/icons/tech/" + pAsset.path_icon + ".png");
            __instance.GetComponent<Image>().sprite = sprite;
            return false;
        }
        //文化窗口
        [HarmonyPrefix]
        [HarmonyPatch(typeof(CultureWindow), "showInfo")]
        public static bool showInfo_Total(CultureWindow __instance)
        {
            if (Config.selectedCulture == null)
            {
                return false;
            }
            MapBox.instance.cultures.recalcCultureValues();
            __instance.cultureElement.sprite = (Sprite)Resources.Load(Config.selectedCulture.icon_element, typeof(Sprite));
            __instance.cultureDecor.sprite = (Sprite)Resources.Load(Config.selectedCulture.icon_decor, typeof(Sprite));
            __instance.cultureElement.color = Toolbox.makeColor(Config.selectedCulture.color);
            __instance.cultureDecor.color = Toolbox.makeColor(Config.selectedCulture.color);
            if (Reflection.GetField(typeof(CultureWindow), __instance, "tech_prefab") == null)
            {
                Reflection.SetField(__instance, "tech_prefab", Resources.Load<TechElement>("ui/PrefabTechElement"));
            }
            __instance.nameInput.setText(Config.selectedCulture.name);
            __instance.text_description.text = string.Empty;
            __instance.text_values.text = string.Empty;
            __instance.population.text.text = Config.selectedCulture.followers.ToString() ?? "";
            __instance.cities.text.text = Config.selectedCulture.cities.ToString() ?? "";
            __instance.kingdoms.text.text = Config.selectedCulture.kingdoms.ToString() ?? "";
            __instance.knowledge_gain.text.text = Config.selectedCulture.knowledge_gain.ToString("0.0") ?? "";
            __instance.level.text.text = Config.selectedCulture.getCurrentLevel().ToString() ?? "";
            __instance.zones.text.text = Config.selectedCulture.zones.Count.ToString() ?? "";
            __instance.spreadSpeed.text.text = Config.selectedCulture.stats.culture_spread_speed.value.ToString("0.0") ?? "";
            int num = (int)(Config.selectedCulture.stats.culture_spread_convert_chance.value * 100f);
            __instance.convertChance.text.text = num.ToString() + "%";
            int num2 = MapBox.instance.mapStats.year - Config.selectedCulture.year + 1;
            if (!string.IsNullOrEmpty(Config.selectedCulture.village_origin))
            {
                __instance.CallMethod("showStat", "culture_founded_in", Config.selectedCulture.village_origin);
            }
            __instance.CallMethod("showStat", "age", num2);
            float pVal = Config.selectedCulture.research_progress;
            float pMax;
            if (!string.IsNullOrEmpty(Config.selectedCulture.researching_tech))
            {
                pMax = Config.selectedCulture.getKnowledgeCostForResearch();
                CultureTechAsset cultureTechAsset = AssetManager.culture_tech.get(Config.selectedCulture.researching_tech);
                Sprite sprite;
                if (cultureTechAsset.path_icon.StartsWith("tech"))
                {
                    sprite = (Sprite)Resources.Load("ui/Icons/" + cultureTechAsset.path_icon, typeof(Sprite));
                }
                else
                {
                    sprite = Utils.ResourcesHelper.loadSprite($"{Main.mainPath}/EmbededResources/icons/tech/" + cultureTechAsset.path_icon + ".png");
                }
                __instance.iconCurrentTech.GetComponent<Image>().sprite = sprite;
                __instance.iconCurrentTech.gameObject.SetActive(true);
            }
            else
            {
                pVal = 0f;
                pMax = 0f;
                __instance.iconCurrentTech.gameObject.SetActive(false);
            }
            __instance.researchBar.setBar(pVal, pMax, pVal.ToString("0.0") + "/" + pMax.ToString("0"));
            __instance.CallMethod("clearPrevTechElements");
            __instance.CallMethod("loadTechButtons");
            Race race = AssetManager.raceLibrary.get(Config.selectedCulture.race);
            __instance.text_rare_knowledges.text = Config.selectedCulture.CallMethod("countCurrentRareTech").ToString() + "/" + race.culture_rate_tech_limit.ToString();
            __instance.text_description.GetComponent<LocalizedText>().CallMethod("checkTextFont");
            __instance.text_values.GetComponent<LocalizedText>().CallMethod("checkTextFont");
            __instance.text_description.GetComponent<LocalizedText>().CallMethod("checkSpecialLanguages");
            __instance.text_values.GetComponent<LocalizedText>().CallMethod("checkSpecialLanguages");
            if (LocalizedTextManager.isRTLLang())
            {
                __instance.text_description.alignment = TextAnchor.UpperRight;
                __instance.text_values.alignment = TextAnchor.UpperLeft;
                return false;
            }
            __instance.text_description.alignment = TextAnchor.UpperLeft;
            __instance.text_values.alignment = TextAnchor.UpperRight;
            return false;
        }

    }
}
