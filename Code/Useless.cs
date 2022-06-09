using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cultivation_Way.Code
{
    class Useless
    {
        





        void Circum_node()
        {
            //if (false)
            //{
            //    if (Main.instance.zoneCalculator.getZone(zone.x - 1, zone.y) == null)
            //    {
            //        if (Main.instance.zoneCalculator.getZone(zone.x, zone.y - 1) == null || !dirInZones[2])//左下
            //        {
            //            removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x, y + 1));
            //            conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
            //            removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x, y + 1));
            //            conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].nodeWallTiles.Add(world.GetTile(x, y + 1));
            //            conditions[cityKey].nodeDirections.Add(0);
            //        }
            //        if (Main.instance.zoneCalculator.getZone(zone.x, zone.y + 1) == null || !dirInZones[3])//左上
            //        {
            //            removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x, y + 1));
            //            conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
            //            removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x, y + 1));
            //            conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].nodeWallTiles.Add(world.GetTile(x, y + 7));
            //            conditions[cityKey].nodeDirections.Add(1);

            //        }
            //    }
            //    if (Main.instance.zoneCalculator.getZone(zone.x + 1, zone.y) == null)
            //    {
            //        if (Main.instance.zoneCalculator.getZone(zone.x, zone.y - 1) == null || !dirInZones[2])//右下
            //        {
            //            removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x + 6, y + 1));
            //            conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
            //            removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x + 6, y + 1));
            //            conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].nodeWallTiles.Add(world.GetTile(x + 6, y + 1));
            //            conditions[cityKey].nodeDirections.Add(0);
            //        }
            //        if (Main.instance.zoneCalculator.getZone(zone.x, zone.y + 1) == null || !dirInZones[3])//右上
            //        {
            //            removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x + 6, y + 6));
            //            conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
            //            removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x + 6, y + 7));
            //            conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].nodeWallTiles.Add(world.GetTile(x + 6, y + 7));
            //            conditions[cityKey].nodeDirections.Add(1);

            //        }
            //    }
            //    if (Main.instance.zoneCalculator.getZone(zone.x, zone.y - 1) == null)
            //    {
            //        if (Main.instance.zoneCalculator.getZone(zone.x - 1, zone.y) != null && !dirInZones[0])//左下
            //        {
            //            removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x, y + 1));
            //            conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
            //            removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x, y + 1));
            //            conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].nodeWallTiles.Add(world.GetTile(x, y + 1));
            //            conditions[cityKey].nodeDirections.Add(0);
            //        }
            //        if (Main.instance.zoneCalculator.getZone(zone.x + 1, zone.y) != null && !dirInZones[1])//右下
            //        {
            //            removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x + 6, y + 1));
            //            conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
            //            removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x + 6, y + 1));
            //            conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].nodeWallTiles.Add(world.GetTile(x + 6, y + 1));
            //            conditions[cityKey].nodeDirections.Add(0);
            //        }
            //    }
            //    if (Main.instance.zoneCalculator.getZone(zone.x, zone.y + 1) == null)
            //    {
            //        if (Main.instance.zoneCalculator.getZone(zone.x - 1, zone.y) != null && !dirInZones[0])//左上
            //        {
            //            removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x, y + 6));
            //            conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
            //            removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x, y + 7));
            //            conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].nodeWallTiles.Add(world.GetTile(x, y + 7));
            //            conditions[cityKey].nodeDirections.Add(1);

            //        }
            //        if (Main.instance.zoneCalculator.getZone(zone.x + 1, zone.y) != null && !dirInZones[1])//右上
            //        {
            //            removeIndex = conditions[cityKey].vertWallTiles.IndexOf(world.GetTile(x + 6, y + 6));
            //            conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].vertWallTiles.RemoveAt(removeIndex);
            //            removeIndex = conditions[cityKey].horiWallTiles.IndexOf(world.GetTile(x + 6, y + 7));
            //            conditions[cityKey].horiWallTiles.RemoveAt(removeIndex);
            //            conditions[cityKey].nodeWallTiles.Add(world.GetTile(x + 6, y + 7));
            //            conditions[cityKey].nodeDirections.Add(1);

            //        }
            //    }
            //}
        }
    }
}
