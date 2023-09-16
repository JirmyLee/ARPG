using System;
using UGG.Combat;
using UGG.Move;
using UnityEngine;

namespace UGG.Health
{
    public abstract class CharacterHealthSystemBase : MonoBehaviour, IDamagar
    {
        
        //引用
        protected Animator _animator;
        protected CharacterMovementBase _movement;
        protected CharacterCombatSystemBase _combatSystem;
        protected AudioSource _audioSource;
        
        //攻击者
        protected Transform currentAttacker;
        
        //AnimationID
        protected int animationMoveID = Animator.StringToHash("AnimationMove");
        protected int animationJumpID = Animator.StringToHash("AnimationJump");
        
        //HitAnimationMoveSpeedMult
        public float hitAnimationMoveMult;
        
        //特效位置
        public Transform ParryPos;
        public Transform HitPos;


        protected virtual void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _movement = GetComponent<CharacterMovementBase>();
            _combatSystem = GetComponentInChildren<CharacterCombatSystemBase>();
            _audioSource = _movement.GetComponentInChildren<AudioSource>();
        }


        protected virtual void Update()
        {
            HitAnimaitonMove();
        }
        
        
        /// <summary>
        /// 设置攻击者
        /// </summary>
        /// <param name="attacker">攻击者</param>
        public virtual void SetAttacker(Transform attacker)
        {
            if (currentAttacker != attacker || currentAttacker == null)
                currentAttacker = attacker;
        }

        protected virtual void HitAnimaitonMove()
        {
            if(!_animator.CheckAnimationTag("Hit")) 
                return;
            _movement.CharacterMoveInterface(transform.forward,_animator.GetFloat(animationMoveID) * hitAnimationMoveMult,true);
            //_movement.CharacterJumpInterface(transform.up,_animator.GetFloat(animationJumpID) * hitAnimationMoveMult,true);
        }

        #region 接口

        public virtual void TakeDamager(float damager)
        {
            throw new NotImplementedException();
        }

        public virtual void TakeDamager(string hitAnimationName)
        {
            _animator.Play(hitAnimationName,0,0f);
            GameAssets.Instance.PlaySoundEffect(_audioSource,SoundAssetsType.hit);
        }

        public virtual void TakeDamager(float damager, string hitAnimationName)
        {
            throw new NotImplementedException();
        }

        public virtual void TakeDamager(float damagar, string hitAnimationName, Transform attacker)
        {
            SetAttacker(attacker);
            
        }

        #endregion
        
        
        #region 外部接口

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="animationName"></param>
        public void PlayAnimation(string animationName)
        {
            _animator.Play(animationName, 0, 0f);
        }

        /// <summary>
        /// 判断是否是当前正在播放的动画标签
        /// </summary>
        /// <returns></returns>
        public bool GetAnimation(string animationTag)
        {
            return _animator.CheckAnimationTag(animationTag);
        }

        #endregion
        
        
    }
}

