﻿using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    class MoreDrops
    {
        internal void init()
        {
            AssetManager.drops.add(new DropAsset
            {
                id = "exp",
                texture = "drops/drop_acid",
                random_frame = true,
                default_scale = 0.2f,
                fallingHeight = new Vector2(30f, 45f),
                action_landed = new DropsAction(action_exp)
            });
        }

        public static void action_exp(WorldTile pTile = null, string pDropID = null)
        {
            MapBox.instance.CallMethod("getObjectsInChunks", pTile, 3, MapObjectType.Actor);
            List<BaseSimObject> temp_map_objects = Reflection.GetField(typeof(MapBox), MapBox.instance, "temp_map_objects") as List<BaseSimObject>;
            int count = temp_map_objects.Count;
            for (int i = 0; i < count; i++)
            {
                Actor actor = (Actor)temp_map_objects[i];
                actor.CallMethod("addExperience", actor.getExpToLevelup());
                actor.startShake(0.3f, 0.1f, true, true);
                actor.startColorEffect("white");
            }
        }
    }
}
