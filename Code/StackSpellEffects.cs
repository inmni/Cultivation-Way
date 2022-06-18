using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    /// <summary>
    /// Copied from game
    /// </summary>
    internal class StackSpellEffects : MonoBehaviour
    {
        internal SpriteAnimation spriteAnimation;

        internal MapObjectShadow shadow;

        internal Vector3 lastShadowPos;

        internal Vector3 lastShadowScale;

        public WorldTile currentTile;

        public Vector2 currentPosition;

        public Vector3 currentScale;

        internal bool created;

        protected MapBox world;

        internal Transform m_transform;

        internal GameObject m_gameObject;

        public BaseSpellEffectController prefabController;

        internal Dictionary<string, BaseSpellEffectController> dictionary;

        internal List<BaseSpellEffectController> list;

        private float timeOutFireworks;

        internal void Awake()
        {
            world = MapBox.instance;
            dictionary = new Dictionary<string, BaseSpellEffectController>();
            list = new List<BaseSpellEffectController>();
            m_gameObject = new GameObject("stackSpellEffects");
            prefabController = m_gameObject.AddComponent<BaseSpellEffectController>();
            loadEffect("example", true, 0.1f, "EffectsTop", 100);
            loadEffect("lightning", true, 0.1f, "EffectsTop", 100);
            loadEffect("lightningPunishment", true, 0.1f, "EffectsTop", 100);
            loadEffect("explosion", true, 0.1f, "EffectsTop", 100);
            loadEffect("swordsArray", true, 0.1f, "EffectsTop", 10);
            loadEffect("default_lightning", true, 0.1f, "EffectsTop", 100);
            loadEffect("default_fire", true, 0.1f, "EffectsTop", 100);
            loadEffect("JiaoDragon_laser", true, 0.1f, "EffectsTop", 10);
            loadEffect("goldBar", true, 0.1f, "EffectsTop", 10);
            loadEffect("goldBarDown", true, 0.1f, "EffectsTop", 10);
            loadEffect("happySpringFestival", true, 0.1f, "EffectsTop", 5);
            loadEffect("firework", true, 0.1f, "EffectsTop", 100);
            loadEffect("LXST", true, 0.1f, "EffectsTop", 100);
            loadEffect("XTDT", true, 0.1f, "EffectsTop", 100);
            loadEffect("HWMT", true, 0.1f, "EffectsTop", 100);
            loadEffect("FT", true, 0.1f, "EffectsTop", 100);
            loadEffect("summon", true, 0.1f, "EffectsTop", 100);
            loadEffect("summonTian", true, 0.1f, "EffectsTop", 100);
            //this.add(prefabLightning, "lightning", 100);
        }
        internal void Update()
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].update(world.getCurElapsed());
            }
            if (timeOutFireworks > 0f)
            {
                timeOutFireworks -= Time.deltaTime;
            }
        }
        private void loadEffect(string pName, bool pUseBasicPrefab = true, float pTimeBetweenFrames = 0.1f, string pSortingLayerName = "EffectsTop", int pLimit = 60)
        {
            string path;
            if (pUseBasicPrefab)
            {
                path = "effects/fx_basic";
            }
            else
            {
                path = "effects/" + pName;
            }
            GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>((GameObject)Resources.Load(path, typeof(GameObject)), base.transform);
            gameObject.transform.name = pName;
            gameObject.gameObject.SetActive(false);
            if (pUseBasicPrefab)
            {
                SpriteAnimation component = gameObject.GetComponent<SpriteAnimation>();
                component.timeBetweenFrames = pTimeBetweenFrames;
                component.frames = Utils.ResourcesHelper.loadAllSprite("effects/" + pName, 0.55f, 0.2f);
                ((SpriteRenderer)Reflection.GetField(typeof(SpriteAnimation), component, "spriteRenderer")).sortingLayerName = pSortingLayerName;
            }
            BaseSpellEffectController baseEffectController = add(gameObject, pName, pLimit);

            baseEffectController.addNewObject(gameObject.AddComponent<BaseSpellEffect>());
            baseEffectController.transform.SetParent(base.transform);
            baseEffectController.gameObject.SetActive(true);
        }
        private BaseSpellEffectController add(GameObject pPrefab, string pID, int pLimit = 0)
        {
            BaseSpellEffectController baseEffectController = UnityEngine.Object.Instantiate<BaseSpellEffectController>(prefabController);
            baseEffectController.create();
            baseEffectController.transform.parent = base.transform;
            baseEffectController.transform.name = pID;
            baseEffectController.prefab = pPrefab.transform;
            baseEffectController.objectLimit = pLimit;
            dictionary.Add(pID, baseEffectController);
            list.Add(baseEffectController);
            return baseEffectController;
        }
        internal BaseSpellEffectController get(string pID)
        {
            return dictionary[pID];
        }
        internal void clear()
        {
            for (int i = 0; i < list.Count; i++)
            {
                list[i].clear();
            }
        }
    }
}
