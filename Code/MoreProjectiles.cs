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
                speed = 15f,
                texture = "pr_freeze_orb",
                parabolic = true,
                hitShake = true,
                startScale = 0.5f,
                targetScale = 0.01f,
                playImpactSound = true,
                world_actions = (WorldAction)Delegate.Combine(new WorldAction(ActionLibrary.castLightning), new WorldAction(ActionLibrary.castLightning)),
                impactSoundID = "explosion medium",
            }) ;
            //Main.instance.moreProjectiles.Add("lightning_orb");
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Projectile),"start")]
        public static bool start_Prefix(Projectile __instance,string pAssetID)
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
                _frames = ResourcesHelper.loadAllSprite("projectiles/"+asset.id,-0.5f);
            }
            return true;
        }
    }
}
