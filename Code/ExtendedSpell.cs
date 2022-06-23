namespace Cultivation_Way
{
    internal class ExtendedSpell
    {
        public string spellAssetID;
        public int cost;

        public float might;//威力因子
        public ExtendedSpell()
        {
            spellAssetID = "example";
            cost = 0;
            might = 0;
        }
        public ExtendedSpell(string spellID)
        {
            spellAssetID = spellID;
            ExtendedSpellAsset spellAsset = AddAssetManager.extensionSpellLibrary.get(spellAssetID);
            cost = spellAsset.baseCost;
            might = spellAsset.might;
        }
        public ExtendedSpell(ExtendedSpell spell)
        {
            spellAssetID = spell.spellAssetID;
            might = spell.might;
        }
        public ExtendedSpellAsset GetSpellAsset()
        {
            ExtendedSpellAsset result;
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
            if (pTarget == null)
            {
                pTarget = pAttacker;
            }
            return AddAssetManager.extensionSpellLibrary.get(spellAssetID).spellAction(this, pAttacker, pTarget);
        }


    }
}
