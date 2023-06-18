using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ConditionSO : ScriptableObject
{
    //引用
    protected AICombatSystem combatSystem;
    
    [SerializeField] protected int priority;//条件优先级

    public virtual void Init(StateMachineSystem stateSystem) { }
    
    public void InitCondition(StateMachineSystem stateSystem)
    {
        combatSystem = stateSystem.GetComponentInChildren<AICombatSystem>();

        // _movement = stateSystem.GetComponent<AIMovement>();
        //
        // _healthSystem = stateSystem.GetComponent<AIHealthSystem>();
        //
        // animator = stateSystem.GetComponentInChildren<Animator>();
    }
    
    public abstract bool ConditionSetUp();//条件是否成立

    /// <summary>
    /// 获取当前条件的优先级
    /// </summary>
    /// <returns></returns>
    public int GetConditionPriority() => priority;
}
