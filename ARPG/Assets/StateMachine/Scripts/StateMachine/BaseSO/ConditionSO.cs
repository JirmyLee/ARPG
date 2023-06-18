using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class ConditionSO : ScriptableObject
{
    //����
    protected AICombatSystem combatSystem;
    
    [SerializeField] protected int priority;//�������ȼ�

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
    
    public abstract bool ConditionSetUp();//�����Ƿ����

    /// <summary>
    /// ��ȡ��ǰ���������ȼ�
    /// </summary>
    /// <returns></returns>
    public int GetConditionPriority() => priority;
}
