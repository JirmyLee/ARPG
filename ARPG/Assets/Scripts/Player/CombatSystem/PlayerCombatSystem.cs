using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UGG.Combat
{
    public class PlayerCombatSystem : CharacterCombatSystemBase
    {
        
        //Speed
        [SerializeField, Header("攻击移动速度倍率"), Range(.1f, 10f)]
        private float attackMoveMult;
        
        //检测
        [SerializeField, Header("检测敌人")] private Transform detectionCenter;
        [SerializeField] private float detectionRang;

        //缓存
        private Collider[] detectionedTarget = new Collider[1];
        
        //当前目标的引用
        [SerializeField] private Transform currentTarget;
        
        private void Update()
        {
            PlayerAttackAction();
            DetectionTarget();
            ActionMotion();
            UpdateCurrentTarget();
        }

        private void LateUpdate()
        {
            OnAttackActionAutoLock();
        }

        //是否允许攻击输入
        private bool CanInputAttack()
        {
            //攻击动画过渡时间必须从0.27%以后开始，且不能勾选exit time
            if (_animator.CheckAnimationTag("Motion") || _animator.CheckCurrentTagAnimationTimeIsExceed("Attack", 0.27f))
            {
                return true;
            }
            
            return false;
        }
        
        private void PlayerAttackAction()
        {
            if (!CanInputAttack())
            {
                return;
            }
            
            if (_characterInputSystem.playerLAtk)
            {
                _animator.SetTrigger(lAtkID);
            }
            
            //如果玩按下鼠标左键
            if (_characterInputSystem.playerLAtk)
            {
                //触发默认攻击动画
                _animator.SetTrigger(lAtkID);
            }
            
            //如果玩家一直按住鼠标右键
            if (_characterInputSystem.playerRAtk)
            {
                //并且按下左键
                if (_characterInputSystem.playerLAtk)
                {
                    //触发大剑攻击动画
                    _animator.SetTrigger(lAtkID);
                }
            }
            
            _animator.SetBool(sWeaponID,_characterInputSystem.playerRAtk);
        }

        //攻击动作自动锁定目标
        private void OnAttackActionAutoLock()
        {
            if (CanAttackLockOn())
            {
                if (_animator.CheckAnimationTag("Attack") || _animator.CheckAnimationTag("GSAttack"))
                {
                    transform.root.rotation = transform.LockOnTarget(currentTarget, transform.root,50f);    //旋转当前角色的root子节点，LockOnTarget为拓展方法
                }
            }
        }

        private void ActionMotion()
        {
            if (_animator.CheckAnimationTag("Attack") || _animator.CheckAnimationTag("GSAttack"))
            {
                _characterMovementBase.CharacterMoveInterface(transform.forward,_animator.GetFloat(animationMoveID) * attackMoveMult,true);
            }
        }

        #region 动作检测
        
        /// <summary>
        /// 攻击状态是否允许自动锁定敌人
        /// </summary>
        /// <returns></returns>
        private bool CanAttackLockOn()
        {
            if (_animator.CheckAnimationTag("Attack") ||_animator.CheckAnimationTag("GSAttack"))
            {
                if (_animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.75f)
                {
                    return true;
                }
            }
            return false;
        }


        private void DetectionTarget()
        {
            int targetCount = Physics.OverlapSphereNonAlloc(detectionCenter.position, detectionRang, detectionedTarget,
                enemyLayer);

            if (targetCount > 0)
            {
                SetCurrentTarget(detectionedTarget[0].transform);
            }
        }

        private void SetCurrentTarget(Transform target)
        {
            //当前目标为空或传进来的目标和当前目标不是同一个
            if (currentTarget == null || currentTarget != target)
            {
                currentTarget = target; //如果想把当前目标砍死 应该去掉currentTarget != target
            }
        }

        private void UpdateCurrentTarget()
        {
            
            if (_animator.CheckAnimationTag("Motion"))
            {
                if (_characterInputSystem.playerMovement.sqrMagnitude > 0)
                {
                    //移动时清空当前目标
                    currentTarget = null;
                }
            }
        }
        
        #endregion
    }
}

