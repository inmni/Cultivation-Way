using Cultivation_Way.Utils;
using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using UnityEngine;

namespace Cultivation_Way
{
    class MoreProjectiles
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
            foreach(ProjectileAsset p in AssetManager.projectiles.list)
            {
                p.parabolic = false;
            }
            AssetManager.projectiles.get("arrow").parabolic =true;
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
                world_actions = new WorldAction(ExtensionSpellActionLibrary.bLightningSpell),//落地后的行为
                impactSoundID = "explosion medium",//碰撞声音
            });
            
            Main.instance.addProjectiles.Add("lightning_orb", new Vector2(0,7f));//添加，用于加载，后面的Vector2确定发射位置
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
            Main.instance.addProjectiles.Add("lightningFire_orb", new Vector2(0,7f));
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "magicArrow",
                speed = 25f,
                parabolic = false,
                hitShake = true,
                startScale = 0.02f,
                targetScale = 0.02f,
                animation_speed = 0.1f,
                playImpactSound = true,
                look_at_target = true,
                impactSoundID = "explosion medium",
            });
            Main.instance.addProjectiles.Add("magicArrow", new Vector2(0,20f));
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "red_magicArrow",
                speed = 25f,
                parabolic = false,
                hitShake = true,
                startScale = 0.04f,
                targetScale = 0.04f,
                animation_speed = 0.1f,
                playImpactSound = true,
                look_at_target = true,
                endEffect = "fireballExplosion",
                impactSoundID = "explosion medium",
            });
            Main.instance.addProjectiles.Add("red_magicArrow", Vector2.zero);
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "water_orb",
                speed = 30f,                    //投掷物移动速度
                parabolic = false,              //是否开启抛物线
                hitShake = true,                //受击者是否颤抖
                startScale = 0.4f,             //发射起始大小
                targetScale = 0.4f,            //到达目的地大小
                world_actions = new WorldAction(ExtensionSpellActionLibrary.aWaterPoleDamage)
            }) ;
            Main.instance.addProjectiles.Add("water_orb", new Vector2(0, 7f));
            AssetManager.projectiles.add(new ProjectileAsset
            {
                id = "firework",
                speed = 20f,                    //投掷物移动速度
                parabolic = true,              //是否开启抛物线
                rotate = true,
                hitShake = true,                //受击者是否颤抖
                startScale = 0.04f,             //发射起始大小
                targetScale = 0.04f,            //到达目的地大小
                world_actions = new WorldAction(ExtensionSpellActionLibrary.aFireworkDamage)
            });
            Main.instance.addProjectiles.Add("firework", Vector2.zero);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(Projectile), "start")]
        public static bool start_Prefix(Projectile __instance, ref Vector3 pStart, string pAssetID)
        {
            if (!Main.instance.addProjectiles.ContainsKey(pAssetID))
            {
                return true;
            }
            if (!((bool)Reflection.GetField(typeof(Projectile), __instance, "created")))
            {
                __instance.CallMethod("create");
            }
            ProjectileAsset asset = (ProjectileAsset)Reflection.GetField(typeof(Projectile), __instance, "asset");
            asset = AssetManager.projectiles.get(pAssetID);
            Sprite[] _frames = (Sprite[])Reflection.GetField(typeof(ProjectileAsset), asset, "_frames");
            if (_frames == null || _frames.Length == 0)
            {
                Reflection.SetField(asset, "_frames", ResourcesHelper.loadAllSprite("projectiles/" + asset.id, 0.8f));
            }
            pStart.x += Main.instance.addProjectiles[pAssetID].x;
            pStart.y += Main.instance.addProjectiles[pAssetID].y;
            return true;
        }
        [HarmonyPrefix]
        [HarmonyPatch(typeof(Projectile), "targetReached")]
        public static bool targetReached_Prefix(Projectile __instance)
        {
            ProjectileAsset p = (ProjectileAsset)Reflection.GetField(typeof(Projectile), __instance, "asset");
            Vector3 pos = (Vector3)Reflection.GetField(typeof(Projectile), __instance, "vecTarget");
            WorldTile targetTile = MapBox.instance.GetTile((int)pos.x, (int)pos.y);
            if (p.world_actions != null && targetTile != null)
            {
                p.world_actions((BaseSimObject)Reflection.GetField(typeof(Projectile), __instance, "byWho"), targetTile);
            }
            return true;
        }
    }
}
