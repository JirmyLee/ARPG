using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachineSystem : MonoBehaviour
{
    public NB_Transition transition;
    public StateActionSO currentState;

    private void Awake()
    {
        transition?.InitTransition(this);
        currentState?.OnEnter(this);
    }


    private void Update()
    {
        StateMachineTick();
    }

    private void StateMachineTick() 
    {
        transition?.TryGetApplyCondition();//每一帧都去找是否有成立的条件
        currentState?.OnUpdate();


    }
}
