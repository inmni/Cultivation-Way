using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ai.behaviours;
namespace Cultivation_Way
{
    class CityBehBuildWall:BehaviourActionCity
    {
        private List<WorldTile> horiWallTiles = new List<WorldTile>();
        private List<WorldTile> vertWallTiles = new List<WorldTile>();
        private List<WorldTile> nodeWallTiles = new List<WorldTile>();
        private List<WorldTile> gateTiles = new List<WorldTile>();
        public override BehResult execute(City pCity)
        {
            getTiles(pCity);
            if (horiWallTiles.Count == 0 || vertWallTiles.Count == 0 || nodeWallTiles.Count == 0 || gateTiles.Count == 0)
            {
                return BehResult.Continue;
            }
            buildWalls(pCity);
            return BehResult.Continue;
        }
        #region 建造城墙
        /// <summary>
        /// 建造城墙
        /// </summary>
        /// <param name="pCity"></param>
        private void buildWalls(City pCity)
        {
            buildNode(pCity);
            buildMain(pCity);
            buildGate(pCity);
        }
        private void buildMain(City pCity)
        {

        }
        private void buildNode(City pCity)
        {

        }
        private void buildGate(City pCity)
        {

        }
        #endregion
        #region 获取城墙位置
        /// <summary>
        /// 获取城墙位置
        /// </summary>
        /// <param name="pCity"></param>
        private void getTiles(City pCity)
        {
            getHoriTiles(pCity);
            getVertTiles(pCity);
            getNodeTiles(pCity);
            getGateTiles(pCity);
        }
        private void getHoriTiles(City pCity)
        {
            
        }
        private void getVertTiles(City pCity)
        {

        }
        private void getNodeTiles(City pCity)
        {

        }
        private void getGateTiles(City pCity)
        {

        }
        #endregion
    }
}
