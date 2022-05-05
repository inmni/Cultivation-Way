namespace Cultivation_Way
{
    class ExtensionSpell
    {
        public string spellAssetID;

        public int cost;//蓝耗

        public int cooldown;//冷却（年
        public int leftCool;//剩余

        public float might;//威力因子

        public bool isDirective;//类型
        public ExtensionSpell()
        {
            spellAssetID = "example";
            ExtensionSpellAsset spellAsset = AddAssetManager.extensionSpellLibrary.get(spellAssetID);
            cost = spellAsset.baseCost;
            cooldown = spellAsset.coolDown;
            leftCool = 0;
            might = spellAsset.might;
        }
        public ExtensionSpell(string spellID)
        {
            spellAssetID = spellID;
            ExtensionSpellAsset spellAsset = AddAssetManager.extensionSpellLibrary.get(spellAssetID);
            cost = spellAsset.baseCost;
            cooldown = spellAsset.coolDown;
            leftCool = 0;
            might = spellAsset.might;
        }
        public ExtensionSpell(ExtensionSpell spell)
        {
            spellAssetID = spell.spellAssetID;
            cost = spell.cost;
            cooldown = spell.cooldown;
            leftCool = 0;
            might = spell.might;
        }
        public ExtensionSpellAsset GetSpellAsset()
        {
            ExtensionSpellAsset result = null;
            if (AddAssetManager.extensionSpellLibrary.dict.TryGetValue(spellAssetID, out result))
            {
                return result;
            }
            else
            {
                spellAssetID = "example";
                return AddAssetManager.extensionSpellLibrary.get(spellAssetID);
            }
        }
        public bool castSpell(BaseSimObject pAttacker, BaseSimObject pTarget = null)
        {
            ExtensionSpellAsset spellAsset = AddAssetManager.extensionSpellLibrary.get(spellAssetID);
            ////判断合法性
            //if (!isValid(pAttacker,pTarget))
            //{
            //    return false;
            //}
            //若为概率触发，则开始随机
            if (spellAsset.type.byChance && Toolbox.randomChance(0.99f))
            {
                return false;
            }
            if (pTarget == null)
            {
                pTarget = pAttacker;
            }

            return spellAsset.spellAction(this, pAttacker, pTarget);
        }
        //暂时弃用
        private bool isValid(BaseSimObject pAttacker, BaseSimObject pTarget)
        {
            ExtensionSpellAsset spellAsset = AddAssetManager.extensionSpellLibrary.get(spellAssetID);
            if (pTarget == null)//如果无施法对象
            {
                return true;
            }
            else if (pAttacker.objectType == MapObjectType.Actor && pTarget.objectType == MapObjectType.Actor)
            {
                ActorStatus attackerData = ((Actor)pAttacker).GetData();
                ActorStatus targetData = ((Actor)pTarget).GetData();
                //法术反制
                if (attackerData.level >= targetData.level && Toolbox.randomChance(0.8f))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (pAttacker.objectType == MapObjectType.Actor)
            {
                return true;
            }
            else if (pTarget.objectType == MapObjectType.Actor)
            {
                return true;
            }
            else
            {
                return false;
            }
        }//判断施法合法性


    }
}
