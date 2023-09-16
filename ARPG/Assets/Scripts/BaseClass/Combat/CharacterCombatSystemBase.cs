using System;
using System.Collections;
using System.Collections.Generic;
using TrailsFX;
using UGG.Move;
using UnityEngine;

namespace UGG.Combat
{
    public abstract class CharacterCombatSystemBase : MonoBehaviour
    {
        protected Animator _animator;
        protected CharacterInputSystem _characterInputSystem;
        protected CharacterMovementBase _characterMovementBase;
        protected AudioSource _audioSource;
        
        //aniamtionID
        protected int lAtkID = Animator.StringToHash("LAtk");
        protected int rAtkID = Animator.StringToHash("RAtk");
        protected int parryID = Animator.StringToHash("Parry");
        protected int animationMoveID = Animator.StringToHash("AnimationMove");
        protected int animationJumpID = Animator.StringToHash("AnimationJump");
        protected int sWeaponID = Animator.StringToHash("SWeapon");
        
        //攻击检测
        [SerializeField, Header("攻击检测")] protected Transform attackDetectionCenter;
        [SerializeField] protected float attackDetectionRang;
        [SerializeField] protected LayerMask enemyLayer;

        //特效相关
        public Transform effectsRoot;       //特效根节点
        public TrailEffect[] trailEffects;  //拖尾特效节点
        private Dictionary<string, GameObject> Effects = new Dictionary<string, GameObject>();  //维护特效文件
        public Transform EffectPos;
        
        protected virtual void Awake()
        {
            _animator = GetComponent<Animator>();
            _characterInputSystem = GetComponentInParent<CharacterInputSystem>();
            _characterMovementBase = GetComponentInParent<CharacterMovementBase>();
            _audioSource = _characterMovementBase.GetComponentInChildren<AudioSource>();
            
            this.Effects.Clear();
            if(this.effectsRoot != null && this.effectsRoot.childCount > 0)
            {
                for(int i = 0; i < this.effectsRoot.childCount;i++)
                {
                    this.Effects[this.effectsRoot.GetChild(i).name] = this.effectsRoot.GetChild(i).gameObject;
                    //Debug.LogFormat("add effectsRoot {0}",this.effectsRoot.GetChild(i).name);
                }
            }
        }

        
        /// <summary>
        /// 攻击动画攻击检测事件
        /// </summary>
        /// <param name="hitName">传递受伤动画名</param>
        protected virtual void OnAnimationAttackEvent(string hitName)
        {
            Collider[] attackDetectionTargets = new Collider[4];

            int counts = Physics.OverlapSphereNonAlloc(attackDetectionCenter.position, attackDetectionRang,
                attackDetectionTargets, enemyLayer);

            if (counts > 0)
            {
                for (int i = 0; i < counts; i++)
                {
                    if (attackDetectionTargets[i].TryGetComponent(out IDamagar damagar))
                    {
                        damagar.TakeDamager(0f,hitName,transform.root.transform);
                        
                    }
                }
            }
            PlayWeaponEffect();
        }
        
        private void PlayWeaponEffect()
        {
            if (_animator.CheckAnimationTag("Attack"))
            {
                //Debug.LogFormat("play sound SoundAssetsType.swordWave");
                GameAssets.Instance.PlaySoundEffect(_audioSource,SoundAssetsType.swordWave);
            }
            
            if (_animator.CheckAnimationTag("GSAttack"))
            {
                //Debug.LogFormat("play sound SoundAssetsType.GSwordWave");
                GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.GSwordWave);
            }
        }
        
        /// <summary>
        /// 粒子特效动画事件
        /// </summary>
        /// <param name="hitName">传递受伤动画名</param>
        protected virtual void OnAnimationEffectEvent(string effectName)
        {
            if(this.Effects.ContainsKey(effectName))
            {
                //this.Effects[name].transform.rotation = this.transform.rotation;
                this.Effects[effectName].SetActive(true);
            }
            else
            {
                Transform pos = EffectPos;
                pos.position = new Vector3(pos.position.x,0.05f,pos.position.z);
                FXManager.Instance.PlayEffect(effectName,pos);
            }
        }

        //显示拖尾特效
        void ShowTrailsFX(int show)
        {
            foreach (var trailEffect in this.trailEffects)
            {
                if (show > 0)
                {
                    trailEffect.enabled = true;
                }
                else
                {
                    trailEffect.enabled = false;
                }
            }
        }
        
        

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackDetectionCenter.position, attackDetectionRang);
        }
    }
}
