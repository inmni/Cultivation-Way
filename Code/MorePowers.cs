using CultivationWay;
using NCMS.Utils;
using System.IO;
using System.Net;
using System.Text;
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

            button = PowerButtons.CreateButton("Update", Sprites.LoadSprite($"{Main.mainPath}/icon.png", -0.5f, -0.5f),
           "前往更新模组", "目前不支持自动更新", Vector3.zero, ButtonType.Click, null, clickForUpdate);
            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("CultivationWayLaw", Resources.Load<Sprite>("ui/icons/iconworldlaws"),
                "世界道则", "", Vector3.zero, ButtonType.Click, null, clickForWindow_MoreWorldLaws);

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
            toggleIcon.GetComponent<Image>().transform.localScale = new Vector3(0.5f, 0.5f, 0);
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
            button = PowerButtons.CreateButton("spawnEasternHuman", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconEasternHuman.png"),
                "东方人族", "", Vector3.zero, ButtonType.GodPower, null, null);
            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("spawnTian", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconTian.png"),
                "天族", "天，颠也。颠者，人之顶也。", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("spawnYao", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconYao.png"),
                "妖族", "妖者，盖精气之依物者也", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("spawnMing", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconMing.png"),
                "冥族", "冥者，明之藏也。", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);

            Utils.TabHelper.AddLine();

            button = PowerButtons.CreateButton("spawnEasternDragon", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconEasternDragon.png"),
                "龙", "", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("spawnFairyFox", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconFairyFox.png"),
                "仙狐", "", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);
            #region 妖圣
            button = PowerButtons.CreateButton("spawnMonkeySheng1", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconMonkeySheng1.png"),
                "齐天大圣", "", Vector3.zero, ButtonType.GodPower, null, null);
            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("spawnMonkeySheng2", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconMonkeySheng2.png"),
                "通风大圣", "", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);
            button = PowerButtons.CreateButton("spawnCowSheng", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconCowSheng.png"),
                "平天大圣", "", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);
            button = PowerButtons.CreateButton("spawnCatSheng", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconCatSheng.png"),
                "移山大圣", "", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);
            button = PowerButtons.CreateButton("spawnWolfSheng", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconWolfSheng.png"),
                "驱神大圣", "", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);
            button = PowerButtons.CreateButton("spawnSnakeSheng", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconSnakeSheng.png"),
                "覆海大圣", "", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);
            button = PowerButtons.CreateButton("spawnChickenSheng", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconChickenSheng.png"),
                "混天大圣", "", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);
            #endregion
            Utils.TabHelper.AddLine();
            #endregion

            #region BOSS
            button = PowerButtons.CreateButton("spawnJiaoDragon", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconJiaoDragon.png"),
                "蛟龙", "蛟，龙之属也。", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);

            //button = PowerButtons.CreateButton("spawnXieDragon", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/iconXieDragon.png"),
            //    "邪龙", "我是你们爸爸", Vector3.zero, ButtonType.GodPower, null, null);

            //Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("spawnFuRen", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconFuRen.png"),
                "福人", "", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);

            button = PowerButtons.CreateButton("spawnNian", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconNian.png"),
                "年","", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);
            #endregion

            #region 彩蛋
            button = PowerButtons.CreateButton("spawnMengZhu", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/actors/iconMengZhu.png"),
                "盟主", "変態盟主", Vector3.zero, ButtonType.GodPower, null, null);

            Utils.TabHelper.AddButtonToTab(button);
            #endregion
        }
        public static void clickForWindow_MoreWorldLaws()
        {
            Windows.ShowWindow("window_MoreWorldLaws");
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

            foreach(string id in Main.instance.actorToMoreData.Keys)
            {
                Actor a = MapBox.instance.getActorByID(id);
                if (a== null)
                {
                    MonoBehaviour.print("[MorePowers]actorToMoreData存在未删除数据"+id);
                }
                else
                {
                    MonoBehaviour.print("[MorePowers]actorToMoreData过早删除数据"+id+"\n"+a.GetData().statsID);

                }
            }
        }
        public static void clickForWindow_AboutThis()
        {
            Windows.ShowWindow("window_AboutThis");
        }
        public static void clickForWindow_Top()
        {
            WindowTops windowTops = new WindowTops();
            windowTops.set();
            Windows.ShowWindow("window_Top");
        }
        public static void clickForUpdate()
        {
            //Thread t = new Thread(new ThreadStart(update));
            //t.Start();
            update();
        }

        public static void update()
        {
            string webSite = @"https://www.worldboxmod.com/forum.php?mod=viewthread&tid=280&extra=page%3D1"; //这里url必须带上协议
            Application.OpenURL(webSite);
            //var wc = new WebClient();
            //wc.DownloadFile("link", $"D:\\Steam\\steamapps\\common\\worldbox\\{text}.rar");



        }
        public static string Fetch(string url, string charset)
        {
            Encoding encoding;
            HttpWebRequest request;
            HttpWebResponse response = null;
            Stream resStream = null;
            StreamReader sr = null;
            string result = string.Empty;
            try
            {
                request = (HttpWebRequest)HttpWebRequest.Create(url);
                response = (HttpWebResponse)request.GetResponse();
                resStream = response.GetResponseStream();
                if (!string.IsNullOrEmpty(charset))
                {
                    encoding = Encoding.GetEncoding(charset);
                }
                else if (!string.IsNullOrEmpty(response.CharacterSet))
                {
                    encoding = Encoding.GetEncoding(response.CharacterSet);
                }
                else
                {
                    encoding = Encoding.Default;
                }
                sr = new StreamReader(resStream, encoding);
                result = sr.ReadToEnd();
            }
            //catch (Exception ex)          
            //{             
            //    throw ex;
            //}            
            finally
            {
                if (sr != null)
                {
                    sr.Close();
                }
                if (resStream != null)
                {
                    resStream.Close();
                }
                if (response != null)
                {
                    response.Close();
                }
            }
            return result;
        }
    }
}

