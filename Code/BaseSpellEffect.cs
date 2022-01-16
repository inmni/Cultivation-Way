using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionUtility;
using UnityEngine;
namespace Cultivation_Way
{
    public class BaseSpellEffect : MonoBehaviour
    {
        /// <summary>
        /// 直接cv游戏BaseEffect
        /// </summary>
        /// 

        internal SpriteAnimation spriteAnimation;

        internal MapObjectShadow shadow;

        internal Vector3 lastShadowPos;

        internal Vector3 lastShadowScale;

        public WorldTile currentTile;

        public Vector3 zPosition;

        public Vector2 currentPosition;

        public Vector3 currentScale;

        internal bool created;

        protected MapBox world;

        internal Transform m_transform;

        internal GameObject m_gameObject;

        public const int STATE_START = 1;

        public const int STATE_ON_DEATH = 2;

        public const int STATE_KILLED = 3;

        public WorldTile tile;

        public SpriteRenderer spriteRenderer;

        protected float scale;//特效尺寸

        protected float alpha;//特效不透明度

        internal BaseSpellEffectController controller;

        internal bool active;

        internal bool autoYZ;//是否自动设置高度

        internal int state;

        internal int effectIndex;

        internal bool isCycle;//是否循环

        internal float leftTime;//剩余播放时间

        internal float Xoffset;

        internal float Yoffset;

        internal Actor follow;//跟随的对象

        internal BaseCallback callback;

        internal int callbackOnFrame = -1;
        private void Awake()
        {
            this.spriteRenderer = base.GetComponent<SpriteRenderer>();
        }
        internal void create()
        {
            this.created = true;
            this.m_gameObject = base.gameObject;
            this.m_transform = this.m_gameObject.transform;
            this.setWorld();
            this.spriteAnimation = base.gameObject.GetComponent<SpriteAnimation>();
            if (this.spriteAnimation != null)
            {
                this.spriteAnimation.create();
            }
        }
        //设置特效控制器
        internal void makeParentController()
        {
            base.transform.SetParent(this.controller.transform, true);
        }
        internal virtual void prepare(WorldTile tile, float pScale = 0.5f)
        {
            this.state = STATE_START;
            float z = 0f;
            if (this.autoYZ)
            {
                z = (float)tile.pos.y;
            }
            base.transform.localEulerAngles = Vector3.zero;
            base.transform.localPosition = new Vector3((float)tile.pos.x + 0.5f, (float)tile.pos.y, z);
            this.setScale(pScale);
            this.setAlpha(1f);
            if (base.GetComponent<SpriteAnimation>() != null)
            {
                base.GetComponent<SpriteAnimation>().resetAnim(0);
            }
        }
        internal virtual void prepare(Vector3 pVector, float pScale = 1f)
        {
            this.state = STATE_START;
            float z = 0f;
            if (this.autoYZ)
            {
                z = pVector.y;
            }
            base.transform.rotation = Quaternion.identity;
            base.transform.localPosition = new Vector3(pVector.x, pVector.y, z);
            this.setScale(pScale);
            this.setAlpha(1f);
            if (base.GetComponent<SpriteAnimation>() != null)
            {
                base.GetComponent<SpriteAnimation>().resetAnim(0);
            }
        }
        internal virtual void prepare(Vector3 pVector, Vector3 scale, Vector3 eulerAngle)
        {
            this.state = STATE_START;
            float z = 0f;
            if (this.autoYZ)
            {
                z = pVector.y;
            }
            base.transform.rotation = Quaternion.identity;
            base.transform.localPosition = new Vector3(pVector.x, pVector.y, z);
            base.transform.localScale = scale;
            base.transform.localEulerAngles = eulerAngle;
            this.setAlpha(1f);
            if (base.GetComponent<SpriteAnimation>() != null)
            {
                base.GetComponent<SpriteAnimation>().resetAnim(0);
            }
        }
        public void update(float pElapsed)
        {
            if (follow != null)
            {
                base.transform.localPosition = new Vector3(Xoffset + follow.currentPosition.x, Yoffset + follow.currentPosition.y);
            }
            if (isCycle)
            {
                leftTime -= pElapsed;
            }
            #region this.spriteAnimation.update(pElapsed)
            if (this.spriteAnimation != null)
            {
                this.spriteAnimation.CallMethod("update",pElapsed);
                if (spriteAnimation.useNormalDeltaTime)
                {
                    pElapsed = Time.deltaTime;
                }
                if (spriteAnimation.dirty)
                {
                    spriteAnimation.dirty = false;
                    spriteAnimation.forceUpdateFrame();
                    return;
                }
                if (!spriteAnimation.isOn)
                {
                    if ((bool)Reflection.GetField(typeof(SpriteAnimation),spriteAnimation,"stopFrameTrigger"))
                    {
                        Reflection.SetField(spriteAnimation, "stopFrameTrigger", false);
                        spriteAnimation.CallMethod("updateFrame");
                    }
                    return;
                }
                if (spriteAnimation.nextFrameTime > 0f)
                {
                    spriteAnimation.nextFrameTime -= pElapsed;
                    return;
                }
                spriteAnimation.nextFrameTime = spriteAnimation.timeBetweenFrames;
                if (spriteAnimation.playType == AnimPlayType.Forward)
                {
                    if (spriteAnimation.currentFrameIndex + 1 >= spriteAnimation.frames.Length)
                    {
                        if (spriteAnimation.returnToPool&&(!isCycle||leftTime<=0f))
                        {
                            base.GetComponent<BaseSpellEffect>().kill();
                            return;
                        }
                        if (!spriteAnimation.looped)
                        {
                            return;
                        }
                        spriteAnimation.currentFrameIndex = 0;
                    }
                    else
                    {
                        spriteAnimation.currentFrameIndex++;
                    }
                }
                else if (spriteAnimation.currentFrameIndex - 1 < 0)
                {
                    if (spriteAnimation.returnToPool && (!isCycle || leftTime <= 0f))
                    {
                        base.GetComponent<BaseSpellEffect>().kill();
                        return;
                    }
                    if (!spriteAnimation.looped)
                    {
                        return;
                    }
                    spriteAnimation.currentFrameIndex = spriteAnimation.frames.Length - 1;
                }
                else
                {
                    spriteAnimation.currentFrameIndex--;
                }
                spriteAnimation.CallMethod("updateFrame");
            }
            #endregion
            
            if (this.callbackOnFrame != -1 && this.spriteAnimation.currentFrameIndex == this.callbackOnFrame)
            {
                
                this.callback();
                this.clear();
            }
        }
        internal void setWorld()
        {
            this.world = MapBox.instance;
        }
        internal void setCycle(bool cycle = false,Actor follow=null,float totalTime=0f,float Xoffset = 0f,float Yoffset = 0f)
        {
            isCycle = true;
            leftTime = totalTime;
            this.follow = follow;
            this.Xoffset = Xoffset;
            this.Yoffset = Yoffset;
        }
        //设置特效大小
        public void setScale(float pScale)
        {
            this.scale = pScale;
            if (this.scale < 0f)
            {
                this.scale = 0f;
            }
            base.transform.localScale = new Vector3(pScale, pScale);
        }
        //设置特效不透明度
        protected void setAlpha(float pVal)
        {
            this.alpha = pVal;
            if (this.spriteRenderer == null)
            {
                this.spriteRenderer = base.GetComponent<SpriteRenderer>();
            }
            Color color = this.spriteRenderer.color;
            color.a = this.alpha;
            this.spriteRenderer.color = color;
        }
        //设置特效位置
        internal void setTile(WorldTile pTile)
        {
            this.tile = pTile;
            base.transform.localPosition = pTile.posV3;
        }
        public void setCallback(int pFrame, BaseCallback pCallback)
        {
            this.callbackOnFrame = pFrame;
            this.callback = pCallback;
        }
        
        //准备结束
        internal void prepareKill()
        {
            this.state = STATE_ON_DEATH;
        }
        //删除对象
        public void kill()
        {
            this.state = STATE_KILLED;
            this.controller.killObject(this);
        }
        public void clear()
        {
            this.callback = null;
            this.callbackOnFrame = -1;
        }
    }
}
