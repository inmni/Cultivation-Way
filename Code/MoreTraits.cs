using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
                path_icon = "ui/Icons/iconPoisonImmune",
                opposite = "cursed"
            });
            AssetManager.traits.get("cursed").opposite = "cursed_immune";
            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "asylum",
                path_icon = "ui/Icons/iconAsylum",
                birth = 0f
            }).id);

            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "realm",
                birth = 0f,
                path_icon = "ui/Icons/default_1",
                unlockedWithAchievement = true,
                achievement_id = "Lost"
            }).id);
            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "cultivationBook",
                birth = 0f,
                path_icon = "ui/Icons/iconCultivationBook",
                unlockedWithAchievement=true,
                achievement_id="Lost"
            }).id);
            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "element",
                birth = 0f,
                path_icon = "ui/Icons/iconTalent",
                unlockedWithAchievement = true,
                achievement_id = "Lost"
            }).id);
            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "race",
                birth = 0f,
                path_icon = "ui/Icons/iconEasternHuman",
                unlockedWithAchievement = true,
                achievement_id = "Lost"
            }).id);
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
