﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using NCMS.Utils;
using CultivationWay;
using UnityEngine.UI;
using ReflectionUtility;
using System.Runtime.InteropServices;

namespace Cultivation_Way
{
    public class WindowTops : MonoBehaviour
    {
		private static WindowTops instance;

		private static ScrollWindow windowTop;

        public static RectTransform rectTransform;

		public static RectTransform rectTransform1;

		private static List<GameObject> elements = new List<GameObject>();

		private static List<Actor> actors = new List<Actor>();

		private static List<PowerButton> buttons = new List<PowerButton>();

		private static ActorStatus actorData;

        private float offset = 44f;

		private int curWidth;

		private int curHeight;

		private const int maxWidth = 1938;

		private const int maxHeight = 1038;
		[DllImport("User32.dll", EntryPoint = "FindWindow")]
		private static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		static extern bool GetWindowRect(IntPtr hWnd, ref RECT lpRect);

		[StructLayout(LayoutKind.Sequential)]
		public struct RECT
		{
			public int Left; //最左坐标
			public int Top; //最上坐标
			public int Right; //最右坐标
			public int Bottom; //最下坐标
		}
		public static void init()
        {
            windowTop = Windows.CreateNewWindow("windowTop","天榜");
			windowTop.transform.Find("Background").Find("Scroll View").gameObject.SetActive(true);

			GameObject gameObject = GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/windowTop/Background/Scroll View/Viewport");
			GameObject gameObject1 = GameObject.Find("/Canvas Container Main/Canvas - Windows/windows/windowTop/Background/Scroll View/Viewport/Content");

			rectTransform = gameObject.GetComponent<RectTransform>();
			rectTransform1 = gameObject1.GetComponent<RectTransform>();

			rectTransform.sizeDelta = new Vector2(0f, 20f);
			rectTransform1.pivot = new Vector2(0f, 1f);

			PowerButton button = PowerButtons.CreateButton("combatRank",Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/stats/iconDamage.png"),
															"战力榜",
															"",
															new Vector3(120f, 35f), ButtonType.Click,windowTop.transform,
															setCombat);
			button.GetComponent<Image>().sprite = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/backButtonRight.png");

			PowerButton button1 = PowerButtons.CreateButton("talentRank", Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/stats/iconTalent.png"),
															"天资榜",
															"",
															new Vector3(-120f, 35f), ButtonType.Click, windowTop.transform,
															setTalent);
			button1.GetComponent<Image>().sprite = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/backButtonLeft.png");
			//foreach(string scrollWindow in NCMS.Utils.Windows.AllWindows.Keys)
			//         {
			//	print(scrollWindow);
			//         }
		}
		public void set()
		{
			clear();
			actors.Clear();
			instance = this;
			RECT fx = new RECT();
			IntPtr hWnd = FindWindow(null,"WorldBox");
			GetWindowRect(hWnd, ref fx);

			instance.curHeight = fx.Bottom - fx.Top;
			instance.curWidth = fx.Right - fx.Left;

			foreach (Actor actor in MapBox.instance.units)
			{
                if (actor.stats.id.StartsWith("summon")||!actor.GetData().alive)
                {
					continue;
                }
				actors.Add(actor);
			}
			actors.Sort((a1,a2) =>{
				ActorStatus data1 = a1.GetData();
				ActorStatus data2 = a2.GetData();
				if (data1.level> data2.level)
                {
					return -1;
                }
				else if (data1.level == data2.level)
                {
                    if (data1.kills > data2.kills)
                    {
						return -1;
                    }
                    else if(data1.kills ==data2.kills)
                    {
                        if (data1.age > data2.age)
                        {
							return 1;
                        }
                        else if(data1.age==data2.age)
                        {
                            if (data1.GetHashCode()>data2.GetHashCode())
                            {
								return 1;
                            }
                            else
                            {
								return -1;
                            }
                        }
                        else
                        {
							return -1;
                        }
                    }
                    else
                    {
						return 1;
                    }
                }
                else
                {
					return 1;
                }
			});
			int index = 0;
			foreach (Actor pActor in actors)
			{
				actorData = pActor.GetData();
				showElement(pActor,index);
				index++;
                if (index > 9)
                {
					break;
                }
			}
			rectTransform1.sizeDelta = new Vector2(0f, (float)elements.Count * this.offset + 50f);//滚轮
		}
		public static void setCombat()
        {
			instance.clear();
			actors.Sort((a1, a2) => {
				ActorStatus data1 = a1.GetData();
				ActorStatus data2 = a2.GetData();
				if (data1.level > data2.level)
				{
					return -1;
				}
				else if (data1.level == data2.level)
				{
					if (data1.kills > data2.kills)
					{
						return -1;
					}
					else if (data1.kills == data2.kills)
					{
						if (data1.age > data2.age)
						{
							return 1;
						}
						else if (data1.age == data2.age)
						{
							if (data1.GetHashCode() > data2.GetHashCode())
							{
								return 1;
							}
							else
							{
								return -1;
							}
						}
						else
						{
							return -1;
						}
					}
					else
					{
						return 1;
					}
				}
				else
				{
					return 1;
				}
			});
			int index = 0;
			foreach (Actor pActor in actors)
			{
				actorData = pActor.GetData();
				if (!actorData.alive)
				{
					continue;
				}
				instance.showElement(pActor, index);
				index++;
				if (index > 9)
				{
					break;
				}
			}
			rectTransform1.sizeDelta = new Vector2(0f, (float)elements.Count * instance.offset + 50f);//滚轮
		}
		public static void setTalent()
        {
			instance.clear();
			actors.Sort((a1, a2) => {
				ChineseElement element1 = Main.instance.actorToMoreStats[a1.GetData().actorID].element;
				ChineseElement element2 = Main.instance.actorToMoreStats[a2.GetData().actorID].element;
                if (element1.element.rarity > element2.element.rarity)
                {
					return -1;
                }
				else if(element1.element.rarity == element2.element.rarity)
                {
                    if (element1.getPurity() < element2.getPurity())
                    {
						return -1;
                    }
                    else
                    {
						return 1;
                    }
                }
                else
                {
					return 1;
                }
			});
			int index = 0;
			foreach (Actor pActor in actors)
			{
				actorData = pActor.GetData();
				if (!actorData.alive)
				{
					continue;
				}
				instance.showElement(pActor, index);
				index++;
				if (index > 9)
				{
					break;
				}
			}
			rectTransform1.sizeDelta = new Vector2(0f, (float)elements.Count * instance.offset + 50f);//滚轮
		}
		private void clear()
        {
			while (elements.Count > 0)
			{
				GameObject gameObject = elements[elements.Count - 1];
				elements.RemoveAt(elements.Count - 1);
                Destroy(gameObject);
			}
		}
		private void showElement(Actor pActor,int index)
		{
			GameObject actorBackground = setBackground(index);
			setInspect(pActor,actorBackground);
			setAvator(pActor, actorBackground);
            setHealth(pActor, actorBackground);
            setDamage(pActor, actorBackground);
            setLevel(pActor, actorBackground);
            setName(pActor, actorBackground);
        }
		private GameObject setBackground(int index)
        {
			GameObject gameObject = new GameObject("actorBackgound");
			elements.Add(gameObject);

			gameObject.transform.SetParent(rectTransform1.transform);
			Image image = gameObject.AddComponent<Image>();
			image.sprite = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/windowInnerSlicedCut.png");
			gameObject.transform.localPosition = new Vector3(35f, -((float)elements.Count * this.offset - 20f));
			gameObject.transform.localScale = new Vector3(1.9f, 0.25f, 0f);
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.pivot = new Vector2(0f, 1f);
			return gameObject;
		}
		private void setInspect(Actor pActor,GameObject parent)
        {
			GameObject gameObject = new GameObject("inspectActor");
			gameObject.transform.SetParent(parent.transform);

			Image image = gameObject.AddComponent<Image>();
			image.sprite = Resources.Load<Sprite>("ui/icons/iconInspect");

			InspectActor inspectActor = gameObject.AddComponent<InspectActor>();
			inspectActor.actor = pActor;
			inspectActor.icon = image;
			inspectActor.transform.localScale = new Vector3(0.08f,0.5f);
			
			RectTransform component = gameObject.GetComponent<RectTransform>();
			component.pivot = new Vector2(0f, 1f);
			component.sizeDelta = new Vector2(75f, 75f);
			gameObject.transform.localPosition = new Vector3(85f, 0f, 0f);

		}
        private void setAvator(Actor actor,GameObject parent)
        {
			
			//框
			GameObject actorAvator1 = new GameObject("actorAvator");
			actorAvator1.transform.SetParent(parent.transform);
			Image image1 = actorAvator1.AddComponent<Image>();
			image1.sprite = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/windowAvatarElement.png");
			actorAvator1.transform.localScale = new Vector3(0.15f, 1.2f);
			actorAvator1.transform.localPosition = new Vector3(6f, -50f, 0f);

			//人
			GameObject actorAvator = new GameObject("actorAvator");
			actorAvator.transform.SetParent(parent.transform);
			if (!actor.stats.specialAnimation)
			{
				actor.CallMethod("updateAnimation",0f, true);
				((SpriteAnimation)Reflection.GetField(typeof(Actor), actor, "spriteAnimation")).setFrameIndex(0);
			}
			actor.forceAnimation();
			try
			{
				actor.CallMethod("checkSpriteConstructor");
			}
            catch
            {
				actor.killHimself();
            }
			Image image = actorAvator.AddComponent<Image>();
			image.sprite = ((SpriteRenderer)Reflection.GetField(typeof(Actor), actor, "spriteRenderer")).sprite;
			actorAvator.transform.localScale = new Vector3(actor.stats.inspectAvatarScale*0.029f, actor.stats.inspectAvatarScale*0.256f, actor.stats.inspectAvatarScale);
			actorAvator.transform.localPosition = new Vector3(6f, -58f, 0f);

			//RectTransform component = actorAvator.GetComponent<RectTransform>();
			//component.pivot = new Vector2(0f, 1f);
			//Button button = actorAvator.AddComponent<Button>();
			//string name = "dddddddd";
			//string description = "dddd";
			//Localization.addLocalization(name, "名");
			//Localization.addLocalization(description, "描述");
			//button.OnHover(delegate
			//{
			//	Tooltip.instance.show(actorAvator.gameObject, "normal", name, description);
			//});
		}
		private void setName(Actor actor, GameObject parent)
        {
			//GameObject gameObject = new GameObject("actorName");
			//gameObject.transform.SetParent(parent.transform);
			//Text name = gameObject.AddComponent<Text>();
			//name.text = actorData.firstName;
			//name.color = Color.red;
			//name.fontSize = 10;

			//gameObject.transform.localPosition = new Vector3(20f, -30f);
			
			//Copied from NCMS(Nickon)
			string elemName = "actorName";
			Vector3 elemPosition = new Vector3(35f, -12f, 0f);
			Color fontColor = Color.red;
			HorizontalWrapMode fontWrap = HorizontalWrapMode.Overflow;
			string textStr = actorData.firstName;
			if (!actor.kingdom.name.Contains("a")&&!actor.kingdom.name.Contains("u")&&!actor.kingdom.name.Contains("_"))
			{ 
				textStr = textStr + "(" + actor.kingdom.name + ")"; 
			}
			Vector2 sizeDelta = new Vector2(100f, 100f);
			setText(elemName, elemPosition, 18*curWidth/maxWidth, fontColor, fontWrap, textStr, sizeDelta, parent);
		}
		private void setLevel(Actor actor, GameObject parent)
        {
			
			int level = actorData.level;
			if (level > 10)
			{
				level = (level + 9) / 10 + 9;
			}
			GameObject gameObject = new GameObject("actorLevel");
			gameObject.transform.SetParent(parent.transform);

			Image image = gameObject.AddComponent<Image>();
			image.sprite = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/stats/iconLevels.png");
			image.transform.localScale = new Vector3(0.07f, 0.58f);

			GameObject gameObject2 = new GameObject("actorLevelText");
			gameObject2.transform.SetParent(gameObject.transform);
			Text text = gameObject2.AddComponent<Text>();
			text.font = Resources.Load<Font>("fonts/MPLUSRounded1c-Medium");
			text.fontSize = 18 * curWidth / maxWidth;
			text.alignment = TextAnchor.UpperLeft;
			text.color = Color.yellow;
			text.horizontalOverflow = HorizontalWrapMode.Overflow;
			text.text = ((CultisystemLibrary)AssetManager.instance.dict["cultisystem"]).get(Main.instance.actorToMoreStats[actorData.actorID].cultisystem).realms[level - 1] + "(" + level.ToString() + ")";
			RectTransform component = gameObject2.GetComponent<RectTransform>();
			component.pivot = new Vector2(0f, 1f);
			component.sizeDelta = new Vector2(100f, 100f);
			gameObject2.transform.localPosition = new Vector3(34f, 28f);

			gameObject.transform.localPosition = new Vector3(60f,-70f);
		}
		private void setHealth(Actor actor, GameObject parent)
        {
			int maxHealth = actor.GetCurStats().health;
			int fontsize = 18;
            if (maxHealth > 1000)
            {
				fontsize -= 2 * (maxHealth.ToString().Length - 3);

			}
			fontsize = fontsize * curWidth / maxWidth;
			GameObject gameObject = new GameObject("actorHealth");
			gameObject.transform.SetParent(parent.transform);

			Image image = gameObject.AddComponent<Image>();
			image.sprite = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/stats/iconHealth.png");
			image.transform.localScale = new Vector3(0.07f, 0.58f);

			GameObject gameObject2 = new GameObject("actorHealthText");
			gameObject2.transform.SetParent(gameObject.transform);
			Text text = gameObject2.AddComponent<Text>();
			text.font = Resources.Load<Font>("fonts/MPLUSRounded1c-Medium");
			text.fontSize = fontsize;
			text.alignment = TextAnchor.UpperLeft;
			text.color = Color.yellow;
			text.horizontalOverflow = HorizontalWrapMode.Overflow;
			text.text = actorData.health + "/" + maxHealth;
			RectTransform component = gameObject2.GetComponent<RectTransform>();
			component.pivot = new Vector2(0f, 1f);
			component.sizeDelta = new Vector2(100f, 100f);
			gameObject2.transform.localPosition = new Vector3(28f, 28f);

			gameObject.transform.localPosition = new Vector3(20f, -74f);
		}
		private void setDamage(Actor actor, GameObject parent)
        {

			GameObject gameObject = new GameObject("actorHealth");
			gameObject.transform.SetParent(parent.transform);

			Image image = gameObject.AddComponent<Image>();
			image.sprite = Sprites.LoadSprite($"{Main.mainPath}/EmbededResources/icons/stats/iconDamage.png");
			image.transform.localScale = new Vector3(0.07f, 0.58f);

			GameObject gameObject2 = new GameObject("actorHealthText");
			gameObject2.transform.SetParent(gameObject.transform);
			Text text = gameObject2.AddComponent<Text>();
			text.font = Resources.Load<Font>("fonts/MPLUSRounded1c-Medium");
			text.fontSize = 18 * curWidth / maxWidth;
			text.alignment = TextAnchor.UpperLeft;
			text.color = Color.yellow;
			text.horizontalOverflow = HorizontalWrapMode.Overflow;
			text.text = actor.GetCurStats().damage.ToString();
			RectTransform component = gameObject2.GetComponent<RectTransform>();
			component.pivot = new Vector2(0f, 1f);
			component.sizeDelta = new Vector2(100f, 100f);
			gameObject2.transform.localPosition = new Vector3(28f, 28f);

			gameObject.transform.localPosition = new Vector3(45f, -72f);
		}
		private static GameObject setText(string elemName, Vector3 elemPosition, int fontSize, Color fontColor, HorizontalWrapMode fontWrap, string textStr, Vector2 sizeDelta, GameObject modBackgound)
		{
			//Copied from NCMS(Nickon)
			GameObject gameObject = new GameObject(elemName + "Wrap");
			gameObject.transform.SetParent(modBackgound.transform);
			gameObject.transform.localPosition = elemPosition;
			GameObject gameObject2 = new GameObject(elemName);
			gameObject2.transform.SetParent(gameObject.transform);
			Text text = gameObject2.AddComponent<Text>();
			text.font = Resources.Load<Font>("fonts/MPLUSRounded1c-Medium");
			text.fontSize = fontSize;
			text.alignment = TextAnchor.UpperLeft;
			text.color = fontColor;
			text.horizontalOverflow = fontWrap;
			text.text = textStr;
			RectTransform component = gameObject2.GetComponent<RectTransform>();
			component.pivot = new Vector2(0f, 1f);
			component.sizeDelta = sizeDelta;
			gameObject2.transform.localPosition = new Vector3(0f, 0f, 0f);
			return gameObject;
		}

		public void clickInspect()
        {

        }
		public WindowTops()
		{
		}
	}
}
