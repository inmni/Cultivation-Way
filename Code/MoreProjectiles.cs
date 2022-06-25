using Cultivation_Way.Utils;
using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using System;
using UnityEngine;

namespace Cultivation_Way
{
    internal class MoreProjectiles
    {
        internal void init()
        {
            //animated                          是否播放动画                  bool
            //animation_speed                   动画播放速度                  float
            //looped                            动画是否循环播放              bool
            //stayInGround                      投掷物是否滞留在地面          bool
            //speed_random                      随机速度范围（即速度变化范围  float
            //terraformOption                   落地引起的terraform           string
            //terraformRange                    terraform范围                 int
            //endEffect                         结束动画效果                  string
            //rotate                            旋转                          bool
            //look_at_target                    指向目标                      bool
            //trailEffect_enabled               是否开启轨迹效果              bool
            //trailEffefct_texture              轨迹效果材质                  string
            //trailEffect_scale                 轨迹效果大小                  float
            //trailEffect_timer                 轨迹效果持续时间              float
            //hitFreeze                         命中是否引起冰冻              bool
            //hitShake                          命中是否引起颤抖              bool
            //texture_shadow                    阴影                          string
            foreach (ProjectileAsset p in AssetManager.projectiles.list)
            {
                p.parabolic = false;
            }
            AssetManager.projectiles.get("arrow").parabolic = true;
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "lightning_orb",
                speed = 10f,                    //投掷物移动速度
                texture = "lightning_orb",      //贴图，在模组加载的投掷物中无作用
                parabolic = false,              //是否开启抛物线
                hitShake = true,                //受击者是否颤抖
                startScale = 0.03f,             //发射起始大小
                targetScale = 0.03f,            //到达目的地大小
                playImpactSound = true,         //是否有碰撞声音
                world_actions = new WorldAction(ExtendedWorldActions.bLightningSpell),//落地后的行为
                impactSoundID = "explosion medium",//碰撞声音
            });
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "plasma_ball_Tian",
                speed = 22f,                    
                texture = "pr_plasma_ball_Tian",
                trailEffect_enabled = true,
                trailEffect_scale = 0.1f,
                trailEffect_timer = 0.1f,
                trailEffect_texture = "fx_plasma_trail_Tian",
                look_at_target = true,
                looped = true,
                endEffect = "fireballExplosion",          
                startScale = 0.03f,      
                targetScale = 0.25f,       
                world_actions = new WorldAction(ExtendedWorldActions.aSimpleRangeDamage),
            });
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "lightningFire_orb",
                speed = 15f,
                parabolic = false,
                texture = "lightningFire_orb",
                hitShake = true,
                startScale = 0.02f,
                targetScale = 0.02f,
                animation_speed = 0.1f,
                playImpactSound = true,
                impactSoundID = "explosion medium",
            });
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "magicArrow",
                speed = 25f,
                parabolic = false,
                hitShake = true,
                texture = "magicArrow",
                startScale = 0.02f,
                targetScale = 0.02f,
                animation_speed = 0.1f,
                playImpactSound = true,
                look_at_target = true,
                impactSoundID = "explosion medium",
            });
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "red_magicArrow",
                speed = 25f,
                parabolic = false,
                hitShake = true,
                texture = "red_magicArrow",
                startScale = 0.04f,
                targetScale = 0.04f,
                animation_speed = 0.1f,
                playImpactSound = true,
                look_at_target = true,
                endEffect = "fireballExplosion",
                impactSoundID = "explosion medium",
            });
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "water_orb",
                texture = "water_orb",
                speed = 30f,                    //投掷物移动速度
                parabolic = false,              //是否开启抛物线
                hitShake = true,                //受击者是否颤抖
                startScale = 0.4f,             //发射起始大小
                targetScale = 0.4f,            //到达目的地大小
                world_actions = new WorldAction(ExtendedWorldActions.aSimpleRangeDamage)
            });
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "firework",
                texture = "firework",
                speed = 20f,                    //投掷物移动速度
                parabolic = true,              //是否开启抛物线
                rotate = true,
                hitShake = true,                //受击者是否颤抖
                startScale = 0.04f,             //发射起始大小
                targetScale = 0.04f,            //到达目的地大小
                world_actions = new WorldAction(ExtendedWorldActions.aFireworkDamage)
            });
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "swordkee",
                texture = "swordkee",
                speed = 30f,       
                parabolic = false,       
                hitShake = true,
                looped=false,
                look_at_target=true,
                startScale = 0.4f,            
                targetScale = 0.4f,            
                world_actions = new WorldAction(ExtendedWorldActions.aSimpleRangeDamage)
            });
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "water_orb1",
                texture = "water_orb1",
                speed = 30f,
                parabolic = false,
                look_at_target=true,
                looped=true,
                hitShake = true,
                startScale = 0.4f,
                targetScale = 0.4f,
                world_actions = new WorldAction(ExtendedWorldActions.aSimpleRangeDamage)
            }); 
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "wind_blade",
                texture = "wind_blade",
                speed = 30f,
                parabolic = false,
                hitShake = true,
                looped=false,
                look_at_target=true,
                startScale = 0.4f,
                targetScale = 0.4f,
                world_actions = new WorldAction(ExtendedWorldActions.aSimpleRangeDamage)
            });
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "ice_blade",
                texture = "ice_blade",
                speed = 30f,
                parabolic = false,
                hitShake = true,
                looped = false,
                look_at_target = true,
                startScale = 0.4f,
                targetScale = 0.4f,
                hitFreeze=true,
                world_actions = new WorldAction(ExtendedWorldActions.aSimpleRangeDamage)
            });
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "poison_blade",
                texture = "poison_blade",
                speed = 30f,
                parabolic = false,
                hitShake = true,
                looped = false,
                look_at_target = true,
                startScale = 0.4f,
                targetScale = 0.4f,
                world_actions = new WorldAction(ExtendedWorldActions.aSimpleRangeDamage)
            });
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Projectile), "targetReached")]
        public static bool targetReached_Prefix(Projectile __instance)
        {
            ProjectileAsset p = __instance.GetValue<ProjectileAsset>("asset");
            if (p.world_actions == null)
            {
                return true;
            }
            Vector3 pos = __instance.GetValue<Vector3>("vecTarget");
            WorldTile targetTile = MapBox.instance.GetTile((int)pos.x, (int)pos.y);
            if (p.world_actions != null && targetTile != null)
            {
                p.world_actions(__instance.GetValue<BaseSimObject>("byWho"), targetTile);
            }
            return true;
        }
    }
}
