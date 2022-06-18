namespace Cultivation_Way
{
    internal class MoreTopTileType
    {
        internal void init()
        {
            TopTileType wall_base = AssetManager.topTiles.clone("wall_base", "field");
            wall_base.ground = false;
            wall_base.block = true;
            wall_base.walkMod = 0.1f;
            wall_base.damagedWhenWalked = false;
            wall_base.strength = int.MaxValue;
            wall_base.farm_field = false;
            wall_base.canBuildOn = true;
            wall_base.canBeSetOnFire = false;
            wall_base.burnable = false;
            wall_base.remove_on_freeze = false;
            wall_base.remove_on_heat = false;
            wall_base.cost = 9999;
            wall_base.setDrawLayer(TileZIndexes.mountains);
        }
    }
}
