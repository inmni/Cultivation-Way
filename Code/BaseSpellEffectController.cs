using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    /// <summary>
    /// Copied from game
    /// </summary>
    class BaseSpellEffectController : MonoBehaviour
    {

        public WorldTile currentTile;

        public Vector3 zPosition;

        public Vector2 currentPosition;

        public Vector3 currentScale;

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
            this.created = true;
            this.m_gameObject = base.gameObject;
            this.m_transform = this.m_gameObject.transform;
            this.setWorld();
            this.list = new List<BaseSpellEffect>();
            this.timer_interval = 0.9f;
        }
        public BaseSpellEffect GetObject()
        {
            BaseSpellEffect baseEffect;
            if (this.list.Count > this.activeIndex)
            {
                baseEffect = this.list[this.activeIndex];
            }
            else
            {
                baseEffect = UnityEngine.Object.Instantiate<Transform>(this.prefab).gameObject.GetComponent<BaseSpellEffect>();
                this.addNewObject(baseEffect);
                if (!baseEffect.created)
                {
                    baseEffect.create();
                }
                this.list.Add(baseEffect);
                baseEffect.effectIndex = this.list.Count;
            }
            this.activeIndex++;
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
            this.makeInactive(pObject);
            int num = pObject.effectIndex - 1;
            int num2 = this.activeIndex - 1;
            if (num != num2)
            {
                this.switchObject = this.list[num2];
                this.list[num2] = pObject;
                this.list[num] = this.switchObject;
                pObject.effectIndex = num2 + 1;
                this.switchObject.effectIndex = num + 1;
            }
            if (this.activeIndex > 0)
            {
                this.activeIndex--;
            }
            this.switchObject = null;
        }
        private void makeInactive(BaseSpellEffect pObject)
        {
            pObject.active = false;
            pObject.transform.SetParent(base.transform);
            pObject.gameObject.SetActive(false);
        }
        public void setWorld()
        {
            this.world = MapBox.instance;
        }
        protected virtual void onStart()
        {
            this.setWorld();
        }
        private void Start()
        {
            this.onStart();
            if (!this.created)
            {
                this.create();
            }
        }
        public void update(float pElapsed)
        {
            this.updateChildren(pElapsed);
            this.updateSpawn(pElapsed);
        }

        private void updateSpawn(float pElapsed)
        {
            if ((bool)ReflectionUtility.Reflection.GetField(typeof(MapBox), world, "_isPaused"))
            {
                return;
            }
            if (this.useInterval)
            {
                if (this.timer > 0f)
                {
                    this.timer -= pElapsed;
                    return;
                }
                this.timer = this.timer_interval;
                this.spawn();
            }
        }

        private void updateChildren(float pElapsed)
        {
            for (int i = this.activeIndex - 1; i >= 0; i--)
            {
                BaseSpellEffect baseEffect = this.list[i];
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
            if (this.isInLimit())
            {
                return null;
            }
            BaseSpellEffect @object = this.GetObject();
            if (@object.spriteAnimation != null)
            {
                @object.spriteAnimation.resetAnim(0);
            }
            return @object;
        }

        internal virtual BaseSpellEffect spawnAt(WorldTile pTile, float pScale = 0.5f, bool cycle = false, Actor follow = null, float totalTime = 0f, float Xoffset = 0f, float Yoffset = 0f)
        {
            if (this.isInLimit())
            {
                return null;
            }
            BaseSpellEffect @object = this.GetObject();
            @object.prepare(pTile, pScale);
            @object.setCycle(cycle, follow, totalTime, Xoffset, Yoffset);
            return @object;
        }

        internal virtual BaseSpellEffect spawnAt(Vector3 pVector, float pScale = 0.5f, bool cycle = false, Actor follow = null, float totalTime = 0f, float Xoffset = 0f, float Yoffset = 0f)
        {
            if (this.isInLimit())
            {
                return null;
            }
            BaseSpellEffect @object = this.GetObject();
            @object.prepare(pVector, pScale);
            @object.setCycle(cycle, follow, totalTime, Xoffset, Yoffset);
            return @object;
        }
        internal virtual BaseSpellEffect spawnAt(Vector3 pVector, Vector3 scale, Vector3 eulerAngle, bool cycle = false, Actor follow = null, float totalTime = 0f, float Xoffset = 0f, float Yoffset = 0f)
        {
            if (this.isInLimit())
            {
                return null;
            }
            BaseSpellEffect @object = this.GetObject();
            @object.prepare(pVector, scale, eulerAngle);
            @object.setCycle(cycle, follow, totalTime, Xoffset, Yoffset);
            return @object;
        }

        public BaseSpellEffect spawnAtRandomScale(WorldTile pTile, float pScaleMin = 1f, float pScaleMax = 1f)
        {
            if (this.isInLimit())
            {
                return null;
            }
            BaseSpellEffect @object = this.GetObject();
            float pScale = Toolbox.randomFloat(pScaleMin, pScaleMax);
            @object.prepare(pTile, pScale);
            return @object;
        }

        private bool isInLimit()
        {
            return this.objectLimit != 0 && this.activeIndex > this.objectLimit;
        }

        internal void clear()
        {
            for (int i = 0; i < this.list.Count; i++)
            {
                BaseSpellEffect pObject = this.list[i];
                this.makeInactive(pObject);
            }
            this.activeIndex = 0;
        }

        public BaseSpellEffectController()
        {
        }
    }
}
