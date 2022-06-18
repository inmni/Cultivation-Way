
using System.Collections.Generic;
using UnityEngine;
namespace Cultivation_Way
{
    internal class NewEffectController
    {
        public Sprite[] defaultFrames;
        public List<NewSpriteAnimation> anims;
        public int limit;
        public string defaultLayer;
        public float defaultInterval;

        public void create(Sprite[] frames, int pLimit, float frameInterval, string layer)
        {
            limit = pLimit;
            defaultLayer = layer;
            defaultFrames = frames;
            defaultInterval = frameInterval;
            anims = new List<NewSpriteAnimation>(pLimit + 5);
        }
        public void setDefaultFrames(Sprite[] frames)
        {
            defaultFrames = frames;
        }
        public void update(float elapse)
        {
            for (int i = 0; i < anims.Count; i++)
            {
                anims[i].update(elapse);
                if (!anims[i].isOn)
                {
                    anims[i].kill();
                    anims.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
