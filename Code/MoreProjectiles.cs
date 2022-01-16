using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CultivationWay;
using Cultivation_Way.Utils;
using HarmonyLib;
using ReflectionUtility;
using UnityEngine;

namespace Cultivation_Way
{
    class MoreProjectiles
    {
        internal void init()
        {
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "lightning_orb",
                speed = 10f,
                texture = "lightning_orb",//无作用
                parabolic = false,
                hitShake = true,
                startScale = 0.03f,
                targetScale = 0.03f,
                playImpactSound = true,
                world_actions = new WorldAction(ExtensionSpellActionLibrary.bLightningSpell),
                impactSoundID = "explosion medium",
            });
            Main.instance.moreProjectiles.Add("lightning_orb");
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "lightningFire_orb",
                speed = 15f,
                parabolic = false,
                hitShake = true,
                startScale = 0.02f,
                targetScale = 0.02f,
                animation_speed = 0.1f,
                playImpactSound = true,
                impactSoundID = "explosion medium",
            });
            Main.instance.moreProjectiles.Add("lightningFire_orb");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Projectile),"start")]
        public static bool start_Prefix(Projectile __instance,ref Vector3 pStart,string pAssetID)
        {
            if (!Main.instance.moreProjectiles.Contains(pAssetID))
            {
                return true;
            }
            if (!((bool)Reflection.GetField(typeof(Projectile),__instance,"created")))
            {
                __instance.CallMethod("create");
            }
            ProjectileAsset asset = (ProjectileAsset)Reflection.GetField(typeof(Projectile),__instance,"asset");
            asset = AssetManager.projectiles.get(pAssetID);
            Sprite[] _frames = (Sprite[])Reflection.GetField(typeof(ProjectileAsset), asset, "_frames");
            if (_frames == null || _frames.Length == 0)
            {
                Reflection.SetField(asset,"_frames",ResourcesHelper.loadAllSprite("projectiles/"+asset.id,0.8f));
            }
            pStart.y += 7f;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Projectile),"targetReached")]
        public static bool targetReached_Prefix(Projectile __instance)
        {
            ProjectileAsset p = (ProjectileAsset)Reflection.GetField(typeof(Projectile), __instance, "asset");
            Vector3 pos = (Vector3)Reflection.GetField(typeof(Projectile), __instance, "vecTarget");
            WorldTile targetTile = MapBox.instance.GetTile((int)pos.x, (int)pos.y);
            if (p.world_actions != null&& targetTile != null)
            {
                p.world_actions((BaseSimObject)Reflection.GetField(typeof(Projectile), __instance, "byWho"),targetTile);
            }
            return true;
        }
    }
}
