using CultivationWay;
using HarmonyLib;
using ReflectionUtility;
using System.Linq;
using UnityEngine;
namespace Cultivation_Way
{
    public class MoreMapModes
    {
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
            if (MapBox.instance.showElementZones())
            {
                SpriteRenderer sprRnd = (SpriteRenderer)Reflection.GetField(typeof(ZoneCalculator), __instance, "sprRnd");
                sprRnd.enabled = true;
                __instance.CallMethod("redrawZones");
                __instance.CallMethod("checkAutoDisable");
                if (sprRnd.enabled)
                {
                    __instance.CallMethod("UpdateDirty", pElapsed);
                }
            }

        }
        //待补充拦截checkHighLight
        [HarmonyPostfix]
        [HarmonyPatch(typeof(ZoneCalculator), "redrawZones")]
        public static void redrawZones_Postfix(ZoneCalculator __instance)
        {
            if (((SpriteRenderer)Reflection.GetField(typeof(ZoneCalculator), __instance, "sprRnd")).enabled)
            {
                switch (Main.instance.addMapMode)
                {
                    case "map_reki_zones":
                        for (int i = 0; i < ExtendedWorldData.instance.chunks.Count; i++)
                        {
                            MapChunk chunk = ExtendedWorldData.instance.chunks[i];
                            Color32 color = Utils.OthersHelper.GetColor32ByElement(ExtendedWorldData.instance.chunkToElement[chunk.id]);
                            __instance.colorModeElement(chunk.zone, color);
                        }
                        Reflection.SetField(__instance, "_dirty", true);
                        if (((HashSetTileZone)Reflection.GetField(typeof(ZoneCalculator), __instance, "_toCleanUp")).Any<TileZone>())
                        {
                            __instance.CallMethod("clearDrawnZones");
                        }
                        if ((bool)Reflection.GetField(typeof(ZoneCalculator), __instance, "_dirty"))
                        {
                            __instance.CallMethod("updatePixels");
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

    static class ZonesCalculatorExtension
    {
        public static void colorModeElement(this ZoneCalculator zoneCalculator, TileZone pZone, Color32 color)
        {
            ((HashSetTileZone)Reflection.GetField(typeof(ZoneCalculator), zoneCalculator, "_currentDrawnZones")).Add(pZone);
            ((HashSetTileZone)Reflection.GetField(typeof(ZoneCalculator), zoneCalculator, "_toCleanUp")).Remove(pZone);
            Color32[] pixels = Reflection.GetField(typeof(ZoneCalculator), zoneCalculator, "pixels") as Color32[];
            for (int i = 0; i < pZone.tiles.Count; i++)
            {
                WorldTile worldTile = pZone.tiles[i];
                pixels[worldTile.data.tile_id] = color;
            }
        }
    }
}
