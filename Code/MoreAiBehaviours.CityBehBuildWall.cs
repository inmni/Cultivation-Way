using ai.behaviours;
using CultivationWay;
using ReflectionUtility;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace Cultivation_Way.MoreAiBehaviours
{
    internal class CityWallCondition
    {
        internal bool isBuilding = false;
        internal bool done = false;
        internal bool slow = false;
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
        internal List<WorldTile> nodeWallTiles = new List<WorldTile>(16);
        internal List<int> nodeDirections = new List<int>(16);
        internal List<WorldTile> gateTiles = new List<WorldTile>(16);
        internal List<int> gateDirections = new List<int>(16);
    }

    internal class CityBehBuildWall : BehaviourActionCity
    {
        private static int cityKey;
        //private static int wallCount;
        private static TileType tileType = AssetManager.tiles.get("mountains");
        private static TopTileType topTileType = AssetManager.topTiles.get("wall_base");
        private static Vector2Int[] dirs = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down };
        private static Vector2Int[] dirAll = new Vector2Int[] { Vector2Int.left, Vector2Int.right, Vector2Int.up, Vector2Int.down, new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, 1), new Vector2Int(-1, -1) };
        internal Dictionary<int, CityWallCondition> conditions = new Dictionary<int, CityWallCondition>();

        public override BehResult execute(City pCity)
        {
            if (!pCity.isCapitalCity())
            {
                return BehResult.Continue;
            }
            cityKey = pCity.GetHashCode();
            if (!conditions.ContainsKey(cityKey))
            {
                conditions[cityKey] = new CityWallCondition();
            }
            //若已完成则跳过
            if (conditions[cityKey].done)
            {
                return BehResult.Continue;
            }
            if (conditions[cityKey].isBuilding)
            {
                //若正在建造，则继续建造
                conditions[cityKey].isBuilding = !buildWalls(pCity);
                if (conditions[cityKey].isBuilding)
                {
                    if (conditions[cityKey].slow)
                    {
                        return BehResult.Continue;
                    }
                    return BehResult.RepeatStep;
                }
                else
                {
                    conditions[cityKey].done = true;
                    return BehResult.Continue;
                }
            }
            else
            {
                //若不在建造，则判断是否可以建造
                conditions[cityKey].slow = false;
                Culture culture = pCity.getCulture();
                if (culture == null || !culture.list_tech_ids.Exists(x => x.StartsWith("Circumvallation")))
                {
                    return BehResult.Continue;
                }
                int temp;
                bool toBuild = false;
                //寻找第一个等级高于当前城墙等级的城墙科技
                foreach (string tech in culture.list_tech_ids)
                {
                    if (tech.StartsWith("Circumvallation"))
                    {
                        temp = tech[tech.Length - 1] - '0';
                        if (temp > conditions[cityKey].level)
                        {
                            conditions[cityKey].level = temp;
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
                if (conditions[cityKey].raceID == string.Empty)
                {
                    conditions[cityKey].raceID = (Reflection.GetField(typeof(City), pCity, "race") as Race).id;
                }
                //设定建造类型
                conditions[cityKey].horiWall = AssetManager.buildings.get($"{conditions[cityKey].level}Circumvallation_hori_{conditions[cityKey].raceID}");
                conditions[cityKey].vertWall = AssetManager.buildings.get($"{conditions[cityKey].level}Circumvallation_vert_{conditions[cityKey].raceID}");
                conditions[cityKey].node1 = AssetManager.buildings.get($"{conditions[cityKey].level}Circumvallation_node1_{conditions[cityKey].raceID}");
                conditions[cityKey].node2 = AssetManager.buildings.get($"{conditions[cityKey].level}Circumvallation_node2_{conditions[cityKey].raceID}");
                conditions[cityKey].horiGate1 = AssetManager.buildings.get($"{conditions[cityKey].level}Circumvallation_horiGate1_{conditions[cityKey].raceID}");
                conditions[cityKey].horiGate2 = AssetManager.buildings.get($"{conditions[cityKey].level}Circumvallation_horiGate2_{conditions[cityKey].raceID}");
                conditions[cityKey].vertGate1 = AssetManager.buildings.get($"{conditions[cityKey].level}Circumvallation_vertGate1_{conditions[cityKey].raceID}");
                conditions[cityKey].vertGate2 = AssetManager.buildings.get($"{conditions[cityKey].level}Circumvallation_vertGate2_{conditions[cityKey].raceID}");
                if (conditions[cityKey].horiWallTiles.Count == 0 || conditions[cityKey].vertWallTiles.Count == 0 || conditions[cityKey].nodeWallTiles.Count == 0 || conditions[cityKey].gateTiles.Count == 0)
                {
                    return BehResult.Continue;
                }
                conditions[cityKey].isBuilding = true;
                return BehResult.Continue;
            }
        }
        #region 建造城墙
        /// <summary>
        /// 建造城墙，返回所有部件是否完成建造
        /// </summary>
        /// <param name="pCity"></param>
        private bool buildWalls(City pCity)
        {
            //若有建筑正在建造，则跳过
            if (Reflection.GetField(typeof(City), pCity, "underConstructionBuilding") != null && conditions[cityKey].slow)
            {
                return false;
            }
            //按照节点、横向城墙、纵向城墙、城门顺序建造
            if (conditions[cityKey].nodeWallTiles.Count > 0)
            {
                buildNode(pCity, conditions[cityKey].nodeWallTiles[0], conditions[cityKey].nodeDirections[0]);
                conditions[cityKey].nodeWallTiles.RemoveAt(0);
                conditions[cityKey].nodeDirections.RemoveAt(0);
            }
            else if (conditions[cityKey].vertWallTiles.Count > 0)
            {
                buildMain(pCity, conditions[cityKey].vertWallTiles[0], conditions[cityKey].vertWall);
                conditions[cityKey].vertWallTiles.RemoveAt(0);
            }
            else if (conditions[cityKey].horiWallTiles.Count > 0)
            {
                if (conditions[cityKey].horiWallTiles.Count <= 1)
                {
                    conditions[cityKey].slow = true;

                }
                buildMain(pCity, conditions[cityKey].horiWallTiles[0], conditions[cityKey].horiWall);
                conditions[cityKey].horiWallTiles.RemoveAt(0);
            }
            else if (conditions[cityKey].gateTiles.Count > 0)
            {
                buildGate(pCity, conditions[cityKey].gateTiles[0], conditions[cityKey].gateDirections[0]);
                conditions[cityKey].gateTiles.Clear();
                conditions[cityKey].gateDirections.Clear();
            }
            else
            {
                return true;
            }
            return false;
        }
        private void buildMain(City pCity, WorldTile tile, BuildingAsset buildingAsset)
        {
            build(pCity, tile, buildingAsset);
        }
        private void buildNode(City pCity, WorldTile tile, int direction)
        {
            //需要确定此处是北面还是南面
            if (direction == 0)
            {
                build(pCity, tile, conditions[cityKey].node2);
            }
            else
            {
                build(pCity, tile, conditions[cityKey].node1);
            }
        }
        private void buildGate(City pCity, WorldTile tile, int direction)
        {

            if (direction == 2)
            {
                build(pCity, tile, conditions[cityKey].horiGate2);
            }
            else if (direction == 3)
            {
                build(pCity, tile, conditions[cityKey].horiGate1);
            }
            else if (direction == 0)
            {
                build(pCity, tile, conditions[cityKey].vertGate1);
            }
            else if (direction == 1)
            {
                build(pCity, tile, conditions[cityKey].vertGate2);
            }
        }
        private void build(City pCity, WorldTile tile, BuildingAsset buildingAsset)
        {
            bool canBuild = false;
            foreach (WorldTile neighbour in tile.neighboursAll)
            {
                canBuild |= neighbour.canBuildOn(buildingAsset);
            }
            if (!canBuild)
            {
                return;
            }

            Building building = MapBox.instance.CallMethod("addBuilding", buildingAsset.id, tile, null, false, false, BuildPlacingType.New) as Building;
            pCity.addBuilding(building);
            if (conditions[cityKey].slow)
            {
                Reflection.SetField(pCity, "underConstructionBuilding", building);
                building.getBuildingData().underConstruction = true;
                building.setSpriteUnderConstruction();
            }
            else
            {
                building.getBuildingData().underConstruction = false;
            }
            if (buildingAsset.fundament.right == 0)
            {
                tile.setTopTileType(topTileType);
                tile.setTileType(tileType);

                //wallCount++;
            }
            else if (!buildingAsset.id.Contains("Gate"))
            {
                //MonoBehaviour.print(buildingAsset.id);
                List<WorldTile> tiles = Reflection.GetField(typeof(Building), building, "tiles") as List<WorldTile>;
                foreach (WorldTile tile1 in tiles)
                {
                    tile1.setTopTileType(topTileType);
                    tile1.setTileType(tileType);
                    //wallCount++;
                }
            }
            //MonoBehaviour.print(wallCount);
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
            if (conditions[cityKey].cityZones == null)
            {
                conditions[cityKey].cityZones = Reflection.GetField(typeof(City), pCity, "zones") as List<TileZone>;
            }
            int flag;
            for (int i = 0; i < conditions[cityKey].cityZones.Count; i++)
            {
                flag = 0;
                foreach (Vector2Int dir in dirs)
                {
                    if (Main.instance.zoneCalculator.getZone(conditions[cityKey].cityZones[i].x + dir.x, conditions[cityKey].cityZones[i].y + dir.y) == null
                        || Main.instance.zoneCalculator.getZone(conditions[cityKey].cityZones[i].x + dir.x, conditions[cityKey].cityZones[i].y + dir.y).city != conditions[cityKey].cityZones[i].city)
                    {
                        flag++;
                    }
                }
                if (flag == 3)
                {
                    continue;
                }
                conditions[cityKey].zones.Add(conditions[cityKey].cityZones[i]);
            }
            conditions[cityKey].horiWallTiles.Clear();
            conditions[cityKey].vertWallTiles.Clear();
            conditions[cityKey].nodeWallTiles.Clear();
            conditions[cityKey].gateTiles.Clear();
            getHoriTiles();
            getVertTiles();
            getNodeTiles();
            getGateTiles();
            //printALL(pCity);
        }
        private void printALL(City pCity)
        {
            MonoBehaviour.print("*************************************");
            MonoBehaviour.print(pCity);
            MonoBehaviour.print("horiWall");
            StringBuilder line = new StringBuilder();
            foreach (WorldTile tile in conditions[cityKey].horiWallTiles)
            {
                line.Append($"({tile.x},{tile.y})");
            }
            MonoBehaviour.print(line.ToString());

            MonoBehaviour.print("vertWall");
            line.Clear();
            foreach (WorldTile tile in conditions[cityKey].vertWallTiles)
            {
                line.Append($"({tile.x},{tile.y})");
            }
            MonoBehaviour.print(line.ToString());

            MonoBehaviour.print("node");
            line.Clear();
            foreach (WorldTile tile in conditions[cityKey].nodeWallTiles)
            {
                line.Append($"({tile.x},{tile.y})");
            }
            MonoBehaviour.print(line.ToString());

            MonoBehaviour.print("gate");
            line.Clear();
            foreach (WorldTile tile in conditions[cityKey].gateTiles)
            {
                line.Append($"({tile.x},{tile.y})");
            }
            MonoBehaviour.print(line.ToString());
            MonoBehaviour.print("*************************************");
        }
        private void getHoriTiles()
        {
            foreach (TileZone zone in conditions[cityKey].zones)
            {
                int x = zone.x << 3;
                int y = zone.y << 3;
                if (Main.instance.zoneCalculator.getZone(zone.x, zone.y - 1) == null || Main.instance.zoneCalculator.getZone(zone.x, zone.y - 1).city != zone.city || !isInZones(zone, 0, -1))
                {
                    int maxX = 8;
                    if (Main.instance.zoneCalculator.getZone(zone.x + 1, zone.y) == null || Main.instance.zoneCalculator.getZone(zone.x + 1, zone.y).city != zone.city || !isInZones(zone, 1, 0))
                    {
                        maxX = 7;
                    }
                    for (int i = 0; i < maxX; i++)
                    {
                        conditions[cityKey].horiWallTiles.Add(world.GetTile(i + x, y + 1));
                    }
                }
                if (Main.instance.zoneCalculator.getZone(zone.x, zone.y + 1) == null || Main.instance.zoneCalculator.getZone(zone.x, zone.y + 1).city != zone.city || !isInZones(zone, 0, 1))
                {
                    int maxX = 8;
                    if (Main.instance.zoneCalculator.getZone(zone.x + 1, zone.y) == null || Main.instance.zoneCalculator.getZone(zone.x + 1, zone.y).city != zone.city || !isInZones(zone, 1, 0))
                    {
                        maxX = 7;
                    }
                    for (int i = 0; i < maxX; i++)
                    {
                        conditions[cityKey].horiWallTiles.Add(world.GetTile(i + x, y + 7));
                    }
                }
            }
        }
        private void getVertTiles()
        {
            foreach (TileZone zone in conditions[cityKey].zones)
            {
                if (!isInZones(zone, -1, 0))
                {
                    int x = zone.x << 3;
                    int y = zone.y << 3;
                    int minY = 0;
                    if (!isInZones(zone, 0, -1))
                    {
                        minY = 1;
                    }
                    for (int i = minY; i < 8; i++)
                    {
                        conditions[cityKey].vertWallTiles.Add(world.GetTile(x, y + i));
                    }
                }
                if (!isInZones(zone, 1, 0))
                {
                    int x = zone.x << 3;
                    int y = zone.y << 3;
                    int minY = 0;
                    if (!isInZones(zone, 0, -1))
                    {
                        minY = 1;
                    }
                    for (int i = minY; i < 8; i++)
                    {
                        conditions[cityKey].vertWallTiles.Add(world.GetTile(x + 6, y + i));
                    }
                }
            }
        }
        private void getNodeTiles()
        {
            int removeIndex;
            bool[] dirInZones = new bool[8] { false, false, false, false, false, false, false, false };
            foreach (TileZone zone in conditions[cityKey].zones)
            {
                int x = zone.x << 3;
                int y = zone.y << 3;
                dirInZones[0] = isInZones(zone, -1, 0);
                dirInZones[1] = isInZones(zone, 1, 0);
                dirInZones[2] = isInZones(zone, 0, -1);
                dirInZones[3] = isInZones(zone, 0, 1);
                dirInZones[4] = isInZones(zone, -1, -1);
                dirInZones[5] = isInZones(zone, -1, 1);
                dirInZones[6] = isInZones(zone, 1, -1);
                dirInZones[7] = isInZones(zone, 1, 1);
                if (!dirInZones[4] && dirInZones[0] == dirInZones[2])//左下
                {
                    if (!dirInZones[0])
                    {
                        removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x, y + 1));
                        conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                        removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x, y + 1));
                        conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                        conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                    }
                    conditions[cityKey].nodeWallTiles.Add(world.GetTile(x, y + 1 - 1));
                    conditions[cityKey].nodeDirections.Add(0);
                }
                if (!dirInZones[5] && dirInZones[0] == dirInZones[3])//左上
                {
                    if (!dirInZones[0])
                    {
                        removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x, y + 6));
                        conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                        conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                        removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x, y + 7));
                        conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                        conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                    }
                    conditions[cityKey].nodeWallTiles.Add(world.GetTile(x, y + 7 - 1));
                    conditions[cityKey].nodeDirections.Add(dirInZones[0] ? 0 : 1);
                }
                if (!dirInZones[6] && dirInZones[1] == dirInZones[2])//右下
                {
                    if (!dirInZones[1])
                    {
                        removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x + 6, y + 1));
                        conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                        removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x + 6, y + 1));
                        conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                    }
                    conditions[cityKey].nodeWallTiles.Add(world.GetTile(x + 6, y + 1 - 1));
                    conditions[cityKey].nodeDirections.Add(0);
                }
                if (!dirInZones[7] && dirInZones[1] == dirInZones[3])//右上
                {
                    if (!dirInZones[1])
                    {
                        removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x + 6, y + 6));
                        conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                        conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                        removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x + 6, y + 7));
                        conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                    }
                    conditions[cityKey].nodeWallTiles.Add(world.GetTile(x + 6, y + 7 - 1));
                    conditions[cityKey].nodeDirections.Add(dirInZones[1] ? 0 : 1);

                }

            }
        }
        private void getGateTiles()
        {
            //此处int类型为四+n位二进制，前四位表示优先方向，后n位表示该节点在优先方向上的长度
            Dictionary<TileZone, int> possibleNode = new Dictionary<TileZone, int>();
            int store;
            int maxLength = 1;
            int tempLength;
            foreach (WorldTile node in conditions[cityKey].nodeWallTiles)
            {
                TileZone node_zone = node.zone;
                store = 0;
                tempLength = 0;
                if ((tempLength = getMaxLengthIn(node_zone, 1, 0)) > maxLength)
                {//横向
                    maxLength = tempLength;
                    store = 8;
                    store += maxLength << 4;
                    possibleNode[node_zone] = store;
                }
                if ((tempLength = getMaxLengthIn(node_zone, 0, 1)) > maxLength)
                {//纵向
                    maxLength = tempLength;
                    store = 2;
                    store += maxLength << 4;
                    possibleNode[node_zone] = store;
                }
            }
            TileZone maxZone = null;
            maxLength = 0;
            foreach (TileZone zone in possibleNode.Keys)
            {
                if (possibleNode[zone] >> 4 >= maxLength >> 4)
                {
                    maxZone = zone;
                    maxLength = possibleNode[zone];
                }
            }
            getCenterTile(maxZone, maxLength);//存在多个城门位置
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
            int checkCount;
            int x = zone.x;
            int y = zone.y;
            int maxX = Config.ZONE_AMOUNT_X * 8;
            int maxY = Config.ZONE_AMOUNT_Y * 8;
            TileZone nextZone;
            if (!(isInZones(zone, -yOffset, -xOffset) ^ isInZones(zone, yOffset, xOffset)) ||
                isOverEdge(zone, -yOffset, -xOffset) || isOverEdge(zone, yOffset, xOffset))
            {//垂直方向上相邻的两个同属于城内或同不属于城内，则返回零
                return 0;
            }
            while (x >= 0 && y >= 0 && x < maxX && y < maxY)
            {
                x += xOffset;
                y += yOffset;
                checkCount = 0;
                nextZone = Main.instance.zoneCalculator.getZone(x, y);
                if (!isInZones(nextZone))
                {
                    break;
                }
                length++;
                foreach (TileZone neighour in nextZone.neighbours)
                {
                    if (isInZones(neighour))
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
        private void getCenterTile(TileZone zone, int vector)
        {
            if (zone == null)
            {
                //此处问题
                throw new Exception($"无法找到出发节点");
            }
            int length = vector >> 4;
            vector -= length << 4;
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
            TileZone centerZone = Main.instance.zoneCalculator.getZone(zone.x + xOffset * (length >> 1), zone.y + yOffset * (length >> 1));
            //MonoBehaviour.print($"center:{centerZone.x},{centerZone.y};vector:{length},{xOffset},{yOffset}");
            int x = centerZone.x << 3;
            int y = centerZone.y << 3;
            int xOffset1 = 0;
            int yOffset1 = 0;
            if (!isInZones(centerZone, 1, 0))
            {
                xOffset1 = -1;
            }
            if (!isInZones(centerZone, 0, -1))
            {
                yOffset1 = 1;
            }
            if (vector == 8 || vector == 4)
            {
                if (!isInZones(centerZone, 0, -1))//南门
                {
                    removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x + 2, y + yOffset1));
                    conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].gateTiles.Add(world.GetTile(x + 2, y + 1 - 1));
                    conditions[cityKey].gateDirections.Add(2);
                }
                else if (!isInZones(centerZone, 0, 1))//北门
                {
                    removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x + 2, y + 7));
                    conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].gateTiles.Add(world.GetTile(x + 2, y + 7 - 1));
                    conditions[cityKey].gateDirections.Add(3);
                }
            }
            else if (vector == 2 || vector == 1)
            {
                if (!isInZones(centerZone, -1, 0))//西门
                {
                    removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x, y + 2));
                    conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].gateTiles.Add(world.GetTile(x, y + 5));
                    conditions[cityKey].gateDirections.Add(0);
                }
                else if (!isInZones(centerZone, 1, 0))//东门
                {
                    removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x + 7 + xOffset1, y + 2));
                    conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
                    conditions[cityKey].gateTiles.Add(world.GetTile(x + 6, y + 5));
                    conditions[cityKey].gateDirections.Add(1);
                }
            }
        }
        private bool isInZones(TileZone zone, int xOffset = 0, int yOffset = 0)
        {
            if (xOffset == 0 && yOffset == 0)
            {
                return zone != null && conditions[cityKey].zones.Contains(zone);
            }
            if (zone == null)
            {
                return false;
            }
            TileZone zone1 = Main.instance.zoneCalculator.getZone(zone.x + xOffset, zone.y + yOffset);
            return zone1 != null && zone1.city == zone.city &&
                conditions[cityKey].zones.Contains(zone1);
        }
        private bool isOverEdge(TileZone zone, int xOffset = 0, int yOffset = 0)
        {
            if (zone == null)
            {
                return false;
            }
            TileZone zone1 = Main.instance.zoneCalculator.getZone(zone.x + xOffset, zone.y + yOffset);
            return zone1 == null;
        }
        #endregion
    }
}
