using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NCMS.Utils;
using UnityEngine;
using UnityEngine.UI;
using CultivationWay;
using ReflectionUtility;

namespace Cultivation_Way
{
    class MoreWorldLaws
    {
        private ScrollWindow window;
        private GameObject content;
        /// <summary>
        /// 修炼有关
        /// </summary>
        private GameObject Cultivation;
        /// <summary>
        /// 文明有关
        /// </summary>
        private GameObject Civilization;
        /// <summary>
        /// 世界
        /// </summary>
        private GameObject World;
        internal void init()
        {
            initWindow();
            initButton();
        }
        private void initWindow()
        {
            window = Windows.CreateNewWindow("window_MoreWorldLaws", "天地道则");
            content = window.transform.Find("Background").Find("Scroll View")
                                      .Find("Viewport").Find("Content").gameObject;
            for(int i = 0; i < content.transform.childCount; i++)
            {
                GameObject.Destroy(content.transform.GetChild(i).gameObject);
            }
            Reflection.CallStaticMethod(typeof(ScrollWindow), "checkWindowExist", "world_laws");
            GameObject worldLaws = GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/world_laws");
            worldLaws.SetActive(false);

            GameObject civ = GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/world_laws/Background/Scroll View/Viewport/Content/Civ");
            #region 文明栏
            Civilization = GameObject.Instantiate(civ);
            Civilization.name = "culti_Civilization";
            Civilization.transform.SetParent(content.transform);
            for (int i = 0; i < Civilization.transform.childCount; i++)
            {
                if (Civilization.transform.GetChild(i).gameObject.name != "Title")
                {
                    GameObject.Destroy(Civilization.transform.GetChild(i).gameObject);
                }
            }

            GameObject civTitle = Civilization.transform.Find("Title").gameObject;
            GameObject.Destroy(civTitle.GetComponent<LocalizedText>());
            civTitle.transform.GetComponent<Text>().text = "文明";

            Civilization.transform.localPosition = new Vector2(130f, -65f);
            Civilization.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
            #endregion
            #region 世界栏
            World = GameObject.Instantiate(civ);
            World.name = "culti_World";
            World.transform.SetParent(content.transform);
            for (int i = 0; i < World.transform.childCount; i++)
            {
                if (World.transform.GetChild(i).gameObject.name != "Title")
                {
                    GameObject.Destroy(World.transform.GetChild(i).gameObject);
                }
            }

            GameObject worldTitle = World.transform.Find("Title").gameObject;
            GameObject.Destroy(worldTitle.GetComponent<LocalizedText>());
            worldTitle.transform.GetComponent<Text>().text = "天地";

            World.transform.localPosition = new Vector2(130f, -100f);
            World.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 50f);
            #endregion
            window.transform.Find("Background").Find("Scroll View").gameObject.SetActive(true);
        }
        private void initButton()
        {
            Utils.WorldLawHelper.createWorldLaw("YaoKingdom", "妖族建国", "关闭后妖族将化为一盘散沙", Civilization.transform, simpleClick,0, true);
            Utils.WorldLawHelper.createWorldLaw("MoreDisasters", "更多天灾", "注意！！", World.transform, simpleClick,0, false);
        }
        private void simpleClick(WorldLawElement wle)
        {
            wle.click();
        }
    }
}
