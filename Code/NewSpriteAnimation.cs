using UnityEngine;
using UnityEngine.UI;

namespace Cultivation_Way
{
    internal class NewSpriteAnimation
    {
        public float frameInterval = 0.1f;
        public float nextFrameTime = 0.1f;
        public int currFrameIndex = 0;
        public int stopFrameIndex;
        public bool stopTriggered = false;
        public bool loop;
        public bool isOn;
        public AnimPlayType direction;
        public Sprite[] frames;
        public Image currFrame;
        public GameObject m_gameobject;
        public void create()
        {
            m_gameobject = UnityEngine.Object.Instantiate<GameObject>(NewEffectManager.prefab);
            direction = AnimPlayType.Forward;
        }
        public void start()
        {
            isOn = true;
        }
        public void stop()
        {
            isOn = false;
        }
        public void stopAt(int stopIndex)
        {
            stopFrameIndex = stopIndex;
            stopTriggered = true;
        }
        public void setFrames(Sprite[] newFrames, bool restart = false)
        {
            frames = newFrames;
            if (restart)
            {
                currFrameIndex = 0;
                updateFrame();
            }
        }
        public void setFrame(int index)
        {
            if (index < 0 || index >= frames.Length)
            {
                Debug.LogWarning("Unexpected frame index");
                return;
            }
            currFrameIndex = index;
            updateFrame();
        }
        public void update(float elapse)
        {
            if (!isOn || (stopTriggered && stopFrameIndex == currFrameIndex)
                || Config.paused || (nextFrameTime -= elapse) > 0f)
            {
                return;
            }
            nextFrameTime = frameInterval;
            nextFrame();
        }
        public void nextFrame(int forceFrameIndex = -1)
        {
            if (forceFrameIndex >= 0)
            {
                if (direction == AnimPlayType.Forward)
                {
                    currFrameIndex++;
                    if (currFrameIndex == frames.Length)
                    {
                        if (!loop)
                        {
                            stop();
                            return;
                        }
                        currFrameIndex = 0;
                    }
                }
                else
                {
                    currFrameIndex--;
                    if (currFrameIndex == -1)
                    {
                        if (!loop)
                        {
                            stop();
                            return;
                        }
                        currFrameIndex = frames.Length - 1;
                    }
                }
            }
            else
            {
                currFrameIndex = forceFrameIndex;
            }
            if (stopTriggered && currFrameIndex == stopFrameIndex)
            {
                isOn = false;
            }
            updateFrame();
        }
        public void updateFrame()
        {
            currFrame.sprite = frames[currFrameIndex];
        }
        public void kill()
        {

        }
    }
}
