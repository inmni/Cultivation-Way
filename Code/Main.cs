using Cultivation_Way;
using Cultivation_Way.Utils;
using HarmonyLib;
using NCMS;
using NCMS.Utils;
using ReflectionUtility;
using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using static Config;
/*
MonoBehaviour.print("[修真之路Cultivation Way]:测试点n");//测试用测试点格式

*/
namespace CultivationWay
{
    [ModEntry]
    class Main : MonoBehaviour
    {
        //注意：武器贴图加载存在一定问题
        //注意：Culture.create()已在ChineseNameGenerator添加完全拦截

        public static Dictionary<string, int> dsa = new Dictionary<string, int>();

        public static Main instance;

        private static int flag = 0;

        #region 映射词典
        public Dictionary<string, MoreStats> actorToMoreStats = new Dictionary<string, MoreStats>();//单位与更多属性映射词典

        public Dictionary<Actor, string> actorToID = new Dictionary<Actor, string>();//单位与编号映射词典

        public Dictionary<Actor, MoreActorData> actorToMoreData = new Dictionary<Actor, MoreActorData>();//单位与更多数据映射词典

        public Dictionary<Building, MoreStats> buildingToMoreStats = new Dictionary<Building, MoreStats>();//建筑与更多属性映射词典

        public Dictionary<int, ChineseElement> chunkToElement = new Dictionary<int, ChineseElement>();//区块与元素映射词典

        public Dictionary<string, Family> familys = new Dictionary<string, Family>();//家族映射表
        #endregion

        #region 更多玩意
        public MoreTraits MoreTraits = new MoreTraits();
        public MoreActors MoreActors = new MoreActors();
        public MoreProjectiles MoreProjectiles = new MoreProjectiles();
        public MoreRaces MoreRaces = new MoreRaces();
        public MoreKingdoms MoreKingdoms = new MoreKingdoms();
        public MoreBuildings MoreBuildings = new MoreBuildings();
        public MoreGodPowers MoreGodPowers = new MoreGodPowers();
        public MoreDrops MoreDrops = new MoreDrops();
        public MoreCultureTech MoreCultureTech = new MoreCultureTech();
        public List<string> moreActors = new List<string>();
        public List<string> moreRaces = new List<string>();
        public List<string> moreProjectiles = new List<string>();
        #endregion

        #region 初始化n件套
        private bool initiated = false;
        void Awake()
        {
            instance = this;
            MonoBehaviour.print("[修真之路Cultivation Way]:开始加载");
            //开启拦截
            MonoBehaviour.print("[修真之路Cultivation Way]:启用拦截");
            patchHarmony();
            MonoBehaviour.print("[修真之路Cultivation Way]:启用拦截成功");
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
                initChunkElement();
                createOrResetFamily();
            }
            if (instance.chunkToElement.Count != Config.ZONE_AMOUNT_X * Config.ZONE_AMOUNT_Y << 6) ;
            {
                initChunkElement();
            }
        }
        void patchHarmony()
        {
            //new Harmony("me.xiaoye97.plugin.Tutorial").PatchAll();
            Harmony.CreateAndPatchAll(typeof(AddAssetManager));
            Harmony.CreateAndPatchAll(typeof(ChineseNameGenerator));
            Harmony.CreateAndPatchAll(typeof(Main));
            Harmony.CreateAndPatchAll(typeof(MoreActors));
            Harmony.CreateAndPatchAll(typeof(MoreBuildings));
            Harmony.CreateAndPatchAll(typeof(MoreRaces));
            Harmony.CreateAndPatchAll(typeof(MoreTraits));
            Harmony.CreateAndPatchAll(typeof(MoreCultureTech));
            Harmony.CreateAndPatchAll(typeof(MoreProjectiles));
            Harmony.CreateAndPatchAll(typeof(SaveAndLoadManager));
        }
        void addForLocalization()
        {
            Localization.addLocalization("Button_Cultivation_Way", "修真之路");
            string[] temp = new string[] {
                "QQ群总部：602184962",
                "混沌轮回天地间，人生道尽又一年。\n路走人间似神仙，观望百态红尘间。",
                "道可道，非恒道;\n名可名，非恒名",
                "魔前一叩三千年，回首凡尘不作仙"
            };
            Localization.addLocalization("tab_cw", temp.GetRandom());
            Localization.addLocalization("Tians", "天族");
            Localization.addLocalization("Mings", "冥族");
            Localization.addLocalization("JiaoDragons", "蛟龙");

            #region 属性
            Localization.addLocalization("spells", "法术");
            Localization.addLocalization("spellRange", "施法距离");
            Localization.addLocalization("magic", "灵气值");
            Localization.addLocalization("vampire", "吸血");
            Localization.addLocalization("antiInjury", "反伤");
            Localization.addLocalization("spellRelief", "法伤赦免");
            Localization.addLocalization("cultisystem", "修炼体系");
            Localization.addLocalization("realm", "境界");
            Localization.addLocalization("talent", "天赋");
            Localization.addLocalization("elementType", "灵根");
            Localization.addLocalization("family", "家族");
            Localization.addLocalization("cultivationBook", "功法");
            Localization.setLocalization("Gold", "金");
            Localization.addLocalization("Wood", "木");
            Localization.addLocalization("Water", "水");
            Localization.setLocalization("Fire", "火");
            Localization.addLocalization("Ground", "土");
            #endregion

            #region 特质
            Localization.addLocalization("trait_cursed_immune", "诅咒免疫");
            #endregion

            #region 文化科技
            Localization.addLocalization("tech_culti_normal", "仙路");
            Localization.addLocalization("tech_culti_bodying", "炼体");
            #endregion
        }
        void initWindows()
        {
            WindowAboutThis.init();
            WindowMoreStats.init();
        }
        public void initChunkElement()
        {
            instance.chunkToElement.Clear();
            foreach (MapChunk chunk in ((MapChunkManager)Reflection.GetField(typeof(MapBox), MapBox.instance, "mapChunkManager")).list)
            {
                instance.chunkToElement.Add(chunk.id, new ChineseElement(new int[] { 20, 20, 20, 20, 20 }).getRandom());
            }
        }
        public void createOrResetFamily()
        {
            instance.familys.Clear();
            foreach(string familyName in ChineseNameAsset.familyNameTotal)
            {
                if (!familys.ContainsKey(familyName))
                {
                    familys.Add(familyName, new Family(familyName));
                }
            }
        }
        public void resetActorMoreStats()
        {
            instance.actorToMoreStats.Clear();
            foreach(Actor actor in MapBox.instance.units)
            {
                    MoreStats moreStats = new MoreStats(actor);
                string id = ((ActorStatus)Reflection.GetField(typeof(Actor), actor, "data")).actorID;
                instance.actorToID.Add(actor, id);
                    Main.instance.actorToMoreStats.Add(id, moreStats);
                    string name = ((ActorStatus)Reflection.GetField(typeof(Actor), actor, "data")).firstName;
                    foreach (string fn in ChineseNameAsset.familyNameTotal)
                    {
                        if (name.StartsWith(fn))
                        {
                            moreStats.family = Main.instance.familys[fn];
                            break;
                        }
                    }

                    ChineseElementLibrary elementLibrary = (ChineseElementLibrary)AssetManager.instance.dict["element"];
                    if (Main.dsa.ContainsKey(elementLibrary.dict[moreStats.element.element.id].name))
                    {
                        Main.dsa[elementLibrary.dict[moreStats.element.element.id].name]++;
                    }
                    else
                    {
                        Main.dsa.Add(elementLibrary.dict[moreStats.element.element.id].name, 1);
                    }
            }
        }
        void updateChunkElement()
        {
            /*采用原地算法
             * 
             * 初始化：
             * 遍历所有区块，若词典中不存在，则全部重新录入
             * 
             * 更新：
             * 中心点元素增量采用四周点的元素量模1024所得值再乘以1024
             * 
             * 最后总结：
             * 各个区块的所有元素整除1024得到最终结果
             * 
             */
            #region 初始化
            //initChunkElement();
            #endregion
            #region 更新
            List<MapChunk> mapChunks = ((MapChunkManager)Reflection.GetField(typeof(MapBox), MapBox.instance, "mapChunkManager")).list;
            foreach (MapChunk chunk in mapChunks)
            {
                for (int type = 0; type < 5; type++)
                {
                    instance.chunkToElement[chunk.id].baseElementContainer[type] +=
                        instance.chunkToElement[chunk.id].baseElementContainer[type] << 10;
                }
                
                foreach (MapChunk neighbourChunk in chunk.neighbours)
                {
                    for (int type = 0; type < 5; type++)
                    {
                        instance.chunkToElement[chunk.id].baseElementContainer[type] +=
                            (instance.chunkToElement[neighbourChunk.id].baseElementContainer[OthersHelper.getBePromotedBy(type)] % 1024) << 4;
                        instance.chunkToElement[chunk.id].baseElementContainer[type] -=
                            (instance.chunkToElement[neighbourChunk.id].baseElementContainer[OthersHelper.getBeOppsitedBy(type)] % 1024) << 4;
                    }
                }
            }
            #endregion
            #region 总结
            foreach (int chunkID in instance.chunkToElement.Keys)
            {
                for (int i = 0; i < 5; i++)
                {
                    instance.chunkToElement[chunkID].baseElementContainer[i] >>= 10;
                    if (instance.chunkToElement[chunkID].baseElementContainer[i] > 100) { instance.chunkToElement[chunkID].baseElementContainer[i] = Toolbox.randomInt(0, 31); }
                    if (instance.chunkToElement[chunkID].baseElementContainer[i] < 0) { instance.chunkToElement[chunkID].baseElementContainer[i] = Toolbox.randomInt(0, 31); }
                }
            }
            #endregion
        }
        #endregion

        #region 一些不知道放哪的拦截
        //绑定人物和更多属性
        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(MapBox),"createNewUnit")]
        //public static void bindActorToMoreStats(Actor __result)
        //{
        //    MoreStats moreStats = new MoreStats(__result);
        //    instance.actorToMoreStats.Add(__result, moreStats);

        //}
        //解除绑定
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapBox), "destroyActor", new Type[] { typeof(Actor) })]
        public static void unBindActorToMoreStats(Actor pActor)
        {
            instance.actorToMoreStats.Remove(((ActorStatus)Reflection.GetField(typeof(Actor), pActor, "data")).actorID);
            
        }
        //更新区块元素
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapBox), "updateObjectAge")]
        public static void toUpdateChunkElement()
        {
            if (MapBox.instance.mapStats.year % 10 == 0)
            {
                Thread t = new Thread(new ThreadStart(instance.updateChunkElement));
                t.Start();
                
            }
        }
        //调整语言材质
        [HarmonyPrefix]
        [HarmonyPatch(typeof(LocalizedTextManager), "setLanguage")]
        public static bool updateText(ref string pLanguage)
        {
            if (flag<=5)
            {
                flag++;
                pLanguage = "cz";
            }
            return true;
        }
        ////额外窗口
        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(ScrollWindow), "showWindow", new Type[] { typeof(string) })]
        //public static void showWindow_Postfix(string pWindowID)
        //{
        //    switch (pWindowID)
        //    {
        //        case "inspect_unit":
        //            WindowMoreStats.show();
        //            break;
        //    }
        //}

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
            foreach (string race in instance.moreRaces)
            {
                sfx.MusicMan.races.Add(race, new sfx.MusicRaceContainer());
            }
        }
        #endregion
    }
}