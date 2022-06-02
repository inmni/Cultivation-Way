using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.MoreAiBehaviours
{
    class BehaviourTaskCityLibrary
    {
        public void init()
        {
            BehaviourTaskCity buildWall = new BehaviourTaskCity
            {
                id = "build_walls"
            };
            buildWall.addBeh(new CityBehBuildWall());
            AssetManager.tasks_city.add(buildWall);
            AssetManager.job_city.get("city").addTask("build_walls");
        }
    }
}
