using HarmonyLib;
using NCMS.Utils;
using ReflectionUtility;
using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way
{
    internal class WindowCreatureInfoHelper
    {
        internal WindowCreatureInfo instance;

        public WindowCreatureInfoHelper i;

        public WindowCreatureInfoHelper()
        {
            i = this;
        }

        internal void showAttribute(Text pText, int pValue)
        {//根据效果修改颜色
            string text = pValue.ToString() ?? "";
            if (pValue < 4)
            {
                text = Toolbox.coloredText(text, Toolbox.color_negative, false);
            }
            else if (pValue >= 20)
            {
                text = Toolbox.coloredText(text, Toolbox.color_positive, false);
            }
            pText.text = text;
        }
        internal void showStat(string pID, object pValue)
        {//展示文本
            Text text = instance.text_description;
            text.text = text.text + LocalizedTextManager.getText(pID, null) + "\n";
            Text text2 = instance.text_values;
            text2.text = text2.text + ((pValue != null) ? pValue.ToString() : null) + "\n";
        }
        internal void showStat(string pID, object pValue, Color32? pColor)
        {//展示带颜色的文本
            Text text = instance.text_description;
            text.text = text.text + LocalizedTextManager.getText(pID, null) + "\n";
            Text text2 = instance.text_values;
            text2.text = text2.text + Toolbox.coloredString(pValue.ToString(), pColor) + "\n";
        }
        internal void clearPrevButtons()
        {//清除上次打开的所有按钮
            for (int i = 0; i < instance.traitsParent.childCount; i++)
            {
                Transform child = instance.traitsParent.GetChild(i);
                if (!(child.name == "Title"))
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
            }
            for (int j = 0; j < instance.equipmentParent.childCount; j++)
            {
                Transform child = instance.equipmentParent.GetChild(j);
                if (!(child.name == "Title"))
                {
                    UnityEngine.Object.Destroy(child.gameObject);
                }
            }
        }
        internal void loadCultiTraits()
        {//加载特质
            ExtendedActor selectedUnit = (ExtendedActor)Config.selectedUnit;
            int num = 0;
            ActorStatus data = selectedUnit.easyData;

            int count = data.traits.Count + 4;
            loadRaceButton(selectedUnit.stats.race, num, count);
            num++;
            loadRealmButton(selectedUnit, num, count, data.level);
            num++;
            loadElementButton(data.actorID, num, count, data.level);
            num++;
            loadCultivationBookButton(selectedUnit, num, count, data.level);
            num++;

            Localization.setLocalization("trait_element", AddAssetManager.chineseElementLibrary.get(selectedUnit.extendedData.status.chineseElement.id).name + "灵根");
            Localization.setLocalization("trait_cultivationBook", (selectedUnit.extendedData.status.cultisystem=="default"||!selectedUnit.extendedData.status.canCultivate)?
                                                                            "无":selectedUnit.extendedData.status.cultiBook.bookName+ selectedUnit.extendedData.status.cultiBook.rank.ToString()) ;
            if (data.traits != null)
            {
                for (int i = 0; i < data.traits.Count; i++)
                {
                    loadNormalTraitButton(data.traits[i], num, count);
                    num++;
                }
            }
        }
        internal void loadNormalTraitButton(string pID, int pIndex, int pTotal)
        {
            TraitButton traitButton = UnityEngine.Object.Instantiate<TraitButton>(instance.prefabTrait, instance.traitsParent);
            traitButton.CallMethod("load", pID);
            RectTransform component = traitButton.GetComponent<RectTransform>();
            float num = 10f;
            float num2 = 22.4f;
            float num3 = 136f - num * 1.5f;
            float num4 = num2 * 0.7f;
            if ((float)pTotal * num4 >= num3)
            {
                num4 = num3 / (float)pTotal;
            }
            float x = num + num4 * (float)pIndex;
            float y = -11f;
            component.anchoredPosition = new Vector2(x, y);
        }
        internal void loadRaceButton(string raceID, int pIndex, int pTotal)
        {
            TraitButton traitButton = UnityEngine.Object.Instantiate<TraitButton>(instance.prefabTrait, instance.traitsParent);


            Reflection.SetField(traitButton, "trait_asset", AssetManager.traits.get("race"));
            Race race = AssetManager.raceLibrary.dict[raceID];
            if (!AssetManager.raceLibrary.dict[raceID].civilization)
            {
                Localization.setLocalization("trait_race", "其他");
            }
            else
            {
                Localization.setLocalization("trait_race", LocalizedTextManager.getText(AssetManager.raceLibrary.dict[raceID].nameLocale));
            }
            Sprite sprite;
            try
            {
                sprite = Resources.Load<Sprite>(race.path_icon);
            }
            catch(Exception)
            {
                sprite = Resources.Load<Sprite>("ui/Icons/iconOther");
            }
            traitButton.transform.Find("icon").GetComponent<Image>().sprite = sprite;

            RectTransform component = traitButton.GetComponent<RectTransform>();
            float num = 10f;
            float num2 = 22.4f;
            float num3 = 136f - num * 1.5f;
            float num4 = num2 * 0.7f;
            if ((float)pTotal * num4 >= num3)
            {
                num4 = num3 / (float)pTotal;
            }
            float x = num + num4 * (float)pIndex;
            float y = -11f;
            component.anchoredPosition = new Vector2(x, y);
        }
        internal void loadRealmButton(ExtendedActor actor, int pIndex, int pTotal, int level)
        {
            //境界贴图名由路线+等级组成
            //功法贴图固定
            //灵根贴图固定
            //其他待定

            MoreStats moreStats = actor.extendedCurStats;
            TraitButton traitButton = UnityEngine.Object.Instantiate<TraitButton>(instance.prefabTrait, instance.traitsParent);
            StringBuilder description = new StringBuilder();
            if (actor.extendedData.status.spells.Count != 0)
            {
                description.Append("法术\n");

                foreach (ExtendedSpell spell in actor.extendedData.status.spells)
                {
                    description.Append("\n" + spell.GetSpellAsset().name);
                }
            }
            Localization.setLocalization("trait_realm_info", description.ToString());

            Reflection.SetField(traitButton, "trait_asset", AssetManager.traits.get("realm"));

            Sprite sprite = Resources.Load<Sprite>("ui/Icons/" + actor.extendedData.status.cultisystem + "_" + 1);
            traitButton.transform.Find("icon").GetComponent<Image>().sprite = sprite;
            Localization.setLocalization("trait_realm", actor.getRealmName());

            RectTransform component = traitButton.GetComponent<RectTransform>();
            float num = 10f;
            float num2 = 22.4f;
            float num3 = 136f - num * 1.5f;
            float num4 = num2 * 0.7f;
            if ((float)pTotal * num4 >= num3)
            {
                num4 = num3 / (float)pTotal;
            }
            float x = num + num4 * (float)pIndex;
            float y = -11f;
            component.anchoredPosition = new Vector2(x, y);
        }
        internal void loadCultivationBookButton(Actor actor, int pIndex, int pTotal, int level)
        {
            ExtendedActor extendedActor = (ExtendedActor)actor;
            TraitButton traitButton = UnityEngine.Object.Instantiate<TraitButton>(instance.prefabTrait, instance.traitsParent);
            StringBuilder description = new StringBuilder();
            List<ExtendedSpell> spells = extendedActor.extendedData.status.spells;
            for (int i = 0; i < spells.Count; i++)
            {
                description.Append("\n");
                description.Append(spells[i].GetSpellAsset().name);
                description.Append("    ");
                description.Append(spells[i].might);
            }
            if (spells.Count > 0)
            {
                Localization.setLocalization("trait_cultivationBook_info", "法术\n" + description.ToString());
            }
            else
            {
                Localization.setLocalization("trait_cultivationBook_info", string.Empty);
            }

            Reflection.SetField(traitButton, "trait_asset", AssetManager.traits.get("cultivationBook"));
            Sprite sprite = Resources.Load<Sprite>("ui/Icons/iconCultivationBook");
            traitButton.transform.Find("icon").GetComponent<Image>().sprite = sprite;

            RectTransform component = traitButton.GetComponent<RectTransform>();
            float num = 10f;
            float num2 = 22.4f;
            float num3 = 136f - num * 1.5f;
            float num4 = num2 * 0.7f;
            if ((float)pTotal * num4 >= num3)
            {
                num4 = num3 / (float)pTotal;
            }
            float x = num + num4 * (float)pIndex;
            float y = -11f;
            component.anchoredPosition = new Vector2(x, y);
        }
        internal void loadElementButton(string actorID, int pIndex, int pTotal, int level)
        {

            TraitButton traitButton = UnityEngine.Object.Instantiate<TraitButton>(instance.prefabTrait, instance.traitsParent);

            Localization.setLocalization("trait_element_info", "");

            Reflection.SetField(traitButton, "trait_asset", AssetManager.traits.get("element"));
            Sprite sprite = Resources.Load<Sprite>("ui/Icons/iconTalent");
            traitButton.transform.Find("icon").GetComponent<Image>().sprite = sprite;

            RectTransform component = traitButton.GetComponent<RectTransform>();
            float num = 10f;
            float num2 = 22.4f;
            float num3 = 136f - num * 1.5f;
            float num4 = num2 * 0.7f;
            if ((float)pTotal * num4 >= num3)
            {
                num4 = num3 / (float)pTotal;
            }
            float x = num + num4 * (float)pIndex;
            float y = -11f;
            component.anchoredPosition = new Vector2(x, y);
        }
        internal void loadEquipment()
        {
            Actor selectedUnit = Config.selectedUnit;
            List<ItemData> temp_equipment = ReflectionUtility.Reflection.GetField(typeof(WindowCreatureInfo), instance, "temp_equipment") as List<ItemData>;

            temp_equipment.Clear();
            instance.equipmentParent.gameObject.SetActive(false);
            if (selectedUnit.equipment == null)
            {
                return;
            }
            instance.equipmentParent.gameObject.SetActive(true);
            if (selectedUnit.equipment.weapon.data != null)
            {
                temp_equipment.Add(selectedUnit.equipment.weapon.data);
            }
            if (selectedUnit.equipment.helmet.data != null)
            {
                temp_equipment.Add(selectedUnit.equipment.helmet.data);
            }
            if (selectedUnit.equipment.armor.data != null)
            {
                temp_equipment.Add(selectedUnit.equipment.armor.data);
            }
            if (selectedUnit.equipment.boots.data != null)
            {
                temp_equipment.Add(selectedUnit.equipment.boots.data);
            }
            if (selectedUnit.equipment.ring.data != null)
            {
                temp_equipment.Add(selectedUnit.equipment.ring.data);
            }
            if (selectedUnit.equipment.amulet.data != null)
            {
                temp_equipment.Add(selectedUnit.equipment.amulet.data);
            }
            int num = 0;
            int count = temp_equipment.Count;
            if (temp_equipment.Count > 0)
            {
                for (int i = 0; i < temp_equipment.Count; i++)
                {
                    loadEquipmentButton(temp_equipment[i], num, count);
                    num++;
                }
            }
        }
        internal void loadEquipmentButton(ItemData pData, int pIndex, int pTotal)
        {
            EquipmentButton equipmentButton = UnityEngine.Object.Instantiate<EquipmentButton>(instance.prefabEquipment, instance.equipmentParent);
            equipmentButton.CallMethod("load", pData);
            RectTransform component = equipmentButton.GetComponent<RectTransform>();
            float num = 10f;
            float num2 = 22.4f;
            float num3 = 136f - num * 1.5f;
            float num4 = num2 * 0.8f;
            if ((float)pTotal * num4 >= num3)
            {
                num4 = num3 / (float)pTotal;
            }
            float x = num + num4 * (float)pIndex;
            float y = -11f;
            component.anchoredPosition = new Vector2(x, y);
        }
        internal void updateFavoriteIconFor(ExtendedActor pUnit)
        {
            if (pUnit.easyData.favorite)
            {
                instance.iconFavorite.color = Color.white;
                return;
            }
            instance.iconFavorite.color = new Color(1f, 1f, 1f, 0.5f);
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(WindowCreatureInfo), "loadTraits")]
        public static bool loadTraits_PrefixForConvert(WindowCreatureInfo __instance)
        {
            WindowCreatureInfoHelper helper = new WindowCreatureInfoHelper();
            helper.instance = __instance;
            helper.loadCultiTraits();
            return false;
        }
    }
}
