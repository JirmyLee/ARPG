using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//右键菜单添加创建技能资源类，可以直接设置技能CD等属性
[CreateAssetMenu(fileName = "NormalSkill", menuName = "Skill/NormalSkill")]
//通用技能，如果新加不同类型技能，可以另外加一个类，写上不同逻辑
public class NormalSkill : CombatSkillBase
{
    public override void InvokeSkill()
    {
        if (animator.CheckAnimationTag("Motion") && skillCanCast)
        {
            float distance = combat.GetCurrentTargetDistance();
            //当技能被激活 但还没进入允许释放距离
            if (distance > skillUseDistance + 0.1f)
            {
                animator.SetFloat(verticalID, 1f, 0.25f, Time.deltaTime);
                animator.SetFloat(horizontalID, 0f, 0.25f, Time.deltaTime);

                if (distance > skillUseDistance * 5)   //距离远，直接跑
                {
                    animator.SetFloat(runID, 1f, 0.25f, Time.deltaTime);
                    movement.CharacterMoveInterface(combat.GetDirectionForTarget(), 5f, true);
                }
                else
                {
                    //慢慢减速，这里(distance-skillUseDistance)/(5*skillUseDistance)和distance/skillUseDistance只是为了效果优化的调试值
                    animator.SetFloat(runID, (distance-skillUseDistance)/(5*skillUseDistance), 0.25f, Time.deltaTime);
                    movement.CharacterMoveInterface(combat.GetDirectionForTarget(), distance/skillUseDistance, true);
                }
            }
            else
            {
                UseSkill();
            }
        }
    }
}
