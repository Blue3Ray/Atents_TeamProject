using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneEnemy : MonoBehaviour
{
    // 공격 기능 ---------------------------------------------------------------

    float attackMaxCoolTime = 3.0f;
    float attackCurrentCoolTime = 3.0f;

    /// <summary>
    /// 공격 범위
    /// </summary>
    AttackArea attackArea;

    /// <summary>
    /// 공격할 대상
    /// </summary>
    CharacterBase attackTarget;

    // 이동 기능 ---------------------------------------------------------------
    public float maxMoveSpeed;
    float currentMoveSpeed;
    public float CurrentMoveSpeed
    {
        get => currentMoveSpeed;
        set
        {
            if(currentMoveSpeed != value)
            {
                currentMoveSpeed = value;
                animator.SetFloat(Hash_Speed, currentMoveSpeed);
            }
        }
    }

    Vector2 moveDir = Vector2.right;
    public Vector2 MoveDir
    {
        get => moveDir;
        set
        {
            if(moveDir != value)
            {
                moveDir = value;
                transform.localScale = new Vector3(moveDir.x, 1, 1);
            }
        }
            
    }

    /// <summary>
    /// 생성 될때 기준 좌우로 -3, 3까지
    /// </summary>
    public Vector2[] waypoints;

    /// <summary>
    /// 현재 목표하는 웨이포인트 인덱스
    /// </summary>
    int currentWaypointIndex = 0;

    /// <summary>
    /// 대기 시간
    /// </summary>
    public float waitTime = 2.0f;

    /// <summary>
    /// 대기 시간 측정 용
    /// </summary>
    float waitTimer = 2.0f;
    protected float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if (waitTimer < 0)
            {
                State = EnemyState.Patrol;
            }
        }
    }

    /// <summary>
    /// 추적할 대상
    /// </summary>
    Transform chaseTarget;

    /// <summary>
    /// 원거리 감지 범위
    /// </summary>
    float farSightRange = 5.0f;

    /// <summary>
    /// 근거리 감지 범위
    /// </summary>
    float closeSightRange = 1.5f;

    // 상태 머신 ---------------------------------------------------------------
    protected enum EnemyState
    {
        Wait = 0,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    EnemyState state = EnemyState.Patrol;
    EnemyState State
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
                        WaitTimer = waitTime;           // 대기 상태 들어가면 최대 대기시간 설정
                        onStateUpdate = Update_Wait;
                        break;
                    case EnemyState.Patrol:             // Wait, Patrol, Chase는 Speed에 따라서 애니메이션 바뀜
                        CurrentMoveSpeed = maxMoveSpeed;
                        onStateUpdate = Update_Patrol;
                        break;
                    case EnemyState.Chase:
                        CurrentMoveSpeed = maxMoveSpeed;
                        onStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:
                        CurrentMoveSpeed = 0;
                        animator.SetTrigger(Hash_IsAttack);
                        attackCurrentCoolTime = attackMaxCoolTime;
                        onStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Dead:
                        CurrentMoveSpeed = 0;
                        animator.SetTrigger(Hash_IsDead);
                        onStateUpdate = Update_Dead;
                        // StartCoroutine(LifeOver(3.0f));
                        break;
                }
            }
        }
    }

    System.Action onStateUpdate;

    // 기타 컴포넌트 ---------------------------------------------------------------

    Animator animator;

    readonly int Hash_GetHit = Animator.StringToHash("GetHit");
    readonly int Hash_IsDead = Animator.StringToHash("IsDead");
    readonly int Hash_Speed = Animator.StringToHash("Speed");
    readonly int Hash_IsAttack = Animator.StringToHash("IsAttack");


    Rigidbody2D rb;


    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

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

        OnInitialize();
    }

    public void OnInitialize()
    {
        waypoints = new Vector2[2];
        waypoints[0] = transform.position + new Vector3(3, 0);
        waypoints[1] = transform.position + new Vector3(-3, 0);
        State = EnemyState.Wait;
    }

    private void Update()
    {
        onStateUpdate();
    }

    void Update_Wait()
    {
        if(SearchPlayer())
        {
            Debug.Log("Player를 찾음");
            State = EnemyState.Chase;
        }
        else
        {
            WaitTimer -= Time.deltaTime;
        }
    }

    void Update_Patrol()
    {
        if (SearchPlayer())
        {
            Debug.Log("Player를 찾음");
            State = EnemyState.Chase;
        }
        else
        {
            rb.MovePosition(transform.position + (Vector3) (CurrentMoveSpeed * Time.deltaTime * 0.5f * MoveDir));
            //transform.Translate(CurrentMoveSpeed * Time.deltaTime * dir);
            if (Mathf.Abs(waypoints[currentWaypointIndex].x - transform.position.x) < 0.1f)
            {
                currentWaypointIndex++;
                currentWaypointIndex %= waypoints.Length;
                MoveDir *= new Vector2(-1, 0);
                State = EnemyState.Wait;
            }
        }
    }

    void Update_Chase()
    {
        if(SearchPlayer())
        {
            // 타켓한테 다가가기
            Vector3 targetPos = chaseTarget.transform.position;

            if(targetPos.x > transform.position.x)
            {
                MoveDir = new Vector2(1, 0);
            }
            else
            {
                MoveDir = new Vector2(-1, 0);
            }
            rb.MovePosition(transform.position + (Vector3)(CurrentMoveSpeed * Time.deltaTime * moveDir));
        }
        else
        {
            // 타켓 대상을 잃어버릴 때 대기 상태로 돌입
            State = EnemyState.Wait;
        }
    }

    void Update_Attack()
    {
        attackCurrentCoolTime -= Time.deltaTime;
        
        if (attackCurrentCoolTime < 0)
        {
            animator.SetTrigger(Hash_IsAttack);
            attackCurrentCoolTime = attackMaxCoolTime;
            //(attackTarget);
        }
    }

    void Update_Dead()
    {

    }


    // ---------------------------------

    bool SearchPlayer()
    {
        bool result = false;
        chaseTarget = null;

        Collider2D target = Physics2D.OverlapCircle(transform.position, farSightRange, LayerMask.GetMask("Player"));
        //Physics2D.Raycast()

        //원거리 감지 범위에 들어가면 target을 인지함(아직 state바뀌지는 않음)
        if (target != null)
        {
            Vector3 player = target.transform.position;
            
            if (Vector2.SqrMagnitude(player - transform.position) < closeSightRange * closeSightRange)
            {
                // 근접 범위이면
                chaseTarget = target.transform;
                result = true;
            }
            else
            {
                // 근접 범위 밖이면
                chaseTarget = target.transform;
                result = true;
            }
        }
        return result;
    }
}
