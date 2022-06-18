using System.Collections.Generic;
using System.Linq;
using UnityEngine;
namespace Cultivation_Way
{
    internal class NewEffectManager : MonoBehaviour
    {
        private bool initialized = false;
        public static GameObject prefab = new GameObject();
        public List<NewEffectController> list;
        public Dictionary<string, NewEffectController> controllers = new Dictionary<string, NewEffectController>();

        private void Awake()
        {
            if (!initialized)
            {
                initialized = true;
                prefab.AddComponent<SpriteRenderer>();
                prefab.SetActive(false);
                load("example", 0.1f, "EffectsTop", 100);
                load("lightning", 0.1f, "EffectsTop", 100);
                load("lightningPunishment", 0.1f, "EffectsTop", 100);
                load("explosion", 0.1f, "EffectsTop", 100);
                load("swordsArray", 0.1f, "EffectsTop", 10);
                load("default_lightning", 0.1f, "EffectsTop", 100);
                load("default_fire", 0.1f, "EffectsTop", 100);
                load("JiaoDragon_laser", 0.1f, "EffectsTop", 10);
                load("goldBar", 0.1f, "EffectsTop", 10);
                load("goldBarDown", 0.1f, "EffectsTop", 10);
                load("happySpringFestival", 0.1f, "EffectsTop", 5);
                load("firework", 0.1f, "EffectsTop", 100);
                load("LXST", 0.1f, "EffectsTop", 100);
                load("XTDT", 0.1f, "EffectsTop", 100);
                load("HWMT", 0.1f, "EffectsTop", 100);
                load("FT", 0.1f, "EffectsTop", 100);
                load("summon", 0.1f, "EffectsTop", 100);
                load("summonTian", 0.1f, "EffectsTop", 100);
                list = controllers.Values.ToList();
            }
        }

        private void Update()
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].update(Time.deltaTime);
            }
        }
        private NewEffectController load(string id, float frameInterval, string layerOrder, int limit)
        {
            NewEffectController controller = new NewEffectController();
            controller.create(Resources.LoadAll<Sprite>("effects/" + id), limit, frameInterval, layerOrder);
            controllers[id] = controller;
            return controller;
        }


        public void public_load(string id, float frameInterval, string layerOrder, int limit)
        {
            list.Add(load(id, frameInterval, layerOrder, limit));
        }
    }
}
