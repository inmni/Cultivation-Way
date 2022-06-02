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
                    path_icon = "tech/icon"+cultisystem.id,
                    type = TechType.Rare,
                    enabled = true,
                });
            }
            AssetManager.culture_tech.get("culti_default").enabled = false;
            //城墙1级
            AssetManager.culture_tech.add(new CultureTechAsset
            {
                id = "Circumvallation_1",
                path_icon = "tech/iconCircumvallation_1",
                type = TechType.Common,
                requirements = new System.Collections.Generic.List<string>() { "housing_1" },
                enabled = true,
            });
            //城墙2级
            AssetManager.culture_tech.add(new CultureTechAsset
            {
                id = "Circumvallation_2",
                path_icon = "tech/iconCircumvallation_2",
                type = TechType.Common,
                requirements = new System.Collections.Generic.List<string>() { "housing_2", "Circumvallation_1" },
                enabled = false,
            });
            //城墙3级
            AssetManager.culture_tech.add(new CultureTechAsset
            {
                id = "Circumvallation_3",
                path_icon = "tech/iconCircumvallation_3",
                type = TechType.Common,
                requirements = new System.Collections.Generic.List<string>() { "housing_3", "Circumvallation_2" },
                enabled = false,
            });
        }

    }
}
