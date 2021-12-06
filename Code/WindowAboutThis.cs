using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way
{
    class WindowAboutThis
    {
        internal static void init()
        {
            ScrollWindow window_AboutThis = Windows.CreateNewWindow("AboutThis", "修真之路");
            window_AboutThis.transform.Find("Background").Find("Scroll View").gameObject.SetActive(true);

            GameObject aboutThisContent = GameObject.Find("/Canvas/CanvasWindows/windows/AboutThis/Background/Scroll View/Viewport/Content");

            #region 内容设置
            string content =
                "模组介绍：\n" +
                "这是一个简简单单的修真模组\n" +
                "真的很简单的\n" +
                "就加了亿点点的小功能\n" +
                "丰富亿点点游戏体验\n" +
                "当然，我们也加入了亿点点的特性\n"+
                "\n" +
                "\n" +
                "\n" +
                "Made By:\n" +
                "启源制作组\n" +
                "美工：盲目吃鱼之神\n" +
                "技术：一米，星棋盘，人间\n" +
                "策划：萧玄瑾，屹昂\n"+
                "宣传：変態盟主\n"+
                "贡献人员：野生星剑\n"+
                "QQ群总部：602184962";
            #endregion
            GameObject contentComponent = window_AboutThis.transform
                                .Find("Background")
                                .Find("Name").gameObject;
            Text contentText = contentComponent.GetComponent<Text>();
            contentText.text = content;
            contentText.supportRichText = true;
            contentText.transform.SetParent(window_AboutThis.transform
                .Find("Background")
                .Find("Scroll View")
                .Find("Viewport")
                .Find("Content"));
            contentComponent.SetActive(true);
            //设置滚轮
            RectTransform rect = contentComponent.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0.5f, 1);
            rect.anchorMax = new Vector2(0.5f, 1);
            rect.offsetMin = new Vector2(-90f, contentText.preferredHeight * -1);
            rect.offsetMax = new Vector2(90f, -17);
            rect.sizeDelta = new Vector2(180, contentText.preferredHeight + 50);
            aboutThisContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentText.preferredHeight + 50);

            contentComponent.transform.localPosition = new Vector2(contentComponent.transform.localPosition.x, ((contentText.preferredHeight / 2) + 30) * -1);

        }
    }
}
