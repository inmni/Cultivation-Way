using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.SimpleZip;
using HarmonyLib;
using Newtonsoft.Json;
using UnityEngine;
using CultivationWay;
using System.Threading;

namespace Cultivation_Way
{
    class SaveAndLoadManager
    {
        internal const string name_main_save = "cultivation.wb";

        //获取即将存入的数据
        public static SavedModData getSavedModData()
        {
            SavedModData savedModData = new SavedModData();
            savedModData.create();
            return savedModData;
        }
        //将数据加载到游戏
        public static void loadModData(SavedModData savedModData)
        {
            Main instance = Main.instance;

            
            if (savedModData == null)
            {
                AddAssetManager.specialBodyLibrary.reset();
                instance.initChunkElement();
                instance.createOrResetFamily();
                instance.resetActorMore();
                instance.SpecialBodyLimit = 100;
                instance.summonTian1Limit = 1;
                return;
            }
            instance.SpecialBodyLimit = savedModData.specialBodyLimit;
            instance.summonTian1Limit = savedModData.summonTian1Limit;
            AddAssetManager.specialBodyLibrary.clear();
            instance.familys.Clear();
            instance.actorToMoreStats.Clear();
            instance.actorToMoreData.Clear();
            instance.actorToCurStats.Clear();
            instance.actorToData.Clear();

            instance.familys = new Dictionary<string, Family>();
            instance.actorToMoreStats = new Dictionary<string, MoreStats>();
            instance.actorToMoreData = new Dictionary<string, MoreActorData>();

            foreach (Family family in savedModData.familys)
            {
                if (family == null)
                {
                    MonoBehaviour.print(family.id+"家族为空");
                }
                instance.familys.Add(family.id, family);
            }
            foreach (Actor actor in MapBox.instance.units)
            {
                string id = actor.GetData().actorID;
                MoreStats moreStats = new MoreStats();
                MoreActorData moreData = new MoreActorData();
                instance.actorToMoreStats.Add(id, moreStats);
                instance.actorToMoreData.Add(id, moreData);
                try
                {
                    moreStats.cultisystem = savedModData.actorToMoreData[id].cultisystem;
                    moreStats.element = savedModData.actorToMoreData[id].element;
                    moreData.cultisystem = moreStats.cultisystem;
                    moreData.element = moreStats.element;
                    moreData.magic = savedModData.actorToMoreData[id].magic;
                    moreData.bonusStats = savedModData.actorToMoreData[id].bonusStats;
                    moreData.coolDown = savedModData.actorToMoreData[id].coolDown;
                    moreData.specialBody = savedModData.actorToMoreData[id].specialBody;

                    moreStats.family = instance.familys[savedModData.actorToMoreData[id].familyID];
                    moreData.familyID = moreStats.family.id;
                }
                catch(KeyNotFoundException e)
                {
                    ActorStatus data = actor.GetData();
                    //设置家族
                    string name = data.firstName;
                    foreach (string fn in ChineseNameAsset.familyNameTotal)
                    {
                        if (name.StartsWith(fn))
                        {
                            moreStats.family = Main.instance.familys[fn];
                            moreData.familyID = moreStats.family.id;
                            break;
                        }
                    }
                    //设置修炼体系
                    if (actor.getCulture() != null && Main.instance.actorToMoreData[data.actorID].cultisystem == "default")
                    {
                        List<string> cultisystem = new List<string>();
                        foreach (string tech in actor.getCulture().list_tech_ids)
                        {
                            if (tech.StartsWith("culti_"))
                            {
                                cultisystem.Add(tech);
                            }
                        }
                        if (cultisystem.Count > 0)
                        {
                            Main.instance.actorToMoreStats[data.actorID].cultisystem = cultisystem.GetRandom().Remove(0, 6);
                            Main.instance.actorToMoreData[data.actorID].cultisystem = Main.instance.actorToMoreStats[data.actorID].cultisystem;
                        }
                    }
                    //添加种族特色
                    moreStats.spells.AddRange(Main.instance.raceFeatures[actor.stats.race].raceSpells);
                }
            }
            AddAssetManager.specialBodyLibrary.clear();
            foreach (SpecialBody specialBody in savedModData.specialBodies)
            {
                AddAssetManager.specialBodyLibrary.add(specialBody);
            }
            ((ChineseElementLibrary)AssetManager.instance.dict["element"]).reset();
            instance.chunkToElement = savedModData.chunkToElement;
            //foreach(string id in ((ChineseElementLibrary)AssetManager.instance.dict["element"]).dict.Keys)
            //{
            //    MonoBehaviour.print(id);
            //}
        }
        //通过路径获取数据
        public static SavedModData getDataFromPath(string pMainPath)
        {
            pMainPath = folderPath(pMainPath);

            
            string path = pMainPath + name_main_save+"ox";
            //string t = Zip.Decompress(File.ReadAllBytes(path));
            //File.WriteAllText(pMainPath + "tmp.txt", t);
            if (File.Exists(path))
            {
                string json = Zip.Decompress(File.ReadAllBytes(path));

                //JsonSerializerSettings s = new JsonSerializerSettings();
                //s.ObjectCreationHandling = ObjectCreationHandling.Reuse;
                JsonConvert.DeserializeObject<SavedModData>(json);//问题
                return JsonConvert.DeserializeObject<SavedModData>(Zip.Decompress(File.ReadAllBytes(path)));
            }
            path = pMainPath + name_main_save+"ax";
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
        public static void writeIn(object pFolder,object pCompress)
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
        [HarmonyPatch(typeof(SaveManager),"saveMapData")]
        public static void saveModData(string pFolder,bool pCompress = true)
        {
            //Thread t = new Thread(() => writeIn(pFolder, pCompress));
            //t.Start();
            writeIn(pFolder, pCompress);
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(SaveManager),"loadWorld",typeof(string),typeof(bool))]
        public static void loadModData(string pPath)
        {
            
            SavedModData mapFromPath = getDataFromPath(pPath);
            
            loadModData(mapFromPath);
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(SaveManager), "loadWorld", typeof(string), typeof(bool))]
        public static bool prepareLoad()
        {
            Main.instance.actorToData.Clear();
            Main.instance.actorToCurStats.Clear();
            Main.instance.actorToMoreStats.Clear();
            Main.instance.actorToMoreData.Clear();
            return true;
        }
    }
}
