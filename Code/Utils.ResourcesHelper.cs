using NCMS.Utils;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using CultivationWay;
using Cultivation_Way;
using System.ComponentModel;

namespace Cultivation_Way.Utils
{
    class ResourcesHelper
    {
        public static Sprite[] loadAllSprite(string path, float offsetX = 0f, float offsetY = 0f)//路径
        {
            string p = $"{Main.mainPath}/EmbededResources/{path}";
            DirectoryInfo folder = new DirectoryInfo(p);
            List<Sprite> res = new List<Sprite>();
            foreach (FileInfo file in folder.GetFiles("*.png"))
            {
                Sprite sprite = Sprites.LoadSprite($"{file.FullName}", offsetX, offsetY);
                sprite.name = file.Name.Replace(".png", "");
                res.Add(sprite);
            }
            return res.ToArray();
        }

        public static string LoadTextAsset(string path)
        {
            string result = File.ReadAllText($"{Main.mainPath}/EmbededResources/" + path);
            return result;
        }

        public static BaseSpellEffect playSpell(string spellID,Vector2 start,Vector2 end,float size)
        {
            //播放法术动画
            BaseSpellEffectController baseEffectController = Main.instance.spellEffects.get(spellID);
            BaseSpellEffect baseEffect = ((baseEffectController != null) ? baseEffectController.spawnAt(end, size/50f) : null);
            if (baseEffect == null)
            {
                return null;
            }
            //Sfx.play("lightning", true, -1f, -1f);
            baseEffect.spriteRenderer.flipX = Toolbox.randomBool();
            return baseEffect;
        }
    }
}
