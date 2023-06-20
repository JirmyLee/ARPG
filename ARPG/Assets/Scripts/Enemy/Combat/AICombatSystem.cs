using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UGG.Combat;

public class AICombatSystem : CharacterCombatSystemBase
{
    [SerializeField, Header("检测范围")] private Transform detectionCenter; //AI视野检测中心
    [SerializeField] private float detectionRange;      //AI视野检测范围
    [SerializeField] private LayerMask whatisEnemy;      //检测敌人层级
    [SerializeField] private LayerMask whatisObs;         //检测障碍物层级
    
    private Collider[] colliderTarget = new Collider[1];
    [SerializeField, Header("目标")] private Transform currentTarget;
    
    //AnimationID
    private int lockOnID = Animator.StringToHash("LockOn");
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        AIView();
        LockOnTarget();
        UpdateAnimationMove();
    }

    private void AIView()
    {
        int targetCount = Physics.OverlapSphereNonAlloc(detectionCenter.position, detectionRange, colliderTarget, whatisEnemy);
        
        if (targetCount > 0)
        {
            //Debug.LogFormat("targetCount:{0},name:{1}",targetCount,colliderTarget[0].name);
            //Debug.DrawLine(transform.root.position + transform.root.up * 0.5f, colliderTarget[0].transform.position,Color.red);
            float enemyRange = (colliderTarget[0].transform.position - transform.root.position).magnitude;
            float rayLength = enemyRange > detectionRange ? detectionRange : enemyRange;
            //向检测目标位置发射一条射线，查看是否有障碍物遮挡
            if (!Physics.Raycast((transform.root.position + transform.root.up * 0.5f), (colliderTarget[0].transform.position - transform.root.position).normalized, out var hitInfo,
                    rayLength, whatisObs))
            {
                float angle = Vector3.Dot((colliderTarget[0].transform.position - transform.root.position).normalized, transform.root.forward);
                //Debug.LogFormat("not obs zudang!!angle:{0}",angle);
                if (angle > 0.35f)   //垂直是0
                {
                    currentTarget = colliderTarget[0].transform;
                    return;
                }
            }
        }

        //currentTarget = null;
    }

    private void LockOnTarget()
    {
        if (_animator.CheckAnimationTag("Motion") && currentTarget != null)
        {
            _animator.SetFloat(lockOnID,1);
            transform.root.rotation = transform.LockOnTarget(currentTarget, transform.root.transform, 50f);
        }
        else
        {
            _animator.SetFloat(lockOnID,0);
        }
    }
    
    public Transform GetCurrentTarget()
    {
        return currentTarget;
    }

    public float GetCurrentTargetDistance() => Vector3.Distance(currentTarget.position, transform.root.position);

    public Vector3 GetDirectionForTarget() => (currentTarget.position - transform.root.position).normalized;
    
    public float GetAttackRange() => attackDetectionRang;

    private void UpdateAnimationMove()
    {
        if (_animator.CheckAnimationTag("Roll"))
        {
            _characterMovementBase.CharacterMoveInterface(transform.root.forward,_animator.GetFloat(animationMoveID),true);
        }
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(detectionCenter.position, detectionRange);
    }
    
    /// <summary>
    /// 判断target是否在扇形区域内
    /// </summary>
    /// <param name="sectorAngle">扇形角度</param>
    /// <param name="sectorRadius">扇形半径</param>
    /// <param name="attacker">攻击者的transform信息</param>
    /// <param name="target">目标</param>
    /// <returns>目标target在扇形区域内返回true 否则返回false</returns>
    public bool IsInSectorRange(float sectorAngle, float sectorRadius, Transform attacker, Transform target)
    {
        //攻击者位置指向目标位置的向量
        Vector3 direction = target.position - attacker.position;
        //点乘积结果
        float dot = Vector3.Dot(direction.normalized, transform.forward);
        //反余弦计算角度
        float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg; //弧度转度
        return offsetAngle < sectorAngle * .5f && direction.magnitude < sectorRadius;
    }
    
    public CombatSkillBase GetAnDoneSkill()
    {
        // for (int i = 0; i < skills.Count; i++)
        // {
        //     if (skills[i].GetSkillIsDone()) return skills[i];
        //     else continue;
        // }

        return null;
    }
}
