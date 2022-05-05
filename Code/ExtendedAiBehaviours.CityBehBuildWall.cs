using ai.behaviours;
using System.Collections.Generic;
using ReflectionUtility;
using UnityEngine;
using System.Text;
using CultivationWay;
using System;

namespace Cultivation_Way.ExtendedAiBehaviours
{
    class CityWallCondition
    {
        internal bool isBuilding = false;
        internal bool done = false;
        internal int level = 0;
        internal string raceID = "";
        internal BuildingAsset horiWall;
        internal BuildingAsset vertWall;
        internal BuildingAsset node1;
        internal BuildingAsset node2;
        internal BuildingAsset horiGate1;
        internal BuildingAsset horiGate2;
        internal BuildingAsset vertGate1;
        internal BuildingAsset vertGate2;
        internal List<TileZone> cityZones;
        internal List<TileZone> zones = new List<TileZone>();
        internal List<WorldTile> horiWallTiles = new List<WorldTile>(4);
        internal List<WorldTile> vertWallTiles = new List<WorldTile>(4);
        internal List<WorldTile> nodeWallTiles = new List<WorldTile>(2);
        internal List<WorldTile> gateTiles = new List<WorldTile>(2);
        internal List<int> gateDirections = new List<int>(2);
    }
    class CityBehBuildWall:BehaviourActionCity
    {
        private static int instanceID;
        private static Vector2Int[] dirs = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
        private static Vector2Int[] dirAll = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down,new Vector2Int(1,1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1) };
        private Dictionary<int, CityWallCondition> conditions = new Dictionary<int, CityWallCondition>();
        
        public override BehResult execute(City pCity)
        {
            instanceID = pCity.GetInstanceID();
            if (!conditions.ContainsKey(instanceID))
            {
                conditions[instanceID] = new CityWallCondition();
            }
            //若已完成则跳过
            if (conditions[instanceID].done)
            {
                return BehResult.Continue;
            }
            if (conditions[instanceID].isBuilding)
            {
                //若正在建造，则继续建造
                conditions[instanceID].isBuilding = !buildWalls(pCity);
                if (conditions[instanceID].isBuilding)
                {
                    return BehResult.Continue;
                }
                else
                {
                    conditions[instanceID].done = true;
                    return BehResult.Continue;
                }
            }
            else
            {
                //若不在建造，则判断是否可以建造
                Culture culture = pCity.getCulture();
                if (culture == null || !culture.list_tech_ids.Exists(x => x.StartsWith("Circumvallation")))
                {
                    return BehResult.Continue;
                }
                int temp;
                bool toBuild = false;
                //寻找第一个等级高于当前城墙等级的城墙科技
                foreach(string tech in culture.list_tech_ids)
                {
                    if (tech.StartsWith("Circumvallation"))
                    {
                        temp = tech[tech.Length - 1] - '0';
                        if (temp > conditions[instanceID].level)
                        {
                            conditions[instanceID].level = temp;
                            toBuild = true;
                            break;
                        }
                    }
                }
                //若找不到，则跳过
                if (!toBuild)
                {
                    return BehResult.Continue;
                }
                //获取位置
                getTiles(pCity);
                if (conditions[instanceID].raceID == string.Empty)
                {
                    conditions[instanceID].raceID = (Reflection.GetField(typeof(City), pCity, "race") as Race).id;
                }
                //设定建造类型
                conditions[instanceID].horiWall = AssetManager.buildings.get($"Circumvallation_hori_{conditions[instanceID].level}_{conditions[instanceID].raceID}");
                conditions[instanceID].vertWall = AssetManager.buildings.get($"Circumvallation_vert_{conditions[instanceID].level}_{conditions[instanceID].raceID}");
                conditions[instanceID].node1 = AssetManager.buildings.get($"Circumvallation_node1_{conditions[instanceID].level}_{conditions[instanceID].raceID}");
                conditions[instanceID].node2 = AssetManager.buildings.get($"Circumvallation_node2_{conditions[instanceID].level}_{conditions[instanceID].raceID}");
                conditions[instanceID].horiGate1 = AssetManager.buildings.get($"Circumvallation_horiGate1_{conditions[instanceID].level}_{conditions[instanceID].raceID}");
                conditions[instanceID].horiGate2 = AssetManager.buildings.get($"Circumvallation_horiGate2_{conditions[instanceID].level}_{conditions[instanceID].raceID}");
                conditions[instanceID].vertGate1 = AssetManager.buildings.get($"Circumvallation_vertGate1_{conditions[instanceID].level}_{conditions[instanceID].raceID}");
                conditions[instanceID].vertGate2 = AssetManager.buildings.get($"Circumvallation_vertGate2_{conditions[instanceID].level}_{conditions[instanceID].raceID}");
                if (conditions[instanceID].horiWallTiles.Count == 0 || conditions[instanceID].vertWallTiles.Count == 0 || conditions[instanceID].nodeWallTiles.Count == 0 || conditions[instanceID].gateTiles.Count == 0)
                {
                    return BehResult.Continue;
                }
                conditions[instanceID].isBuilding = true;
                return BehResult.RepeatStep;
            }
        }
        #region 建造城墙
        /// <summary>
        /// 建造城墙，返回所有部件是否完成建造
        /// </summary>
        /// <param name="pCity"></param>
        private bool buildWalls(City pCity)
        {
            ////若有建筑正在建造，则跳过
            //if(Reflection.GetField(typeof(City),pCity, "underConstructionBuilding") != null)
            //{
            //    return false;
            //}
            //按照节点、横向城墙、纵向城墙、城门顺序建造
            if (conditions[instanceID].nodeWallTiles.Count > 0)
            {
                buildNode(pCity, conditions[instanceID].nodeWallTiles[0]);
                conditions[instanceID].nodeWallTiles.RemoveAt(0);
            }
            else if (conditions[instanceID].vertWallTiles.Count > 0)
            {
                buildMain(pCity, conditions[instanceID].vertWallTiles[0], conditions[instanceID].vertWall);
                conditions[instanceID].vertWallTiles.RemoveAt(0);
            }
            else if (conditions[instanceID].horiWallTiles.Count > 0)
            {
                buildMain(pCity, conditions[instanceID].horiWallTiles[0], conditions[instanceID].horiWall);
                conditions[instanceID].horiWallTiles.RemoveAt(0);
            }
            else if (conditions[instanceID].gateTiles.Count>0)
            {
                buildGate(pCity, conditions[instanceID].gateTiles[0], conditions[instanceID].gateDirections[0]);
                conditions[instanceID].gateTiles.Clear();
                conditions[instanceID].gateDirections.Clear();
            }
            else
            {
                return true;
            }
            return false;
        }
        private void buildMain(City pCity,WorldTile tile,BuildingAsset buildingAsset)
        {
            build(pCity, tile, buildingAsset);
        }
        private void buildNode(City pCity, WorldTile tile)
        {
            //需要确定此处是北面还是南面
            if(conditions[instanceID].vertWallTiles.Contains(tile.tile_down.tile_down))
            {
                build(pCity, tile, conditions[instanceID].node1);
            }
            else
            {
                build(pCity, tile, conditions[instanceID].node2);
            }
        }
        private void buildGate(City pCity, WorldTile tile,int direction)
        {

            if (direction==2)
            {
                build(pCity, tile, conditions[instanceID].horiGate2);
            }
            else if(direction==3)
            {
                build(pCity, tile, conditions[instanceID].horiGate1);
            }
            else if (direction==0)
            {
                build(pCity, tile, conditions[instanceID].vertGate2);
            }
            else if (direction==1)
            {
                build(pCity, tile, conditions[instanceID].vertGate1);
            }
        }
        private void build(City pCity, WorldTile tile, BuildingAsset buildingAsset)
        {
            bool canBuild = false;
            foreach(WorldTile neighbour in tile.neighboursAll)
            {
                canBuild |= neighbour.canBuildOn(buildingAsset);
            }
            if (!canBuild)
            {
                return;
            }
            Building building = MapBox.instance.CallMethod("addBuilding", buildingAsset.id, tile,null,false,false,BuildPlacingType.New) as Building;
            pCity.addBuilding(building);
            Reflection.SetField(pCity, "underConstructionBuilding", building);
            building.GetData().underConstruction = true;
            building.setSpriteUnderConstruction();
            pCity.CallMethod("spendResourcesFor", buildingAsset.cost);
            Sfx.play("constructing", pRestart: true, building.transform.localPosition.x, building.transform.localPosition.y);
        }
        #endregion
        #region 获取城墙位置
        /// <summary>
        /// 获取城墙位置
        /// </summary>
        /// <param name="pCity"></param>
        private void getTiles(City pCity)
        {
            if (conditions[instanceID].cityZones == null)
            {
                conditions[instanceID].cityZones = Reflection.GetField(typeof(City), pCity, "zones") as List<TileZone>;
            }
            int flag;
            for(int i=0;i<conditions[instanceID].cityZones.Count;i++)
            {
                flag = 0;
                foreach (Vector2Int dir in dirs)
                {
                    if (Main.instance.zoneCalculator.getZone(conditions[instanceID].cityZones[i].x + dir.x, conditions[instanceID].cityZones[i].y + dir.y) == null 
                        || Main.instance.zoneCalculator.getZone(conditions[instanceID].cityZones[i].x + dir.x, conditions[instanceID].cityZones[i].y + dir.y).city != conditions[instanceID].cityZones[i].city)
                    {
                        flag++;
                    }
                }
                if (flag == 3)
                {
                    continue;
                }
                flag = 0;
                foreach (Vector2Int dir in dirAll)
                {
                    if (Main.instance.zoneCalculator.getZone(conditions[instanceID].cityZones[i].x + dir.x, conditions[instanceID].cityZones[i].y + dir.y) == null
                        || Main.instance.zoneCalculator.getZone(conditions[instanceID].cityZones[i].x + dir.x, conditions[instanceID].cityZones[i].y + dir.y).city != conditions[instanceID].cityZones[i].city)
                    {
                        flag++;
                    }
                }
                if (flag != 0)
                {
                    conditions[instanceID].zones.Add(conditions[instanceID].cityZones[i]);
                }
            }
            conditions[instanceID].horiWallTiles.Clear();
            conditions[instanceID].vertWallTiles.Clear();
            conditions[instanceID].nodeWallTiles.Clear();
            conditions[instanceID].gateTiles.Clear();
            getHoriTiles();
            getVertTiles();
            getNodeTiles();
            getGateTiles();
            printALL();
        }
        private void printALL()
        {
            MonoBehaviour.print("horiWall");
            StringBuilder line = new StringBuilder();
            foreach(WorldTile tile in conditions[instanceID].horiWallTiles)
            {
                line.Append($"({tile.x},{tile.y})");
            }
            MonoBehaviour.print(line.ToString());

            MonoBehaviour.print("vertWall");
            line.Clear();
            foreach (WorldTile tile in conditions[instanceID].vertWallTiles)
            {
                line.Append($"({tile.x},{tile.y})");
            }
            MonoBehaviour.print(line.ToString());

            MonoBehaviour.print("node");
            line.Clear();
            foreach (WorldTile tile in conditions[instanceID].nodeWallTiles)
            {
                line.Append($"({tile.x},{tile.y})");
            }
            MonoBehaviour.print(line.ToString());

            MonoBehaviour.print("gate");
            line.Clear();
            foreach (WorldTile tile in conditions[instanceID].gateTiles)
            {
                line.Append($"({tile.x},{tile.y})");
            }
            MonoBehaviour.print(line.ToString());

        }
        private void getHoriTiles()
        {
            int flag;
            foreach(TileZone zone in conditions[instanceID].zones)
            {
                int x = zone.x << 3;
                int y = zone.y << 3;
                if (Main.instance.zoneCalculator.getZone(zone.x,zone.y-1)==null || conditions[instanceID].zones.Contains(Main.instance.zoneCalculator.getZone(zone.x, zone.y - 1)))
                {
                    int maxX=8;
                    if(Main.instance.zoneCalculator.getZone(zone.x+1, zone.y) == null|| conditions[instanceID].zones.Contains(Main.instance.zoneCalculator.getZone(zone.x+1, zone.y)))
                    {
                        maxX = 7;
                    }
                    for (int i = 0; i < maxX; i++)
                    {
                        conditions[instanceID].horiWallTiles.Add(world.GetTile(i + x, y+1));
                    }
                }
                if (Main.instance.zoneCalculator.getZone(zone.x, zone.y + 1) == null|| conditions[instanceID].zones.Contains(Main.instance.zoneCalculator.getZone(zone.x, zone.y + 1)))
                {
                    int maxX = 8;
                    if (Main.instance.zoneCalculator.getZone(zone.x + 1, zone.y) == null || conditions[instanceID].zones.Contains(Main.instance.zoneCalculator.getZone(zone.x+1, zone.y)))
                    {
                        maxX = 7;
                    }
                    for (int i = 0; i < maxX; i++)
                    {
                        conditions[instanceID].horiWallTiles.Add(world.GetTile(i + x, y+7));
                    }
                }
            }
        }
        private void getVertTiles()
        {
            int flag;
            foreach (TileZone zone in conditions[instanceID].zones)
            {
                if (Main.instance.zoneCalculator.getZone(zone.x-1, zone.y) == null|| conditions[instanceID].zones.Contains(Main.instance.zoneCalculator.getZone(zone.x-1, zone.y)))
                {
                    int x = zone.x << 3;
                    int y = zone.y << 3;
                    int minY = 0;
                    if (Main.instance.zoneCalculator.getZone(zone.x, zone.y-1) == null || Main.instance.zoneCalculator.getZone(zone.x, zone.y-1).city != zone.city)
                    {
                        minY = 1;
                    }
                    for (int i = minY; i < 8; i++)
                    {
                        conditions[instanceID].vertWallTiles.Add(world.GetTile(x, y + i));
                    }
                }
                if (Main.instance.zoneCalculator.getZone(zone.x+1, zone.y) == null|| Main.instance.zoneCalculator.getZone(zone.x + 1, zone.y).city != zone.city)
                {
                    int x = zone.x << 3;
                    int y = zone.y << 3;
                    int minY = 0;
                    if (Main.instance.zoneCalculator.getZone(zone.x, zone.y - 1) == null || Main.instance.zoneCalculator.getZone(zone.x, zone.y - 1).city != zone.city)
                    {
                        minY = 1;
                    }
                    for (int i = minY; i < 8; i++)
                    {
                        conditions[instanceID].vertWallTiles.Add(world.GetTile(x+6, y + i));
                    }
                }
            }
        }
        private void getNodeTiles()
        {
            int removeIndex = 0;
            int flag;
            foreach (TileZone zone in conditions[instanceID].zones)
            {
                int x = zone.x << 3;
                int y = zone.y << 3;
                if (!zone.world_edge)
                {
                    if (zone.neighboursAll[4].city != zone.city && zone.neighboursAll[0].city == zone.neighboursAll[2].city)
                    {
                        if (zone.neighbours[2].city != zone.city)
                        {
                            removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x, y + 1));
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x, y + 1));
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                        }
                        conditions[instanceID].nodeWallTiles.Add(world.GetTile(x, y + 1));
                    }
                    if (zone.neighboursAll[5].city != zone.city && zone.neighboursAll[0].city == zone.neighboursAll[3].city)
                    {
                        if (zone.neighbours[0].city != zone.city)
                        {
                            removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x, y + 6));
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x, y + 7));
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                        }
                        conditions[instanceID].nodeWallTiles.Add(world.GetTile(x, y + 7));
                    }
                    if (zone.neighboursAll[6].city != zone.city && zone.neighboursAll[1].city == zone.neighboursAll[2].city)
                    {
                        if (zone.neighbours[1].city != zone.city)
                        {
                            removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x + 6, y + 1));
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x + 6, y + 1));
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                        }
                        conditions[instanceID].nodeWallTiles.Add(world.GetTile(x + 6, y + 1));
                    }
                    if (zone.neighboursAll[7].city != zone.city && zone.neighboursAll[1].city == zone.neighboursAll[3].city)
                    {
                        if (zone.neighbours[1].city != zone.city)
                        {
                            removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x + 6, y + 6));
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x + 6, y + 7));
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                        }
                        conditions[instanceID].nodeWallTiles.Add(world.GetTile(x + 6, y + 6));
                    }
                }
                else
                {
                    if (Main.instance.zoneCalculator.getZone(zone.x-1,zone.y)==null)
                    {
                        if (Main.instance.zoneCalculator.getZone(zone.x, zone.y-1) == null|| Main.instance.zoneCalculator.getZone(zone.x, zone.y - 1).city != zone.city)
                        {
                            removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x, y + 1));
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x, y + 1));
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].nodeWallTiles.Add(world.GetTile(x, y + 1));
                        }
                        if (Main.instance.zoneCalculator.getZone(zone.x, zone.y + 1) == null || Main.instance.zoneCalculator.getZone(zone.x, zone.y + 1).city != zone.city)
                        {
                            removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x, y + 1));
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x, y + 1));
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].nodeWallTiles.Add(world.GetTile(x, y + 7));
                        }
                    }
                    if (Main.instance.zoneCalculator.getZone(zone.x + 1, zone.y) == null)
                    {
                        if (Main.instance.zoneCalculator.getZone(zone.x, zone.y - 1) == null || Main.instance.zoneCalculator.getZone(zone.x, zone.y - 1).city != zone.city)
                        {
                            removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x + 6, y + 1));
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x + 6, y + 1));
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].nodeWallTiles.Add(world.GetTile(x + 6, y + 1));
                        }
                        if (Main.instance.zoneCalculator.getZone(zone.x, zone.y + 1) == null || Main.instance.zoneCalculator.getZone(zone.x, zone.y + 1).city != zone.city)
                        {
                            removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x + 6, y + 6));
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x + 6, y + 7));
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].nodeWallTiles.Add(world.GetTile(x + 6, y + 7));
                        }
                    }
                    if (Main.instance.zoneCalculator.getZone(zone.x,zone.y-1)==null)
                    {
                        if (Main.instance.zoneCalculator.getZone(zone.x-1, zone.y) != null && Main.instance.zoneCalculator.getZone(zone.x-1, zone.y).city != zone.city)
                        {
                            removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x, y + 1));
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x, y + 1));
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].nodeWallTiles.Add(world.GetTile(x, y + 1));
                        }
                        if (Main.instance.zoneCalculator.getZone(zone.x+1, zone.y) != null && Main.instance.zoneCalculator.getZone(zone.x+1, zone.y).city != zone.city)
                        {
                            removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x + 6, y + 1));
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x + 6, y + 1));
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].nodeWallTiles.Add(world.GetTile(x + 6, y + 1));
                        }
                    }
                    if (Main.instance.zoneCalculator.getZone(zone.x, zone.y + 1) == null)
                    {
                        if (Main.instance.zoneCalculator.getZone(zone.x - 1, zone.y) != null && Main.instance.zoneCalculator.getZone(zone.x - 1, zone.y).city != zone.city)
                        {
                            removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x, y + 6));
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x, y + 7));
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].nodeWallTiles.Add(world.GetTile(x, y + 7));
                        }
                        if (Main.instance.zoneCalculator.getZone(zone.x + 1, zone.y) != null && Main.instance.zoneCalculator.getZone(zone.x + 1, zone.y).city != zone.city)
                        {
                            removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x + 6, y + 6));
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                            removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x + 6, y + 7));
                            conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                            conditions[instanceID].nodeWallTiles.Add(world.GetTile(x + 6, y + 7));
                        }
                    }
                }
            }
        }
        private void getGateTiles()
        {
            //此处int类型为四+n位二进制，前四位表示优先方向，后n位表示该节点在优先方向上的长度
            Dictionary<WorldTile,int> possibleNode = new Dictionary<WorldTile, int>();
            int store;
            int maxLength;
            int tempLength;
            foreach (WorldTile node in conditions[instanceID].nodeWallTiles)
            {
                TileZone node_zone = node.chunk.zone;
                store = 0;
                maxLength = 2;
                tempLength = 0;
                if ((tempLength=getMaxLengthIn(node_zone, 1, 0)) > maxLength)
                {
                    maxLength = tempLength;
                    store = 8;
                }
                if((tempLength = getMaxLengthIn(node_zone, -1, 0)) > maxLength)
                {
                    maxLength = tempLength;
                    store = 4;
                }
                if ((tempLength = getMaxLengthIn(node_zone, 0, 1)) > maxLength)
                {
                    maxLength = tempLength;
                    store = 2;
                }
                if ((tempLength = getMaxLengthIn(node_zone, 0, -1)) > maxLength)
                {
                    maxLength = tempLength;
                    store = 1;
                }
                store += maxLength << 4;
                if (maxLength > 1)
                {
                    possibleNode.Add(node, store);
                }
            }
            WorldTile maxTile = null;
            maxLength=3;
            foreach(WorldTile tile in possibleNode.Keys)
            {
                if (possibleNode[tile]>>4 >= maxLength)
                {
                    maxTile=tile;
                    maxLength = possibleNode[tile]>>4;
                }
            }
            try { 
                getCenterTile(maxTile, possibleNode[maxTile]);//存在多个城门位置
            }
            catch (Exception e)
            {
                MonoBehaviour.print(maxTile.pos);
            }
        }
        #endregion

        #region 部分工具
        /// <summary>
        /// 获取在该方向上的最大长度
        /// </summary>
        /// <param name="zone"></param>
        /// <param name="xOffset"></param>
        /// <param name="yOffset"></param>
        /// <returns></returns>
        private int getMaxLengthIn(TileZone zone, int xOffset = 0, int yOffset = 0)
        {
            int length = 1;
            int checkCount = 0;
            int x = zone.x;
            int y = zone.y;
            int maxX = Config.ZONE_AMOUNT_X * 8;
            int maxY = Config.ZONE_AMOUNT_Y * 8;
            TileZone nextZone = zone;
            if(Main.instance.zoneCalculator.getZone(zone.x-yOffset,zone.y-xOffset)==null^Main.instance.zoneCalculator.getZone(zone.x+yOffset,zone.y+xOffset)==null)
            {
                return 0;
            }
            while (x >= 0 && y >= 0 && x < maxX && y < maxY)
            {
                x += xOffset;
                y += yOffset;
                checkCount = 0;
                nextZone = Main.instance.zoneCalculator.getZone(x, y);
                if (nextZone==null||nextZone.city != zone.city)
                {
                    break;
                }
                length++;
                foreach(TileZone neighour in nextZone.neighbours)
                {
                    if(neighour.city == zone.city)
                    {
                        checkCount++;
                    }
                }
                if (checkCount == 4)
                {
                    break;
                }
            }
            return length;
        }
        /// <summary>
        /// 获取带方向矢量的中点
        /// </summary>
        /// <param name="tile"></param>
        /// <param name="vector"></param>
        /// <returns></returns>
        private void getCenterTile(WorldTile tile, int vector)
        {
            int length = vector >> 4;
            vector -= length<<4;
            int xOffset;
            int yOffset;
            int removeIndex = 0;
            //将vector转化成方向
            switch (vector)
            {
                case 8:
                    xOffset = 1;
                    yOffset = 0;
                    break;
                case 4:
                    xOffset = -1;
                    yOffset = 0;
                    break;
                case 2:
                    xOffset = 0;
                    yOffset = 1;
                    break;
                case 1:
                    xOffset = 0;
                    yOffset = -1;
                    break;
                default:
                    return;
            }

            TileZone centerZone = Main.instance.zoneCalculator.getZone(tile.zone.x + xOffset * (length >> 1), tile.zone.y + yOffset * (length >> 1));
            int x = centerZone.x << 3;
            int y = centerZone.y << 3;
            int xOffset1 =0;
            int yOffset1 =0;
            if(Main.instance.zoneCalculator.getZone(centerZone.x+1,centerZone.y)==null|| Main.instance.zoneCalculator.getZone(centerZone.x + 1, centerZone.y).city != centerZone.city)
            {
                xOffset1 = -1;
            }
            if (Main.instance.zoneCalculator.getZone(centerZone.x, centerZone.y-1) == null || Main.instance.zoneCalculator.getZone(centerZone.x, centerZone.y-1).city != centerZone.city)
            {
                yOffset1 = -1;
            }
            if (vector == 8 || vector == 4)
            {
                if (Main.instance.zoneCalculator.getZone(centerZone.x, centerZone.y - 1)==null||Main.instance.zoneCalculator.getZone(centerZone.x, centerZone.y-1).city != centerZone.city)
                {
                    removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x + 2, y + yOffset1));
                    conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].gateTiles.Add(world.GetTile(x + 2, y + 1));
                    conditions[instanceID].gateDirections.Add(2);
                }
                else
                {
                    removeIndex = conditions[instanceID].horiWallTiles.IndexOf(world.GetTile(x + 2, y + 7));
                    conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].horiWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].gateTiles.Add(world.GetTile(x + 2, y + 7));
                    conditions[instanceID].gateDirections.Add(3);
                } 
            }
            else if (vector == 2 || vector == 1) 
            { 
                if (Main.instance.zoneCalculator.getZone(centerZone.x - 1, centerZone.y)==null||Main.instance.zoneCalculator.getZone(centerZone.x-1,centerZone.y).city != centerZone.city)
                {
                    removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x, y + 2));
                    conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].gateTiles.Add(world.GetTile(x, y + 5));
                    conditions[instanceID].gateDirections.Add(0);
                }
                else
                {
                    removeIndex = conditions[instanceID].vertWallTiles.IndexOf(world.GetTile(x + 7 + xOffset1, y + 2));
                    conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].vertWallTiles.RemoveAt(removeIndex);
                    conditions[instanceID].gateTiles.Add(world.GetTile(x + 6, y + 5));
                    conditions[instanceID].gateDirections.Add(1);
                }
            }
        }
        #endregion
    }
}
