using HarmonyLib;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    class MoreItems
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
            ItemAsset t2 = AssetManager.items.clone("summonTian", "summonTian1");
            t2.projectile = "magicArrow";
            t2.baseStats.projectiles = 2;
            ItemAsset firework = AssetManager.items.clone("firework", "summonTian");
            firework.projectile = "firework";
            firework.baseStats.projectiles = 1;
            //Main.instance.moreItems.Add(t1.id);



            ActorAnimationLoader aal = new ActorAnimationLoader();
            Dictionary<string, Sprite> dictItems = Traverse.Create(aal).Field("dictItems").GetValue() as Dictionary<string, Sprite>;
            
            Sprite[] addSprites = Utils.ResourcesHelper.loadAllSprite("items/", 0.5f,0,true);
            foreach (Sprite sprite in addSprites)
            {
                dictItems.Add(sprite.name, sprite);
            }
            Traverse.Create(aal).Field("dictItems").SetValue(dictItems);
        }
    }
}
