using NCMS.Utils;
using UnityEngine;

namespace Cultivation_Way.Utils
{
    class TabHelper
    {
        private static PowersTab powersTab = GetPowersTab("Tab_Cultivation_Way");
        private static int Buttons = 0;
        private static float startX = 72f;
        private static float addX = 18f;
        private static float upY = 18f;
        private static float downY = -18f;
        private static float lineStep = 23f;

        public static void AddButtonToTab(PowerButton button)
        {

            ((Component)button).transform.SetParent(((Component)powersTab).transform);

            //计算位置
            Buttons++;
            float x = startX + (((Buttons - 1) >> 1) << 1) * addX;
            float y = Buttons % 2 == 1 ? upY : downY;


            Vector3 position = new Vector3(x, y);


            ((Component)button).transform.localPosition = position;
            ((Component)button).transform.localScale = new Vector3(1f, 1f);
        }
        public static void AddLine()
        {

            GameObject line = GameObject.Find("CanvasBottom/BottomElements/BottomElementsMover/CanvasScrollView/Scroll View/Viewport/Content/buttons/Tab_Other/LINE");
            GameObject addLine = GameObject.Instantiate(line, powersTab.transform);

            //计算位置
            float x = startX + lineStep + (((Buttons - 1) >> 1) << 1) * addX;
            addLine.transform.localPosition = new Vector2(x, addLine.transform.localPosition.y);

            startX += (lineStep - addX) * 2;
            //如果按钮数是奇数，则加一
            if (Buttons % 2 == 1)
            {
                Buttons++;
            }

        }
        private static PowersTab GetPowersTab(string tab)
        {
            GameObject val = GameObjects.FindEvenInactive(tab);
            return val.GetComponent<PowersTab>();
        }
    }
}
