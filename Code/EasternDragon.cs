using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    class EasternDragon : BaseActorComponent
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

        private ExtensionSpell spellPrepared;//预备施法

        private bool _died;

        private static List<EasternDragonState> possibleActions = new List<EasternDragonState>();

        private SpriteAnimation spriteAnimation;
        private bool _justGotHit;

        private void Start()
        {
            this.onStart();
            if (!this.created)
            {
                this.create();
            }
        }
        internal void create()
        {
            this.actor = (ExtendedActor)base.gameObject.GetComponent<Actor>();
            this.created = true;
            this.m_gameObject = base.gameObject;
            this.m_transform = this.m_gameObject.transform;
            this.setWorld();
            this.spriteAnimation = base.GetComponent<SpriteAnimation>();
            this.easternDragonAsset = getAsset();
            this.actor.callbacks_get_hit.Add(new BaseActionActor(this.getHit));
            
            this.actor.setStatsDirty();
            setState(new EasternDragonState());
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
            this.state = pState;
            EasternDragonAssetContainer asset = this.easternDragonAsset.getAsset(pState);
            this.animationDoneOnce = false;
            if (this.state.shape != EasternDragonState.Shape.Human)
            {
                switch (this.state.actionState)
                {
                    case EasternDragonState.ActionState.Move:
                        List<WorldTile> possibleTiles = Utils.OthersHelper.getTilesInRange(actor.currentTile, 5f);
                        this.actor.moveTo(possibleTiles.GetRandom());
                        break;
                    case EasternDragonState.ActionState.Attack:
                        setFlying(true);
                        break;
                    case EasternDragonState.ActionState.Stop:

                        break;
                    case EasternDragonState.ActionState.Death:
                        this.spriteAnimation.looped = false;
                        break;
                    case EasternDragonState.ActionState.Up:

                        setFlying(true);
                        break;
                }
            }
            else
            {
                switch (this.state.actionState)
                {
                    case EasternDragonState.ActionState.Move:
                        List<WorldTile> possibleTiles =Utils.OthersHelper.getTilesInRange(actor.currentTile, 5f);
                        this.actor.moveTo(possibleTiles.GetRandom());
                        break;
                    case EasternDragonState.ActionState.Death:
                        this.spriteAnimation.looped = false;
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
            this.spriteAnimation.timeBetweenFrames = asset.frameSpeed;
            this.spriteAnimation.resetAnim(0);
        }
        private void setFlying(bool flying)
        {
            ReflectionUtility.Reflection.SetField(this.actor, "flying", flying);
            ReflectionUtility.Reflection.SetField(this.actor, "hitboxZ", flying ? 8f : 2f);
        }
        protected override void onStart()
        {
            this.setWorld();
        }
        internal EasternDragonState getState()
        {
            return this.state;
        }
        private bool shouldFly()
        {
            return (this.actor.easyData.health < this.actor.GetCurStats().health * 4 / 5) && this.actor.easyData.alive;
        }
        private bool shouldLand()
        {
            return (this.actor.currentTile.Type.ground && this.actor.easyData.health > this.actor.GetCurStats().health * 9 / 10);
        }
        public void getHit(Actor pActor)
        {
            this._justGotHit = true;
            BaseSimObject target = (BaseSimObject)Reflection.GetField(typeof(Actor), pActor, "attackTarget");
            if (target == null)
            {
                return;
            }
            if (target.objectType == MapObjectType.Actor)
            {
                this.actorToAttack = (Actor)Reflection.GetField(typeof(Actor), pActor, "attackTarget");
            }
            else
            {
                return;
            }
        }
        public override void update(float pElapsed)
        {
            if (this.spriteAnimation.currentFrameIndex > 0)
            {
                this.animationDoneOnce = true;
            }
            if (!this.actor.easyData.alive)
            {
                this.actionTime = -1f;
            }
            //死亡的情况
            if (this.state.actionState == EasternDragonState.ActionState.Death
                && this.spriteAnimation.currentFrameIndex == this.spriteAnimation.frames.Length - 1)
            {
                this.actor.CallMethod("updateDeadBlackAnimation");
                if (this._died)
                {
                    return;
                }
                this._died = true;

                return;
            }
            //施法的情况
            if (this.state.actionState == EasternDragonState.ActionState.Spell
                && this.spriteAnimation.currentFrameIndex == 11)
            {
                bool flip = (bool)Reflection.GetField(typeof(Actor), actor, "flip");
                Projectile p = Utils.OthersHelper.startProjectile("water_orb", actor, actorToAttack,flip?23f:-23f,0f);
                
                Reflection.SetField(p, "byWho", actor);
                p.setStats(actor.GetCurStats());
                p.targetObject = actorToAttack;
                if (spellPrepared != null)
                {
                    spellPrepared.castSpell(actor, actorToAttack);
                    spellPrepared = null;
                }
            }
            //落地的高度变化
            if(this.state.actionState == EasternDragonState.ActionState.Landing)
            {
                float height = 2f;
                height +=6f*this.spriteAnimation.currentFrameIndex / this.spriteAnimation.frames.Length;
                Reflection.SetField(this.actor, "hitboxZ", 6f-height);
                Reflection.SetField(this.actor,"_positionDirty",true);
            }
            //if ((bool)Reflection.GetField(typeof(Actor), this.actor, "is_moving"))
            //{
            //    return;
            //}
            if (this.shouldLand() && this.state.shape == EasternDragonState.Shape.Dragon)
            {
                this._justGotHit = false;
                this.actionTime = -1f;
            }
            if (this.actionTime > 0f)
            {
                this.actionTime -= pElapsed;
                return;
            }
            this.nextAction();

        }
        private void nextAction()
        {
            EasternDragonState newState = new EasternDragonState();
            if (this.spriteAnimation.currentFrameIndex != 0 || !this.animationDoneOnce)
            {
                return;
            }
            if (!this.actor.easyData.alive)
            {
                if (this.state.actionState != EasternDragonState.ActionState.Death)
                {
                    this.setState(new EasternDragonState() { actionState = EasternDragonState.ActionState.Death });
                    return;
                }
            }
            //如果是人形态，并且血量健康，则执行原动作
            if (this.state.shape == EasternDragonState.Shape.Human && !this.shouldFly())
            {
                if (this.actorToAttack == null)
                {
                    this.setState(new EasternDragonState() { shape = EasternDragonState.Shape.Human, actionState = EasternDragonState.ActionState.Move });
                }
                else
                {
                    this.setState(new EasternDragonState() { shape = EasternDragonState.Shape.Human, actionState = EasternDragonState.ActionState.Attack });
                }
                return;
            }
            //如果血量不健康，则切换为龙形态
            else if (this.state.shape == EasternDragonState.Shape.Human)
            {
                newState.shape = EasternDragonState.Shape.Dragon;
                newState.actionState = EasternDragonState.ActionState.Up;
                setState(newState);
                return;
            }
            //如果不是人形态（上面已经检验过了）,且血量健康，则切换为人形态
            else if (this.shouldLand())
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
            if (this.state.actionState == EasternDragonState.ActionState.Up)
            {
                newState.shape = EasternDragonState.Shape.Dragon;
                newState.actionState = EasternDragonState.ActionState.Attack;
                setState(newState);
                return;
            }
            //如果切换人形态完成，则暂停一段时间
            else if (this.state.actionState == EasternDragonState.ActionState.Landing)
            {
                newState.shape = EasternDragonState.Shape.Human;
                newState.actionState = EasternDragonState.ActionState.Stop;
                setState(newState);
                return;
            }
            //如果正在攻击
            if (this.state.actionState == EasternDragonState.ActionState.Attack)
            {
                //如果攻击目标不存在
                if (this.actorToAttack == null)
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
                        this.actorToAttack = targets.GetRandom();
                    }
                }
                foreach (ExtensionSpell spell in this.actor.extendedCurStats.spells)
                {
                    if (actor.extendedData.status.magic > spell.cost && spell.leftCool == 0)
                    {
                        if (spell.GetSpellAsset().type.attacking)
                        {
                            this.spellPrepared = spell;
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
            if (this.state.actionState == EasternDragonState.ActionState.Spell
                && this.spriteAnimation.currentFrameIndex == this.spriteAnimation.frames.Length - 1)
            {
                newState.shape = EasternDragonState.Shape.Dragon;
                newState.actionState = EasternDragonState.ActionState.Attack;
                setState(newState);
            }
            //如果无攻击动作，则寻找攻击目标，如攻击目标不存在，则继续移动 
            if (this.state.actionState == EasternDragonState.ActionState.Stop || this.state.actionState == EasternDragonState.ActionState.Move)
            {
                if (this.actorToAttack == null)
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
                        this.actorToAttack = targets.GetRandom();
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
