using NCMS.Utils;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Cultivation_Way.Utils
{
    class ResourcesHelper
    {
        public static Sprite[] loadAllSprite(string path, float offsetX = 0f, float offsetY = 0f)//路径
        {
            string p = $"Mods/Cultivation-Way/EmbededResources/{path}";
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
            string result = File.ReadAllText($"Mods/Cultivation-Way/EmbededResources/" + path);
            return result;
        }
    }
}
