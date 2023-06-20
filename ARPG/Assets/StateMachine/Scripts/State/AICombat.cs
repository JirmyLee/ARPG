using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//战斗状态
[CreateAssetMenu(fileName = "AICombat",menuName = "StateMachine/State/AICombat")]
public class AICombat : StateActionSO
{
    private int randomHorizontal;

    private float maxCombatDirection = 1.5f;

    [SerializeField] private CombatSkillBase currentSkill;
    
    public override void OnUpdate()
    {
        AICombatAction();
    }

    public override void OnExit()
    {

    }

    private void AICombatAction()
    {
        if(currentSkill == null)
        {
            //如果当前没技能 就执行AI移动函数
            NoCombatMove();
            GetSkill();
        }
        else
        {
            currentSkill.InvokeSkill();

            if (!currentSkill.GetSkillIsDone())
            {
                currentSkill = null;
            }
        }
    }

    private void GetSkill()
    {
        if(currentSkill == null)
        {
            currentSkill = combatSystem.GetAnDoneSkill();
        }
    }

    private void NoCombatMove()
    {
        //如果动画处于Motion状态
        if (animator.CheckAnimationTag("Motion"))
        {
            if (combatSystem.GetCurrentTargetDistance() < 2f + 0.1f)
            {
                movement.CharacterMoveInterface(-combatSystem.GetDirectionForTarget(), 1.4f, true);

                animator.SetFloat(verticalID, -1f, 0.25f, Time.deltaTime);
                animator.SetFloat(horizontalID, 0f, 0.25f, Time.deltaTime);

                randomHorizontal = GetRandomHorizontal();

                if (combatSystem.GetCurrentTargetDistance() < 1.5 + 0.05f)
                {
                    if (!animator.CheckAnimationTag("Hit") || !animator.CheckAnimationTag("Defen"))
                    {
                        animator.Play("Attack_0", 0, 0f);

                        randomHorizontal = GetRandomHorizontal();
                    }
                }
            }
            else if (combatSystem.GetCurrentTargetDistance() > 2f + 0.1f && combatSystem.GetCurrentTargetDistance() < 6.1 + 0.5f)
            {
                if (HorizontalDirectionHasObject(randomHorizontal))
                {
                    switch (randomHorizontal)
                    {
                        case 1:
                            randomHorizontal = -1;
                            break;
                        case -1:
                            randomHorizontal = 1;
                            break;
                        default:
                            break;
                    }
                }

                movement.CharacterMoveInterface(movement.transform.right * ((randomHorizontal == 0) ? 1 : randomHorizontal), 1.4f, true);

                animator.SetFloat(verticalID, 0f, 0.25f, Time.deltaTime);
                animator.SetFloat(horizontalID, ((randomHorizontal == 0) ? 1 : randomHorizontal), 0.25f, Time.deltaTime);
            }
            else if (combatSystem.GetCurrentTargetDistance() > 6.1 + 0.5f)
            {
                movement.CharacterMoveInterface(movement.transform.forward, 1.4f, true);

                animator.SetFloat(verticalID, 1f, 0.25f, Time.deltaTime);
                animator.SetFloat(horizontalID, 0f, 0.25f, Time.deltaTime);
            }
        }
        else
        {
            animator.SetFloat(verticalID, 0f);
            animator.SetFloat(horizontalID, 0f);
            animator.SetFloat(runID, 0f);
        }
    }

    private bool HorizontalDirectionHasObject(int direction)
    {
        return Physics.Raycast(movement.transform.position, movement.transform.right * direction, 1.5f, 1 << 8);
    }

    private void SetAnimationValue(float movement,float horizontal,float vertical,float run)
    {
        animator.SetFloat(movementID, movement, 0.15f, Time.deltaTime);
        animator.SetFloat(horizontalID, horizontal, 0.15f, Time.deltaTime);
        animator.SetFloat(verticalID, vertical, 0.15f, Time.deltaTime);
        animator.SetFloat(runID, run, 0.15f, Time.deltaTime);
    }

    private int GetRandomHorizontal() => Random.Range(-1, 2);
}
