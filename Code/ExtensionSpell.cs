using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReflectionUtility;

namespace Cultivation_Way
{
    class ExtensionSpell
    {
        public ExtensionSpellAsset spellAsset;

        public float cost;//蓝耗因子

        public float cooldown;//冷却因子

        private float might;//威力因子

        private bool isDirective;//类型

        private ActorStatus attackerData;

        private ActorStatus targetData;

        public bool castSpell(BaseSimObject pAttacker, BaseSimObject pTarget = null)
        {
            //判断合法性
            if (!isValid(pAttacker,pTarget))
            {
                return false;
            }
            //若为概率触发，则开始随机
            if (spellAsset.type.byChance&&Toolbox.randomChance(0.99f))
            {
                return false;
            }
            if (pTarget == null)
            {
                pTarget = pAttacker;
                targetData = attackerData;
            }
            return callSpell(pAttacker, pTarget);
        }
        
        private bool isValid(BaseSimObject pAttacker, BaseSimObject pTarget)
        {
            if (pTarget == null)//如果无施法对象
            {
                if (spellAsset.type.action)//如果是作用于对象的法术，则是非法的
                {
                    return false;
                }
                //否则是合法的
                return true;
            }
            else if (pAttacker.objectType == MapObjectType.Actor && pTarget.objectType == MapObjectType.Actor)
            {
                attackerData = Reflection.GetField(typeof(Actor), pAttacker, "data") as ActorStatus;
                targetData = Reflection.GetField(typeof(Actor), pTarget, "data") as ActorStatus;
                //法术反制
                if (attackerData.level >= targetData.level && Toolbox.randomChance(0.8f))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else if (pAttacker.objectType == MapObjectType.Actor)
            {
                attackerData = Reflection.GetField(typeof(Actor), pAttacker, "data") as ActorStatus;
                return true;
            }
            else if(pTarget.objectType == MapObjectType.Actor)
            {
                targetData = Reflection.GetField(typeof(Actor), pTarget, "data") as ActorStatus;
                return true;
            }
            else
            {
                return false;
            }
        }//判断施法合法性

        private bool callSpell(BaseSimObject pAttacker, BaseSimObject pTarget)
        {
            if(spellAsset.type.isDirective)
            {

            }
            else
            {

            }
            return true;
        }
    }
}
