using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HarmonyLib;
using ReflectionUtility;
using UnityEngine;
using CultivationWay;
namespace Cultivation_Way
{
    public class MoreMapModes
    {
        internal void add()
        {
            PlayerConfig.dict.Add("map_reki_zones",new PlayerOptionData("map_reki_zones") { boolVal = false});
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ZoneCalculator),"update",typeof(float))]
        public static void update_Prefix(float pElapsed,ZoneCalculator __instance)
        {
            SpriteRenderer sprRnd = (SpriteRenderer)Reflection.GetField(typeof(ZoneCalculator), __instance, "sprRnd");
            if (MapBox.instance.showElementZones())
            {
                sprRnd.enabled = true;
            }
            __instance.CallMethod("redrawZones");
            __instance.CallMethod("checkAutoDisable");
            if (sprRnd.enabled)
            {
                __instance.CallMethod("UpdateDirty", pElapsed);
            }
        }
        //待补充拦截checkHighLight
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ZoneCalculator),"redrawZones")]
        public static void redrawZones_Prefix(ZoneCalculator __instance)
        {
            if(((SpriteRenderer)Reflection.GetField(typeof(ZoneCalculator), __instance, "sprRnd")).enabled)
            {
                switch (Main.instance.addMapMode)
                {
                    case "map_reki_zones":
                        for (int i = 0; i < Main.instance.chunks.Count; i++)
                        {
                            MapChunk chunk = Main.instance.chunks[i];
                            Color32 color = Utils.OthersHelper.GetColor32ByElement(Main.instance.chunkToElement[chunk.id]);
                            __instance.colorModeElement(chunk.zone,color);
                        }
                        Reflection.SetField(__instance, "_dirty", true);
                        break;
                }
            }
            if (((HashSetTileZone)Reflection.GetField(typeof(ZoneCalculator), __instance, "_toCleanUp")).Any<TileZone>())
            {
                __instance.CallMethod("clearDrawnZones");
            }
            if ((bool)Reflection.GetField(typeof(ZoneCalculator), __instance, "_dirty"))
            {
                __instance.CallMethod("updatePixels");
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

    static class ZonesCalculatorExtension
    {
        public static void colorModeElement(this ZoneCalculator zoneCalculator,TileZone pZone,Color32 color)
        {
            Color32[] pixels = Reflection.GetField(typeof(ZoneCalculator), zoneCalculator, "pixels") as Color32[];
            for(int i = 0; i < pZone.tiles.Count; i++)
            {
                WorldTile worldTile = pZone.tiles[i];
                pixels[worldTile.data.tile_id] = color;
            }
        }
    }
}
