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
                path_icon = "iconPoisonImmune",
                opposite = "cursed"
            });
            AssetManager.traits.get("cursed").opposite = "cursed_immune";
            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "asylum",
                path_icon = "iconAsylum",
                birth = 0f
            }).id);

            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "realm",
                birth = 0f,
                path_icon = "default_1",
                unlockedWithAchievement = true,
                achievement_id = "Lost"
            }).id);
            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "cultivationBook",
                birth = 0f,
                path_icon = "cultivationBook",
                unlockedWithAchievement=true,
                achievement_id="Lost"
            }).id);
            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "element",
                birth = 0f,
                path_icon = "element",
                unlockedWithAchievement = true,
                achievement_id = "Lost"
            }).id);
            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "race",
                birth = 0f,
                path_icon = "human",
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
        [HarmonyPrefix]
        [HarmonyPatch(typeof(TraitButton), "load")]
        public static bool load_Prefix(TraitButton __instance, string pTrait)
        {
            if (Main.instance.MoreTraits.addTraits.Contains(pTrait))
            {
                ActorTrait trait = AssetManager.traits.get(pTrait);
                Reflection.SetField(__instance, "trait_asset", trait);
                Sprite sprite = Utils.ResourcesHelper.loadSprite($"{Main.mainPath}/EmbededResources/icons/traits/" + trait.path_icon + ".png");
                __instance.GetComponent<Image>().sprite = sprite;
                return false;
            }
            return true;
        }
    }
}
