using System.Collections;
using System.Collections.Generic;
using UGG.Move;
using UnityEngine;

public abstract class CombatSkillBase : ScriptableObject
{
    [SerializeField] protected string skillName;
    [SerializeField] protected int skillID;
    [SerializeField] protected float skillCDTime;
    [SerializeField] protected float skillUseDistance;  //技能释放距离
    [SerializeField] protected bool skillCanCast;       //技能是否能释放

    protected Animator animator;
    protected AICombatSystem combat;
    protected CharacterMovementBase movement;

    //AnimationID
    protected int horizontalID = Animator.StringToHash("Horizontal");
    protected int verticalID = Animator.StringToHash("Vertical");
    protected int runID = Animator.StringToHash("Run");

    /// <summary>
    /// 供外部调用技能
    /// </summary>
    public abstract void InvokeSkill();

    protected void UseSkill()
    {
        animator.Play(skillName, 0, 0f);
        skillCanCast = false;
        ResetSkill();
    }

    public void ResetSkill()
    {
        //技能CD
        //从对象池拿一个计时器 通过拿出来的计时器获取它计时脚本中的创建计时器函数
        //当传入的的时间递减为0时 内部会执行委托：skillCanCast=true
        GameObjectPoolSystem.Instance.TakeGameObject("Timer").GetComponent<Timer>().CreateTime(skillCDTime, () => skillCanCast = true, false);
    }

    #region 接口

    public void InitSkill(Animator _animator, AICombatSystem _combat, CharacterMovementBase _movement)
    {
        this.animator = _animator;
        this.combat = _combat;
        this.movement = _movement;
    }

    public string GetSkillName() => skillName; 

    public int GetSkillID() => skillID;

    public bool GetSkillCanCast() => skillCanCast;

    public void SetSkillState(bool done) => skillCanCast = done;

    #endregion
}