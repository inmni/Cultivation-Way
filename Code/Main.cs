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


namespace CultivationWay{
    [ModEntry]
    class Main : MonoBehaviour{
        //注意：Culture.create()已在ChineseNameGenerator添加后置补丁
        //需要在Actor死亡后删除MoreStats
        public static Dictionary<Actor,MoreStats> actorToMoreStats = new Dictionary<Actor, MoreStats>();//单位与更多属性映射词典

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
                //初始化
                #region
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
        }
        void addForLocalization()
        {
            Localization.addLocalization("Button_Cultivation_Way", "修真之路");
            Localization.addLocalization("tab_cw", "简简单单的修真");
        }
        void initWindows()
        {
            WindowAboutThis.init();
        }

        //一些不知道放哪的拦截
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapBox),"createNewUnit")]

        //绑定人物和更多属性
        public static void bindActorToMoreStats(Actor __result)
        {
            MoreStats moreStats = new MoreStats(__result);
            actorToMoreStats.Add(__result,moreStats);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapBox), "destroyActor",new Type[] { typeof(Actor) })]
        public static void unBindActorToMoreStats(Actor pActor)
        {
            actorToMoreStats.Remove(pActor);
        }
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
    }
}