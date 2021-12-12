using System;
using System.Collections;
using System.Collections.Generic;
using ReflectionUtility;
using static Config;
using NCMS;
using NCMS.Utils;
using UnityEngine;
using HarmonyLib;

using Cultivation_Way;
using Cultivation_Way.Utils;
using System.Linq;
using System.IO;
/*
MonoBehaviour.print("[修真之路Cultivation Way]:测试点n");//测试用测试点格式

*/
namespace CultivationWay
{
    [ModEntry]
    class Main : MonoBehaviour{
        //注意：职业头部贴图加载存在一定问题
        //注意：Culture.create()已在ChineseNameGenerator添加后置补丁
        //需要在Actor死亡后删除MoreStats
        public static MoreActors MoreActors = new MoreActors();
        public static MoreRaces MoreRaces = new MoreRaces();
        public static MoreKingdoms MoreKingdoms = new MoreKingdoms();
        public static MoreBuildings MoreBuildings = new MoreBuildings();
        public static MoreGodPowers MoreGodPowers = new MoreGodPowers();


        public static Dictionary<Actor,MoreStats> actorToMoreStats = new Dictionary<Actor, MoreStats>();//单位与更多属性映射词典

        public static List<string> moreActors = new List<string>();
        public static List<string> moreRaces = new List<string>();

        public static ModDeclaration.Info Info;

        private bool initiated = false;
        void Awake(){
            MonoBehaviour.print("[修真之路Cultivation Way]:开始加载");

        }
        void Start()
        {
        }
        void Update()
        {
            if (!gameLoaded)
            {
                return;
            }
            if (!initiated)
            {
                initiated = true;
                
                #region 初始化
                MonoBehaviour.print("[修真之路Cultivation Way]:初始化");
                
                AddInitLibs.initMyLibs();
                MonoBehaviour.print("[修真之路Cultivation Way]:初始化库成功");
                initWindows();
                MonoBehaviour.print("[修真之路Cultivation Way]:初始化窗口成功");
                //开启拦截
                MonoBehaviour.print("[修真之路Cultivation Way]:启用拦截");
                patchHarmony();
                MonoBehaviour.print("[修真之路Cultivation Way]:启用拦截成功");
                //添加Asset
                MonoBehaviour.print("[修真之路Cultivation Way]:添加Asset");
                AddAssetManager.addAsset();
                MonoBehaviour.print("[修真之路Cultivation Way]:添加Asset成功");
                //添加文字资源
                MonoBehaviour.print("[修真之路Cultivation Way]:添加文字资源");
                addForLocalization();
                MonoBehaviour.print("[修真之路Cultivation Way]:添加文字资源成功");
                //创建按钮栏
                MonoBehaviour.print("[修真之路Cultivation Way]:创建按钮栏");
                MorePowers.createPowerTab();
                MonoBehaviour.print("[修真之路Cultivation Way]:创建按钮栏成功");
                //加载按钮
                MonoBehaviour.print("[修真之路Cultivation Way]:加载按钮");
                MorePowers.createButtons();
                MonoBehaviour.print("[修真之路Cultivation Way]:加载按钮成功");
                //添加其他
                MonoBehaviour.print("[修真之路Cultivation Way]:添加其他");
                sfx_MusicMan_racesAdd();
                MoreRaces.kingdomColorsDataInit();
                MonoBehaviour.print("[修真之路Cultivation Way]:添加其他成功");
                #endregion
                MonoBehaviour.print("[修真之路Cultivation Way]:加载成功");
            }
        }


        void patchHarmony()
        {
            //new Harmony("me.xiaoye97.plugin.Tutorial").PatchAll();
            Harmony.CreateAndPatchAll(typeof(AddAssetManager));
            Harmony.CreateAndPatchAll(typeof(ChineseNameGenerator));
            Harmony.CreateAndPatchAll(typeof(Main));
            Harmony.CreateAndPatchAll(typeof(MoreRaces));
        }
        void addForLocalization()
        {
            Localization.addLocalization("Button_Cultivation_Way", "修真之路");
            string[] temp = new string[] { "QQ群总部：602184962", "混沌轮回天地间，人生道尽又一年。\n路走人间似神仙，观望百态红尘间。","道可道，非恒道;\n名可名，非恒名", "魔前一叩三千年，回首凡尘不作仙" };
            Localization.addLocalization("tab_cw", temp.GetRandom());
            Localization.addLocalization("Tians", "天族");
            Localization.addLocalization("Mings", "冥族");
        }
        void initWindows()
        {
            WindowAboutThis.init();
        }

        #region 一些不知道放哪的拦截
        //绑定人物和更多属性
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapBox),"createNewUnit")]
        public static void bindActorToMoreStats(Actor __result)
        {
            MoreStats moreStats = new MoreStats(__result);
            actorToMoreStats.Add(__result,moreStats);
        }
        //解除绑定
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapBox), "destroyActor",new Type[] { typeof(Actor) })]
        public static void unBindActorToMoreStats(Actor pActor)
        {
            actorToMoreStats.Remove(pActor);
        }
        //额外窗口
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ScrollWindow), "showWindow", new Type[] { typeof(string) })]
        public static void showWindow_Postfix(string pWindowID)
        {
            switch (pWindowID)
            {
                case "inspect_unit":
                    WindowMoreStats.init();
                    break;
            }
        }
        
        //测试1（暂存
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Building), "getColor")]
        public static bool setBuilding_Prefix(Building __instance)
        {
            BuildingAsset stats = Reflection.GetField(typeof(Building), __instance, "stats") as BuildingAsset;
            if (stats.sprites == null)
            {
                MonoBehaviour.print("[修真之路Cultivation Way]:sprites为空");
                return true;
            }
            if (stats.sprites.mapIcon == null)
            {
                MonoBehaviour.print("[修真之路Cultivation Way]:mapIcon为空");
            }
            return true;
        }
        #endregion

        #region 乱七八糟的初始化
        public static void sfx_MusicMan_racesAdd()
        {
            foreach (string race in moreRaces)
            {
                sfx.MusicMan.races.Add(race, new sfx.MusicRaceContainer());
            }
        }
        #endregion
    }
}