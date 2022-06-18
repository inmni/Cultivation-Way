using ReflectionUtility;
using System.Collections.Generic;
using UnityEngine;

namespace Cultivation_Way
{
    internal class SpecialActor : BaseActorComponent
    {
        internal ExtendedActor actor;

        internal bool created;

        internal Transform m_transform;

        internal GameObject m_gameObject;

        private SpecialActorAsset specialActorAsset;

        private SpecialActorState state;

        private bool animationDoneOnce;

        private float actionTime;

        private ExtendedActor actorToAttack;

        private bool _died;

        private static List<SpecialActorState> possibleActions = new List<SpecialActorState>();

        private SpriteAnimation spriteAnimation;

        private ExtensionSpell spellPrepared;
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
            actor = base.gameObject.GetComponent<ExtendedActor>();
            created = true;
            m_gameObject = base.gameObject;
            m_transform = m_gameObject.transform;
            setWorld();
            spriteAnimation = base.GetComponent<SpriteAnimation>();
            specialActorAsset = getAsset();
            actor.callbacks_get_hit.Add(new BaseActionActor(getHit));

            actor.setStatsDirty();
            setState(new SpecialActorState());
        }
        private SpecialActorAsset getAsset()
        {
            return MoreActors.specialActorAssets[actor.stats.id];
        }
        private void setState(SpecialActorState pState)
        {
            state = pState;
            SpecialActorAssetContainer asset = specialActorAsset.getAsset(pState);
            animationDoneOnce = false;
            switch (state)
            {
                case SpecialActorState.Move:
                    List<WorldTile> possibleTiles = Utils.OthersHelper.getTilesInRange(actor.currentTile, 10f);
                    actor.moveTo(possibleTiles.GetRandom());
                    break;
                case SpecialActorState.Attack:
                    if (actor.currentPosition.x <= actorToAttack.currentPosition.x + 1f)
                    {
                        Reflection.SetField(actor, "flip", true);
                    }
                    else
                    {
                        Reflection.SetField(actor, "flip", false);
                    }
                    Utils.OthersHelper.hitEnemiesInRange(actor, actor.currentTile, 15f, actor.easyCurStats.damage * 10f, null);
                    break;
                case SpecialActorState.Spell:
                    bool flip = (bool)Reflection.GetField(typeof(Actor), actor, "flip");
                    Projectile p = Utils.OthersHelper.startProjectile("red_magicArrow", actor, actorToAttack, flip ? 6f : -6f, 1.1f);

                    Reflection.SetField(p, "byWho", actor);
                    p.setStats(actor.easyCurStats);
                    p.targetObject = actorToAttack;
                    break;
                case SpecialActorState.Stop:

                    break;
                case SpecialActorState.Death:
                    spriteAnimation.looped = false;
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
            spriteAnimation.timeBetweenFrames = asset.frameSpeed;
            spriteAnimation.resetAnim(0);
        }
        protected override void onStart()
        {
            setWorld();
        }
        internal SpecialActorState getState()
        {
            return state;
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
                actorToAttack = (ExtendedActor)Reflection.GetField(typeof(Actor), pActor, "attackTarget");
            }
            else
            {
                return;
            }
        }
        public override void update(float pElapsed)
        {
            actor.zPosition.y = -1.5f;

            if (spriteAnimation.currentFrameIndex > 0)
            {
                animationDoneOnce = true;
            }
            if (!actor.easyData.alive)
            {
                actionTime = -1f;
            }
            //死亡的情况
            if (state == SpecialActorState.Death
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
            if ((bool)Reflection.GetField(typeof(Actor), actor, "is_moving"))
            {
                return;
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
            if (spriteAnimation.currentFrameIndex != 0 || !animationDoneOnce)
            {
                return;
            }
            if (!actor.easyData.alive)
            {
                if (state != SpecialActorState.Death)
                {
                    setState(SpecialActorState.Death);
                    return;
                }
            }
            if (actorToAttack != null && actorToAttack.base_data.alive)
            {
                if (Toolbox.DistVec3(actor.currentPosition, actorToAttack.currentPosition) < actor.easyCurStats.size + actorToAttack.easyCurStats.size)
                {
                    setState(SpecialActorState.Attack);
                }
                else
                {
                    setState(SpecialActorState.Spell);
                }
                return;
            }
            if (actorToAttack == null || !actorToAttack.base_data.alive)
            {
                List<Actor> targets = Utils.OthersHelper.getEnemyObjectInRange(actor, actor.currentTile, 13f);
                if (targets.Count != 0)
                {
                    actorToAttack = (ExtendedActor)targets.GetRandom();
                    if (Toolbox.DistVec3(actor.currentPosition, actorToAttack.currentPosition) < 3f + actor.easyCurStats.size + actorToAttack.easyCurStats.size)
                    {
                        setState(SpecialActorState.Attack);
                    }
                    else
                    {
                        setState(SpecialActorState.Spell);
                    }
                    return;

                }
                if (Toolbox.randomBool())
                {
                    setState(SpecialActorState.Move);
                    return;
                }
                else
                {
                    setState(SpecialActorState.Stop);
                    return;
                }
            }
            if (Toolbox.randomChance(0.1f))
            {
                setState(SpecialActorState.Move);
                return;
            }
        }
    }


}

