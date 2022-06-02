using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using CultivationWay;
using NCMS.Utils;
using ReflectionUtility;
using HarmonyLib;
namespace Cultivation_Way.Utils
{
    class WorldLawHelper
    {
        /// <summary>
        /// 存放初始数据，用于重建地图时
        /// </summary>
        internal static Dictionary<string, PlayerOptionData> originLaws = new Dictionary<string, PlayerOptionData>();
        private static float xStart = -75f;
        private static float yStart = 0f;
        private static float xOffset = 57.5f;
        private static float yOffset = -37f;
        private static Vector2 buttonScale = new Vector2(1.5f, 1.5f);

        private static GameObject originLawButton = GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/world_laws/Background/Scroll View/Viewport/Content/Units/Grid/world_law_old_age").GetComponent<WorldLawElement>().gameObject;
        public static void createWorldLaw(string id,string name,string description,Transform parent,UnityAction<WorldLawElement> action,int num,bool defaultActive = true)
        {
            GameObject newLawButton = null;

                originLawButton.SetActive(false);
                newLawButton = GameObject.Instantiate(originLawButton);
                originLawButton.SetActive(true);
            newLawButton.name = id;
            newLawButton.transform.SetParent(parent);
            newLawButton.transform.localPosition = getPos(num);
            newLawButton.transform.localScale = buttonScale;

            Image image = newLawButton.transform.Find("Button").Find("LawIcon").GetComponent<Image>();
            image.sprite = Resources.Load<Sprite>($"ui/Icons/{id}");

            WorldLawElement wle = newLawButton.GetComponent<WorldLawElement>();
            wle.icon.color = defaultActive ? Toolbox.makeColor("#FFFFFF") : Toolbox.makeColor("#666666");
            wle.toggleIcon.GetComponent<Image>().sprite = defaultActive ? wle.toggleIcon.spriteON : wle.toggleIcon.spriteOFF;
            Button newLawButton_button = newLawButton.transform.Find("Button").GetComponent<Button>();
            newLawButton_button.onClick = new Button.ButtonClickedEvent();
            newLawButton_button.onClick.AddListener(() => action(wle));

                PlayerOptionData data = new PlayerOptionData(id) { boolVal = defaultActive };
            
                originLaws.Add(id, data);
            newLawButton.SetActive(true);
        }
        private static Vector2 getPos(int num)
        {
            float x = xStart + (num % 5) * xOffset;
            float y = yStart + (num / 5) * yOffset;
            return new Vector2(x, y);
        }
        [HarmonyPostfix]
        [HarmonyPatch(typeof(WorldLaws),"init")]
        public static void initWorldLaws()
        {
            foreach(string id in originLaws.Keys)
            {
                MapBox.instance.worldLaws.dict[id]= originLaws[id];
                
            }
        }
    }
}
