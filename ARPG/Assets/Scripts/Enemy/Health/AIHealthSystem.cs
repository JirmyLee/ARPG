//using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGG.Health
{
    public class AIHealthSystem : CharacterHealthSystemBase
    {
        [SerializeField] private int maxParryCount;             //最大格挡次数
        [SerializeField] private int counterattackParryCount;   //当格挡次数大于设置的值，自动触发反击技能

        [SerializeField] private int maxHitCount;               //最大连续受击次数
        [SerializeField] private int hitCount;                  //如果受伤次数超过最大受伤次数 触发脱身技能
        
        private void LateUpdate()
        {
            OnHitLockTarget();
        }

        public override void TakeDamager(float damagar, string hitAnimationName, Transform attacker)
        {
            SetAttacker(attacker);  //设置攻击者
            
            if (maxParryCount > 0 && !OnInvincibleState())
            {
                //如果反击格挡次数等于2
                if (counterattackParryCount >= 3)
                {
                    //触发格挡反击技能
                    _animator.Play("CounterAttack", 0, 0f);
                    counterattackParryCount = 0;
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                }
                else
                {
                    OnParry(hitAnimationName);
                }
                maxParryCount--;
            }
            else
            {
                if (hitCount >= maxHitCount && !_animator.CheckAnimationTag("Flick_0"))
                {
                    //受击达到最大次数，触发脱身技能,随机3种后撤方法
                    int rollType = Random.Range(0, 3);
                    Debug.LogFormat("roll type :{0}",rollType);
                    if (rollType == 0)
                    {
                        _animator.Play("Roll_B", 0, 0f);
                    }
                    else if (rollType == 1)
                    {
                        _animator.Play("Roll_B1", 0, 0f);
                    }
                    else
                    {
                        _animator.Play("Roll_B2", 0, 0f);
                    }
                    

                    hitCount = 0;
                    maxParryCount += Random.Range(1, 4);    //脱身后恢复一定次数的格挡
                }
                else
                {
                    if (!OnInvincibleState())
                    {
                        _animator.Play(hitAnimationName, 0, 0f);  //播放对应的受击动画
                        GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.hit);
                        //transform.rotation = transform.LockOnTarget(attacker, transform, 50f);
                        hitCount++;
                    }
                } 
            }
        }
        
        /// <summary>
        /// 处于翻滚、处决、或跳跃攻击状态无敌不受到伤害（可以再加个霸体特效）
        /// </summary>
        private bool OnInvincibleState()
        {
            if (_animator.CheckAnimationTag("Roll") || _animator.CheckAnimationTag("CounterAttack") || _animator.CheckAnimationName("Attack_Sliding")) 
                return true;

            return false;
        }

        private void OnHitLockTarget()
        {
            if (_animator.CheckAnimationTag("Hit"))
            {
                transform.rotation = transform.LockOnTarget(currentAttacker, transform, 50f);
            }
        }
        
        private void OnParry(string hitName)
        {
            switch (hitName)
            {
                default:
                    _animator.Play(hitName, 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.hit);
                    break;
                case "Hit_D_Up":
                    //_animator.Play("ParryF", 0, 0f);
                    //GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    _animator.Play("Avoid_R", 0, 0f);
                    counterattackParryCount++;  //增加格挡反击计数
                    break;
                case "Hit_D_Right":
                    _animator.Play("Avoid_L", 0, 0f);
                    counterattackParryCount++;
                    break;
                case "Hit_H_Left":
                    _animator.Play("Parry_L", 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    counterattackParryCount++;
                    break;
                case "Hit_H_Right":
                    _animator.Play("Parry_R", 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    counterattackParryCount++;
                    break;
                case "Hit_Up_Left":
                    _animator.Play("Avoid_Up", 0, 0f);
                    //GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    counterattackParryCount++;
                    break;
                case "Hit_Up_Right":
                    _animator.Play("Parry_F", 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    counterattackParryCount++;
                    break;
            }
        }
    }
}