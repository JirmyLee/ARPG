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
        
        //允许攻击输入
        [SerializeField] private bool allowAttackInput;
        
        //当前目标的引用
        [SerializeField] private Transform currentTarget;
        
        private void Update()
        {
            PlayerAttackAction();
            DetectionTarget();
            ActionMotion();
            UpdateCurrentTarget();
            PlayerParryAction();
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
        
        //是否允许格挡输入
        private bool CanInputParry()
        {
            //玩家移动、格挡和受伤超过0.2秒后允许格挡输入
            if (_animator.CheckAnimationTag("Motion") || _animator.CheckAnimationTag("Parry") || _animator.CheckCurrentTagAnimationTimeIsExceed("Hit",0.20f))
            {
                return true;
            }
            
            return false;
        }
        
        //玩家攻击事件
        private void PlayerAttackAction()
        {
            // if (!CanInputAttack())
            // {
            //     return;
            // }
            //当玩家处于Motion状态(idle)且不处于过渡状态也允许玩家输入攻击信号
            if (!allowAttackInput)
            {
                if (_animator.CheckCurrentTagAnimationTimeIsExceed("Motion", 0.01f) && !_animator.IsInTransition(0))
                {
                    SetAllowAttackInput(true);
                }
                else
                {
                    return;
                }
            }
            
            //如果玩按下鼠标左键
            if (_characterInputSystem.playerLAtk)
            {
                //触发默认攻击动画
                _animator.SetTrigger(lAtkID);
                SetAllowAttackInput(false); //攻击后禁用输入直到允许输入时间到
            }

            //如果玩家一直按住鼠标右键
            if (_characterInputSystem.playerRAtk)
            {
                //并且按下左键
                if (_characterInputSystem.playerLAtk)
                {
                    //触发大剑攻击动画
                    _animator.SetTrigger(lAtkID);
                    SetAllowAttackInput(false);
                }
            }
            
            _animator.SetBool(sWeaponID,_characterInputSystem.playerRAtk);
        }

        //玩家格挡操作
        private void PlayerParryAction()
        {
            if (CanInputParry())
            {
                _animator.SetBool(parryID,_characterInputSystem.playerDefen);
                return;
            }
            
            _animator.SetBool(parryID,false);
            return;
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
            else if(_animator.CheckAnimationTag("Parry"))
            {
                transform.root.rotation = transform.LockOnTarget(currentTarget, transform.root,500f);    //旋转当前角色的root子节点，LockOnTarget为拓展方法
            }
        }

        private void ActionMotion()
        {
            if (_animator.CheckAnimationTag("Attack") || _animator.CheckAnimationTag("GSAttack"))
            {
                _characterMovementBase.CharacterMoveInterface(transform.forward,_animator.GetFloat(animationMoveID) * attackMoveMult,true);
                //_characterMovementBase.CharacterMoveInterface(transform.up,_animator.GetFloat(animationJumpID) * attackMoveMult,true);
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
        
        /// <summary>
        /// 获取当前是否允许玩家攻击输入
        /// </summary>
        /// <returns></returns>
        public bool GetAllowAttackInput() => allowAttackInput;

        /// <summary>
        /// 设置是否允许玩家输入攻击信号 
        /// </summary>
        /// <param name="allow"></param>
        public void SetAllowAttackInput(bool allow) => allowAttackInput = allow;
        
        #endregion
    }
}

