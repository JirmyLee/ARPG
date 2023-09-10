using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UGG.Health
{
    public class PlayerHealthSystem : CharacterHealthSystemBase
    {
        private bool canExecute = false;

        protected override void Update()
        {
            base.Update();

            OnHitLockTarget();
        }

        public override void TakeDamager(float damagar, string hitAnimationName, Transform attacker)
        {
            SetAttacker(attacker);

            if (CanParry())
            {
                Parry(hitAnimationName);    //格挡
            }
            else
            {
                _animator.Play(hitAnimationName, 0, 0f);
                GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.hit);
            }
        }
        
        #region Parry         //格挡

        private bool CanParry()
        {
            //出于格挡或者格挡成功时允许格挡
            if (_animator.CheckAnimationTag("Parry") || _animator.CheckAnimationTag("ParryHit"))
            {
                return true;
            }
            return false;
        }

        //根据攻击方向播放格挡动画
        private void Parry(string hitName)
        {
            if (!CanParry()) 
                return;

            switch (hitName)
            {
                //这里是根据AI攻击传的攻击方向进行枚举，实际应该每种受击方向都加进去
                case "Hit_D_Up":
                    // _animator.Play("Parry_F", 0, 0f);    //播放对应的格挡动画
                    // GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);

                    if(currentAttacker.TryGetComponent(out CharacterHealthSystemBase health))
                    {
                        health.PlayAnimation("Flick_0");  //获取目标身上的脚本组件并播放弹刀动画
                        GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    }
                    
                    canExecute = true;

                    //游戏时间缓慢 给玩家处决反应时间
                    Time.timeScale = 0.2f;
                    //创建定时器，在时间结束之后恢复时间缩放
                    GameObjectPoolSystem.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(0.25f, () =>
                    {
                        canExecute = false;

                        if (Time.timeScale < 1f)
                        {
                            Time.timeScale = 1f;
                        }
                    }, false);
                    break;
                case "Hit_H_Right":
                    _animator.Play("Parry_R", 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    break;
                case "Hit_H_Left":
                    _animator.Play("Parry_L", 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    break;
                case "Hit_D_Right":
                    _animator.Play("Parry_D_R", 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    break;
                case "Hit_D_Left":
                    _animator.Play("Parry_D_L", 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    break;
                case "Hit_Up_Right":
                    _animator.Play("Parry_R", 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    break;
                case "Hit_Up_Left":
                    _animator.Play("Parry_L", 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    break;
                case "Hit_Front":
                    _animator.Play("Parry_Broken", 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.parry);
                    break;
                default:
                    _animator.Play(hitName, 0, 0f);
                    GameAssets.Instance.PlaySoundEffect(_audioSource, SoundAssetsType.hit);
                    break;
            }
        }

        #endregion

        #region Hit

        private bool CanHitLockAttacker()
        {
            return true;
        }

        //被攻击时面向攻击者
        private void OnHitLockTarget()
        {
            //检测当前动画是否处于受击和格挡状态
            if ((_animator.CheckAnimationTag("Hit") && (!_animator.CheckAnimationName("Hit_Executed_1"))) || _animator.CheckAnimationTag("ParryHit"))
            {
                transform.rotation = transform.LockOnTarget(currentAttacker, transform, 50f);
            }
        }

        #endregion

        //外部接口，返回处决状态
        public bool GetCanExecute() => canExecute;
    }
}

