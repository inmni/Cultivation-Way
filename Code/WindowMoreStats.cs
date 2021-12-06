using ReflectionUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using NCMS.Utils;
using UnityEngine.UI;
using CultivationWay;
using HarmonyLib;

namespace Cultivation_Way
{
    class WindowMoreStats
    {
        private static ScrollWindow window_MoreStats;
        private static Actor selectedUnit;
        private static bool initiated;
        
        internal static void init()//此处init仅作为入口
        {
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "inspect_unit");
            GameObject inspect_unit = GameObject.Find("/Canvas/CanvasWindows/windows/inspect_unit");
            Transform inspect_unitContent = inspect_unit.transform.Find("/Canvas/CanvasWindows/windows/inspect_unit/Background/Scroll View/Viewport/Content");
            inspect_unit.SetActive(true);
            initWindow_MoreStats(inspect_unitContent);
        }
        private static void initWindow_MoreStats(Transform inspect_unitContent)
        {
            if (!initiated)
            {
                initiated = true;
                //添加进入窗口的按钮
                PowerButton checkMoreStats = PowerButtons.CreateButton("CheckMoreStats", Resources.Load<Sprite>("ui/icons/iconInspect"),
                    "查看详细信息",
                    "",
                    new Vector2(247.70f, -150f),
                    ButtonType.Click,
                    inspect_unitContent,
                    clickForWindow_MoreStats);
                checkMoreStats.GetComponent<RectTransform>().sizeDelta = new Vector2(30f, 30f);
                checkMoreStats.transform.Find("Icon").GetComponent<RectTransform>().sizeDelta = new Vector2(18f, 18f);
                checkMoreStats.GetComponent<Image>().sprite = Sprites.LoadSprite($"Mods/Cultivation-Way/EmbededResources/backButtonRight.png");


                window_MoreStats = Windows.CreateNewWindow("window_MoreStats", "详细信息");
                window_MoreStats.transform.Find("Background").Find("Scroll View").gameObject.SetActive(true);
            }
            else
            {
                PowerButtons.CustomButtons["CheckMoreStats"].gameObject.SetActive(true);
            }
            selectedUnit = Config.selectedUnit;
            
            GameObject aboutThisContent = GameObject.Find("/Canvas/CanvasWindows/windows/AboutThis/Background/Scroll View/Viewport/Content");
            //文本内容设置
            #region
            string content = getContent();
            #endregion
            GameObject contentComponent = window_MoreStats.transform
                                .Find("Background")
                                .Find("Name").gameObject;
            Text contentText = contentComponent.GetComponent<Text>();
            contentText.text = content;
            contentText.supportRichText = true;
            contentText.transform.SetParent(window_MoreStats.transform
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
        private static string getContent()
        {
            if (selectedUnit == null)
            {
                return "出错";
            }
      
            List<string> item = new List<string>();
            List<string> value = new List<string>();
            item.Add("灵力");
            value.Add(Main.actorToMoreStats[selectedUnit].magic.ToString());
            return toFormat(item,value);
        }
        private static string toFormat(List<string> item,List<string> value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int length = item.Count;
            for(int i = 0; i < length; i++)
            {
                stringBuilder.Append(item[i] + "                                                 " + value[i]+"\n");
            }
            return stringBuilder.ToString();
        }
        private static void clickForWindow_MoreStats()
        {
            Windows.ShowWindow("window_MoreStats");
        }
    }
}
