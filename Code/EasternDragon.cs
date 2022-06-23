using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    internal class EasternDragon : BaseActorComponent
    {

        internal ExtendedActor actor;

        internal bool created;

        internal Transform m_transform;

        internal GameObject m_gameObject;

        private EasternDragonAsset easternDragonAsset;

        private EasternDragonState state;

        private bool animationDoneOnce;

        private float actionTime;

        private Actor actorToAttack;

        private ExtendedSpell spellPrepared;//预备施法

        private bool _died;

        private static List<EasternDragonState> possibleActions = new List<EasternDragonState>();

        private SpriteAnimation spriteAnimation;
        private bool _justGotHit;

        private void Start()
        {
            onStart();
            if (!created)
            {
                create();
            }
        }
        internal void create()
        {
            created = true;
            actor = gameObject.GetComponent<ExtendedActor>();
            m_gameObject = gameObject;
            m_transform = m_gameObject.transform;
            spriteAnimation = GetComponent<SpriteAnimation>();
            easternDragonAsset = getAsset();
            actor.callbacks_get_hit.Add(new BaseActionActor(getHit));
            actor.setStatsDirty();
            setState(new EasternDragonState());
            //setWorld();
        }
        //待修改
        private EasternDragonAsset getAsset()
        {
            return MoreActors.easternDragonAsset;
        }
        private void setState(EasternDragonState pState)
        {
            //MonoBehaviour.print("当前状态");
            //MonoBehaviour.print(pState.shape.ToString());
            //MonoBehaviour.print(pState.actionState.ToString());
            state = pState;
            EasternDragonAssetContainer asset = easternDragonAsset.getAsset(pState);
            animationDoneOnce = false;
            if (state.shape != EasternDragonState.Shape.Human)
            {
                switch (state.actionState)
                {
                    case EasternDragonState.ActionState.Move:
                        List<WorldTile> possibleTiles = Utils.OthersHelper.getTilesInRange(actor.currentTile, 5f);
                        actor.moveTo(possibleTiles.GetRandom());
                        break;
                    case EasternDragonState.ActionState.Attack:
                        setFlying(true);
                        break;
                    case EasternDragonState.ActionState.Stop:

                        break;
                    case EasternDragonState.ActionState.Death:
                        spriteAnimation.looped = false;
                        break;
                    case EasternDragonState.ActionState.Up:

                        setFlying(true);
                        break;
                }
            }
            else
            {
                switch (state.actionState)
                {
                    case EasternDragonState.ActionState.Move:
                        List<WorldTile> possibleTiles = Utils.OthersHelper.getTilesInRange(actor.currentTile, 5f);
                        actor.moveTo(possibleTiles.GetRandom());
                        break;
                    case EasternDragonState.ActionState.Death:
                        spriteAnimation.looped = false;
                        break;
                    case EasternDragonState.ActionState.Landing:
                        setFlying(false);
                        break;
                }
            }
            if (spriteAnimation.frames != asset.frames)
            {
                spriteAnimation.frames = asset.frames;
                if (spriteAnimation.currentFrameIndex >= asset.frames.Length)
                {
                    spriteAnimation.currentFrameIndex = 0;
                }
                spriteAnimation.CallMethod("updateFrame");
            }
            spriteAnimation.timeBetweenFrames = asset.frameSpeed;
            spriteAnimation.resetAnim(0);
        }
        private void setFlying(bool flying)
        {
            ReflectionUtility.Reflection.SetField(actor, "flying", flying);
            ReflectionUtility.Reflection.SetField(actor, "hitboxZ", flying ? 8f : 2f);
        }
        protected override void onStart()
        {
            setWorld();
        }
        internal EasternDragonState getState()
        {
            return state;
        }
        private bool shouldFly()
        {
            return (actor.easyData.health < actor.easyCurStats.health * 4 / 5) && actor.easyData.alive;
        }
        private bool shouldLand()
        {
            return (actor.currentTile.Type.ground && actor.easyData.health > actor.easyCurStats.health * 9 / 10);
        }
        public void getHit(Actor pActor)
        {
            _justGotHit = true;
            BaseSimObject target = (BaseSimObject)Reflection.GetField(typeof(Actor), pActor, "attackTarget");
            if (target == null)
            {
                return;
            }
            if (target.objectType == MapObjectType.Actor)
            {
                actorToAttack = (Actor)Reflection.GetField(typeof(Actor), pActor, "attackTarget");
            }
            else
            {
                return;
            }
        }
        public override void update(float pElapsed)
        {
            if (spriteAnimation.currentFrameIndex > 0)
            {
                animationDoneOnce = true;
            }
            if (!actor.easyData.alive)
            {
                actionTime = -1f;
            }
            //死亡的情况
            if (state.actionState == EasternDragonState.ActionState.Death
                && spriteAnimation.currentFrameIndex == spriteAnimation.frames.Length - 1)
            {
                actor.CallMethod("updateDeadBlackAnimation");
                if (_died)
                {
                    return;
                }
                _died = true;

                return;
            }
            //施法的情况
            if (state.actionState == EasternDragonState.ActionState.Spell
                && spriteAnimation.currentFrameIndex == 11)
            {
                bool flip = (bool)Reflection.GetField(typeof(Actor), actor, "flip");
                Projectile p = Utils.OthersHelper.startProjectile("water_orb", actor, actorToAttack, flip ? 23f : -23f, 0f);

                Reflection.SetField(p, "byWho", actor);
                p.setStats(actor.easyCurStats);
                p.targetObject = actorToAttack;
                if (spellPrepared != null)
                {
                    spellPrepared.castSpell(actor, actorToAttack);
                    spellPrepared = null;
                }
            }
            //落地的高度变化
            if (state.actionState == EasternDragonState.ActionState.Landing)
            {
                float height = 2f;
                height += 6f * spriteAnimation.currentFrameIndex / spriteAnimation.frames.Length;
                Reflection.SetField(actor, "hitboxZ", 6f - height);
                Reflection.SetField(actor, "_positionDirty", true);
            }
            //if ((bool)Reflection.GetField(typeof(Actor), this.actor, "is_moving"))
            //{
            //    return;
            //}
            if (shouldLand() && state.shape == EasternDragonState.Shape.Dragon)
            {
                _justGotHit = false;
                actionTime = -1f;
            }
            if (actionTime > 0f)
            {
                actionTime -= pElapsed;
                return;
            }
            nextAction();

        }
        private void nextAction()
        {
            EasternDragonState newState = new EasternDragonState();
            if (spriteAnimation.currentFrameIndex != 0 || !animationDoneOnce)
            {
                return;
            }
            if (!actor.easyData.alive)
            {
                if (state.actionState != EasternDragonState.ActionState.Death)
                {
                    setState(new EasternDragonState() { actionState = EasternDragonState.ActionState.Death });
                    return;
                }
            }
            //如果是人形态，并且血量健康，则执行原动作
            if (state.shape == EasternDragonState.Shape.Human && !shouldFly())
            {
                if (actorToAttack == null)
                {
                    setState(new EasternDragonState() { shape = EasternDragonState.Shape.Human, actionState = EasternDragonState.ActionState.Move });
                }
                else
                {
                    setState(new EasternDragonState() { shape = EasternDragonState.Shape.Human, actionState = EasternDragonState.ActionState.Attack });
                }
                return;
            }
            //如果血量不健康，则切换为龙形态
            else if (state.shape == EasternDragonState.Shape.Human)
            {
                newState.shape = EasternDragonState.Shape.Dragon;
                newState.actionState = EasternDragonState.ActionState.Up;
                setState(newState);
                return;
            }
            //如果不是人形态（上面已经检验过了）,且血量健康，则切换为人形态
            else if (shouldLand())
            {
                newState.shape = EasternDragonState.Shape.Human;
                newState.actionState = EasternDragonState.ActionState.Landing;
                setState(newState);
                return;
                //newState.shape = EasternDragonState.Shape.Dragon;
                //newState.actionState = EasternDragonState.ActionState.Attack;
                //setState(newState);
                //return;
            }
            //如果切换龙形态完成，直接进入攻击
            if (state.actionState == EasternDragonState.ActionState.Up)
            {
                newState.shape = EasternDragonState.Shape.Dragon;
                newState.actionState = EasternDragonState.ActionState.Attack;
                setState(newState);
                return;
            }
            //如果切换人形态完成，则暂停一段时间
            else if (state.actionState == EasternDragonState.ActionState.Landing)
            {
                newState.shape = EasternDragonState.Shape.Human;
                newState.actionState = EasternDragonState.ActionState.Stop;
                setState(newState);
                return;
            }
            //如果正在攻击
            if (state.actionState == EasternDragonState.ActionState.Attack)
            {
                //如果攻击目标不存在
                if (actorToAttack == null)
                {
                    List<Actor> targets = Utils.OthersHelper.getEnemyObjectInRange(actor, actor.currentTile, 8f);
                    if (targets.Count == 0)
                    {
                        newState.shape = EasternDragonState.Shape.Dragon;
                        newState.actionState = EasternDragonState.ActionState.Move;
                        setState(newState);
                        return;
                    }
                    else
                    {
                        actorToAttack = targets.GetRandom();
                    }
                }
                foreach (ExtendedSpell spell in actor.extendedData.status.spells)
                {
                    if (actor.easyData.experience > spell.cost)
                    {
                        if (spell.GetSpellAsset().type==ExtendedSpellType.ATTACK)
                        {
                            spellPrepared = spell;
                            break;
                        }
                    }
                }
                newState.shape = EasternDragonState.Shape.Dragon;
                newState.actionState = EasternDragonState.ActionState.Spell;
                setState(newState);
                return;
                ////如果无法术可以释放，则移动
                //newState.shape = EasternDragonState.Shape.Dragon;
                //newState.actionState = EasternDragonState.ActionState.Move;
                //setState(newState);
                //return;
            }
            //如果施法结束
            if (state.actionState == EasternDragonState.ActionState.Spell
                && spriteAnimation.currentFrameIndex == spriteAnimation.frames.Length - 1)
            {
                newState.shape = EasternDragonState.Shape.Dragon;
                newState.actionState = EasternDragonState.ActionState.Attack;
                setState(newState);
            }
            //如果无攻击动作，则寻找攻击目标，如攻击目标不存在，则继续移动 
            if (state.actionState == EasternDragonState.ActionState.Stop || state.actionState == EasternDragonState.ActionState.Move)
            {
                if (actorToAttack == null)
                {
                    List<Actor> targets = Utils.OthersHelper.getEnemyObjectInRange(actor, actor.currentTile, 30f);
                    if (targets.Count == 0)
                    {
                        newState.shape = EasternDragonState.Shape.Dragon;
                        newState.actionState = EasternDragonState.ActionState.Move;
                        setState(newState);
                        return;
                    }
                    else
                    {
                        actorToAttack = targets.GetRandom();
                        newState.shape = EasternDragonState.Shape.Dragon;
                        newState.actionState = EasternDragonState.ActionState.Attack;
                        setState(newState);
                        return;
                    }
                }
                else
                {
                    newState.shape = EasternDragonState.Shape.Dragon;
                    newState.actionState = EasternDragonState.ActionState.Attack;
                    setState(newState);
                    return;
                }
            }
            newState.shape = EasternDragonState.Shape.Dragon;
            newState.actionState = EasternDragonState.ActionState.Move;
            setState(newState);
            return;

        }
    }
}
