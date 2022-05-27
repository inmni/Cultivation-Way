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
                foreach (string id in Main.instance.tempMoreData.Keys)
                {
                    tempMoreData[id] = Main.instance.tempMoreData[id];
                }
                foreach (Actor actor in MapBox.instance.units.getSimpleList())
                {
                    if(actor.GetData().alive==false || actor.stats.skipSave)
                    {
                        continue;
                    }
                    ExtendedActor extendedActor = (ExtendedActor)actor;
                    extendedActor.extendedData.status.element = extendedActor.extendedCurStats.element.baseElementContainer;
                    extendedActor.extendedData.status.compositionsID = new string[extendedActor.compositions.Count];
                    for(int i=0;i<extendedActor.compositions.Count;i++)
                    {
                        if (extendedActor.compositions[i].objectType == MapObjectType.Building)
                        {
                            extendedActor.extendedData.status.compositionsID[i] = (ReflectionUtility.Reflection.GetField(typeof(Building),(Building)(extendedActor.compositions[i]),"data") as BuildingData).objectID;
                        }
                        else if (extendedActor.compositions[i].objectType == MapObjectType.Actor)
                        {
                            extendedActor.extendedData.status.compositionsID[i] = ((Actor)(extendedActor.compositions[i])).GetData().actorID;
                        }
                    }
                    moreActorData.Add(extendedActor.extendedData);
                }
                foreach (Building building in MapBox.instance.buildings.getSimpleList())
                {
                    ExtendedBuilding extendedBuilding = (ExtendedBuilding)building;
                    extendedBuilding.extendedData.status.element = extendedBuilding.extendedCurStats.element.baseElementContainer;
                    extendedBuilding.extendedData.status.compositionsID = new string[extendedBuilding.compositions.Count];
                    for (int i = 0; i < extendedBuilding.compositions.Count; i++)
                    {
                        if (extendedBuilding.compositions[i].objectType == MapObjectType.Building)
                        {
                            extendedBuilding.extendedData.status.compositionsID[i] = (ReflectionUtility.Reflection.GetField(typeof(Building), (Building)(extendedBuilding.compositions[i]), "data") as BuildingData).objectID;
                        }
                        else if (extendedBuilding.compositions[i].objectType == MapObjectType.Actor)
                        {
                            extendedBuilding.extendedData.status.compositionsID[i] = ((Actor)(extendedBuilding.compositions[i])).GetData().actorID;
                        }
                    }
                    moreBuildingData.Add(extendedBuilding.extendedData);
                }
            }, "Prepare Mod Data(2/3): Prepare units and buildings data", false);
            SmoothLoader.add(delegate
            {
                foreach (string kingdomID in Main.instance.kingdomBindActors.Keys)
                {
                    kingdomBindActors[kingdomID] = new List<string>();
                    foreach (Actor actor in Main.instance.kingdomBindActors[kingdomID])
                    {
                        kingdomBindActors[kingdomID].Add(actor.GetData().actorID);
                    }
                }
            }, "Prepare Mod Data(2/3): Prepare kingdoms data", false);
        }
        public SavedModData() { }
    }
}
