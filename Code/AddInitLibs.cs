using UnityEngine;

namespace Cultivation_Way
{
    class AddInitLibs : MonoBehaviour
    {
        public static bool initiated = false;
        public static void initMyLibs()
        {
            if (initiated)
            {
                return;
            }
            initiated = true;
            ChineseNameGenerator.init();
        }
    }
}
