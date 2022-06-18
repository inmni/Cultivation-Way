using System.Collections.Generic;

namespace Cultivation_Way
{
    internal class ExtendedBuilding : Building
    {
        internal MoreStats extendedCurStats = new MoreStats();
        internal MoreData extendedData = new MoreData();
        public BuildingData easyData;
        public BuildingAsset easyStats;
        public List<BaseSimObject> compositions = new List<BaseSimObject>();
    }

    internal class ExtendedBuildingStats
    {
        private static TileType soil_high = AssetManager.tiles.get("soil_high");
        private static TopTileType grass_high = AssetManager.topTiles.get("grass_high");
        internal static Dictionary<string, WorldAction> destroy_actions = new Dictionary<string, WorldAction>();
        internal static void init()
        {
            destroy_actions.Add("Circumvallation", resumeTiles);
        }
        public static WorldAction GetWorldAction(string pID)
        {
            try
            {
                return destroy_actions[pID];
            }
            catch (KeyNotFoundException)
            {
                foreach (string key in destroy_actions.Keys)
                {
                    if (pID.Contains(key))
                    {
                        return destroy_actions[key];
                    }
                }
            }
            return null;
        }
        public static bool resumeTiles(BaseSimObject pTarget, WorldTile pTile = null)
        {
            if (pTarget.city != null)
            {
                int cityKey = pTarget.city.GetHashCode();
                MoreAiBehaviours.CityWallCondition condition = MoreAiBehaviours.BehaviourTaskCityLibrary.buildWall.conditions[cityKey];
                condition.isBuilding = true;
                condition.done = false;
                string circumType = BuildingTools.getCircumType((Building)pTarget);
                switch (circumType)
                {
                    case "hori":
                        condition.horiWallTiles.Add(pTarget.currentTile);
                        break;
                    case "vert":
                        condition.vertWallTiles.Add(pTarget.currentTile);
                        break;
                    case "horiGate1":
                        condition.gateTiles.Add(pTarget.currentTile);
                        condition.gateDirections.Add(3);
                        break;
                    case "horiGate2":
                        condition.gateTiles.Add(pTarget.currentTile);
                        condition.gateDirections.Add(2);
                        break;
                    case "vertGate1":
                        condition.gateTiles.Add(pTarget.currentTile);
                        condition.gateDirections.Add(0);
                        break;
                    case "vertGate2":
                        condition.gateTiles.Add(pTarget.currentTile);
                        condition.gateDirections.Add(1);
                        break;
                    case "node1":
                        condition.nodeWallTiles.Add(pTarget.currentTile);
                        condition.nodeDirections.Add(1);
                        break;
                    case "node2":
                        condition.nodeWallTiles.Add(pTarget.currentTile);
                        condition.nodeDirections.Add(0);
                        break;
                }
            }
            List<WorldTile> tiles = ReflectionUtility.Reflection.GetField(typeof(Building), (Building)pTarget, "tiles") as List<WorldTile>;
            foreach (WorldTile tile in tiles)
            {
                tile.setTileType(soil_high);
                tile.setTopTileType(grass_high);
            }
            return true;
        }
    }
}
