using HarmonyLib;

namespace Cultivation_Way
{
    internal class MoreTraits
    {
        internal void init()
        {
            AssetManager.traits.add(new ActorTrait
            {
                id = "cursed_immune",
                path_icon = "ui/Icons/iconCursedImmune",
                opposite = "cursed"
            });
            AssetManager.traits.get("cursed").opposite = "cursed_immune";
            AssetManager.traits.add(new ActorTrait
            {
                id = "asylum",
                path_icon = "ui/Icons/iconAsylum",
                birth = 0f
            });

            AssetManager.traits.add(new ActorTrait
            {
                id = "realm",
                birth = 0f,
                path_icon = "ui/Icons/default_1",
                unlocked_with_achievement = true,
                achievement_id = "Lost"
            });
            AssetManager.traits.add(new ActorTrait
            {
                id = "cultivationBook",
                birth = 0f,
                path_icon = "ui/Icons/iconCultivationBook",
                unlocked_with_achievement = true,
                achievement_id = "Lost"
            });
            AssetManager.traits.add(new ActorTrait
            {
                id = "element",
                birth = 0f,
                path_icon = "ui/Icons/iconTalent",
                unlocked_with_achievement = true,
                achievement_id = "Lost"
            });
            AssetManager.traits.add(new ActorTrait
            {
                id = "race",
                birth = 0f,
                path_icon = "ui/Icons/iconEasternHuman",
                unlocked_with_achievement = true,
                achievement_id = "Lost"
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
