using System.Collections.Generic;

namespace Cultivation_Way
{
    class EasternDragonAsset
    {
        public EasternDragonAssetContainer[] list;

        //每个动作对应一个动画容器
        public Dictionary<int, EasternDragonAssetContainer> dict;

        public EasternDragonAssetContainer getAsset(EasternDragonState pState)
        {
            if (this.dict == null)
            {
                this.dict = new Dictionary<int, EasternDragonAssetContainer>();
                foreach (EasternDragonAssetContainer easternDragonAssetContainer in this.list)
                {
                    int id = getID(easternDragonAssetContainer.id);
                    this.dict.Add(id, easternDragonAssetContainer);
                }
            }
            return this.dict[getID(pState)];
        }
        private int getID(EasternDragonState pState)
        {
            return getValue(pState.shape) + getValue(pState.actionState);
        }
        private int getValue(EasternDragonState.Shape shape)
        {
            switch (shape)
            {
                case EasternDragonState.Shape.Human:
                    return 0;
                case EasternDragonState.Shape.Dragon:
                    return 10;
            }
            return 0;
        }
        private int getValue(EasternDragonState.ActionState actionState)
        {
            switch (actionState)
            {
                case EasternDragonState.ActionState.Stop:
                    return 0;
                case EasternDragonState.ActionState.Move:
                    return 1;
                case EasternDragonState.ActionState.Attack:
                    return 2;
                case EasternDragonState.ActionState.Spell:
                    return 3;
                case EasternDragonState.ActionState.Up:
                    return 4;
                case EasternDragonState.ActionState.Landing:
                    return 5;
                case EasternDragonState.ActionState.Death:
                    return 6;
            }
            return 0;
        }
    }
}
