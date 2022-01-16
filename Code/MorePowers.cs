using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;
using CultivationWay;
using System.Linq;
using System.Collections.Generic;
using System;

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
                newTabButton.transform.Find("Icon").GetComponent<Image>().sprite = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconTab.png");

                //设置栏内元素

                GameObject copyTab = GameObjects.FindEvenInactive("Tab_Other");
                //暂时禁用copyTab内元素
                foreach (Transform transform in copyTab.transform)
                {
                    transform.gameObject.SetActive(false);
                }

                newTab = GameObject.Instantiate(copyTab);
                //删除复制来的无用元素
                foreach (Transform transform in newTab.transform)
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


                newTab.SetActive(true);
            }
        }
        public static void createButtons()
        {
            #region 总体
            PowerButton button = PowerButtons.CreateButton("AboutThis", Resources.Load<Sprite>("ui/icons/iconabout"),
                "关于", "修真之路模组介绍", Vector3.zero, ButtonType.Click, null, clickForWindow_AboutThis);

            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("Top", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconTop.png"),
                "天榜", "", Vector3.zero, ButtonType.Click, null, clickForWindow_Top);

            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("checkElement", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconCheckElement.png"),
                "天地灵气", "", Vector3.zero, ButtonType.GodPower, null, null);
            button.type = PowerButtonType.Special;
            #region 一些处理
            GameObject toggleIcon = new GameObject("ToggleIcon");
            toggleIcon.AddComponent<Image>();
            toggleIcon.AddComponent<ToggleIcon>();
            toggleIcon.GetComponent<Image>().sprite = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconCheckElementOff.png");
            toggleIcon.GetComponent<ToggleIcon>().spriteON = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconCheckElementOn.png");
            toggleIcon.GetComponent<ToggleIcon>().spriteOFF = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconCheckElementOff.png");
            toggleIcon.transform.SetParent(button.transform);

            #endregion
            Utils.TabHelper.AddButtonToTab(button);

            Utils.TabHelper.AddLine();
            #endregion

            #region 小功能
            button = PowerButtons.CreateButton("+10", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconClock+10.png"),
                "增加十倍速", "呼~", Vector3.zero, ButtonType.Click, null, addTimeScale);

            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("-1", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconClock-1.png"),
                "减少一倍速", "强迫症福音", Vector3.zero, ButtonType.Click, null, minusTimeScale);

            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("exp", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconExp.png"),
                "帝流浆", "万道金丝，纍纍贯串。", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);

            Utils.TabHelper.AddLine();
            #endregion

            #region 生物
            button = PowerButtons.CreateButton("spawnTian", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconTian.png"),
                "天族", "天，颠也。颠者，人之顶也。", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("spawnMing", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconMing.png"),
                "冥族", "冥者，明之藏也。", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);

            Utils.TabHelper.AddLine();

            button = PowerButtons.CreateButton("spawnEasternDragon", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconEasternDragon.png"),
                "龙", "", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("spawnFairyFox", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconFairyFox.png"),
                "仙狐", "", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);

            Utils.TabHelper.AddLine();
            #endregion

            #region BOSS
            button = PowerButtons.CreateButton("spawnJiaoDragon", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconJiaoDragon.png"),
                "蛟龙", "蛟，龙之属也。", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);

            //button = PowerButtons.CreateButton("spawnXieDragon", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconXieDragon.png"),
            //    "邪龙", "我是你们爸爸", Vector3.zero, ButtonType.GodPower, null, null);

            //Utils.TabHelper.AddButtonToTab(button);
            #endregion

            #region 彩蛋
            button = PowerButtons.CreateButton("spawnMengZhu", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconMengZhu.png"),
                "盟主", "変態盟主", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);
            #endregion
        }

        public static void tabButtonClick()
        {
            GameObject tab = GameObjects.FindEvenInactive("Tab_Cultivation_Way");
            PowersTab powersTab = tab.GetComponent<PowersTab>();
            powersTab.showTab(powersTab.powerButton);
        }
        public static void addTimeScale()
        {
            Config.timeScale += 10;
            WorldTip.showNow("当前时间流速为" + Config.timeScale + "倍", false, "top");
            
        }
        public static void minusTimeScale()
        {
            Config.timeScale -= 1;
            WorldTip.showNow("当前时间流速为" + Config.timeScale + "倍", false, "top");

            //int chunkID = Main.instance.chunkToElement.Keys.ToList().GetRandom();
            //MonoBehaviour.print("尝试清理:"+Main.instance.actorToMoreData.Count);

            //Dictionary<string, MoreActorData> actorToMoreData = new Dictionary<string, MoreActorData>(MapBox.instance.units.Count+500);
            //foreach (string id in Main.instance.actorToMoreData.Keys)
            //{
            //    if (MapBox.instance.getActorByID(id) != null)
            //    {
            //        actorToMoreData.Add(id, Main.instance.actorToMoreData[id]);
            //    }
            //}
            //Main.instance.actorToMoreData = actorToMoreData;
            //GC.Collect();
            //MonoBehaviour.print("=>" + Main.instance.actorToMoreData.Count);
            //MonoBehaviour.print("**************************");
            
            //MonoBehaviour.print("[修真之路Cultivation Way]:某区块五行元素含量依次为:");
            //for (int i = 0; i < 5; i++)
            //{
            //    MonoBehaviour.print(Main.instance.chunkToElement[chunkID].baseElementContainer[i]);
            //}
            //MonoBehaviour.print("**************************");
        }
        public static void clickForWindow_AboutThis()
        {
            Windows.ShowWindow("AboutThis");
        }
        public static void clickForWindow_Top()
        {
            WindowTops windowTops = new WindowTops();
            windowTops.set();
            Windows.ShowWindow("windowTop");
        }
    }
}
