using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyBase : CharacterBase
{
    // 상태 머신
    // 상태 : 대기, 순찰, 추적, 공격, 사망
    protected enum EnemyState
    {
        Wait = 0,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    EnemyState state = EnemyState.Patrol;
    protected EnemyState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                state = value;
                Debug.Log($"{state}");
                switch (state)
                {
                    case EnemyState.Wait:

                        CurrentMoveSpeed = 0;

                        break;
                    case EnemyState.Patrol:

                        CurrentMoveSpeed = maxMoveSpeed;

                        onStateUpdate = Update_Patrol;
                        break;
                    case EnemyState.Chase:

                        CurrentMoveSpeed = maxMoveSpeed;

                        onStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:


                        animator.SetTrigger(Hash_IsAttack);

                        onStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Dead:
                        animator.SetTrigger(Hash_IsDead);

                        onStateUpdate = Update_Dead;
                        break;
                }
            }
        }
    }

    // 이동 기능 부분

    public float maxMoveSpeed;

    float currentMoveSpeed;
    public float CurrentMoveSpeed
    {
        get => currentMoveSpeed;
        set
        {
            if (currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                animator.SetFloat(Hash_Speed, currentMoveSpeed);
            }
        }
    }

    // 공격 기능 부분 ------------------------------------------------------

    AttackArea attackArea;

    AttackArea detectedArea;

    /// <summary>
    /// 공격 대상(1개)
    /// </summary>
    CharacterBase attackTarget;

    CharacterBase chaseTarget;

    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    public float attackCoolTime = 1.0f;

    /// <summary>
    /// 공격 쿨타임 현재 진행 시간(0이되면 공격 가능)
    /// </summary>
    float attackCurrentCoolTime = 1.0f;

    /// <summary>
    /// update 함수 돌릴 델리게이트
    /// </summary>
    System.Action onStateUpdate;

    Animator animator;

    readonly int Hash_GetHit = Animator.StringToHash("GetHit");
    readonly int Hash_IsDead = Animator.StringToHash("IsDead");
    readonly int Hash_Speed = Animator.StringToHash("Speed");
    readonly int Hash_IsAttack = Animator.StringToHash("IsAttack");


    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();

        AttackArea[] areas = GetComponentsInChildren<AttackArea>();
        attackArea = areas[0];
        attackArea.onPlayerIn += (target) =>
        {
            if (State == EnemyState.Chase)       // 추적 상태 였을 때
            {
                attackTarget = target;          // 공격 대상 지정
                State = EnemyState.Attack;      // 공격 상태로 변경
            }
        };

        attackArea.onPlayerOut += (target) =>
        {
            if (attackTarget == target)          // 공격 대상이 나가면 
            {
                attackTarget = null;            // 공격 대상 초기화
                if (State != EnemyState.Dead)    // 죽은 상태가 아니면
                {
                    State = EnemyState.Chase;   // 추적 상태로 변경
                }
            }
        };

        detectedArea = areas[1];
        detectedArea.onPlayerIn += (target) =>
        {
            if (State != EnemyState.Chase)       // 공격상태가 아닐 때
            {
                chaseTarget = target;          // 공격 대상 지정
                State = EnemyState.Chase;      // 공격 상태로 변경
            }
        };

        detectedArea.onPlayerOut += (target) =>
        {
            if (chaseTarget == target)          // 추적 대상이 나가면 
            {
                chaseTarget = null;            // 공격 대상 초기화
                if (State != EnemyState.Dead)    // 죽은 상태가 아니면
                {
                    State = EnemyState.Wait;   // 대기 상태로 변경
                }
            }
        };
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        State = EnemyState.Wait;
    }

    // 업데이트 함수들 -----------------------------------------------------

    private void Update()
    {
        onStateUpdate();
    }

    void Update_Patrol()
    {

    }

    void Update_Chase()
    {

    }

    void Update_Attack()
    {
        attackCurrentCoolTime -= Time.deltaTime;
        if (attackCurrentCoolTime < 0)
        {
            animator.SetTrigger(Hash_IsAttack);
            //Attack(attackTarget);
        }
    }

    void Update_Dead()
    {

    }


    //--------------------------------------------------------

    /// <summary>
    /// 플레이어를 찾는 함수
    /// </summary>
    /// <returns>참이면 찾음, 거짓이면 찾지 못함</returns>
    bool SearchPlayer()
    {
        bool result = false;
        chaseTarget = null;

        //Collider[] targets = Physics.OverlapSphere(transform.position, farSightRange, LayerMask.GetMask("Player"));
        //Physics2D.Raycast()

        //if (targets.Length > 0)
        //{
        //    Vector3 player = targets[0].transform.position;
        //    // 근접 범위안에 들어가면 참
        //    if (Vector3.SqrMagnitude(player - transform.position) < closeSightRange * closeSightRange)
        //    {
        //        chaseTarget = targets[0].transform;
        //        result = true;
        //    }
        //    else
        //    {
        //        // 시야에 들어왔고
        //        if (IsInSightAngle(player - transform.position))
        //        {
        //            // 시야에 가리는게 없으면 참
        //            if (IsSightClear(player - transform.position))
        //            {
        //                chaseTarget = targets[0].transform;
        //                result = true;
        //            }
        //        }
        //    }
        //}
        return result;
    }


    // 공격 기능들-----------------------------------------------

    public override void Attack(CharacterBase target, float knockBackPower)
    {
        base.Attack(target, knockBackPower);
        attackCurrentCoolTime = attackCoolTime;
    }

    public override void Defence(float damage, ElemantalStatus elemantal = null)
    {
        base.Defence(damage, elemantal);
        animator.SetTrigger(Hash_GetHit);
    }

    public override void Defence(float damage, float knockBackPower, ElemantalStatus elemantal = null)
    {
        base.Defence(damage, knockBackPower, elemantal);
        animator.SetTrigger(Hash_GetHit);
    }

    
}
