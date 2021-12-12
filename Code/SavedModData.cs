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
        //public List<Family> familys = new List<Family>();
        public Dictionary<string, Family> familys = new Dictionary<string, Family>();//家族映射表

        public Dictionary<string, MoreStats> actorToMoreStats = new Dictionary<string, MoreStats>();//单位与更多属性映射词典

        public Dictionary<int, ChineseElement> chunkToElement = new Dictionary<int, ChineseElement>();//区块与元素映射词典

        public void create()
        {
            Main instance = Main.instance;
            this.familys = instance.familys;
            this.actorToMoreStats = instance.actorToMoreStats;
            this.chunkToElement = instance.chunkToElement;

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
		public SavedModData() { }
    }
}
