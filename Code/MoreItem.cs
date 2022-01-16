using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using CultivationWay;
using UnityEngine;
using System.Reflection;

namespace Cultivation_Way
{
    class MoreItem
    {
        internal void init()
        {
            ItemAsset t1 = AssetManager.items.clone("summonTian1", "shotgun");
            t1.projectile = "lightningFire_orb";
            t1.materials = new List<string> { "adamantine" };
            t1.baseStats.projectiles = 1;
            t1.baseStats.attackSpeed = 50f;
            t1.baseStats.range = 20f;
            t1.baseStats.damage = 500;
            //Main.instance.moreItems.Add(t1.id);

            ActorAnimationLoader aal = new ActorAnimationLoader();
            Dictionary<string, Sprite> dictItems = Traverse.Create(aal).Field("dictItems").GetValue() as Dictionary<string, Sprite>;
            Sprite[] addSprites = Utils.ResourcesHelper.loadAllSprite("items/",0.5f);
            foreach (Sprite sprite in addSprites)
            {
                dictItems.Add(sprite.name, sprite);
            }
            Traverse.Create(aal).Field("dictItems").SetValue(dictItems);
        }
    }
}
