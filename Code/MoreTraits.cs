using HarmonyLib;

namespace Cultivation_Way
{
    class MoreTraits
    {
        internal void init()
        {
            AssetManager.traits.add(new ActorTrait
            {
                id = "cursed_immune",
                icon = "iconPoisonImmune",
                opposite = "cursed"
            });
            AssetManager.traits.get("cursed").opposite = "cursed_immune";
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(ActionLibrary), "giveCursed")]
        public static bool giveCursed_Prefix(ActorBase pActor)
        {
            if (pActor.haveTrait("cursed_immune"))
            {
                return false;
            }
            return true;
        }
    }
}
