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
        public List<BaseSimObject> compositions = new List<BaseSimObject>();
    }
}
