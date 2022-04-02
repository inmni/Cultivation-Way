using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way
{
    class SpecialActorAsset
    {
        public SpecialActorAssetContainer[] list;

        //每个动作对应一个动画容器
        public Dictionary<int, SpecialActorAssetContainer> dict;

        public SpecialActorAssetContainer getAsset(SpecialActorState pState)
        {
            if (this.dict == null)
            {
                this.dict = new Dictionary<int, SpecialActorAssetContainer>();
                foreach (SpecialActorAssetContainer easternDragonAssetContainer in this.list)
                {
                    int id = getID(easternDragonAssetContainer.id);
                    this.dict.Add(id, easternDragonAssetContainer);
                }
            }
            return this.dict[getID(pState)];
        }
        private int getID(SpecialActorState pState)
        {
            return (int)pState;
        }
    }
}
