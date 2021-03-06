using CultivationWay;
using NCMS.Utils;
using ReflectionUtility;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way
{
    internal class WindowMoreStats
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
            GameObject moreStatsGameObject = UnityEngine.Object.Instantiate(GameObject.Find
                ("/Canvas Container Main/Canvas - Windows/windows/inspect_unit/Background/ButtonContainerTraits"));
            moreStatsGameObject.transform.SetParent(GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/inspect_unit/Background").transform);
            moreStatsGameObject.transform.name = "CheckMoreStats";
            moreStatsGameObject.transform.localScale = Vector3.one;
            moreStatsGameObject.transform.localPosition = new Vector3(116.85f, -2.4204f,0);
            Transform moreStatsButton = moreStatsGameObject.transform.Find("Button");
            moreStatsButton.Find("Icon").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/icons/iconInspect");
            PowerButton checkMoreStats = moreStatsButton.GetComponent<PowerButton>();
            checkMoreStats.type = PowerButtonType.Library;
            checkMoreStats.open_window_id = string.Empty;
            moreStatsButton.GetComponent<Button>().onClick = new Button.ButtonClickedEvent();
            moreStatsButton.GetComponent<Button>().onClick.AddListener(delegate { show(); });
            TipButton tip = moreStatsButton.GetComponent<TipButton>();
            tip.type = "normal";
            tip.textOnClick = "tip_moreStats";
            tip.textOnClickDescription = null;

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
        private static void show()
        {

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
            ExtendedActor actor = (ExtendedActor)Config.selectedUnit;
            SpecialBody specialBody = actor.GetSpecialBody();
            ExtendedActorStatus moredata = actor.extendedData.status;
            item.Add("family");
            value.Add(moredata.familyID + "氏");
            item.Add("specialBody");
            value.Add(specialBody.name);
            item.Add("origin");
            value.Add(specialBody.origin);
            item.Add("madeBy");
            value.Add(specialBody.madeBy);
            item.Add("elementType");
            value.Add(AddAssetManager.chineseElementLibrary.get(moredata.chineseElement.id).name + "灵根");
            item.Add("elementGold");
            value.Add(moredata.chineseElement.baseElementContainer[0] + "%");
            item.Add("elementWood");
            value.Add(moredata.chineseElement.baseElementContainer[1] + "%");
            item.Add("elementWater");
            value.Add(moredata.chineseElement.baseElementContainer[2] + "%");
            item.Add("elementFire");
            value.Add(moredata.chineseElement.baseElementContainer[3] + "%");
            item.Add("elementGround");
            value.Add(moredata.chineseElement.baseElementContainer[4] + "%");
            item.Add("cultisystem");
            value.Add(AddAssetManager.cultisystemLibrary.get(moredata.cultisystem).name);
            item.Add("realm");
            value.Add(actor.getRealmName());

            return toFormat(item, value);
        }
        private static string toFormat(List<string> item, List<string> value)
        {
            StringBuilder stringBuilder = new StringBuilder();
            int length = item.Count;
            for (int i = 0; i < length; i++)
            {
                int tmpLength = lineLength - LocalizedTextManager.getText(item[i]).Length * 2 - value[i].Length;
                stringBuilder.Append(LocalizedTextManager.getText(item[i]));
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
