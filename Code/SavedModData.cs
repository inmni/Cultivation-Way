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

        public Dictionary<string, List<string>> kingdomBindActors = new Dictionary<string, List<string>>();//国家与绑定的生物

        public Dictionary<string, MoreActorData> actorToMoreData = new Dictionary<string, MoreActorData>();//单位与更多数据映射词典

        public Dictionary<int, ChineseElement> chunkToElement = new Dictionary<int, ChineseElement>();//区块与元素映射词典

        public void create()
        {
            SmoothLoader.add(delegate
            {
                familys.Clear();
                actorToMoreData.Clear();
                specialBodies.Clear();
                creatureLimit.Clear();
                kingdomBindActors.Clear();
                actorToMoreData.Clear();
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
                foreach (Actor actor in MapBox.instance.units)
                {
                    if (actor != null)
                    {
                        MoreActorData moreData = new MoreActorData();
                        MoreActorData copyFrom = actor.GetMoreData();
                        moreData.cultisystem = copyFrom.cultisystem;
                        moreData.element = copyFrom.element;
                        moreData.familyID = copyFrom.familyID;
                        moreData.familyName = copyFrom.familyName;
                        moreData.magic = copyFrom.magic;
                        moreData.bonusStats = copyFrom.bonusStats;
                        moreData.coolDown = copyFrom.coolDown;
                        moreData.canCultivate = copyFrom.canCultivate;
                        moreData.specialBody = copyFrom.specialBody;
                        actorToMoreData.Add(actor.GetData().actorID, moreData);
                    }
                }
            }, "Prepare Mod Data(2/3): Prepare units data", false);
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
