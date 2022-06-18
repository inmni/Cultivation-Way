using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;
namespace Cultivation_Way
{
    internal class WindowAboutThis
    {
        internal static void init()
        {
            ScrollWindow window_AboutThis = Windows.CreateNewWindow("window_AboutThis", "修真之路");
            window_AboutThis.transform.Find("Background").Find("Scroll View").gameObject.SetActive(true);

            GameObject aboutThisContent = GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/window_AboutThis/Background/Scroll View/Viewport/Content");

            #region 内容设置
            string content =
                "模组介绍：\n" +
                "这是一个简简单单的修真模组\n" +
                "真的很简单的\n" +
                "就加了亿点点的小功能\n" +
                "丰富亿点点游戏体验\n" +
                "当然，我们也加入了亿点点的特性\n" +
                "\n" +
                "\n" +
                "\n" +
                "Made By:\n" +
                "启源制作组\n" +
                "美工：盲目吃鱼之神，天少，香蕉，変態盟主\n" +
                "技术：一米，星棋盘，人间\n" +
                "策划：萧玄瑾，屹昂\n" +
                "宣传：変態盟主\n" +
                "贡献人员：野生星剑\n" +
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
            //添加图片
            PowerButton button = PowerButtons.CreateButton("Coder1", Resources.Load<Sprite>("ui/Icons/iconRenJian"),
                "人间", "代码", Vector3.zero, ButtonType.Click, contentComponent.transform, null);
            button.transform.localPosition = new Vector3(-20f, -160f);
            button = PowerButtons.CreateButton("Coder2", Resources.Load<Sprite>("ui/Icons/iconINMNI"),
                "一米", "代码", Vector3.zero, ButtonType.Click, contentComponent.transform, null);
            button.transform.localPosition = new Vector3(20f, -160f);
            button = PowerButtons.CreateButton("Coder3", Resources.Load<Sprite>("ui/Icons/iconQiPan"),
                "星棋盘", "代码", Vector3.zero, ButtonType.Click, contentComponent.transform, null);
            button.transform.localPosition = new Vector3(60f, -160f);
            button = PowerButtons.CreateButton("Propagandist", Resources.Load<Sprite>("ui/Icons/iconMengZhu"),
                "変態盟主", "美工、宣传\nB站关注71563146", Vector3.zero, ButtonType.Click, contentComponent.transform, null);
            button.transform.localPosition = new Vector3(0f, -180f);
            button = PowerButtons.CreateButton("Planner1", Resources.Load<Sprite>("ui/Icons/iconQingSi"),
                "青丝", "策划", Vector3.zero, ButtonType.Click, contentComponent.transform, null);
            button.transform.localPosition = new Vector3(40f, -180f);
            button = PowerButtons.CreateButton("Contributor1", Resources.Load<Sprite>("ui/Icons/iconXingJian"),
                "星剑", "贡献", Vector3.zero, ButtonType.Click, contentComponent.transform, null);
            button.transform.localPosition = new Vector3(20f, -200f);
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
