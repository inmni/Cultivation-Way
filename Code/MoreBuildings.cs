using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CultivationWay;
using Cultivation_Way.Utils;
using UnityEngine;

namespace Cultivation_Way
{
    class MoreBuildings
    {
        List<BuildingAsset> humanBuildings = new List<BuildingAsset>();
        internal void init()
        {
            foreach (BuildingAsset humanBuilding in AssetManager.buildings.list)
            {
                if (humanBuilding.race == "human")
                {
                    humanBuildings.Add(humanBuilding);
                }
            }
            addRaceNormalBuildings();

		}
        private void addRaceNormalBuildings()
        {
            
            foreach(string race in Main.moreRaces)
            {
                foreach(BuildingAsset humanBuilding in humanBuildings)
                {
					string buildingName = humanBuilding.id.Remove(humanBuilding.id.Length - 5);
					BuildingAsset newBuilding=AssetManager.buildings.clone(buildingName + race, humanBuilding.id);
					newBuilding.race = race;
					newBuilding.shadow = false;
					loadSprites(newBuilding);
				}
            }
        }
        private void loadSprites(BuildingAsset pTemplate)
        {
			string folder = pTemplate.race;
            if (folder == string.Empty)
            {
				folder = "Others";
            }
			folder = folder + "/" + pTemplate.id.Replace("_" + pTemplate.race, "");
			Sprite[] array = ResourcesHelper.loadAllSprite("buildings/" + folder);
			
			pTemplate.sprites = new BuildingSprites();
			foreach (Sprite sprite in array)
			{
				string[] array2 = sprite.name.Split(new char[] { '_' });
				string text = array2[0];
				int num = int.Parse(array2[1]);
				if (array2.Length == 3)
				{
					int.Parse(array2[2]);
				}
				while (pTemplate.sprites.animationData.Count < num + 1)
				{
					pTemplate.sprites.animationData.Add(null);
				}
				if (pTemplate.sprites.animationData[num] == null)
				{
					pTemplate.sprites.animationData[num] = new BuildingAnimationDataNew();
				}
				BuildingAnimationDataNew buildingAnimationDataNew = pTemplate.sprites.animationData[num];
				if (text.Equals("main"))
				{
					buildingAnimationDataNew.list_main.Add(sprite);
					if (buildingAnimationDataNew.list_main.Count > 1)
					{
						buildingAnimationDataNew.animated = true;
					}
				}
				else if (text.Equals("ruin"))
				{
					buildingAnimationDataNew.list_ruins.Add(sprite);
				}
				else if (text.Equals("shadow"))
				{
					buildingAnimationDataNew.list_shadows.Add(sprite);
				}
				else if (text.Equals("construction"))
				{
					pTemplate.sprites.construction = sprite;
				}
				else if (text.Equals("constructionShadow"))
				{
					pTemplate.sprites.construction_shadow = sprite;
				}
				else if (text.Equals("special"))
				{
					buildingAnimationDataNew.list_special.Add(sprite);
				}
				else if (text.Equals("mini"))
				{
					pTemplate.sprites.mapIcon = new BuildingMapIcon(sprite);
				}
			}
			foreach (BuildingAnimationDataNew buildingAnimationDataNew2 in pTemplate.sprites.animationData)
			{
				buildingAnimationDataNew2.main = buildingAnimationDataNew2.list_main.ToArray();
				buildingAnimationDataNew2.ruins = buildingAnimationDataNew2.list_ruins.ToArray();
				buildingAnimationDataNew2.shadows = buildingAnimationDataNew2.list_shadows.ToArray();
				buildingAnimationDataNew2.special = buildingAnimationDataNew2.list_special.ToArray();
			}
		}
    }
}
