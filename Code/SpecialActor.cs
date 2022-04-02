using ReflectionUtility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cultivation_Way
{
    class SpecialActor : BaseActorComponent
    {
        internal Actor actor;

        internal bool created;

        protected new MapBox world;

        internal Transform m_transform;

        internal GameObject m_gameObject;

        private SpecialActorAsset specialActorAsset;

        private SpecialActorState state;

        private bool animationDoneOnce;

        private float actionTime;

        private Actor actorToAttack;

        private bool _justGotHit;

        private bool _died;

        private static List<SpecialActorState> possibleActions = new List<SpecialActorState>();

        private SpriteAnimation spriteAnimation;

        private ExtensionSpell spellPrepared;
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
            this.actor = base.gameObject.GetComponent<Actor>();
            this.created = true;
            this.m_gameObject = base.gameObject;
            this.m_transform = this.m_gameObject.transform;
            this.setWorld();
            this.spriteAnimation = base.GetComponent<SpriteAnimation>();
            this.specialActorAsset = getAsset();
            this.actor.callbacks_get_hit.Add(new BaseActionActor(this.getHit));

            this.actor.setStatsDirty();
            setState(new SpecialActorState());
        }
        private SpecialActorAsset getAsset()
        {
            return MoreActors.specialActorAssets[actor.stats.id];
        }
        private void setState(SpecialActorState pState)
        {
            this.state = pState;
            SpecialActorAssetContainer asset = this.specialActorAsset.getAsset(pState);
            this.animationDoneOnce = false;
            switch (this.state)
            {
                case SpecialActorState.Move:
                    List<WorldTile> possibleTiles = Utils.OthersHelper.getTilesInRange(actor.currentTile, 10f);
                    this.actor.moveTo(possibleTiles.GetRandom());
                    break;
                case SpecialActorState.Attack:
                    if (actor.currentPosition.x <= actorToAttack.currentPosition.x+1f)
                    {
                        Reflection.SetField(actor, "flip", true);
                    }
                    else
                    {
                        Reflection.SetField(actor, "flip", false);
                    }
                    Utils.OthersHelper.hitEnemiesInRange(this.actor, this.actor.currentTile, 15f, this.actor.GetCurStats().damage * 10f, null);
                    break;
                case SpecialActorState.Spell:
                    bool flip = (bool)Reflection.GetField(typeof(Actor), actor, "flip");
                    Projectile p = Utils.OthersHelper.startProjectile("red_magicArrow", actor, actorToAttack, flip ? 6f : -6f, 1.1f);

                    Reflection.SetField(p, "byWho", actor);
                    p.setStats(actor.GetCurStats());
                    p.targetObject = actorToAttack;
                    break;
                case SpecialActorState.Stop:

                    break;
                case SpecialActorState.Death:
                    this.spriteAnimation.looped = false;
                    break;
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
        protected override void onStart()
        {
            this.setWorld();
        }
        internal SpecialActorState getState()
        {
            return this.state;
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
            this.actor.zPosition.y = -1.5f;

            if (this.spriteAnimation.currentFrameIndex > 0)
            {
                this.animationDoneOnce = true;
            }
            if (!this.actor.GetData().alive)
            {
                this.actionTime = -1f;
            }
            //死亡的情况
            if (this.state == SpecialActorState.Death
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
            if ((bool)Reflection.GetField(typeof(Actor), this.actor, "is_moving"))
            {
                return;
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
            if (this.spriteAnimation.currentFrameIndex != 0 || !this.animationDoneOnce)
            {
                return;
            }
            if (!this.actor.GetData().alive)
            {
                if (this.state != SpecialActorState.Death)
                {
                    this.setState(SpecialActorState.Death);
                    return;
                }
            }
            if (this.actorToAttack != null && this.actorToAttack.base_data.alive)
            {
                if (Toolbox.DistVec3(actor.currentPosition, actorToAttack.currentPosition) < this.actor.GetCurStats().size + actorToAttack.GetCurStats().size)
                {
                    this.setState(SpecialActorState.Attack);
                }
                else
                {
                    this.setState(SpecialActorState.Spell);
                }
                return;
            }
            if (this.actorToAttack == null || !this.actorToAttack.base_data.alive)
            {
                    List<Actor> targets = Utils.OthersHelper.getEnemyObjectInRange(this.actor, this.actor.currentTile, 13f);
                    if (targets.Count != 0)
                    {
                        this.actorToAttack = targets.GetRandom();
                        if (Toolbox.DistVec3(actor.currentPosition, actorToAttack.currentPosition) <3f+ this.actor.GetCurStats().size + actorToAttack.GetCurStats().size)
                        {
                            this.setState(SpecialActorState.Attack);
                        }
                        else
                        {
                            this.setState(SpecialActorState.Spell);
                        }
                        return;
                    
                }
                if (Toolbox.randomBool())
                {
                    this.setState(SpecialActorState.Move);
                    return;
                }
                else
                {
                    this.setState(SpecialActorState.Stop);
                    return;
                }
            }
            if (Toolbox.randomChance(0.1f))
            {
                this.setState(SpecialActorState.Move);
                return;
            }
        }
    }


}

