using Assets.SimpleZip;
using CultivationWay;
using HarmonyLib;
using Newtonsoft.Json;
using ReflectionUtility;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace Cultivation_Way
{
    internal class SaveAndLoadManager
    {
        internal const string name_main_save = "cultivation.wb";
        private static SavedModData savedModData;
        private static SavedMap savedMap;
        private static string pFolder = "";
        private static bool pCompress = false;
        //获取即将存入的数据
        public static SavedModData getSavedModData()
        {
            SavedModData savedModData = new SavedModData();
            savedModData.create();
            return savedModData;
        }

        public static void writeIn()
        {
            SavedModData savedModData = null;
            savedModData = getSavedModData();
            SmoothLoader.add(delegate
            {
                string folder = pFolder;
                bool compress = true;
                compress = pCompress;
                string path = folder + name_main_save;
                if (compress)
                {
                    File.Delete(path + "ox");
                    byte[] bytes = savedModData.toZip();
                    File.WriteAllBytes(path + "ox", bytes);
                }
                else
                {
                    File.Delete(path + "ax");
                    string contents = savedModData.toJson();
                    File.WriteAllText(path + "ax", contents);
                }
            }, "Write mod data", true);
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SaveManager), "saveMapData")]
        public static bool saveModData_pre()
        {
            SmoothLoader.prepare();
            return true;
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SaveManager), "saveMapData")]
        public static void saveModData(string pFolder, bool pCompress = true)
        {
            //Thread t = new Thread(() => writeIn(pFolder, pCompress));
            //t.Start();
            SaveAndLoadManager.pFolder = pFolder;
            SaveAndLoadManager.pCompress = pCompress;
            MapBox.instance.transitionScreen.CallMethod("startTransition", new LoadingScreen.TransitionAction(writeIn));

        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SaveManager), "loadWorld", typeof(string), typeof(bool))]
        public static bool prepareLoad(string pPath)
        {
            savedModData = getDataFromPath(pPath);
            return true;
        }
        //通过路径获取数据
        public static SavedModData getDataFromPath(string pMainPath)
        {
            pMainPath = folderPath(pMainPath);
            string path = pMainPath + name_main_save + "ox";
            //string t = Zip.Decompress(File.ReadAllBytes(path));
            //File.WriteAllText(pMainPath + "tmp.txt", t);
            if (File.Exists(path))
            {
                //JsonSerializerSettings s = new JsonSerializerSettings();
                //s.ObjectCreationHandling = ObjectCreationHandling.Reuse;
                return JsonConvert.DeserializeObject<SavedModData>(Zip.Decompress(File.ReadAllBytes(path)));
            }
            path = pMainPath + name_main_save + "ax";
            if (File.Exists(path))
            {
                return JsonConvert.DeserializeObject<SavedModData>(File.ReadAllText(path));
            }
            return null;
        }
        //获取文件夹路径
        private static string folderPath(string pFolder)
        {
            if (string.IsNullOrEmpty(pFolder))
            {
                return string.Empty;
            }
            string text = Path.DirectorySeparatorChar.ToString();
            string value = Path.AltDirectorySeparatorChar.ToString();
            if (!pFolder.EndsWith(text) && !pFolder.EndsWith(value))
            {
                pFolder += text;
            }
            return pFolder;
        }
        //将数据加载到游戏
        [HarmonyTranspiler]
        [HarmonyPatch(typeof(SaveManager), "loadData", typeof(SavedMap))]
        public static IEnumerable<CodeInstruction> loadData_Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            List<CodeInstruction> codes = instructions.ToList();
            MethodInfo loadModData = AccessTools.Method(typeof(SaveAndLoadManager), "loadModData");
            int offset = 0;
            codes.Insert(offset, new CodeInstruction(OpCodes.Ldarg_1));
            offset++;
            codes.Insert(offset, new CodeInstruction(OpCodes.Callvirt, loadModData));
            offset++;
            codes.Insert(offset, new CodeInstruction(OpCodes.Starg, 1));
            return codes;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SaveManager), "loadActors")]
        public static bool loadActors_Prefix(SaveManager __instance, int startIndex = 0, int pAmount = 0)
        {
            ExtendedWorldData.instance.tempMoreData = new Dictionary<string, MoreData>();
            foreach (string id in savedModData.tempMoreData.Keys)
            {
                ExtendedWorldData.instance.tempMoreData[id] = savedModData.tempMoreData[id];
            }
            savedMap = Reflection.GetField(typeof(SaveManager), __instance, "data") as SavedMap;
            int num = savedMap.actors.Count;
            if (pAmount > 0)
            {
                num = Mathf.Min(startIndex + pAmount, savedMap.actors.Count);
            }
            for (int i = startIndex; i < num; i++)
            {
                ActorData actorData = savedMap.actors[i];
                MoreData moreActorData = null;
                if (savedModData == null)
                {
                    moreActorData = new MoreData();
                }
                else
                {
                    moreActorData = savedModData.moreActorData[i];
                }
                WorldTile tile = MapBox.instance.GetTile(actorData.x, actorData.y);
                ExtendedActor actor = null;
                if (actorData.status.alive)
                {
                    if (actorData.status.gender == ActorGender.Unknown)
                    {
                        if (Toolbox.randomBool())
                        {
                            actorData.status.gender = ActorGender.Male;
                        }
                        else
                        {
                            actorData.status.gender = ActorGender.Female;
                        }
                    }

                    if ((!(actorData.status.statsID == "livingPlants") && !(actorData.status.statsID == "livingHouse")) || !string.IsNullOrEmpty(actorData.status.special_graphics))
                    {
                        actor = (ExtendedActor)MapBox.instance.spawnAndLoadUnit(actorData.status.statsID, actorData, tile);

                        if (!(actor == null) && savedMap.saveVersion < 6)
                        {
                            foreach (string pTrait in actor.stats.traits)
                            {
                                actor.addTrait(pTrait);
                            }
                        }
                    }
                }
                if (actor != null)
                {
                    actor.extendedData = moreActorData;
                    actor.extendedCurStats.element = new ChineseElement(moreActorData.status.element);
                }
                else
                {
                    ExtendedWorldData.instance.tempMoreData[actorData.status.actorID] = moreActorData;
                }
            }
            return false;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SaveManager), "loadBuildings")]
        public static bool loadBuildings_Prefix()
        {
            for (int i = 0; i < savedMap.buildings.Count; i++)
            {
                BuildingData buildingData = savedMap.buildings[i];
                if (buildingData.templateID.Contains("ork"))
                {
                    buildingData.templateID = buildingData.templateID.Replace("ork", "orc");
                }
                BuildingAsset b = AssetManager.buildings.get(buildingData.templateID);
                if (b != null)
                {
                    ExtendedBuilding building = (ExtendedBuilding)MapBox.instance.CallMethod("loadBuilding", buildingData);

                    if (building == null)
                    {
                        continue;
                    }
                    else
                    {
                        if (savedModData != null)
                        {
                            building.easyData = buildingData;
                            building.easyStats = b;
                            building.extendedData = savedModData.moreBuildingData[i];
                            building.extendedCurStats.element = new ChineseElement(building.extendedData.status.element);
                        }
                    }
                    //if (building.kingdom == null)
                    //{
                    //    MonoBehaviour.print(buildingData.templateID);
                    //    MapBox.instance.removeBuildingFully(building);
                    //}
                }
            }
            MapBox.instance.buildings.checkAddRemove();
            return false;
        }
        public static SavedMap loadModData(SavedMap pData)
        {
            Main instance = Main.instance;
            ExtendedWorldData.instance.tempMoreData.Clear();
            ExtendedWorldData.instance.familys.Clear();
            AddAssetManager.specialBodyLibrary.clear();
            instance.resetCreatureLimit();
            foreach (string key in Utils.WorldLawHelper.originLaws.Keys)
            {
                pData.worldLaws.dict[key] = Utils.WorldLawHelper.originLaws[key];
            }
            //此处两个temp为存储不存在项目
            List<string> tempTrait = new List<string>();
            List<string> tempTech = new List<string>();
            #region 处理生物bug
            for (int i = 0; i < pData.actors.Count; i++)
            {
                ActorData actor = pData.actors[i];
                List<string> newTraits = new List<string>();
                foreach (string trait in actor.status.traits)
                {
                    if (!tempTrait.Contains(trait) && AssetManager.traits.dict.ContainsKey(trait))
                    {
                        newTraits.Add(trait);
                    }
                    else if (!tempTrait.Contains(trait))
                    {
                        pData.actors[i].status.favorite = true;
                        tempTrait.Add(trait);
                    }
                }
                if (!AssetManager.moods.dict.ContainsKey(actor.status.mood))
                {
                    pData.actors[i].status.mood = "normal";
                }
                pData.actors[i].status.traits = newTraits;
            }
            #endregion
            #region 城市处理
            for (int i = 0; i < pData.cities.Count; i++)
            {
                CityData cityData = pData.cities[i];
                List<CityStorageSlot> tempResources = new List<CityStorageSlot>();
                foreach (CityStorageSlot resource in cityData.storage.savedResources)
                {
                    if (AssetManager.resources.dict.ContainsKey(resource.id))
                    {
                        tempResources.Add(resource);
                    }
                }
                pData.cities[i].storage.savedResources = tempResources;
            }
            #endregion
            #region 文化科技处理
            for (int i = 0; i < pData.cultures.Count; i++)
            {
                Culture culture = pData.cultures[i];
                List<string> newTech = new List<string>();
                foreach (string tech in culture.list_tech_ids)
                {
                    if (!tempTech.Contains(tech) && AssetManager.culture_tech.dict.ContainsKey(tech))
                    {
                        newTech.Add(tech);
                    }
                    else if (!tempTech.Contains(tech))
                    {
                        tempTech.Add(tech);
                    }
                }
                pData.cultures[i].list_tech_ids = newTech;
            }
            #endregion
            #region 国家处理
            ExtendedWorldData.instance.kingdomBindActors.Clear();
            foreach (Kingdom kingdom in pData.kingdoms)
            {
                ExtendedWorldData.instance.kingdomBindActors[kingdom.id] = new List<ExtendedActor>();
            }
            #endregion
            if (savedModData == null)
            {
                AddAssetManager.specialBodyLibrary.reset();
                instance.initChunkElement();
                instance.createOrResetFamily();
                instance.SpecialBodyLimit = 200;
                instance.resetCreatureLimit();
                return pData;
            }
            if (savedModData.creatureLimit == null)
            {
                savedModData.creatureLimit = new Dictionary<string, int>();
            }
            if (savedModData.creatureLimit.Count >= ExtendedWorldData.instance.creatureLimit.Count)
            {
                ExtendedWorldData.instance.creatureLimit = savedModData.creatureLimit;
            }
            instance.SpecialBodyLimit = savedModData.specialBodyLimit;

            #region 加载家族
            foreach (Family family in savedModData.familys)
            {
                if (family == null)
                {
                    MonoBehaviour.print(family.id + "家族为空");
                }
                ExtendedWorldData.instance.familys.Add(family.id, family);
            }
            foreach (string missing in ChineseNameAsset.familyNameTotal)
            {
                if (ExtendedWorldData.instance.familys.ContainsKey(missing))
                {
                    continue;
                }
                ExtendedWorldData.instance.familys.Add(missing, new Family(missing));
            }
            #endregion
            #region 加载国家与人物绑定关系
            if (savedModData.kingdomBindActors != null)
            {
                foreach (Kingdom kingdom in pData.kingdoms)
                {
                    if (!savedModData.kingdomBindActors.ContainsKey(kingdom.id))
                    {
                        continue;
                    }
                    int limit = savedModData.kingdomBindActors[kingdom.id].Count;
                    List<string> idList = savedModData.kingdomBindActors[kingdom.id];
                    foreach (ExtendedActor actor in kingdom.units)
                    {
                        if (idList.Contains(actor.easyData.actorID))
                        {
                            ExtendedWorldData.instance.kingdomBindActors[kingdom.id].Add(actor);
                            limit--;
                            if (limit == 0)
                            {
                                break;
                            }
                        }
                    }
                }
            }
            #endregion
            foreach (SpecialBody specialBody in savedModData.specialBodies)
            {
                AddAssetManager.specialBodyLibrary.add(specialBody);
            }
            if (savedModData.specialBodies.Count == 0)
            {
                AddAssetManager.specialBodyLibrary.reset();
            }
            ExtendedWorldData.instance.chunkToElement = savedModData.chunkToElement;
            return pData;
        }
    }
}
