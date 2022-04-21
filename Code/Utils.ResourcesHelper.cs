using CultivationWay;
using NCMS.Utils;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Cultivation_Way.Utils
{
    class ResourcesHelper
    {
        public static Sprite loadSprite(string path, float offsetX = 0f, float offsetY = 0f)
        {
            Sprite sprite = Main.resourcesBuffer.getSprite(path);
            if (sprite != null)
            {
                return sprite;
            }
            sprite = Sprites.LoadSprite(path, offsetX, offsetY);
            Main.resourcesBuffer.addSprite(path, sprite);
            return sprite;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">格式为"actors/sda/dsadw"</param>
        /// <param name="offsetX"></param>
        /// <param name="offsetY"></param>
        /// <param name="withFolders"></param>
        /// <returns></returns>
        public static Sprite[] loadAllSprite(string path, float offsetX = 0f, float offsetY = 0f,bool withFolders = false)//路径
        {
            string p = $"{Main.mainPath}/EmbededResources/{path}";
            DirectoryInfo folder = new DirectoryInfo(p);
            List<Sprite> res = new List<Sprite>();
            foreach (FileInfo file in folder.GetFiles("*.png"))
            {
                Sprite sprite = Utils.ResourcesHelper.loadSprite($"{file.FullName}", offsetX, offsetY);
                sprite.name = file.Name.Replace(".png", "");
                res.Add(sprite);
            }
            foreach(DirectoryInfo cFolder in folder.GetDirectories())
            {
                foreach(FileInfo file in cFolder.GetFiles("*.png"))
                {
                    Sprite sprite = Utils.ResourcesHelper.loadSprite($"{file.FullName}", offsetX, offsetY);
                    sprite.name = file.Name.Replace(".png", "");
                    res.Add(sprite);
                }
            }
            return res.ToArray();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="path">格式为"text/dsada.json"</param>
        /// <returns></returns>
        public static string LoadTextAsset(string path)
        {
            string result = File.ReadAllText($"{Main.mainPath}/EmbededResources/" + path);
            return result;
        }

        public static BaseSpellEffect playSpell(string spellID, Vector2 start, Vector2 end, float size)
        {
            //播放法术动画
            BaseSpellEffectController spellEffectController = Main.instance.spellEffects.get(spellID);
            BaseSpellEffect spellEffect = ((spellEffectController != null) ? spellEffectController.spawnAt(end, size / 50f) : null);
            if (spellEffect == null)
            {
                return null;
            }
            //Sfx.play("lightning", true, -1f, -1f);
            spellEffect.spriteRenderer.flipX = Toolbox.randomBool();
            return spellEffect;
        }
    }
}
