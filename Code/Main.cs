using Cultivation_Way;
using Cultivation_Way.Utils;
using HarmonyLib;
using NCMS;
using NCMS.Utils;
using ReflectionUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using static Config;
/*
MonoBehaviour.print("[修真之路Cultivation Way]:测试点n");//测试用测试点格式

*/
namespace CultivationWay
{
    [ModEntry]
    class Main : MonoBehaviour
    {
        //注意：Culture.create()已在ChineseNameGenerator添加完全拦截
        //注意：智慧种族的声音未解决
        public const string mainPath = "Mods/Cultivation-Way";//主路径
        //另一条"worldbox_Data/StreamingAssets/Mods/NCMS/Core/Temp/Mods/修真之路"

        public static int count = 0;

        public static Main instance;

        public StackSpellEffects spellEffects;

        public List<BonusStatsManager> bonusStatsManagers = new List<BonusStatsManager>();

        public string addMapMode = "";

        private bool flag = false;
        private float controlDeltTime = 0.5f;

        internal int SpecialBodyLimit = 200;
        internal Dictionary<string, List<Actor>> kingdomBindActors = new Dictionary<string, List<Actor>>();//国家id与其绑定的生物
        internal Dictionary<string, string> godList = new Dictionary<string, string>();//神明列表及其中文名
        internal Dictionary<string, int> creatureLimit = new Dictionary<string, int>();//生物限制

        #region 映射词典
        public List<MapChunk> chunks = new List<MapChunk>();//方便获取区块

        //public Dictionary<string, MoreStats> actorToMoreStats = new Dictionary<string, MoreStats>();//单位编号与更多属性映射词典

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
        public MoreItems MoreItems = new MoreItems();
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
        public MoreWorldLaws MoreWorldLaws = new MoreWorldLaws();
        public List<string> moreActors = new List<string>();
        public List<string> moreItems = new List<string>();
        public List<string> moreRaces = new List<string>();
        public Dictionary<string, Vector2> moreProjectiles = new Dictionary<string, Vector2>();
        #endregion

        #region 初始化n件套
        private bool initiated = false;
        void Awake()
        {
            instance = this;
            MonoBehaviour.print("[修真之路Cultivation Way]:开始加载");
        }
        void Start()
        {
            //创建按钮栏
            print("[修真之路Cultivation Way]:创建按钮栏");
            MorePowers.createPowerTab();
            print("[修真之路Cultivation Way]:创建按钮栏成功");
            //添加Asset
            print("[修真之路Cultivation Way]:添加Asset");
            AddAssetManager.addAsset();
            print("[修真之路Cultivation Way]:添加Asset成功");
            //加载按钮
            print("[修真之路Cultivation Way]:加载按钮");
            MorePowers.createButtons();
            print("[修真之路Cultivation Way]:加载按钮成功");
            setLanguage_Postfix(Reflection.GetField(typeof(LocalizedTextManager), LocalizedTextManager.instance, "language") as string);
            //开启拦截
            MonoBehaviour.print("[修真之路Cultivation Way]:启用拦截");
            patchHarmony();
            MonoBehaviour.print("[修真之路Cultivation Way]:启用拦截成功");
            spellEffects = this.transform.gameObject.AddComponent<StackSpellEffects>();
            spellEffects.Awake();
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
                print("[修真之路Cultivation Way]:初始化");
                AddInitLibs.initMyLibs();
                print("[修真之路Cultivation Way]:初始化库成功");
                initWindows();
                print("[修真之路Cultivation Way]:初始化窗口成功");
                //添加其他
                print("[修真之路Cultivation Way]:添加其他");
                //sfx_MusicMan_racesAdd();
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
            if (!(bool)Reflection.GetField(typeof(MapBox), MapBox.instance, "alreadyUsedZoom") && ((bool)MapBox.instance.CallMethod("canInspectWithMainTouch") || (bool)MapBox.instance.CallMethod("canInspectWithRightClick"))
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
            Harmony.CreateAndPatchAll(typeof(MoreItems));
            MonoBehaviour.print("Create and patch all:MoreItems");
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
            Harmony.CreateAndPatchAll(typeof(MoreGodPowers));
            MonoBehaviour.print("Create and patch all:MoreGodPowers");
            Harmony.CreateAndPatchAll(typeof(WorldTools));
            MonoBehaviour.print("Create and patch all:WorldTools");
            Harmony.CreateAndPatchAll(typeof(WorldLawHelper));
            MonoBehaviour.print("Create and patch all:WorldLawHelper");
        }

        //  因添加文字材质，废弃
        //
        //void addForLocalization()
        //{
        //    Localization.addLocalization("Button_Cultivation_Way", "修真之路");
        //    string[] temp = new string[] {
        //        "QQ群总部：602184962",
        //        "混沌轮回天地间，人生道尽又一年。\n路走人间似神仙，观望百态红尘间。",
        //        "道可道，非恒道;\n名可名，非恒名",
        //        "魔前一叩三千年，回首凡尘不作仙"
        //    };
        //    Localization.addLocalization("tab_cw", temp.GetRandom());
        //    Localization.addLocalization("Tians", "天族");
        //    Localization.addLocalization("Mings", "冥族");
        //    Localization.addLocalization("Yaos", "妖族");
        //    Localization.addLocalization("EasternHumans", "东方人族");
        //    Localization.addLocalization("JiaoDragons", "蛟龙");
        //    Localization.addLocalization("FairyFox", "仙狐");
        //    #region 属性
        //    Localization.addLocalization("specialBody", "体质");
        //    Localization.addLocalization("origin", "起源体质");
        //    Localization.addLocalization("madeBy", "血脉源头");
        //    Localization.addLocalization("spells", "法术");
        //    Localization.addLocalization("spellRange", "施法距离");
        //    Localization.addLocalization("magic", "灵力");
        //    Localization.addLocalization("vampire", "吸血");
        //    Localization.addLocalization("antiInjury", "反伤");
        //    Localization.addLocalization("spellRelief", "法伤赦免");
        //    Localization.addLocalization("cultisystem", "修炼体系");
        //    Localization.addLocalization("realm", "境界");
        //    Localization.addLocalization("talent", "天赋");
        //    Localization.addLocalization("elementType", "灵根");
        //    Localization.addLocalization("family", "家族");
        //    Localization.addLocalization("cultivationBook", "功法");
        //    Localization.setLocalization("Gold", "金");
        //    Localization.addLocalization("Wood", "木");
        //    Localization.addLocalization("Water", "水");
        //    Localization.setLocalization("Fire", "火");
        //    Localization.addLocalization("Ground", "土");
        //    Localization.setLocalization("creature_statistics_age", "年龄/寿元");

        //    #endregion

        //    #region 特质
        //    Localization.addLocalization("trait_cursed_immune", "诅咒免疫");
        //    Localization.addLocalization("trait_asylum", "天道眷顾");
        //    Localization.addLocalization("trait_asylum_info", "真正的永恒");
        //    Localization.addLocalization("trait_race", "种族");
        //    Localization.addLocalization("trait_realm", "境界");
        //    Localization.addLocalization("trait_element", "灵根");
        //    Localization.addLocalization("trait_cultivationBook", "家族功法");
        //    Localization.addLocalization("trait_realm_info", "境界信息");
        //    Localization.addLocalization("trait_element_info", "灵根信息");
        //    Localization.addLocalization("trait_cultivationBook_info", "功法信息");
        //    #endregion

        //    #region 文化科技
        //    Localization.addLocalization("tech_culti_normal", "仙路");
        //    Localization.addLocalization("tech_culti_bodying", "炼体");
        //    Localization.addLocalization("tech_culti_bushido", "武道");
        //    #endregion
        //    #region 装备
        //    Localization.addLocalization("item_summonTian1", "哈？没名字");

        //    #endregion
        //    #region 世界信息
        //    Localization.addLocalization("baseLog", "用作文本加载");
        //    Localization.addLocalization("Yao_unite", "$king$ 统一妖族，立万世妖庭——$kingdom$");
        //    #endregion
        //}
        void initWindows()
        {
            MoreWorldLaws.init();
            WindowAboutThis.init();
            WindowChunkInfo.init();
            WindowFamily.init();
            WindowMoreStats.init();
            WindowTops.init();
        }
        void addRaceFeature()
        {
            foreach (ActorStats stats in AssetManager.unitStats.list)
            {

                RaceFeature feature = new RaceFeature();
                feature.raceID = stats.race;
                feature.raceSpells = new List<ExtensionSpell>();
                this.raceFeatures.Add(stats.id, feature);
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
            foreach (string familyName in ChineseNameAsset.familyNameTotal)
            {
                if (!familys.ContainsKey(familyName))
                {
                    familys.Add(familyName, new Family(familyName));
                }
            }
        }
        public void resetActorMore()
        {
            instance.actorToMoreData.Clear();
            foreach (Actor actor in MapBox.instance.units)
            {
                MoreStats moreStats = new MoreStats();
                MoreActorData moreData = new MoreActorData();
                instance.actorToMoreData.Add(actor.GetData().actorID, moreData);
                string name = actor.GetData().firstName;
                foreach (string fn in ChineseNameAsset.familyNameTotal)
                {
                    if (actor.stats.unit && name.StartsWith(fn))
                    {
                        moreData.familyID = fn;
                        break;
                    }
                    else if (!actor.stats.unit && name.EndsWith(fn))
                    {
                        moreData.familyID = fn;
                    }
                }
                moreData.cultisystem = "default";
                moreData.specialBody = "FT";
                moreData.element = new ChineseElement();
                moreData.bonusStats = new MoreStats();
                moreData.coolDown = new Dictionary<string, int>();
                moreData.canCultivate = true;
                moreStats.element = moreData.element;
            }
        }
        public void resetCreatureLimit()
        {
            Dictionary<string, int> temp = new Dictionary<string, int>();
            foreach (string id in instance.creatureLimit.Keys)
            {
                temp[id] = 1;
            }
            instance.creatureLimit.Clear();
            instance.creatureLimit = temp;
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
                    if (instance.chunkToElement[chunkID].baseElementContainer[i] < 0) { instance.chunkToElement[chunkID].baseElementContainer[i] = 0; }
                }
                instance.chunkToElement[chunkID].normalize();
            }
            #endregion
            Thread.CurrentThread.Abort();
            Thread.CurrentThread.DisableComObjectEagerCleanup();
        }
        delegate bool checker<T>(T key);
        bool checkAlive(string actorID){
            if (MapBox.instance.getActorByID(actorID) == null)
            {
                return false;
            }
            return true;
        }
        void clearMemory()
        {
            Dictionary<int, ActorStatus> TactorToData = new Dictionary<int, ActorStatus>();//单位与单位数据映射
            Dictionary<int, BaseStats> TactorToCurStats = new Dictionary<int, BaseStats>();//单位与属性映射
            Dictionary<string, MoreActorData> TactorToMoreData = new Dictionary<string, MoreActorData>();//单位编号与更多数据映射词典

            //valueClone(actorToData, TactorToData);
            //valueClone(actorToCurStats, TactorToCurStats);
            valueClone(actorToMoreData, TactorToMoreData,this.checkAlive);

            actorToData.Clear();
            actorToCurStats.Clear();
            actorToMoreData.Clear();

            actorToData = TactorToData;
            actorToCurStats = TactorToCurStats;
            actorToMoreData = TactorToMoreData;

            Thread.CurrentThread.Abort();
            Thread.CurrentThread.DisableComObjectEagerCleanup();
        }
        void valueClone<T1, T2>(Dictionary<T1, T2> from, Dictionary<T1, T2> to,checker<T1> checker = null)
        {
            foreach (T1 key in from.Keys)
            {
                if (key != null && (checker==null || checker(key)))
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

        //重建地图导致问题
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapBox), "generateNewMap", typeof(string))]
        public static void GenerateMap()
        {
            instance.initChunkElement();
            instance.createOrResetFamily();
            instance.resetCreatureLimit();
            instance.SpecialBodyLimit = 200;
            AddAssetManager.specialBodyLibrary.reset();
            instance.actorToCurStats = new Dictionary<int, BaseStats>();
            instance.actorToMoreData = new Dictionary<string, MoreActorData>();
            instance.actorToData = new Dictionary<int, ActorStatus>();
            foreach (string key in WorldLawHelper.originLaws.Keys)
            {
                MapBox.instance.worldLaws.dict.Add(key, WorldLawHelper.originLaws[key]);
            }

        }
        //语言材质加载
        [HarmonyPostfix]
        [HarmonyPatch(typeof(LocalizedTextManager),"setLanguage")]
        public static void setLanguage_Postfix(string pLanguage)
        {
            if (pLanguage != "cz")
            {
                pLanguage = "en";
                ChineseNameGenerator.isBeingUsed = false;
            }
            else
            {
                ChineseNameGenerator.isBeingUsed = true;
            }
            string text = ResourcesHelper.LoadTextAsset("languages/" + pLanguage + ".txt");
            Dictionary<string, object> textDic = Json.Deserialize(text) as Dictionary<string,object>;
            Dictionary<string, string> localizedText = Reflection.GetField(typeof(LocalizedTextManager), LocalizedTextManager.instance, "localizedText") as Dictionary<string, string>;
            foreach(string key in textDic.Keys)
            {
                localizedText[key] = textDic[key] as string;
            }
        }
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
            if (pActor == null || pActor.GetData() == null)
            {
                return true;
            }
            try {
                instance.creatureLimit[pActor.stats.id]++; 
            }
            catch(KeyNotFoundException)
            {
                
            }
            try
            {
                MoreActorData moreData = pActor.GetMoreData();
                Family family = instance.familys[moreData.familyID];
                family.num--;
                if (family.num <= 0)
                {
                    instance.familys[moreData.familyID] = new Family(moreData.familyID);
                }
            }
            catch
            {

            }
            try
            {
                instance.kingdomBindActors[pActor.kingdom.id].Remove(pActor);
            }
            catch
            {
                //直接用于排除非智慧国家，以及与killhimself重叠部分
                //This paragraph is used to exclude non-intelligent kingdoms and the overlap with "killHimself".
            }
            return true;
            }
        //百年事件（更新灵气，清理内存，以及其他
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapBox), "updateObjectAge")]
        public static void toUpdateTimeEvent()
        {
            List<BonusStatsManager> temp = new List<BonusStatsManager>();
            for (int i = 0; i < instance.bonusStatsManagers.Count; i++)
            {
                instance.bonusStatsManagers[i].update();
                if (instance.bonusStatsManagers[i].leftTime <= 0)
                {
                    continue;
                }
                temp.Add(instance.bonusStatsManagers[i]);
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
                        if (User.stats.id == "unit_Tian" && User.GetMoreData().coolDown.ContainsKey("summonTian"))
                        {

                            instance.raceFeatures["unit_Tian"].raceSpells[0].castSpell(User, null);//召唤战舰s
                        }
                    }
                }
                if (MapBox.instance.mapStats.year % 500 == 0)
                {
                    Thread t = new Thread(new ThreadStart(instance.clearMemory));
                    t.Start();
                    foreach (Kingdom kingdom in MapBox.instance.kingdoms.list_civs)
                    {
                        if (kingdom.raceID == "Tian" && kingdom.king != null)
                        {
                            if (kingdom.king.stats.id == "unit_Tian" && kingdom.king.GetMoreData().coolDown["summonTian1"] == 1)
                            {
                                instance.raceFeatures["unit_Tian"].raceSpells[1].castSpell(kingdom.king, null);//召唤机甲
                            }
                        }
                    }
                }
            }
            if (MapBox.instance.worldLaws.dict["MoreDisasters"].boolVal &&MapBox.instance.mapStats.month == 2&&instance.creatureLimit["Nian"]>0&&MapBox.instance.mapStats.year%2022==0)
            {
                WorldTile tile = MapBox.instance.tilesList.GetRandom();
                Actor Nian = MapBox.instance.spawnNewUnit("Nian", tile);
                int level = MapBox.instance.mapStats.year / 2022*10 + 1;
                if (level > 110)
                {
                    level = 110;
                }
                Nian.GetData().level = level;
                Nian.GetData().health = int.MaxValue >> 2;
                instance.creatureLimit["Nian"]--;
                WorldTools.logSomething($"<color={Toolbox.colorToHex(Toolbox.color_log_warning,true)}>年兽入侵！</color>", "iconKingslayer",tile);
            }
        }
        //城市给予物品修复
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "giveItem")]
        public static bool giveItem_Prefix(ref bool __result, Actor pActor, ActorEquipmentSlot pSlot, City pCity)
        {
            if (pActor == null || pActor.equipment == null || pSlot == null || pCity == null)
            {
                __result = false;
                return false;
            }
            return true;
        }
        //城市获取死亡人口的装备修复
        [HarmonyPrefix]
        [HarmonyPatch(typeof(City), "giveItemsToCity")]
        public static bool giveItemsToCity_Prefix(City pCity, Actor pDeadActor)
        {
            if (pDeadActor.stats.use_items)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 乱七八糟的初始化
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(sfx.MusicMan), "clear")]
        public static IEnumerable<CodeInstruction> clear_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo addRaces = AccessTools.Method(typeof(Main), "sfx_MusicMan_racesAdd");
            codes.Insert(23, new CodeInstruction(OpCodes.Callvirt, addRaces));
            return codes.AsEnumerable();
        }
        public static void sfx_MusicMan_racesAdd()
        {
            foreach (string race in instance.moreRaces)
            {
                sfx.MusicMan.races[race] = new sfx.MusicRaceContainer();
            }
        }
        #endregion
    }
}