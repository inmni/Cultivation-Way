using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way
{
    class ResourcesBuffer
    {
        Dictionary<string, Sprite> commonSpriteBuffer = new Dictionary<string, Sprite>();
        Dictionary<string, Sprite[]> commonSpritesBuffer = new Dictionary<string, Sprite[]>(); 

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
