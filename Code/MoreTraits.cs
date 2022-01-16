using HarmonyLib;
using System.Collections.Generic;

namespace Cultivation_Way
{
    class MoreTraits
    {
        public List<string> addTraits = new List<string>();
        internal void init()
        {
            AssetManager.traits.add(new ActorTrait
            {
                id = "cursed_immune",
                icon = "iconPoisonImmune",
                opposite = "cursed"
            });
            AssetManager.traits.get("cursed").opposite = "cursed_immune";
            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "asylum",
                icon = "iconAsylum",
                birth = 0f
            }).id);

            AssetManager.traits.add(new ActorTrait
            {
                id = "realm",
                birth = 0f,
                icon = "normal_1"
            });
            AssetManager.traits.add(new ActorTrait 
            { 
                id = "cultivationBook",
                birth = 0f,
                icon = "cultivationBook"
            });
            AssetManager.traits.add(new ActorTrait
            {
                id = "element",
                birth = 0f,
                icon = "All"
            });
            AssetManager.traits.add(new ActorTrait
            {
                id = "race",
                birth = 0f,
                icon = "race"
            });
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
