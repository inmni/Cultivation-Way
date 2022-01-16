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
        //注意：下一个优化点使用dynamic关键字

        public const string mainPath = "Mods/Cultivation-Way";//主路径
        //另一条"worldbox_Data/StreamingAssets/Mods/NCMS/Core/Temp/Mods/修真之路"

        public static int count = 0;

        public static Main instance;

        public StackSpellEffects spellEffects;

        public List<BonusStatsManager> bonusStatsManagers = new List<BonusStatsManager>();

        public string addMapMode = "";

        private bool flag = false;
        private float controlDeltTime = 5f;

        internal int SpecialBodyLimit = 100;
        internal int summonTian1Limit = 1;

        #region 映射词典
        public List<MapChunk> chunks = new List<MapChunk>();//方便获取区块

        public Dictionary<string, MoreStats> actorToMoreStats = new Dictionary<string, MoreStats>();//单位编号与更多属性映射词典

        //public Dictionary<Actor, string> actorToID = new Dictionary<Actor, string>();//单位与编号映射词典

        public Dictionary<int, ActorStatus> actorToData = new Dictionary<int, ActorStatus>();//单位与单位数据映射

        public Dictionary<int, BaseStats> actorToCurStats = new Dictionary<int, BaseStats>();//单位与属性映射

        public Dictionary<string, MoreActorData> actorToMoreData = new Dictionary<string, MoreActorData>();//单位编号与更多数据映射词典

        public Dictionary<Building, MoreStats> buildingToMoreStats = new Dictionary<Building, MoreStats>();//建筑与更多属性映射词典

        public Dictionary<int, ChineseElement> chunkToElement = new Dictionary<int, ChineseElement>();//区块与元素映射词典

        public Dictionary<string, Family> familys = new Dictionary<string, Family>();//家族映射表

        public Dictionary<string, RaceFeature> raceFeatures = new Dictionary<string, RaceFeature>();//种族id与种族特色对照
        #endregion

        #region 更多玩意
        public MoreItem MoreItem = new MoreItem();
        public MoreTraits MoreTraits = new MoreTraits();
        public MoreActors MoreActors = new MoreActors();
        public MoreProjectiles MoreProjectiles = new MoreProjectiles();
        public MoreRaces MoreRaces = new MoreRaces();
        public MoreKingdoms MoreKingdoms = new MoreKingdoms();
        public MoreBuildings MoreBuildings = new MoreBuildings();
        public MoreGodPowers MoreGodPowers = new MoreGodPowers();
        public MoreDrops MoreDrops = new MoreDrops();
        public MoreCultureTech MoreCultureTech = new MoreCultureTech();
        public MoreMapModes MoreMapModes = new MoreMapModes();
        public List<string> moreActors = new List<string>();
        public List<string> moreItems = new List<string>();
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
            spellEffects = this.transform.gameObject.AddComponent<StackSpellEffects>();
            spellEffects.Awake();
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
                initChunkElement();
                MoreMapModes.add();
                createOrResetFamily();
                addRaceFeature();
                MonoBehaviour.print("[修真之路Cultivation Way]:添加其他成功");
                #endregion
                MonoBehaviour.print("[修真之路Cultivation Way]:加载成功");
                
            }
            if (instance.chunkToElement.Count != Config.ZONE_AMOUNT_X * Config.ZONE_AMOUNT_Y << 6)
            {
                initChunkElement();
            }
            
            if (controlDeltTime > 0)
            {
                controlDeltTime -= Time.deltaTime;
            }
            updateControl();
        }
        void updateControl()
        {
            if (instance.addMapMode == "")
            {
                return;
            }
            if (MapBox.instance.isGameplayControlsLocked())
            {
                return;
            }
            if (MapBox.instance.isOverUI())
            {
                return;
            }
            if (ScrollWindow.isWindowActive())
            {
                return;
            }
            if(!(bool)Reflection.GetField(typeof(MapBox),MapBox.instance,"alreadyUsedZoom") && ((bool)MapBox.instance.CallMethod("canInspectWithMainTouch") || (bool)MapBox.instance.CallMethod("canInspectWithRightClick")) 
                && ((float)Reflection.GetField(typeof(MapBox), MapBox.instance, "inspectTimerClick") < 0.2f) && !MapBox.instance.isActionHappening() && !MoveCamera.cameraDragActivated)
            {
                WorldTile mouseTilePos = MapBox.instance.getMouseTilePos();
                
                if (mouseTilePos != null)
                {
                    if (MapBox.instance.showElementZones())
                    {
                        if (controlDeltTime <= 0f)
                        {
                            PowerActionLibrary.inspectChunk(mouseTilePos);
                        }
                    }
                }
            }
        }
        void patchHarmony()
        {
            //new Harmony("me.xiaoye97.plugin.Tutorial").PatchAll();
            Harmony.CreateAndPatchAll(typeof(AddAssetManager));
            MonoBehaviour.print("Create and patch all:AddAssetManager");
            Harmony.CreateAndPatchAll(typeof(ChineseNameGenerator));
            MonoBehaviour.print("Create and patch all:ChineseNameGenerator");
            Harmony.CreateAndPatchAll(typeof(Main));
            MonoBehaviour.print("Create and patch all:Main");
            Harmony.CreateAndPatchAll(typeof(MoreActors));
            MonoBehaviour.print("Create and patch all:MoreActors");
            Harmony.CreateAndPatchAll(typeof(MoreBuildings));
            MonoBehaviour.print("Create and patch all:MoreBuildings");
            Harmony.CreateAndPatchAll(typeof(MoreItem));
            MonoBehaviour.print("Create and patch all:MoreItem");
            Harmony.CreateAndPatchAll(typeof(MoreMapModes));
            MonoBehaviour.print("Create and patch all:MoreMapModes");
            Harmony.CreateAndPatchAll(typeof(MoreRaces));
            MonoBehaviour.print("Create and patch all:MoreRaces");
            Harmony.CreateAndPatchAll(typeof(MoreTraits));
            MonoBehaviour.print("Create and patch all:MoreTraits");
            Harmony.CreateAndPatchAll(typeof(MoreCultureTech));
            MonoBehaviour.print("Create and patch all:MoreCultureTech");
            Harmony.CreateAndPatchAll(typeof(MoreKingdoms));
            MonoBehaviour.print("Create and patch all:MoreKingdoms");
            Harmony.CreateAndPatchAll(typeof(MoreProjectiles));
            MonoBehaviour.print("Create and patch all:MoreProjectiles");
            Harmony.CreateAndPatchAll(typeof(SaveAndLoadManager));
            MonoBehaviour.print("Create and patch all:SaveAndLoadManager");
            Harmony.CreateAndPatchAll(typeof(WindowCreatureInfoHelper));
            MonoBehaviour.print("Create and patch all:WindowCreatureInfoHelper");
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
            Localization.addLocalization("FairyFox", "仙狐");
            #region 属性
            Localization.addLocalization("specialBody", "体质");
            Localization.addLocalization("origin", "起源体质");
            Localization.addLocalization("madeBy", "血脉源头");
            Localization.addLocalization("spells", "法术");
            Localization.addLocalization("spellRange", "施法距离");
            Localization.addLocalization("magic", "灵力");
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
            Localization.setLocalization("creature_statistics_age", "年龄/寿元");
            
            #endregion

            #region 特质
            Localization.addLocalization("trait_cursed_immune", "诅咒免疫");
            Localization.addLocalization("trait_asylum", "天道眷顾");
            Localization.addLocalization("trai_asylum_info", "真正的永恒");
            Localization.addLocalization("trait_race", "种族");
            Localization.addLocalization("trait_realm", "境界");
            Localization.addLocalization("trait_element", "灵根");
            Localization.addLocalization("trait_cultivationBook", "家族功法");
            Localization.addLocalization("trait_realm_info", "境界信息");
            Localization.addLocalization("trait_element_info", "灵根信息");
            Localization.addLocalization("trait_cultivationBook_info","功法信息");
            #endregion

            #region 文化科技
            Localization.addLocalization("tech_culti_normal", "仙路");
            Localization.addLocalization("tech_culti_bodying", "炼体");
            Localization.addLocalization("tech_culti_bushido", "武道");
            #endregion
            #region 装备
            Localization.addLocalization("item_summonTian1", "哈？没名字");
            #endregion
        }
        void initWindows()
        {
            WindowAboutThis.init();
            WindowChunkInfo.init();
            WindowMoreStats.init();
            WindowTops.init();
        }
        void addRaceFeature()
        {
            foreach(ActorStats stats in AssetManager.unitStats.list)
            {
                if (raceFeatures.ContainsKey(stats.race))
                {
                    continue;
                }
                RaceFeature feature = new RaceFeature();
                feature.raceID = stats.race;
                feature.raceSpells = new List<ExtensionSpell>();
                this.raceFeatures.Add(stats.race, feature);
            }
            MoreRaces.setIntelligentRaceFeature();
            MoreRaces.setOtherRaceFeature();
        }
        public void initChunkElement()
        {
            instance.chunkToElement.Clear();
            instance.chunks = ((MapChunkManager)Reflection.GetField(typeof(MapBox), MapBox.instance, "mapChunkManager")).list;
            foreach (MapChunk chunk in chunks)
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
        public void resetActorMore()
        {
            instance.actorToMoreStats.Clear();
            instance.actorToMoreData.Clear();
            foreach (Actor actor in MapBox.instance.units)
            {
                MoreStats moreStats = new MoreStats();
                MoreActorData moreData = new MoreActorData();
                //ActorStatus data = (ActorStatus)Reflection.GetField(typeof(Actor), actor, "data");
                ActorStatus data = actor.GetData();
                
                string id = data.actorID;
                instance.actorToMoreData.Add(id, moreData);
                instance.actorToMoreStats.Add(id, moreStats);
                string name = data.firstName;
                foreach (string fn in ChineseNameAsset.familyNameTotal)
                {
                        if (name.StartsWith(fn))
                        {
                            moreStats.family = instance.familys[fn];
                            break;
                        }
                }
                if (moreStats.family == null)
                {
                    moreStats.family = Main.instance.familys["甲"];
                }
                moreData.cultisystem = moreStats.cultisystem;
                moreData.element = moreStats.element;
                moreData.bonusStats = new MoreStats();
                moreData.coolDown = new Dictionary<string, int>();
                moreData.familyID = moreStats.family.id;
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
                            (instance.chunkToElement[neighbourChunk.id].baseElementContainer[OthersHelper.getBePromotedBy(type)] % 1024) >> 4;
                        instance.chunkToElement[chunk.id].baseElementContainer[type] -=
                            (instance.chunkToElement[neighbourChunk.id].baseElementContainer[OthersHelper.getBeOppsitedBy(type)] % 1024) >> 4;
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
                    if (instance.chunkToElement[chunkID].baseElementContainer[i] > 100) { instance.chunkToElement[chunkID].baseElementContainer[i] = 100; }
                    if (instance.chunkToElement[chunkID].baseElementContainer[i] < 0) { instance.chunkToElement[chunkID].baseElementContainer[i] =0; }
                }
                instance.chunkToElement[chunkID].normalize();
            }
            #endregion
            Thread.CurrentThread.Abort();
            Thread.CurrentThread.DisableComObjectEagerCleanup();
        }
        void clearMemory()
        {
            Dictionary<string, MoreStats> TactorToMoreStats = new Dictionary<string, MoreStats>();//单位编号与更多属性映射词典
            Dictionary<int, ActorStatus> TactorToData = new Dictionary<int, ActorStatus>();//单位与单位数据映射
            Dictionary<int, BaseStats> TactorToCurStats = new Dictionary<int, BaseStats>();//单位与属性映射
            Dictionary<string, MoreActorData> TactorToMoreData = new Dictionary<string, MoreActorData>();//单位编号与更多数据映射词典

            valueClone<string, MoreStats>(actorToMoreStats,TactorToMoreStats);
            valueClone<int, ActorStatus>(actorToData, TactorToData);
            valueClone<int, BaseStats>(actorToCurStats,TactorToCurStats);
            valueClone<string, MoreActorData>(actorToMoreData, TactorToMoreData);

            actorToMoreStats.Clear();
            actorToData.Clear();
            actorToCurStats.Clear();
            actorToMoreData.Clear();

            actorToMoreStats = TactorToMoreStats;
            actorToData = TactorToData;
            actorToCurStats = TactorToCurStats;
            actorToMoreData = TactorToMoreData;

            Thread.CurrentThread.Abort();
            Thread.CurrentThread.DisableComObjectEagerCleanup();
        }
        void valueClone<T1,T2>(Dictionary<T1,T2> from, Dictionary<T1, T2> to)
        {
            foreach(T1 key in from.Keys)
            {
                if (key != null)
                {
                    to.Add(key, from[key]);
                }
            }
        }
        #endregion

        #region 一些不知道放哪的拦截
        ////绑定人物和ID
        //[HarmonyPostfix]
        //[HarmonyPatch(typeof(MapBox), "createNewUnit")]
        //public static void bindActorToMoreStats(Actor __result)
        //{
        //    instance.actorToID.Add(__result, ((ActorStatus)Reflection.GetField(typeof(Actor), __result, "data")).actorID);

        //    //if (instance.actorToID.ContainsKey(__result))
        //    //{
        //    //    print(__result + "存在");
        //    //}
        //    //else
        //    //{
        //    //    print("添加失败:" + __result);
        //    //}
        //}

        //解决控制问题
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "updateControls")]
        public static bool updateControls_Prefix()
        {
            if (ScrollWindow.isWindowActive())
            {
                instance.controlDeltTime = 0.5f;
            }
            return true;
        }

        //解除绑定
        [HarmonyPrefix]
        [HarmonyPatch(typeof(MapBox), "destroyActor", new Type[] { typeof(Actor) })]
        public static bool unBindActorToMoreStats(Actor pActor)
        {
            //if (!instance.actorToID.ContainsKey(pActor))
            //{
            //    print(pActor+"不存在");
            //}
            instance.actorToCurStats.Remove(pActor.GetInstanceID());
            instance.actorToData.Remove(pActor.GetInstanceID());
            instance.actorToMoreData.Remove(pActor.GetData().actorID);
            instance.actorToMoreStats.Remove(pActor.GetData().actorID);
            return true;
        }
        //百年事件（更新灵气，清理内存，以及其他
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapBox), "updateObjectAge")]
        public static void toUpdateChunkElement()
        {
            List<BonusStatsManager> temp = new List<BonusStatsManager>();
            foreach (BonusStatsManager bonusStatsManager in instance.bonusStatsManagers)
            {
                bonusStatsManager.update();
                if (bonusStatsManager.leftTime <= 0)
                {
                    continue;
                }
                temp.Add(bonusStatsManager);
            }
            instance.bonusStatsManagers = temp;
            if (MapBox.instance.mapStats.year % 100 == 0)
            {
                Thread t = new Thread(new ThreadStart(instance.updateChunkElement));
                t.Start();
            }
            if (MapBox.instance.mapStats.year % 250 == 0)
            {
                foreach (Kingdom kingdom in MapBox.instance.kingdoms.list_civs)
                {
                    if (kingdom.raceID == "Tian" && kingdom.getPopulationTotal() > 0)
                    {
                        Actor User = kingdom.getMaxLevelActor();
                        if (User.GetMoreData().coolDown["summonTian"] == 1)
                        {
                            instance.raceFeatures["Tian"].raceSpells[0].castSpell(User, null);//召唤战舰s
                        }
                    }
                }
                if (MapBox.instance.mapStats.year % 500 == 0)
                {
                    Thread t = new Thread(new ThreadStart(instance.clearMemory));
                    t.Start();
                    if (MapBox.instance.mapStats.year % 1000 == 0)
                    {
                        foreach (Kingdom kingdom in MapBox.instance.kingdoms.list_civs)
                        {
                            if (kingdom.raceID == "Tian" && kingdom.king!=null)
                            {
                                if (kingdom.king.GetMoreData().coolDown["summonTian1"] == 1)
                                {
                                    instance.raceFeatures["Tian"].raceSpells[1].castSpell(kingdom.king, null);//召唤机甲
                                }
                            }
                        }
                    }
                }
            }
        }
        //城市给予物品修复
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City),"giveItem")]
        public static bool giveItem_Prefix(ref bool __result,Actor pActor,ActorEquipmentSlot pSlot,City pCity)
        {
            if (pActor == null||pActor.equipment==null|| pSlot == null || pCity == null)
            {
                __result = false;
                return false;
            }
            return true;
        }
        //城市获取死亡人口的装备修复
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City),"giveItemsToCity")]
        public static bool giveItemsToCity_Prefix(City pCity,Actor pDeadActor)
        {
            if (pDeadActor.stats.use_items)
            {
                return true;
            }
            return false;
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