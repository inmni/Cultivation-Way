using System.Collections.Generic;

namespace Cultivation_Way
{
    internal class SpecialActorAsset
    {
        public SpecialActorAssetContainer[] list;

        //每个动作对应一个动画容器
        public Dictionary<int, SpecialActorAssetContainer> dict;

        public SpecialActorAssetContainer getAsset(SpecialActorState pState)
        {
            if (dict == null)
            {
                dict = new Dictionary<int, SpecialActorAssetContainer>();
                foreach (SpecialActorAssetContainer easternDragonAssetContainer in list)
                {
                    int id = getID(easternDragonAssetContainer.id);
                    dict.Add(id, easternDragonAssetContainer);
                }
            }
            return dict[getID(pState)];
        }
        private int getID(SpecialActorState pState)
        {
            return (int)pState;
        }
    }
}
