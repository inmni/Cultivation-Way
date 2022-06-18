using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    internal class ResourcesBuffer
    {
        private Dictionary<string, Sprite> commonSpriteBuffer = new Dictionary<string, Sprite>();
        private Dictionary<string, Sprite[]> commonSpritesBuffer = new Dictionary<string, Sprite[]>();

        public void addSprite(string name, Sprite sprite)
        {

            commonSpriteBuffer[name] = sprite;
        }
        public Sprite getSprite(string name)
        {
            Sprite val = null;
            commonSpriteBuffer.TryGetValue(name, out val);
            return val;
        }
    }
}
