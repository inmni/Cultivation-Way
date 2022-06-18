using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;
namespace Cultivation_Way
{
    internal class MoreItems
    {
        internal void init()
        {
            addExtendedItemStats();
            addTianItems();
            ItemAsset t1 = clone("summonTian1", "shotgun");
            t1.projectile = "lightningFire_orb";
            t1.materials = new List<string> { "adamantine" };
            t1.baseStats.projectiles = 1;//投掷物数量
            t1.baseStats.attackSpeed = 50f;//攻速加成
            t1.baseStats.range = 20f;//范围加成
            t1.baseStats.damage = 500;//伤害加成
            ItemAsset t2 = clone("summonTian", "summonTian1");
            t2.projectile = "magicArrow";
            t2.baseStats.projectiles = 2;
            ItemAsset firework = clone("firework", "summonTian");
            firework.projectile = "firework";
            firework.baseStats.projectiles = 1;
            //Main.instance.moreItems.Add(t1.id);



            ActorAnimationLoader aal = new ActorAnimationLoader();
            Dictionary<string, Sprite> dictItems = Traverse.Create(aal).Field("dictItems").GetValue() as Dictionary<string, Sprite>;

            Sprite[] addSprites = Utils.ResourcesHelper.loadAllSprite("items/", -0.5f, 0, true);
            foreach (Sprite sprite in addSprites)
            {
                dictItems.Add(sprite.name, sprite);
            }
            Traverse.Create(aal).Field("dictItems").SetValue(dictItems);
        }
        private void addExtendedItemStats()
        {
            foreach (string item in AssetManager.items.dict.Keys)
            {
                Main.instance.extendedItemStatsLibrary[item] = new ExtendedItemStats();
            }
        }
        private ItemAsset clone(string newOne, string oldOne)
        {
            Main.instance.extendedItemStatsLibrary[newOne] = JsonUtility.FromJson<ExtendedItemStats>(JsonUtility.ToJson(Main.instance.extendedItemStatsLibrary[oldOne]));
            return AssetManager.items.clone(newOne, oldOne);
        }
        private void addTianItems()
        {
            ItemAsset knife1 = clone("knife1", "_melee");
            knife1.materials = new List<string> { "base" };
            knife1.baseStats.damage = 20;
            knife1.baseStats.speed = 12;
            knife1.name_templates = new List<string> { "stick_name", "weapon_name_kingdom", "weapon_name_culture" };
            knife1.GetExtendedStats().moreStats.shied = 20;
            knife1.attackAction = new WorldAction((BaseSimObject pTarget, WorldTile pTile)
                                =>
            {
                if (pTarget == null || Toolbox.randomChance(0.9f)) return false;
                pTarget.CallMethod("addStatusEffect", "dizzy", 3f); return true;
            });
            ItemAsset knife2 = clone("knife2", "_melee");
            knife2.materials = new List<string> { "base" };
            knife2.baseStats.damage = 80;
            knife2.baseStats.health = -40;
            knife2.baseStats.speed = -5;
            knife2.baseStats.attackSpeed = -10;
            knife2.setCost(2, "metals", 2);
            knife2.GetExtendedStats().moreStats.shied = -10;
            knife2.attackAction = new WorldAction((BaseSimObject pTarget, WorldTile pTile)
                                =>
            {
                if (pTarget == null || Toolbox.randomChance(0.8f)) return false;
                pTarget.CallMethod("addStatusEffect", "dizzy", 5f); return true;
            });
            ItemAsset minigun1 = clone("minigun1", "_range");
            minigun1.materials = new List<string> { "base" };
            minigun1.baseStats.damage = 10;
            minigun1.baseStats.range = 14f;
            minigun1.baseStats.attackSpeed = 20;
            minigun1.baseStats.projectiles = 1;
            minigun1.projectile = "shotgun_bullet";
            minigun1.setCost(8, "metals", 4, "silver", 2);
            minigun1.attackAction = new WorldAction((BaseSimObject pTarget, WorldTile pTile)
                                =>
            {
                if (pTarget == null || Toolbox.randomChance(0.95f)) return false;
                pTarget.CallMethod("addStatusEffect", "dizzy", 1f); return true;
            });
            ItemAsset gun1 = clone("great_gun1", "_range");
            gun1.baseStats.damage = 500;
            gun1.baseStats.range = 28f;//但愿能生效
            gun1.baseStats.mod_attackSpeed = -90f;
            gun1.baseStats.mod_speed = -90f;
            gun1.baseStats.armor = 5;
            gun1.baseStats.health = 200;
            gun1.projectile = "firebomb";
            gun1.setCost(100, "metals", 20, "silver", 25);
            gun1.attackAction = new WorldAction((BaseSimObject pTarget, WorldTile pTile)
                                 =>
            {
                if (pTarget == null || Toolbox.randomChance(0.2f)) return false;
                pTarget.CallMethod("addStatusEffect", "dizzy", 3f); return true;
            });
            ItemAsset lightning1 = clone("lightning1", "_range");
            lightning1.baseStats.mod_attackSpeed = -90f;
            lightning1.baseStats.mod_speed = -90f;
            lightning1.baseStats.areaOfEffect = 3f;
            lightning1.baseStats.armor = -25;
            lightning1.baseStats.health = -50;
            lightning1.projectile = "lightning_orb";
            lightning1.setCost(500, "mythril", 80, "silver", 100);
            lightning1.attackAction = new WorldAction((BaseSimObject pTarget, WorldTile pTile)
                                 =>
            {
                if (pTarget == null || Toolbox.randomChance(0.9f)) return false;
                pTarget.CallMethod("addStatusEffect", "dizzy", 5f); return true;
            });
        }
    }
}
