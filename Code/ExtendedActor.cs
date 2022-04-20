using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using ReflectionUtility;
namespace Cultivation_Way
{
    class ExtendedActor : Actor
    {
        public MoreStats extendedCurStats = new MoreStats();
        public MoreData extendedData = new MoreData();
        public List<BaseSimObject> compositions = new List<BaseSimObject>();
    }
}
