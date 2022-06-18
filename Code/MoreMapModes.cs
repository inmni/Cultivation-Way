using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using System.Linq;
using UnityEngine;
using Cultivation_Way.Utils;
using System;

namespace Cultivation_Way
{
    public class MoreMapModes
    {
        static HashSetTileZone current;
        static HashSetTileZone toClean;
        static SpriteRenderer sprRnd;
        internal void add()
        {
            PlayerConfig.dict.Add("map_reki_zones", new PlayerOptionData("map_reki_zones") { boolVal = false });
        }
        //设置其他


        //染色
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ZoneCalculator), "update", typeof(float))]
        public static void update_Prefix(float pElapsed, ZoneCalculator __instance)
        {
            if (sprRnd == null)
            {
                sprRnd = (SpriteRenderer)Reflection.GetField(typeof(ZoneCalculator), __instance, "sprRnd");
            }
            if (MapBox.instance.showElementZones())
            {
                sprRnd.enabled = true;
                ((Action<ZoneCalculator>)__instance.GetFastMethod("redrawZones"))(__instance);
                ((Action<ZoneCalculator>)__instance.GetFastMethod("checkAutoDisable"))(__instance);
                if (sprRnd.enabled)
                {
                    ((Action<ZoneCalculator,float>)__instance.GetFastMethod("UpdateDirty"))(__instance,pElapsed);
                }
            }

        }
        //待补充拦截checkHighLight
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ZoneCalculator), "redrawZones")]
        public static void redrawZones_Postfix(ZoneCalculator __instance)
        {
            if (sprRnd == null)
            {
                sprRnd = (SpriteRenderer)Reflection.GetField(typeof(ZoneCalculator), __instance, "sprRnd");
            }
            if (sprRnd.enabled)
            {
                switch (Main.instance.addMapMode)
                {
                    case "map_reki_zones":
                        if (current==null||toClean == null)
                        {
                            current = (HashSetTileZone)Reflection.GetField(typeof(ZoneCalculator), __instance, "_currentDrawnZones");
                            toClean = (HashSetTileZone)Reflection.GetField(typeof(ZoneCalculator), __instance, "_toCleanUp");
                        }
                        Color32[] pixels = __instance.GetValue<Color32[]>("pixels");
                        for (int i = 0; i < ExtendedWorldData.instance.chunks.Count; i++)
                        {
                            MapChunk chunk = ExtendedWorldData.instance.chunks[i];
                            Color32 color = OthersHelper.GetColor32ByElement(ExtendedWorldData.instance.chunkToElement[chunk.id]);
                            __instance.colorModeElement(chunk.zone, color,current,toClean,pixels);
                        }
                        Reflection.SetField(__instance, "_dirty", true);
                        if (toClean.Any())
                        {
                            ((Action<ZoneCalculator>)__instance.GetFastMethod("clearDrawnZones"))(__instance);
                        }
                        if ((bool)Reflection.GetField(typeof(ZoneCalculator), __instance, "_dirty"))
                        {
                            ((Action<ZoneCalculator>)__instance.GetFastMethod("updatePixels"))(__instance);
                        }
                        break;
                }
            }

        }
        //[HarmonyPrefix]
        //[HarmonyPatch(typeof(ZoneCalculator),"colorZone",typeof(TileZone))]
        //public static bool colorZone_Prefix(ZoneCalculator __instance,TileZone pZone)
        //{
        //    switch (Main.instance.addMapMode)
        //    {
        //        case "map_reki_zones":
        //            __instance.colorModeElement(pZone);
        //            break;
        //    }
        //    return true;
        //}
    }

    internal static class ZonesCalculatorExtension
    {
        public static void colorModeElement(this ZoneCalculator _, TileZone pZone, Color32 color,HashSetTileZone current,HashSetTileZone toClean,Color32[] pixels)
        {
            current.Add(pZone);
            toClean.Remove(pZone);
            for (int i = 0; i < pZone.tiles.Count; i++)
            {
                WorldTile worldTile = pZone.tiles[i];
                pixels[worldTile.data.tile_id] = color;
            }
        }
    }
}
