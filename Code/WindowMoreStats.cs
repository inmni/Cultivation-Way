using CultivationWay;
using NCMS.Utils;
using ReflectionUtility;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way
{
    class WindowMoreStats
    {
        private const int lineLength = 50;

        private static ScrollWindow window_MoreStats;

        private static GameObject contentComponent;

        private static Text contentText;

        private static GameObject moreStatsContent;
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
            PowerButton checkMoreStats = PowerButtons.CreateButton("CheckMoreStats", Resources.Load<Sprite>("ui/icons/iconInspect"),
            "查看详细信息",
            "",
            new Vector2(247.70f, -150f),
            ButtonType.Click,
            inspect_unitContent,
            clickForWindow_MoreStats);

            checkMoreStats.GetComponent<RectTransform>().sizeDelta = new Vector2(60f, 60f);
            checkMoreStats.transform.Find("Icon").GetComponent<RectTransform>().sizeDelta = new Vector2(36f, 36f);
            checkMoreStats.GetComponent<Image>().sprite = Sprites.LoadSprite($"Mods/Cultivation-Way/EmbededResources/backButtonRight.png");
            #endregion

            window_MoreStats = Windows.CreateNewWindow("window_MoreStats", "详细信息");
            window_MoreStats.transform.Find("Background").Find("Scroll View").gameObject.SetActive(true);

            moreStatsContent = GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/window_MoreStats/Background/Scroll View/Viewport/Content");

            contentComponent = window_MoreStats.transform
                                .Find("Background")
                                .Find("Name").gameObject;
            contentText = contentComponent.GetComponent<Text>();
            contentText.transform.SetParent(window_MoreStats.transform
                                             .Find("Background")
                                             .Find("Scroll View")
                                             .Find("Viewport")
                                             .Find("Content"));

        }
        private static void clickForWindow_MoreStats()
        {
            Main.instance.actorToMoreStats[((ActorStatus)Reflection.GetField(typeof(Actor), Config.selectedUnit, "data")).actorID].magic++;

            setWindowContent();

            window_MoreStats.clickShow();
        }
        private static void setWindowContent()
        {
            contentComponent = window_MoreStats.transform
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

            moreStatsContent.GetComponent<RectTransform>().sizeDelta = new Vector2(0, contentText.preferredHeight + 50);
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
            //MoreStats moreStats = Mod.actorToMoreStats[Config.selectedUnit];
            //PropertyInfo[] properties = moreStats.GetType().GetProperties();
            //foreach(PropertyInfo property in properties)
            //{
            //    item.Add(property.Name);
            //    value.Add(property.GetValue(moreStats,null).ToString());
            //}
            //Debug.Log(properties.Length);
            MoreStats stats = Main.instance.actorToMoreStats[((ActorStatus)Reflection.GetField(typeof(Actor), Config.selectedUnit, "data")).actorID];
            ActorStatus data = Reflection.GetField(typeof(Actor), Config.selectedUnit, "data") as ActorStatus;
            item.Add("family");
            value.Add(stats.family.id+"氏");
            item.Add("cultivationBook");
            value.Add(stats.family.cultivationBook.bookName);

            item.Add("elementType");
            value.Add(stats.element.element.name + "灵根");
            item.Add("Gold");
            value.Add(stats.element.baseElementContainer[0] + "%");
            item.Add("Wood");
            value.Add(stats.element.baseElementContainer[1] + "%");
            item.Add("Water");
            value.Add(stats.element.baseElementContainer[2] + "%");
            item.Add("Fire");
            value.Add(stats.element.baseElementContainer[3] + "%");
            item.Add("Ground");
            value.Add(stats.element.baseElementContainer[4] + "%");
            item.Add("cultisystem");
            value.Add(((CultisystemLibrary)AssetManager.instance.dict["cultisystem"]).get(stats.cultisystem).name);
            item.Add("realm");
            int realm = data.level;
            if (realm > 10)
            {
                realm = (realm + 9) / 10+8;
            }
            value.Add(((CultisystemLibrary)AssetManager.instance.dict["cultisystem"]).get(stats.cultisystem).realms[realm-1]);
            return toFormat(item, value);
        }
        private static string toFormat(List<string> item, List<string> value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int length = item.Count;
            for (int i = 0; i < length; i++)
            {
                int tmpLength = lineLength - LocalizedTextManager.getText(item[i]).Length*2 - value[i].Length;
                stringBuilder.Append(LocalizedTextManager.getText(item[i]));
                for(int j = 0; j < tmpLength; j++)
                {
                    stringBuilder.Append(" ");
                }
                stringBuilder.Append(value[i] + "\n");
            }
            return stringBuilder.ToString();
        }

    }
}
