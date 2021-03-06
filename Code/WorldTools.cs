using CultivationWay;
using HarmonyLib;
using NCMS.Utils;
using ReflectionUtility;

namespace Cultivation_Way
{
    internal static class WorldTools
    {
        public static bool showElementZones(this MapBox world)
        {
            return Main.instance.addMapMode == "map_reki_zones";
        }
        public static void logUnite(Kingdom pKingdom)
        {
            ExtendedActor king = (ExtendedActor)pKingdom.king;
            WorldLogMessage worldLogMessage = new WorldLogMessage("Yao_unite", pKingdom.name, king.easyData.firstName, null);
            KingdomColor kingdomColor = Reflection.GetField(typeof(Kingdom), pKingdom, "kingdomColor") as KingdomColor;
            worldLogMessage.color_special1 = kingdomColor.colorBorderOut;
            worldLogMessage.unit = king;
            worldLogMessage.location = king.currentPosition;
            worldLogMessage.kingdom = pKingdom;
            worldLogMessage.add();
        }
        public static void logSomething(string text, string icon, WorldTile tile = null)
        {
            WorldLogMessage worldLogMessage = new WorldLogMessage("baseLog");
            Localization.setLocalization("baseLog", text);
            worldLogMessage.icon = icon;
            worldLogMessage.location = tile.posV3;
            worldLogMessage.add();
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(WorldLogMessageExtensions), "getFormatedText")]
        public static void getFormatedText(ref string __result, ref WorldLogMessage pMessage)
        {

            switch (pMessage.text)
            {
                case "baseLog":
                    __result = Localization.getLocalization(pMessage.text);
                    break;
                case "Yao_unite":
                    string text = Localization.getLocalization(pMessage.text);
                    ExtendedActor king = (ExtendedActor)pMessage.unit;
                    text = text.Replace("$king$", string.Concat(new string[] { "<color=", Toolbox.colorToHex(pMessage.color_special1, true), ">", king.easyData.firstName, "</color>" }));
                    text = text.Replace("$kingdom$", string.Concat(new string[] { "<color=", Toolbox.colorToHex(pMessage.color_special1, true), ">", pMessage.kingdom.name, "</color>" }));
                    pMessage.icon = "iconKingdom";
                    __result = text;
                    break;
            }

        }
    }
}
