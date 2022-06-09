using Assets.SimpleZip;
using CultivationWay;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace Cultivation_Way
{
    class SavedModData
    {
        public int saveVersion;

        public int specialBodyLimit = 100;

        public Dictionary<string, int> creatureLimit = new Dictionary<string, int>();//生物数量限制

        public List<SpecialBody> specialBodies = new List<SpecialBody>(150);//特殊体质表

        public List<Family> familys = new List<Family>();//家族表

        public List<MoreData> moreActorData = new List<MoreData>();
        public List<MoreData> moreBuildingData = new List<MoreData>();

        public Dictionary<string, List<string>> kingdomBindActors = new Dictionary<string, List<string>>();//国家与绑定的生物

        public Dictionary<string, MoreData> tempMoreData = new Dictionary<string, MoreData>();//单位与更多数据映射词典

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
                creatureLimit = instance.creatureLimit;
                familys = instance.familys.Values.ToList();
                chunkToElement = instance.chunkToElement;
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
            return Zip.Compress(this.toJson());
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
                    if(actor.easyData.alive==false || actor.stats.skipSave)
                    {
                        continue;
                    }
                    actor.extendedData.status.element = actor.extendedCurStats.element.baseElementContainer;
                    actor.extendedData.status.compositionsID = new string[actor.compositions.Count];
                    for(int i=0;i<actor.compositions.Count;i++)
                    {
                        if (actor.compositions[i].objectType == MapObjectType.Building)
                        {
                            actor.extendedData.status.compositionsID[i] = (ReflectionUtility.Reflection.GetField(typeof(Building),(Building)(actor.compositions[i]),"data") as BuildingData).objectID;
                        }
                        else if (actor.compositions[i].objectType == MapObjectType.Actor)
                        {
                            actor.extendedData.status.compositionsID[i] = ((ExtendedActor)(actor.compositions[i])).easyData.actorID;
                        }
                    }
                    moreActorData.Add(actor.extendedData);
                }
                foreach (ExtendedBuilding building in MapBox.instance.buildings.getSimpleList())
                {
                    building.extendedData.status.element = building.extendedCurStats.element.baseElementContainer;
                    building.extendedData.status.compositionsID = new string[building.compositions.Count];
                    for (int i = 0; i < building.compositions.Count; i++)
                    {
                        if (building.compositions[i].objectType == MapObjectType.Building)
                        {
                            building.extendedData.status.compositionsID[i] = (ReflectionUtility.Reflection.GetField(typeof(Building), (Building)(building.compositions[i]), "data") as BuildingData).objectID;
                        }
                        else if (building.compositions[i].objectType == MapObjectType.Actor)
                        {
                            building.extendedData.status.compositionsID[i] = ((ExtendedActor)(building.compositions[i])).easyData.actorID;
                        }
                    }
                    moreBuildingData.Add(building.extendedData);
                }
            }, "Prepare Mod Data(2/3): Prepare units and buildings data", false);
            SmoothLoader.add(delegate
            {
                foreach (string kingdomID in Main.instance.kingdomBindActors.Keys)
                {
                    kingdomBindActors[kingdomID] = new List<string>();
                    foreach (ExtendedActor actor in Main.instance.kingdomBindActors[kingdomID])
                    {
                        kingdomBindActors[kingdomID].Add(actor.easyData.actorID);
                    }
                }
            }, "Prepare Mod Data(2/3): Prepare kingdoms data", false);
        }
        public SavedModData() { }
    }
}
