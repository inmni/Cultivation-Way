using ReflectionUtility;
using UnityEngine;
using CultivationWay;
namespace Cultivation_Way
{
    internal class MoreDrops
    {
        internal void init()
        {
            AssetManager.drops.add(new DropAsset
            {
                id = "exp",
                path_texture = "drops/drop_lava",
                random_frame = true,
                default_scale = 0.2f,
                fallingHeight = new Vector2(30f, 45f),
                action_landed = new DropsAction(action_exp)
            });
        }

        public static void action_exp(WorldTile pTile = null, string pDropID = null)
        {
            Utils.FastReflection.mapbox_getObjectsInChunks(MapBox.instance, pTile, 3, MapObjectType.Actor);
            foreach (ExtendedActor actor in Main.instance.temp_map_objects)
            {
                actor.extendedData.status.canCultivate = true;
                if (actor.extendedData.status.cultisystem=="default")
                {
                    actor.extendedData.status.cultisystem = "normal";
                }
                ExtendedActor.addExperiece_Prefix(actor, actor.getExpToLevelup());
                actor.startShake(0.3f, 0.1f, true, true);
                actor.startColorEffect("white");
            }
        }
    }
}
