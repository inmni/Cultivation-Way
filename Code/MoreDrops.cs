using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    internal class MoreDrops
    {
        private static List<BaseSimObject> temp_map_objects = null;
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
            MapBox.instance.CallMethod("getObjectsInChunks", pTile, 3, MapObjectType.Actor);
            if (temp_map_objects == null)
            {
                temp_map_objects = (List<BaseSimObject>)Reflection.GetField(typeof(MapBox), MapBox.instance, "temp_map_objects");
            }
            foreach (ExtendedActor actor in temp_map_objects)
            {
                actor.extendedData.status.canCultivate = true;
                if (actor.extendedData.status.cultisystem=="default")
                {
                    actor.extendedData.status.cultisystem = "normal";
                }
                MoreActors.addExperiece_Prefix(actor, actor.getExpToLevelup());
                actor.startShake(0.3f, 0.1f, true, true);
                actor.startColorEffect("white");
            }
        }
    }
}
