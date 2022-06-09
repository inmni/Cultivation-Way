using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.MoreAiBehaviours
{
    class BehaviourTaskCityLibrary
    {
        internal static CityBehBuildWall buildWall;
        public void init()
        {
            BehaviourTaskCity _buildWall = new BehaviourTaskCity
            {
                id = "build_walls"
            };
            _buildWall.addBeh(new CityBehBuildWall());
            AssetManager.tasks_city.add(_buildWall);
            AssetManager.job_city.get("city").addTask("build_walls");
            buildWall = (CityBehBuildWall)_buildWall.get(0);
        }
    }
}
