using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    /// <summary>
    /// Copied from game
    /// </summary>
    internal class BaseSpellEffectController : MonoBehaviour
    {

        public Transform prefab;//动画模板

        public float timer_interval = 1f;//播放间隔

        public bool useInterval = true;//启用播放间隔

        internal bool created;

        protected MapBox world;

        internal Transform m_transform;

        internal GameObject m_gameObject;

        internal int activeIndex;//正在播放的图像索引

        internal int objectLimit;//动画限制

        internal float timer;//计时器

        internal List<BaseSpellEffect> list;//动画列表

        private BaseSpellEffect switchObject;//下个对象

        internal void create()
        {
            created = true;
            m_gameObject = base.gameObject;
            m_transform = m_gameObject.transform;
            setWorld();
            list = new List<BaseSpellEffect>();
            timer_interval = 0.9f;
        }
        public BaseSpellEffect GetObject()
        {
            BaseSpellEffect baseEffect;
            if (list.Count > activeIndex)
            {
                baseEffect = list[activeIndex];
            }
            else
            {
                baseEffect = UnityEngine.Object.Instantiate<Transform>(prefab).gameObject.GetComponent<BaseSpellEffect>();
                addNewObject(baseEffect);
                if (!baseEffect.created)
                {
                    baseEffect.create();
                }
                list.Add(baseEffect);
                baseEffect.effectIndex = list.Count;
            }
            activeIndex++;
            baseEffect.active = true;
            baseEffect.gameObject.SetActive(true);
            baseEffect.state = 1;
            baseEffect.clear();
            return baseEffect;
        }
        internal void addNewObject(BaseSpellEffect pEffect)
        {
            pEffect.world = MapBox.instance;
            pEffect.controller = this;
            pEffect.transform.parent = base.transform;
        }
        public void killObject(BaseSpellEffect pObject)
        {
            if (!pObject.active)
            {
                return;
            }
            makeInactive(pObject);
            int num = pObject.effectIndex - 1;
            int num2 = activeIndex - 1;
            if (num != num2)
            {
                switchObject = list[num2];
                list[num2] = pObject;
                list[num] = switchObject;
                pObject.effectIndex = num2 + 1;
                switchObject.effectIndex = num + 1;
            }
            if (activeIndex > 0)
            {
                activeIndex--;
            }
            switchObject = null;
        }
        private void makeInactive(BaseSpellEffect pObject)
        {
            pObject.active = false;
            pObject.transform.SetParent(base.transform);
            pObject.gameObject.SetActive(false);
        }
        public void setWorld()
        {
            world = MapBox.instance;
        }
        protected virtual void onStart()
        {
            setWorld();
        }
        private void Start()
        {
            onStart();
            if (!created)
            {
                create();
            }
        }
        public void update(float pElapsed)
        {
            updateChildren(pElapsed);
            updateSpawn(pElapsed);
        }

        private void updateSpawn(float pElapsed)
        {
            if ((bool)ReflectionUtility.Reflection.GetField(typeof(MapBox), world, "_isPaused"))
            {
                return;
            }
            if (useInterval)
            {
                if (timer > 0f)
                {
                    timer -= pElapsed;
                    return;
                }
                timer = timer_interval;
                spawn();
            }
        }

        private void updateChildren(float pElapsed)
        {
            for (int i = activeIndex - 1; i >= 0; i--)
            {
                BaseSpellEffect baseEffect = list[i];
                if (baseEffect.created && baseEffect.active)
                {
                    baseEffect.update(pElapsed);
                }
            }
        }

        public virtual void spawn()
        {
        }

        public BaseSpellEffect spawnNew()
        {
            if (isInLimit())
            {
                return null;
            }
            BaseSpellEffect @object = GetObject();
            if (@object.spriteAnimation != null)
            {
                @object.spriteAnimation.resetAnim(0);
            }
            return @object;
        }

        internal virtual BaseSpellEffect spawnAt(WorldTile pTile, float pScale = 0.5f, bool cycle = false, Actor follow = null, float totalTime = 0f, float Xoffset = 0f, float Yoffset = 0f)
        {
            if (isInLimit())
            {
                return null;
            }
            BaseSpellEffect @object = GetObject();
            @object.prepare(pTile, pScale);
            @object.setCycle(cycle, follow, totalTime, Xoffset, Yoffset);
            return @object;
        }

        internal virtual BaseSpellEffect spawnAt(Vector3 pVector, float pScale = 0.5f, bool cycle = false, Actor follow = null, float totalTime = 0f, float Xoffset = 0f, float Yoffset = 0f)
        {
            if (isInLimit())
            {
                return null;
            }
            BaseSpellEffect @object = GetObject();
            @object.prepare(pVector, pScale);
            @object.setCycle(cycle, follow, totalTime, Xoffset, Yoffset);
            return @object;
        }
        internal virtual BaseSpellEffect spawnAt(Vector3 pVector, Vector3 scale, Vector3 eulerAngle, bool cycle = false, Actor follow = null, float totalTime = 0f, float Xoffset = 0f, float Yoffset = 0f)
        {
            if (isInLimit())
            {
                return null;
            }
            BaseSpellEffect @object = GetObject();
            @object.prepare(pVector, scale, eulerAngle);
            @object.setCycle(cycle, follow, totalTime, Xoffset, Yoffset);
            return @object;
        }

        public BaseSpellEffect spawnAtRandomScale(WorldTile pTile, float pScaleMin = 1f, float pScaleMax = 1f)
        {
            if (isInLimit())
            {
                return null;
            }
            BaseSpellEffect @object = GetObject();
            float pScale = Toolbox.randomFloat(pScaleMin, pScaleMax);
            @object.prepare(pTile, pScale);
            return @object;
        }

        private bool isInLimit()
        {
            return objectLimit != 0 && activeIndex > objectLimit;
        }

        internal void clear()
        {
            for (int i = 0; i < list.Count; i++)
            {
                BaseSpellEffect pObject = list[i];
                makeInactive(pObject);
            }
            activeIndex = 0;
        }

        public BaseSpellEffectController()
        {
        }
    }
}
