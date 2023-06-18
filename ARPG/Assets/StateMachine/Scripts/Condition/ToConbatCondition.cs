using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ToCombatCondition", menuName = "StateMachine/Condition/ToCombatCondition")]
public class ToConbatCondition : ConditionSO
{
    public override bool ConditionSetUp()
    {
        //战斗脚本中能获取到当前攻击目标则触发状态转换
        return combatSystem.GetCurrentTarget() != null;
    }
}
