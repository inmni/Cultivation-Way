using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{

    class ExtendedBuilding:Building
    {
        internal MoreStats extendedCurStats = new MoreStats();
        internal MoreData extendedData = new MoreData();
        public BuildingData easyData;
        public BuildingAsset easyStats;
        public List<BaseSimObject> compositions = new List<BaseSimObject>();
    }
    class ExtendedBuildingStats
    {
        private static TileType soil_high = AssetManager.tiles.get("soil_high");
        private static TopTileType grass_high = AssetManager.topTiles.get("grass_high");
        internal static Dictionary<string, WorldAction> destroy_actions = new Dictionary<string, WorldAction>();
        internal static void init()
        {
            destroy_actions.Add("Circumvallation",resumeTiles);
        }
        public static WorldAction GetWorldAction(string pID)
        {
            try
            {
                return destroy_actions[pID];
            }
            catch (KeyNotFoundException)
            {
                foreach(string key in destroy_actions.Keys)
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
            List<WorldTile> tiles = ReflectionUtility.Reflection.GetField(typeof(Building), (Building)pTarget, "tiles") as List<WorldTile>;
            foreach(WorldTile tile in tiles)
            {
                tile.setTileType(soil_high);
                tile.setTopTileType(grass_high);
            }
            return true;
        }
    }
}
