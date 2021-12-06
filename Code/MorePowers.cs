using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;


namespace Cultivation_Way
{
    class MorePowers
    {
        public static GameObject newTab;
        public static PowersTab tabComponent;

        public static void createPowerTab()
        {
            GameObject copyTabButton = GameObjects.FindEvenInactive("Button_Other");
            if (copyTabButton != null)
            {
                GameObject newTabButton = GameObject.Instantiate(copyTabButton);
                newTabButton.name = "Button_Cultivation_Way";
                newTabButton.transform.SetParent(copyTabButton.transform.parent);
                newTabButton.transform.localScale = new Vector3(1f, 1f);
                newTabButton.transform.localPosition = new Vector3(-150f, 49.62f);//x轴待调整
                newTabButton.transform.Find("Icon").GetComponent<Image>().sprite = Sprites.LoadSprite($"Mods/Cultivation Way/EmbededResources/iconTab.png");

             //设置栏内元素

                GameObject copyTab = GameObjects.FindEvenInactive("Tab_Other");
                //暂时禁用copyTab内元素
                foreach(Transform transform in copyTab.transform)
                {
                    transform.gameObject.SetActive(false);
                }
                newTab = GameObject.Instantiate(copyTab);
                //删除复制来的无用元素
                foreach(Transform transform in newTab.transform)
                {
                    if (transform.gameObject.name == "tabBackButton" || transform.gameObject.name == "-space")
                    {
                        transform.gameObject.SetActive(true);
                    }
                    else
                    {
                        GameObject.Destroy(transform.gameObject);
                    }
                }
                //恢复copyTab内元素
                foreach (Transform transform in copyTab.transform)
                {
                    transform.gameObject.SetActive(true);
                }

                newTab.name = "Tab_Cultivation_Way";
                newTab.transform.SetParent(copyTab.transform.parent);

                //子内容设置
                Button buttonComponent = newTabButton.GetComponent<Button>();
                tabComponent = newTab.GetComponent<PowersTab>();
                tabComponent.powerButton = buttonComponent;
                tabComponent.powerButton.onClick = new Button.ButtonClickedEvent();
                tabComponent.powerButton.onClick.AddListener(tabButtonClick);
                tabComponent.tipKey = "tab_cw";
                ReflectionUtility.Reflection.SetField<GameObject>(tabComponent, "parentObj", copyTab.transform.parent.parent.gameObject);

                newTab.transform.Find("Background").gameObject.GetComponent<Image>().sprite = Sprites.LoadSprite($"Mods/Cultivation Way/EmbededResources/iconClock+10.png");

                newTab.SetActive(true);
            }
        }
        public static void createButtons()
        {
            PowerButton button = PowerButtons.CreateButton("AboutThis", Resources.Load<Sprite>("ui/icons/iconabout"),
                "关于", "修真之路模组介绍", Vector3.zero, ButtonType.Click, null, clickForWindow_AboutThis);

            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("+10",Sprites.LoadSprite($"Mods/Cultivation Way/EmbededResources/iconClock+10.png"),
                "增加十倍速", "只增不减", Vector3.zero, ButtonType.Click, null, changeTimeScale);

            Utils.TabHelper.AddButtonToTab(button);

            Utils.TabHelper.AddLine();
        }

        public static void tabButtonClick()
        {
            GameObject tab = GameObjects.FindEvenInactive("Tab_Cultivation_Way");
            PowersTab powersTab = tab.GetComponent<PowersTab>();
            powersTab.showTab(powersTab.powerButton);
        }
        public static void changeTimeScale()
        {
            Config.timeScale += 10;
        }
        public static void clickForWindow_AboutThis()
        {
            Windows.ShowWindow("AboutThis");
        }
    }
}
