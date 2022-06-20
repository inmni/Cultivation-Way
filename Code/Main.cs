using Cultivation_Way;
using Cultivation_Way.MoreAiBehaviours;
using Cultivation_Way.Utils;
using HarmonyLib;
using NCMS;
using ReflectionUtility;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Threading;
using UnityEngine;
using UnityEngine.Purchasing.MiniJSON;
using static Config;
//using UnityEditor;
/*
MonoBehaviour.print("[修真之路Cultivation Way]:测试点n");//测试用测试点格式

*/
namespace CultivationWay
{
    [ModEntry]
    internal class Main : MonoBehaviour
    {
        //注意：Culture.create()已在ChineseNameGenerator添加完全拦截
        //注意：智慧种族的声音未解决
        public const string mainPath = "Mods/Cultivation-Way";//主路径
        //另一条"worldbox_Data/StreamingAssets/Mods/NCMS/Core/Temp/Mods/修真之路"

        public static int count = 0;

        public static Main instance;

        public static ResourcesBuffer resourcesBuffer = new ResourcesBuffer();

        public NewEffectManager effectManager;

        public CustomPrefabs prefabs = new CustomPrefabs();

        public ExtendedWorldData worldData = new ExtendedWorldData();

        public List<BonusStatsManager> bonusStatsManagers = new List<BonusStatsManager>();

        internal Transform transformUnits;
        internal Transform transformCreatures;
        internal GameStatsData gameStatsData;
        internal ZoneCalculator zoneCalculator;
        public string addMapMode = "";

        private float controlDeltTime = 0.5f;
        private Func<MapBox,bool> canInspectWithMainTouch;
        private Func<MapBox,bool> canInspectWithRightClick;
        internal int SpecialBodyLimit = 200;
        #region 更多玩意
        public MoreItems moreItems = new MoreItems();
        public MoreTraits moreTraits = new MoreTraits();
        public MoreActors moreActors = new MoreActors();
        public MoreProjectiles moreProjectiles = new MoreProjectiles();
        public MoreRaces moreRaces = new MoreRaces();
        public MoreKingdoms moreKingdoms = new MoreKingdoms();
        public MoreBuildings moreBuildings = new MoreBuildings();
        public MoreGodPowers moreGodPowers = new MoreGodPowers();
        public MoreDrops moreDrops = new MoreDrops();
        public MoreCultureTech moreCultureTechs = new MoreCultureTech();
        public MoreMapModes moreMapModes = new MoreMapModes();
        public MoreWorldLaws moreWorldLaws = new MoreWorldLaws();
        public MoreStatusEffects moreStatusEffects = new MoreStatusEffects();
        public MoreTopTileType moreTopTileTypes = new MoreTopTileType();
        public BehaviourTaskCityLibrary moreCityTasks = new BehaviourTaskCityLibrary();
        public Dictionary<string, ExtendedActorStats> extendedActorStatsLibrary = new Dictionary<string, ExtendedActorStats>();
        public Dictionary<string, ExtendedItemStats> extendedItemStatsLibrary = new Dictionary<string, ExtendedItemStats>();
        public List<string> addActors = new List<string>();
        public List<string> addItems = new List<string>();
        public List<string> addRaces = new List<string>();
        public Dictionary<string, Vector2> addProjectiles = new Dictionary<string, Vector2>();
        #endregion
        #region 初始化n件套
        private bool initiated = false;

        private void Awake()
        {
            instance = this;
            MonoBehaviour.print("[修真之路Cultivation Way]:开始加载");
        }

        private void Start()
        {
            //创建按钮栏
            MorePowers.createPowerTab();
            print("[修真之路Cultivation Way]:创建按钮栏成功");
            //添加Asset
            AddAssetManager.addAsset();
            print("[修真之路Cultivation Way]:添加Asset成功");
            //加载按钮
            MorePowers.createButtons();
            print("[修真之路Cultivation Way]:加载按钮成功");
            setLanguage_Postfix(Reflection.GetField(typeof(LocalizedTextManager), LocalizedTextManager.instance, "language") as string);
            //开启拦截
            patchHarmony();
            MonoBehaviour.print("[修真之路Cultivation Way]:启用拦截成功");
            effectManager = transform.gameObject.AddComponent<NewEffectManager>();
        }

        private void Update()
        {
            if (!gameLoaded)
            {
                return;
            }
            if (!initiated)
            {
                initiated = true;
                #region 初始化
                canInspectWithMainTouch = (Func<MapBox,bool>)MapBox.instance.GetFastMethod("canInspectWithMainTouch");
                canInspectWithRightClick = (Func<MapBox,bool>)MapBox.instance.GetFastMethod("canInspectWithRightClick");
                initWindows();//窗口
                //sfx_MusicMan_racesAdd();
                WorldLawHelper.initWorldLaws();
                MoreRaces.kingdomColorsDataInit();//国家颜色数据
                initChunkElement();//区块元素
                moreMapModes.add();//地图显示模式
                createOrResetFamily();//家族初始化
                ExtendedActorStats.init();//添加种族特色
                ExtendedBuildingStats.init();
                prefabs.init();//预制体初始化
                instance.gameStatsData = Reflection.GetField(typeof(GameStats), MapBox.instance.gameStats, "data") as GameStatsData;
                instance.zoneCalculator = Reflection.GetField(typeof(MapBox), MapBox.instance, "zoneCalculator") as ZoneCalculator;
                //PrefabUtility.SaveAsPrefabAsset((GameObject)Resources.Load("actors/p_unit", typeof(GameObject)), "p_unit.prefab");

                #endregion
            }
            if (ExtendedWorldData.instance.chunkToElement.Count != Config.ZONE_AMOUNT_X * Config.ZONE_AMOUNT_Y << 6)
            {
                initChunkElement();
            }

            if (controlDeltTime > 0)
            {
                controlDeltTime -= Time.deltaTime;
            }
            updateControl();
        }

        private void updateControl()
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
            if (!MapBox.instance.GetValue<bool>("alreadyUsedZoom")&& (canInspectWithMainTouch(MapBox.instance) || canInspectWithRightClick(MapBox.instance))
                && ((float)MapBox.instance.GetValue<float>("inspectTimerClick") < 0.2f) && !MapBox.instance.isActionHappening() && !MoveCamera.cameraDragActivated)
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

        private void patchHarmony()
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

        private void initWindows()
        {
            moreWorldLaws.init();
            WindowAboutThis.init();
            WindowChunkInfo.init();
            WindowFamily.init();
            WindowMoreStats.init();
            WindowTops.init();
        }
        public void initChunkElement()
        {
            ExtendedWorldData.instance.chunkToElement.Clear();
            ExtendedWorldData.instance.chunks = ((MapChunkManager)Reflection.GetField(typeof(MapBox), MapBox.instance, "mapChunkManager")).list;
            foreach (MapChunk chunk in ExtendedWorldData.instance.chunks)
            {
                ExtendedWorldData.instance.chunkToElement.Add(chunk.id, new ChineseElement(new int[] { 20, 20, 20, 20, 20 }).getRandom());
            }
        }
        public void createOrResetFamily()
        {
            ExtendedWorldData.instance.familys.Clear();
            foreach (string familyName in ChineseNameAsset.familyNameTotal)
            {
                if (!ExtendedWorldData.instance.familys.ContainsKey(familyName))
                {
                    ExtendedWorldData.instance.familys.Add(familyName, new Family(familyName));
                }
            }
        }
        public void resetCreatureLimit()
        {
            Dictionary<string, int> temp = new Dictionary<string, int>();
            foreach (string id in ExtendedWorldData.instance.creatureLimit.Keys)
            {
                temp[id] = 1;
            }
            ExtendedWorldData.instance.creatureLimit.Clear();
            ExtendedWorldData.instance.creatureLimit = temp;
        }

        private void updateChunkElement()
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
            foreach (MapChunk chunk in ExtendedWorldData.instance.chunks)
            {
                for (int type = 0; type < 5; type++)
                {
                    ExtendedWorldData.instance.chunkToElement[chunk.id].baseElementContainer[type] +=
                        ExtendedWorldData.instance.chunkToElement[chunk.id].baseElementContainer[type] << 10;
                }

                foreach (MapChunk neighbourChunk in chunk.neighbours)
                {
                    for (int type = 0; type < 5; type++)
                    {
                        ExtendedWorldData.instance.chunkToElement[chunk.id].baseElementContainer[type] +=
                            (ExtendedWorldData.instance.chunkToElement[neighbourChunk.id].baseElementContainer[OthersHelper.getBePromotedBy(type)] % 1024) >> 4;
                        ExtendedWorldData.instance.chunkToElement[chunk.id].baseElementContainer[type] -=
                            (ExtendedWorldData.instance.chunkToElement[neighbourChunk.id].baseElementContainer[OthersHelper.getBeOppsitedBy(type)] % 1024) >> 4;
                    }
                }
            }
            #endregion
            #region 总结
            foreach (int chunkID in ExtendedWorldData.instance.chunkToElement.Keys)
            {
                for (int i = 0; i < 5; i++)
                {
                    ExtendedWorldData.instance.chunkToElement[chunkID].baseElementContainer[i] >>= 10;
                    if (ExtendedWorldData.instance.chunkToElement[chunkID].baseElementContainer[i] > 100) { ExtendedWorldData.instance.chunkToElement[chunkID].baseElementContainer[i] = 100; }
                    if (ExtendedWorldData.instance.chunkToElement[chunkID].baseElementContainer[i] < 0) { ExtendedWorldData.instance.chunkToElement[chunkID].baseElementContainer[i] = 0; }
                }
                ExtendedWorldData.instance.chunkToElement[chunkID].normalize();
            }
            #endregion
            Thread.CurrentThread.Abort();
            Thread.CurrentThread.DisableComObjectEagerCleanup();
        }

        private void clearMemory()
        {
            //Dictionary<int, ActorStatus> TempactorToData = new Dictionary<int, ActorStatus>();//单位与单位数据映射

            //Dictionary<int, BaseStats> TempactorToCurStats = new Dictionary<int, BaseStats>();//单位与属性映射

            //OthersHelper.valueClone(actorToCurStats, TempactorToCurStats);
            //OthersHelper.valueClone(actorToData, TempactorToData);

            //actorToCurStats.Clear();
            //actorToData.Clear();

            //actorToCurStats = TempactorToCurStats;
            //actorToData = TempactorToData;
            Thread.CurrentThread.Abort();
            Thread.CurrentThread.DisableComObjectEagerCleanup();
        }

        #endregion

        #region 一些不知道放哪的拦截

        //重建地图导致问题
        [HarmonyPostfix]
        [HarmonyPatch(typeof(MapBox), "generateNewMap", typeof(bool))]
        public static void GenerateMap()
        {
            instance.initChunkElement();
            instance.createOrResetFamily();
            instance.resetCreatureLimit();
            instance.SpecialBodyLimit = 200;
            AddAssetManager.specialBodyLibrary.reset();
            instance.gameStatsData = Reflection.GetField(typeof(GameStats), MapBox.instance.gameStats, "data") as GameStatsData;
            instance.zoneCalculator = Reflection.GetField(typeof(MapBox), MapBox.instance, "zoneCalculator") as ZoneCalculator;
            foreach (string key in WorldLawHelper.originLaws.Keys)
            {
                MapBox.instance.worldLaws.dict[key] = WorldLawHelper.originLaws[key];
            }

        }
        //语言材质加载
        [HarmonyPostfix]
        [HarmonyPatch(typeof(LocalizedTextManager), "setLanguage")]
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
            Dictionary<string, object> textDic = Json.Deserialize(text) as Dictionary<string, object>;
            Dictionary<string, string> localizedText = Reflection.GetField(typeof(LocalizedTextManager), LocalizedTextManager.instance, "localizedText") as Dictionary<string, string>;
            foreach (string key in textDic.Keys)
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
                        ExtendedActor User = (ExtendedActor)kingdom.getMaxLevelActor();
                        if (User.stats.id == "unit_Tian")
                        {
                            foreach (ExtensionSpell spell in User.extendedCurStats.spells)
                            {
                                if (spell.spellAssetID == "summonTian")
                                {
                                    spell.castSpell(User, null);//召唤战舰s
                                }
                            }
                        }
                    }
                }
                if (MapBox.instance.mapStats.year % 500 == 0)
                {
                    //Thread t = new Thread(new ThreadStart(instance.clearMemory));
                    //t.Start();
                    foreach (Kingdom kingdom in MapBox.instance.kingdoms.list_civs)
                    {
                        if (kingdom.raceID == "Tian" && kingdom.king != null)
                        {
                            ExtendedActor User = (ExtendedActor)kingdom.king;
                            if (User.stats.id == "unit_Tian")
                            {
                                foreach (ExtensionSpell spell in User.extendedCurStats.spells)
                                {
                                    if (spell.spellAssetID == "summonTian1")
                                    {
                                        spell.castSpell(User, null);//召唤战舰s
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //if (MapBox.instance.worldLaws.dict["MoreDisasters"].boolVal &&MapBox.instance.mapStats.month == 2&&instance.creatureLimit["Nian"]>0&&MapBox.instance.mapStats.year%2022==0)
            //{
            //    WorldTile tile = MapBox.instance.tilesList.GetRandom();
            //    Actor Nian = MapBox.instance.spawnNewUnit("Nian", tile);
            //    int level = MapBox.instance.mapStats.year / 2022*10 + 1;
            //    if (level > 110)
            //    {
            //        level = 110;
            //    }
            //    Nian.easyData.level = level;
            //    Nian.easyData.health = int.MaxValue >> 2;
            //    instance.creatureLimit["Nian"]--;
            //    WorldTools.logSomething($"<color={Toolbox.colorToHex(Toolbox.color_log_warning,true)}>年兽入侵！</color>", "iconKingslayer",tile);
            //}
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
            foreach (string race in instance.addRaces)
            {
                sfx.MusicMan.races[race] = new sfx.MusicRaceContainer();
            }
        }
        #endregion
    }
}