using System.Collections.Generic;
namespace Cultivation_Way
{
    internal class MoreStatusEffects
    {
        internal void init()
        {
            List<string> temp_list;
            StatusEffect dizzy = AssetManager.status.add(new StatusEffect
            {
                id = "dizzy",
                texture = "",
                duration = 3f,
            });
            dizzy.baseStats.speed = -100000f;
            dizzy.baseStats.attackSpeed = -1000000f;
            dizzy.cancelActorJob = true;
        }
    }
}
