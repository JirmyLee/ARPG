using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//战斗状态
[CreateAssetMenu(fileName = "AICombat",menuName = "StateMachine/State/AICombat")]
public class AICombat : StateActionSO
{
    public override void OnUpdate()
    {
        //Debug.Log("AI State: Combat");
        NotCombat();
    }

    private void NotCombat()
    {
        //如果不能攻击，就逃跑
        if (animator.CheckAnimationTag("Motion"))
        {
            if (combatSystem.GetCurrentTargetDistance() < 4.1f + 0.1f)
            {
                //往后退，不退就会挨打
                movement.CharacterMoveInterface(-movement.transform.forward,1.5f,true);
                animator.SetFloat(verticalID,-1f,0.23f,Time.deltaTime);
                animator.SetFloat(horizontalID,0f,0.1f,Time.deltaTime);

                if (combatSystem.GetCurrentTargetDistance() < (combatSystem.GetAttackRange() * 2))
                {
                    animator.Play("Roll_B",0,0);
                }
            }

            if (combatSystem.GetCurrentTargetDistance() > 4.1f + 0.1f)
            {
                //和玩家超过一定距离，往前走
                movement.CharacterMoveInterface(movement.transform.forward,1.5f,true);
                animator.SetFloat(verticalID,1f,0.23f,Time.deltaTime);
                animator.SetFloat(horizontalID,0f,0.1f,Time.deltaTime);
            }
        }
    }
}
