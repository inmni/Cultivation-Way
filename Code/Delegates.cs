using System;

namespace Cultivation_Way
{
    [Serializable]
    internal delegate bool SpellAction(ExtensionSpell spell, BaseSimObject pUser = null, BaseSimObject pTarget = null);
    internal delegate void deleteBonus(BaseSimObject pActor, MoreStats morestats);
    internal delegate bool addExperienceAction(ExtendedActor pActor, int pValue);
}
