using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGG.Health
{
    public class AIHealthSystem : CharacterHealthSystemBase
    {
        private void LateUpdate()
        {
            OnHitLockTarget();
        }

        public override void TakeDamager(float damagar, string hitAnimationName, Transform attacker)
        {
            SetAttacker(attacker);  //设置攻击者
            _animator.Play(hitAnimationName,0,0f);  //播放对应的受击动画
            GameAssets.Instance.PlaySoundEffect(_audioSource,SoundAssetsType.hit);
            //transform.rotation = transform.LockOnTarget(attacker, transform, 50f);
        }

        private void OnHitLockTarget()
        {
            if (_animator.CheckAnimationTag("Hit"))
            {
                transform.rotation = transform.LockOnTarget(currentAttacker, transform, 50f);
            }
        }
    }
}