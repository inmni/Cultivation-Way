using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ReflectionUtility;
using CultivationWay;
namespace Cultivation_Way
{
    internal class NewEffectManager : MonoBehaviour
    {
        private bool initialized = false;
        public static NewEffectManager instance;
        public static GameObject prefab = new GameObject();

        public List<NewEffectController> list;
        public Dictionary<string, NewEffectController> controllers = new Dictionary<string, NewEffectController>();

        private void Awake()
        {
            if (!initialized)
            {
                initialized = true;
                instance = this;
                instance.transform.SetParent(MapBox.instance.transform);
                prefab.transform.name = "basePrefab";
                prefab.transform.SetParent(this.transform);
                prefab.AddComponent<SpriteRenderer>();
                prefab.SetActive(false);
                
                load("example", 0.1f, "EffectsTop", 100);
                
                load("explosion", 0.1f, "EffectsTop", 100, "basicSpell/");
                load("happySpringFestival", 0.1f, "EffectsTop", 1, "basicSpell/");
                load("firework", 0.1f, "EffectsTop", 100, "basicSpell/");
                load("sunkens", 0.1f, "EffectsTop", 100, "basicSpell/");
                
                load("summon", 0.1f, "EffectsTop", 100, "singleSpell/");
                load("summonTian", 0.1f, "EffectsTop", 100, "singleSpell/");
                load("swordsArray", 0.1f, "EffectsTop", 10, "singleSpell/");
                load("default_lightning", 0.1f, "EffectsTop", 100, "singleSpell/");
                load("default_fire", 0.1f, "EffectsTop", 100, "singleSpell/");
                load("JiaoDragon_laser", 0.1f, "EffectsTop", 10, "singleSpell/");
                load("goldBar", 0.1f, "EffectsTop", 1, "singleSpell/");
                load("goldBarDown", 0.1f, "EffectsTop", 1, "singleSpell/");
                load("lightning", 0.1f, "EffectsTop", 100, "singleSpell/");
                load("lightningPunishment", 0.1f, "EffectsTop", 100, "singleSpell/");

                load("LXST", 0.1f, "EffectsTop", 100, "specialBody/");
                load("XTDT", 0.1f, "EffectsTop", 100, "specialBody/");
                load("HWMT", 0.1f, "EffectsTop", 100, "specialBody/");
                load("FT", 0.1f, "EffectsTop", 100, "specialBody/");


                loadToStackEffects("fx_plasma_trail_Tian", true, 0.1f, "EffectsTop", 100);
                list = controllers.Values.ToList();
            }
        }

        private void Update()
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].update(Time.fixedDeltaTime);
            }
        }
        private NewEffectController load(string id, float frameInterval, string layerOrder, int limit,string extendPath="")
        {
            NewEffectController controller = new NewEffectController();
            controller.create(Resources.LoadAll<Sprite>("effects/" + extendPath+id), limit, frameInterval, layerOrder);
            controllers[id] = controller;
            controller.prefab.transform.name = id;
            return controller;
        }
        private void loadToStackEffects(string id,bool useBasicPrefab=true,float frameInterval = 0.1f, string layerOrder="EffectsBack", int limit=60)
        {
            MapBox.instance.stackEffects.CallMethod("loadEffect", id, useBasicPrefab, frameInterval, layerOrder, limit);
        }
        public static NewSpriteAnimation spawnOn(string id,WorldTile pTile, Vector3 scale)
        {
            NewEffectController controller;
            if(instance.controllers.TryGetValue(id,out controller))
            {
                return controller.add(pTile, scale);
            }
            return null;
        }
        public static NewSpriteAnimation spawnOn(string id,BaseSimObject pObject, Vector3 scale)
        {
            NewEffectController controller;
            if (instance.controllers.TryGetValue(id, out controller))
            {
                return controller.add(pObject.gameObject.transform,scale);
            }
            return null;
        }
        public static NewSpriteAnimation spawnOn(string id, Vector3 pos, Vector3 scale)
        {
            NewEffectController controller;
            if (instance.controllers.TryGetValue(id, out controller))
            {
                return controller.add(pos, scale);
            }
            return null;
        }
        public static NewSpriteAnimation spawnOn(string id, WorldTile pTile, float scale)
        {
            return spawnOn(id, pTile, new Vector3(scale, scale, scale));
        }
        public static NewSpriteAnimation spawnOn(string id, BaseSimObject pObject, float scale)
        {
            return spawnOn(id, pObject, new Vector3(scale, scale, scale));
        }
        public static NewSpriteAnimation spawnOn(string id, Vector3 pos, float scale)
        {
            return spawnOn(id, pos, new Vector3(scale, scale, scale));
        }
        public void public_load(string id, float frameInterval, string layerOrder, int limit)
        {
            list.Add(load(id, frameInterval, layerOrder, limit));
        }
    }
}
