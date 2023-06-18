using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGG.Move;
using UGG.Health;

public abstract class StateActionSO : ScriptableObject
{
    //引用
    protected AICombatSystem combatSystem;
    protected AIMovement movement;
    protected AIHealthSystem healthSystem;
    protected Animator animator;
    protected Transform self;
    
    [SerializeField] protected int statePriority;//状态优先级
    
    //animationID
    protected int animationMoveID = Animator.StringToHash("AnimationMove");
    protected int movementID = Animator.StringToHash("Movement");
    protected int horizontalID = Animator.StringToHash("Horizontal");
    protected int verticalID = Animator.StringToHash("Vertical");
    protected int lAtkID = Animator.StringToHash("LAtk");
    protected int runID = Animator.StringToHash("Run");

    //移动速度
    protected float walkSpeed = 1.5f;
    protected float runSpeed = 5f;
    [SerializeField] protected float currentMoveSpeed;
    
    public virtual void OnEnter(StateMachineSystem stateMachineSystem) { }

    public abstract void OnUpdate();

    public virtual void OnExit() { }

    /// <summary>
    /// 获取状态优先级
    /// </summary>
    /// <returns></returns>
    public int GetStatePriority() => statePriority;
    
    public void InitState(StateMachineSystem stateMachineSystem)
    {
        combatSystem = stateMachineSystem.GetComponentInChildren<AICombatSystem>();

        movement = stateMachineSystem.GetComponent<AIMovement>();

        healthSystem = stateMachineSystem.GetComponent<AIHealthSystem>();

        animator = stateMachineSystem.GetComponentInChildren<Animator>();

        self = stateMachineSystem.transform;
    }
    
    protected void SetHorizontalAnimation(float value)
    {
        animator.SetFloat(horizontalID, value);
        currentMoveSpeed = 0.85f;
    }

    protected void SetVerticalAnimation(float value)
    {
        animator.SetFloat(verticalID, value);
        currentMoveSpeed = 1.5f;
    }


    protected void ResetAnimation()
    {
        currentMoveSpeed = 0f;
        animator.SetFloat(verticalID, 0);
        animator.SetFloat(horizontalID, 0);

    }
}
