using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way
{
    internal class InspectActor : MonoBehaviour
    {
        private static Sprite OnIcon;

        private static Sprite OffIcon;

        public Image icon;

        public Actor actor;

        private void Awake()
        {
            Button button = base.gameObject.AddComponent<Button>();
            button.onClick.AddListener(clickInspect);
            OnIcon = Resources.Load<Sprite>("ui/icons/iconInspect");
            OffIcon = Resources.Load<Sprite>("ui/icons/iconInspect");

        }
        private void clickInspect()
        {
            Sfx.play("click");
            Config.selectedUnit = actor;
            ScrollWindow.moveAllToLeftAndRemove(true);
            ScrollWindow.showWindow("inspect_unit");
        }
    }
}
