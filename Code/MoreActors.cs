using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cultivation_Way.Utils;
using CultivationWay;
using HarmonyLib;
using NCMS.Utils;
using ReflectionUtility;
using UnityEngine;

namespace Cultivation_Way
{
    class MoreActors
    {
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
            MingAsset.nameLocale = "Mings";
            MingAsset.nameTemplate = "Ming_name";
            MingAsset.shadowTexture = "unitShadow_5";
            MingAsset.texture_path = "t_Ming";
            MingAsset.texture_heads = "";
            MingAsset.heads = 2;
            MingAsset.animation_idle = "walk_0,walk_1,walk_2,walk_3";
            addActor(MingAsset);
        }

        private void addActor(ActorStats pStats)
        {
            Main.moreActors.Add(pStats.id);
            if (pStats.shadow)
            {
                ReflectionUtility.Reflection.CallMethod(AssetManager.unitStats, "loadShadow", pStats);
            }
            /*
             * 用于自动添加命名，复制人类的命名
            ((ChineseNameLibrary)AssetManager.instance.dict["chineseNameGenerator"]).clone(pStats.nameTemplate, "human_name");
            */
        }
        
    }
}
