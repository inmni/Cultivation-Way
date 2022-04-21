using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CultivationWay;
using NCMS.Utils;
using UnityEngine.UI;
using ReflectionUtility;

namespace Cultivation_Way
{
    class WindowFamily
    {
        private const int lineLength = 50;

        private static ScrollWindow window_Family;

        private static GameObject contentComponent;

        private static Text contentText;

        private static GameObject familyContent;
        internal static void init()//此处init仅作为入口
        {
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "inspect_unit");

            GameObject inspect_unit = GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/inspect_unit");

            Transform inspect_unitContent = inspect_unit.transform.Find("Background").Find("Scroll View").Find("Viewport").Find("Content");

            inspect_unit.SetActive(false);
            initWindow_MoreStats(inspect_unitContent);
        }
        private static void initWindow_MoreStats(Transform inspect_unitContent)
        {

            #region 添加进入窗口的按钮
            PowerButton checkMoreStats = PowerButtons.CreateButton("CheckFamily", Resources.Load<Sprite>("ui/icons/iconInspect"),
            "查看家族信息",
            "",
            new Vector2(247.70f, -90f),
            ButtonType.Click,
            inspect_unitContent,
            clickForWindow_MoreStats);

            checkMoreStats.GetComponent<RectTransform>().sizeDelta = new Vector2(60f, 60f);
            checkMoreStats.transform.Find("Icon").GetComponent<RectTransform>().sizeDelta = new Vector2(36f, 36f);
            checkMoreStats.GetComponent<Image>().sprite = Utils.ResourcesHelper.loadSprite($"{Main.mainPath}/EmbededResources/backButtonRight.png");
            #endregion

            window_Family = Windows.CreateNewWindow("window_Family", "家族信息");
            window_Family.transform.Find("Background").Find("Scroll View").gameObject.SetActive(true);

            familyContent = GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/window_Family/Background/Scroll View/Viewport/Content");

            contentComponent = window_Family.transform
                                .Find("Background")
                                .Find("Name").gameObject;
            contentText = contentComponent.GetComponent<Text>();
            contentText.transform.SetParent(window_Family.transform
                                             .Find("Background")
                                             .Find("Scroll View")
                                             .Find("Viewport")
                                             .Find("Content"));

        }
        private static void clickForWindow_MoreStats()
        {
            setWindowContent();

            window_Family.clickShow();
        }
        private static void setWindowContent()
        {
            contentComponent = window_Family.transform
                                             .Find("Background")
                                             .Find("Scroll View")
                                             .Find("Viewport")
                                             .Find("Content")
                                             .Find("Name").gameObject;
            contentText = contentComponent.GetComponent<Text>();
            contentText.text = getContent();

            RectTransform rect = contentComponent.GetComponent<RectTransform>();
            //rect.anchorMin = new Vector2(0.5f, 1);
            //rect.anchorMax = new Vector2(0.5f, 1);
            //rect.offsetMin = new Vector2(0, contentText.preferredHeight * -1);
            //rect.offsetMax = new Vector2(0, -17);
            rect.sizeDelta = new Vector2(180, contentText.preferredHeight + 50);

            familyContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentText.preferredHeight + 50);
            contentComponent.transform.localPosition = new Vector2(contentComponent.transform.localPosition.x, ((contentText.preferredHeight / 2) + 30) * -1);

            contentComponent.SetActive(true);

        }
        private static string getContent()
        {
            if (Config.selectedUnit == null)
            {
                return "出错";
            }

            List<string> item = new List<string>();
            List<string> value = new List<string>();
            MoreStatus moredata = ((ExtendedActor)Config.selectedUnit).extendedData.status;
            Family family = Main.instance.familys[moredata.familyID];
            item.Add("family");
            value.Add(family.id + "氏");
            item.Add("cultivationBook");
            value.Add(family.cultivationBook.bookName);
            item.Add("历史最强者");
            value.Add(family.honorary);
            item.Add("最强者等级");
            value.Add(family.maxLevel.ToString());
            item.Add("人数");
            value.Add(family.num.ToString());
            return toFormat(item, value);
        }
        private static string toFormat(List<string> item, List<string> value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int length = item.Count;
            for (int i = 0; i < length; i++)
            {
                int tmpLength = 0;
                string text = item[i];
                if (LocalizedTextManager.stringExists(text))
                {
                    text = LocalizedTextManager.getText(text);
                }
                tmpLength = lineLength - text.Length * 2 - value[i].Length;
                stringBuilder.Append(text);
                for (int j = 0; j < tmpLength; j++)
                {
                    stringBuilder.Append(" ");
                }
                stringBuilder.Append(value[i] + "\n");
            }
            return stringBuilder.ToString();
        }
    }
}
