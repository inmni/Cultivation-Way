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

            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "realm",
                birth = 0f,
                icon = "default_1"
            }).id);
            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "cultivationBook",
                birth = 0f,
                icon = "cultivationBook"
            }).id);
            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "element",
                birth = 0f,
                icon = "element"
            }).id);
            addTraits.Add(AssetManager.traits.add(new ActorTrait
            {
                id = "race",
                birth = 0f,
                icon = "human"
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
                Reflection.SetField(__instance, "trait", trait);
                Sprite sprite = NCMS.Utils.Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/traits/" + trait.icon + ".png");
                __instance.GetComponent<Image>().sprite = sprite;
                return false;
            }
            return true;
        }
    }
}
