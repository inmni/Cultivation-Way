using Assets.SimpleZip;
using CultivationWay;
using HarmonyLib;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using UnityEngine;

namespace Cultivation_Way
{
    class SaveAndLoadManager
    {
        internal const string name_main_save = "cultivation.wb";
        private static SavedModData savedModData;
        //获取即将存入的数据
        public static SavedModData getSavedModData()
        {
            SavedModData savedModData = new SavedModData();
            savedModData.create();
            return savedModData;
        }

        public static void writeIn(object pFolder, object pCompress)
        {
            string folder = pFolder as string;
            bool compress = true;
            if (pCompress != null)
            {
                compress = (bool)pCompress;
            }
            SavedModData savedModData = getSavedModData();
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
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(SaveManager), "saveMapData")]
        public static void saveModData(string pFolder, bool pCompress = true)
        {
            //Thread t = new Thread(() => writeIn(pFolder, pCompress));
            //t.Start();
            writeIn(pFolder, pCompress);
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
        public static SavedMap loadModData(SavedMap pData)
        {
            Main instance = Main.instance;
            instance.actorToData.Clear();
            instance.actorToCurStats.Clear();
            instance.actorToMoreStats.Clear();
            instance.actorToMoreData.Clear(); 
            instance.familys.Clear();
            AddAssetManager.specialBodyLibrary.clear();
            instance.resetCreatureLimit();
            foreach (string key in Utils.WorldLawHelper.originLaws.Keys)
            {
                pData.worldLaws.dict[key]=Utils.WorldLawHelper.originLaws[key];
            }
            //此处两个temp为存储不存在项目
            List<string> tempTrait = new List<string>();
            List<string> tempTech = new List<string>();
            #region 处理生物bug
            for(int i = 0; i < pData.actors.Count; i++)
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
            for(int i=0;i<pData.cities.Count;i++)
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
            for(int i=0;i<pData.cultures.Count;i++)
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
            instance.kingdomBindActors.Clear();
                foreach (Kingdom kingdom in pData.kingdoms)
                {
                    instance.kingdomBindActors[kingdom.id] = new List<Actor>();
                }
            #endregion
            if (savedModData == null)
            {
                AddAssetManager.specialBodyLibrary.reset();
                instance.initChunkElement();
                instance.createOrResetFamily();
                instance.SpecialBodyLimit = 200;
                instance.resetCreatureLimit();

                #region 生物处理
                foreach (ActorData actor in pData.actors)
                {
                    #region 处理mod特色
                    string id = actor.status.actorID;
                    MoreStats moreStats = new MoreStats();
                    MoreActorData moreData = new MoreActorData();
                    instance.actorToMoreStats.Add(id, moreStats);
                    instance.actorToMoreData.Add(id, moreData);
                    moreData.cultisystem = "default";
                    moreData.element = new ChineseElement();
                    moreData.magic = 0;
                    moreData.bonusStats = new MoreStats();
                    moreData.coolDown = new Dictionary<string, int>();
                    moreData.specialBody = "FT";
                    moreData.canCultivate = true;
                    moreData.familyID = "甲";
                    moreData.familyName = "甲";
                    moreStats.element = moreData.element;
                    #endregion
                }
                #endregion
                return pData;
            }
            if (savedModData.creatureLimit == null)
            {
                savedModData.creatureLimit = new Dictionary<string, int>();
            }
            if (savedModData.creatureLimit.Count >= instance.creatureLimit.Count)
            {
                instance.creatureLimit = savedModData.creatureLimit;
            }
            instance.SpecialBodyLimit = savedModData.specialBodyLimit;
            instance.familys = new Dictionary<string, Family>();
            instance.actorToMoreStats = new Dictionary<string, MoreStats>();
            instance.actorToMoreData = new Dictionary<string, MoreActorData>();
            #region 加载家族
            foreach (Family family in savedModData.familys)
            {
                if (family == null)
                {
                    MonoBehaviour.print(family.id + "家族为空");
                }
                instance.familys.Add(family.id, family);
            }
            foreach (string missing in ChineseNameAsset.familyNameTotal)
            {
                if (instance.familys.ContainsKey(missing))
                {
                    continue;
                }
                instance.familys.Add(missing, new Family(missing));
            }
            #endregion
            #region 加载人物数据
            foreach (ActorData actor in pData.actors)
            {
                string id = actor.status.actorID;
                
                MoreStats moreStats = new MoreStats();
                MoreActorData moreData = new MoreActorData();
                MoreActorData saved = savedModData.actorToMoreData[id];
                instance.actorToMoreStats.Add(id, moreStats);
                instance.actorToMoreData.Add(id, moreData);

                if (saved.cultisystem == string.Empty || saved.cultisystem == null)
                {
                    saved.cultisystem = "default";
                }
                if (saved.element == null || saved.element.baseElementContainer == null)
                {
                    saved.element = new ChineseElement();
                }
                if (saved.bonusStats == null)
                {
                    saved.bonusStats = new MoreStats();
                }
                if (saved.coolDown == null)
                {
                    saved.coolDown = new Dictionary<string, int>();
                }
                if (saved.specialBody == null || saved.specialBody == string.Empty)
                {
                    saved.specialBody = "FT";
                }
                if (saved.familyID == null || saved.familyID == string.Empty)
                {
                    saved.familyID = "甲";
                }
                if (saved.familyName == null || saved.familyName == string.Empty)
                {
                    saved.familyName = "甲";
                }
                moreData.cultisystem = saved.cultisystem;
                moreData.element = new ChineseElement(saved.element.baseElementContainer);
                moreData.magic = saved.magic;
                moreData.bonusStats = new MoreStats();
                moreData.coolDown = saved.coolDown;
                moreData.specialBody = saved.specialBody;
                moreData.canCultivate = saved.canCultivate;
                moreData.familyID = saved.familyID;
                moreData.familyName = saved.familyName;
                moreStats.element = moreData.element;
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
                        foreach (Actor actor in kingdom.units)
                        {
                            if (idList.Contains(actor.GetData().actorID))
                            {
                                instance.kingdomBindActors[kingdom.id].Add(actor);
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
            instance.chunkToElement = savedModData.chunkToElement;
            return pData;
        }
    }
}
