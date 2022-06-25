using Assets.SimpleZip;
using CultivationWay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Cultivation_Way
{
    internal class SavedModData
    {
        public int saveVersion;

        public int specialBodyLimit = 100;

        public Dictionary<string, int> creatureLimit = new Dictionary<string, int>();//生物数量限制

        public List<SpecialBody> specialBodies = new List<SpecialBody>(150);//特殊体质表

        public List<Family> familys = new List<Family>();//家族表

        public List<ExtendedActorData> moreActorData = new List<ExtendedActorData>();
        public List<ExtendedBuildingData> moreBuildingData = new List<ExtendedBuildingData>();

        public Dictionary<string, List<string>> kingdomBindActors = new Dictionary<string, List<string>>();//国家与绑定的生物

        public Dictionary<string, ExtendedActorData> tempMoreData = new Dictionary<string, ExtendedActorData>();//单位与更多数据映射词典

        public Dictionary<int, ChineseElement> chunkToElement = new Dictionary<int, ChineseElement>();//区块与元素映射词典

        public void create()
        {
            SmoothLoader.add(delegate
            {
                familys.Clear();
                specialBodies.Clear();
                creatureLimit.Clear();
                kingdomBindActors.Clear();
                tempMoreData.Clear();
                moreActorData.Clear();
                moreBuildingData.Clear();
                chunkToElement.Clear();
            }, "Prepare Mod Data(1/3): Clear old data", true);
            prepare();
            SmoothLoader.add(delegate
            {
                Main instance = Main.instance;
                specialBodyLimit = instance.SpecialBodyLimit;
                creatureLimit = ExtendedWorldData.instance.creatureLimit;
                familys = ExtendedWorldData.instance.familys.Values.ToList();
                chunkToElement = ExtendedWorldData.instance.chunkToElement;
                specialBodies = AddAssetManager.specialBodyLibrary.list;
            }, "Prepare Mod Data(3/3): Assign new data", true);
        }
        public string toJson()
        {
            string text = "";
            try
            {
                text = JsonConvert.SerializeObject(this, Formatting.Indented, new JsonSerializerSettings
                {
                    DefaultValueHandling = DefaultValueHandling.IgnoreAndPopulate
                });
                if (string.IsNullOrEmpty(text))
                {
                    text = JsonUtility.ToJson(this);
                }
            }
            catch (Exception message)
            {
                Debug.LogError(message);
                text = JsonUtility.ToJson(this);
            }
            if (string.IsNullOrEmpty(text))
            {
                throw new Exception("Error while creating json");
            }
            return text;
        }

        public byte[] toZip()
        {
            return Zip.Compress(toJson());
        }
        private void prepare()
        {
            SmoothLoader.add(delegate
            {
                foreach (string id in ExtendedWorldData.instance.tempMoreData.Keys)
                {
                    tempMoreData[id] = ExtendedWorldData.instance.tempMoreData[id];
                }
                foreach (ExtendedActor actor in MapBox.instance.units.getSimpleList())
                {
                    if (actor.easyData.alive == false || actor.stats.skipSave)
                    {
                        continue;
                    }
                    moreActorData.Add(actor.extendedData);
                }
                foreach (ExtendedBuilding building in MapBox.instance.buildings.getSimpleList())
                {
                    moreBuildingData.Add(building.extendedData);
                }
            }, "Prepare Mod Data(2/3): Prepare units and buildings data", false);
            SmoothLoader.add(delegate
            {
                foreach (string kingdomID in ExtendedWorldData.instance.kingdomBindActors.Keys)
                {
                    kingdomBindActors[kingdomID] = new List<string>();
                    foreach (ExtendedActor actor in ExtendedWorldData.instance.kingdomBindActors[kingdomID])
                    {
                        kingdomBindActors[kingdomID].Add(actor.easyData.actorID);
                    }
                }
            }, "Prepare Mod Data(2/3): Prepare kingdoms data", false);
        }
        public SavedModData() { }
    }
}
