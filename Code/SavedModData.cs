using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assets.SimpleZip;
using CultivationWay;
using Newtonsoft.Json;
using UnityEngine;


namespace Cultivation_Way
{
    class SavedModData
    {
		public int specialBodyLimit = 100;
		public int summonTian1Limit = 1;

		public List<SpecialBody> specialBodies = new List<SpecialBody>(150);//特殊体质表

        public List<Family> familys = new List<Family>();//家族表

		public Dictionary<string, MoreActorData> actorToMoreData = new Dictionary<string, MoreActorData>();//单位与更多数据映射词典

        public Dictionary<int, ChineseElement> chunkToElement = new Dictionary<int, ChineseElement>();//区块与元素映射词典

        public void create()
        {
			familys.Clear();
			actorToMoreData.Clear();
			specialBodies.Clear();
			actorToMoreData = new Dictionary<string, MoreActorData>();
			prepare();
			Main instance = Main.instance;
			specialBodyLimit = instance.SpecialBodyLimit;
			summonTian1Limit = instance.summonTian1Limit;
            familys = instance.familys.Values.ToList();
			chunkToElement = instance.chunkToElement;
			specialBodies = AddAssetManager.specialBodyLibrary.list;
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
				if (string.IsNullOrEmpty(text) )
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
			Main instance = Main.instance;
			foreach (Actor actor in MapBox.instance.units)
			{
                if (actor != null)
                {
					MoreActorData moreData = new MoreActorData();
					MoreActorData copyFrom = actor.GetMoreData();
					moreData.cultisystem = copyFrom.cultisystem;
					moreData.element = copyFrom.element;
					moreData.familyID = copyFrom.familyID;
					moreData.magic = copyFrom.magic;
					moreData.bonusStats = copyFrom.bonusStats;
					moreData.coolDown = copyFrom.coolDown;
                    if (copyFrom.specialBody == null || copyFrom.specialBody == string.Empty)
                    {
						copyFrom.specialBody = "FT";
						moreData.specialBody = "FT";
                    }
                    else
                    {
						moreData.specialBody = copyFrom.specialBody;

					}
					actorToMoreData.Add(actor.GetData().actorID, moreData);
                }
			}
		}
		public SavedModData() { }
    }
}
