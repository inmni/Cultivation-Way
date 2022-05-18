using CultivationWay;
using NCMS.Utils;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way
{
    class WindowChunkInfo
    {
        private static ScrollWindow window;

        private static GameObject contentComponent;

        private static Text contentText;

        private static GameObject chunkInfoContent;

        private static MapChunk chunk;
        internal static void init()
        {
            window = Windows.CreateNewWindow("window_ChunkInfo", "区块信息");
            window.transform.Find("Background").Find("Scroll View").gameObject.SetActive(true);

            chunkInfoContent = GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/window_ChunkInfo/Background/Scroll View/Viewport/Content");

            contentComponent = window.transform
                                .Find("Background")
                                .Find("Name").gameObject;
            contentText = contentComponent.GetComponent<Text>();
            contentText.transform.SetParent(window.transform
                                             .Find("Background")
                                             .Find("Scroll View")
                                             .Find("Viewport")
                                             .Find("Content"));
        }
        public static void open(MapChunk pChunk)
        {
            chunk = pChunk;
            setWindowContent();
            window.clickShow();
        }
        private static void setWindowContent()
        {
            contentComponent = window.transform
                                             .Find("Background")
                                             .Find("Scroll View")
                                             .Find("Viewport")
                                             .Find("Content")
                                             .Find("Name").gameObject;
            contentText = contentComponent.GetComponent<Text>();
            contentText.text = getContent();

            RectTransform rect = contentComponent.GetComponent<RectTransform>();
            rect.sizeDelta = new Vector2(180, contentText.preferredHeight + 50);

            chunkInfoContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentText.preferredHeight + 50);
            contentComponent.transform.localPosition = new Vector2(contentComponent.transform.localPosition.x, ((contentText.preferredHeight / 2) + 30) * -1);

            contentComponent.SetActive(true);
        }
        private static string getContent()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("区块编号：" + chunk.id + "\n");
            stringBuilder.Append("x：" + chunk.x + "\n");
            stringBuilder.Append("y：" + chunk.y + "\n");
            stringBuilder.Append("元素纯度：" + (int)(125 / Main.instance.chunkToElement[chunk.id].getImPurity() - 25) + "%\n");
            string[] t = new string[5] { "elementGold", "elementWood", "elementWater", "elementFire", "elementGround" };
            for (int i = 0; i < 5; i++)
            {
                if (Main.instance.chunkToElement[chunk.id].baseElementContainer[i] > 9)
                {
                    stringBuilder.Append(Localization.getLocalization(t[i]) + "                   " + Main.instance.chunkToElement[chunk.id].baseElementContainer[i] + "%\n");
                }
                else
                {
                    stringBuilder.Append(Localization.getLocalization(t[i]) + "                     " + Main.instance.chunkToElement[chunk.id].baseElementContainer[i] + "%\n");
                }
            }
            return stringBuilder.ToString();
        }
    }
}
