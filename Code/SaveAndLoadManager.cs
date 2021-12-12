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
                instance.resetActorMoreStats();
                instance.initChunkElement();
                instance.createOrResetFamily();
                return;
            }
            instance.actorToMoreStats = savedModData.actorToMoreStats;
            instance.familys = savedModData.familys;
            instance.chunkToElement = savedModData.chunkToElement;
        }
        //通过路径获取数据
        public static SavedModData getDataFromPath(string pMainPath)
        {
            pMainPath = folderPath(pMainPath);
            string path = pMainPath + name_main_save+"ox";
            if (File.Exists(path))
            {
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
            string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;
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
            SavedModData savedModData = SaveAndLoadManager.getSavedModData();
            string path = folder + name_main_save;
            if (compress)
            {
                byte[] bytes = savedModData.toZip();
                File.WriteAllBytes(path + "ox", bytes);
            }
            else
            {
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
    }
}
