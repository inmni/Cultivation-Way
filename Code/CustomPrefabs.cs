using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way
{
    class CustomPrefabs
    {
        internal GameObject ExtendedActorPrefab = new GameObject("ExtendedActor");
        internal GameObject ExtendedBuildingPrefab = new GameObject("ExtendedBuilding");
        internal void init()
        {
            initExtendedActor();
            initExtendedBuilding();
        }
        private void initExtendedActor()
        {
            ExtendedActorPrefab.AddComponent<ExtendedActor>();
            ExtendedActorPrefab.GetComponent<ExtendedActor>().gameObject.AddComponent<SpriteRenderer>();
            ExtendedActorPrefab.GetComponent<ExtendedActor>().gameObject.AddComponent<UnitSpriteAnimation>();
            ExtendedActorPrefab.GetComponent<ExtendedActor>().gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
            ExtendedActorPrefab.SetActive(false);
        }
        private void initExtendedBuilding()
        {
            ExtendedBuildingPrefab.AddComponent<ExtendedBuilding>();
            ExtendedBuildingPrefab.GetComponent<ExtendedBuilding>().gameObject.AddComponent<SpriteRenderer>();
            ExtendedBuildingPrefab.GetComponent<ExtendedBuilding>().gameObject.AddComponent<BuildingSpriteAnimation>();
            ExtendedBuildingPrefab.GetComponent<ExtendedBuilding>().gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Objects";
            
            ExtendedBuildingPrefab.SetActive(false);
        }
    }
}
