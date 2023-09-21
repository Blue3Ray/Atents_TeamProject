using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoneEnemy : EnemyBase
{
    // 공격 기능 ---------------------------------------------------------------

    /// <summary>
    /// 공격 쿨다운 코루틴을 저장할 변수
    /// </summary>
    IEnumerator attackCoolDown;

    /// <summary>
    /// 공격 근접 범위(AttackArea까지의 거리, trigger 닿자마자 공격하는건 어색하다고 생각해서임)
    /// </summary>
    float attackRange;

    /// <summary>
    /// 공격 애니메이션 도중인지 확인하는 변수(상태머신이랑 별개로 움직임, 공격 도중 움직이는 것을 방지, 공격 도중 맞아도 공격지속시키는 역할함)
    /// </summary>
    bool isAttacking = false;


    // 이동 기능 ---------------------------------------------------------------

    /// <summary>
    /// 생성 될때 기준 좌우로 -3, 3까지
    /// </summary>
    [SerializeField]
    Vector2[] waypoints;

    /// <summary>
    /// 현재 목표하는 웨이포인트 인덱스
    /// </summary>
    int currentWaypointIndex = 0;

    protected override float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if (waitTimer < 0)
            {
                if(State == EnemyState.Wait) State = EnemyState.Patrol;
            }
        }
    }


    // 상태 머신 ---------------------------------------------------------------

    

    /// <summary>
    /// 상태 머신에서 부자연스럽게 빠른 동작 전환하는 것을 방지하기위해 사용하는 기준시간 변수
    /// </summary>
    float currentChaseTime = 0.3f;
    float ChaseMaxUpdateTime = 0.3f;


    protected override void Awake()
    {
        base.Awake();

        AttackArea[] areas = GetComponentsInChildren<AttackArea>();
        AttackArea attackArea = areas[0];

        //공격 범위에 들어왔을 때
        attackArea.onPlayerIn += (target) =>
        {
            if (State == EnemyState.Chase)       // 추적 상태 였을 때
            {
                attackTarget = target;          // 공격 대상 지정
                if(attackCurrentCoolTime < 0) State = EnemyState.Attack;      // 공격 상태로 변경
            }
        };

        //공격 범위에서 나갔을 때
        attackArea.onPlayerOut += (target) =>
        {
            if (attackTarget == target)          // 공격 대상이 나가면 
            {
                attackTarget = null;            // 공격 대상 초기화

                if (State != EnemyState.Dead && !isAttacking)    // 죽은 상태가 아니고, 공격 중이 아닐 때
                {
                    State = EnemyState.Chase;   // 추적 상태로 변경
                }
            }
        };

        // 근접 공격 범위는 근접 알아챔(?) 범위와 같음
        closeSightRange = Mathf.Abs(transform.position.x - attackArea.transform.position.x);

        attackCoolDown = AttackCoolDown();

        OnInitialize();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().isTrigger = false;

        waypoints = new Vector2[2];
        waypoints[0] = transform.position + new Vector3(3, 0);
        waypoints[1] = transform.position + new Vector3(-3, 0);

        attackCurrentCoolTime = -1;

        State = EnemyState.Wait;
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        OnInitialize();
    }


    protected override void Update()
    {
        onStateUpdate();
    }

    protected override void Update_Wait()
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

    protected override void Update_Patrol()
    {
        if (SearchPlayer())
        {
            Debug.Log("Player를 찾음");
            State = EnemyState.Chase;
        }
        else
        {
            transform.Translate(CurrentMoveSpeed * Time.deltaTime * 0.5f * MoveDir * 0.1f);
            //rb.MovePosition(transform.position + (Vector3) (CurrentMoveSpeed * Time.deltaTime * 0.5f * MoveDir));
            
            if (Mathf.Abs(waypoints[currentWaypointIndex].x - transform.position.x) < 0.1f)
            {
                currentWaypointIndex++;
                currentWaypointIndex %= waypoints.Length;
                MoveDir *= new Vector2(-1, 0);
                State = EnemyState.Wait;
            }
        }
    }

    protected override void Update_Chase()
    {
        // 플레이어와 x값이 곂칠 때 좌우 잔상효과를 제거하기 위한 움직임 업데이트 최소 시간 값 부여
        if (currentChaseTime < 0)
        {
            if (SearchPlayer())
            {
                // 타켓한테 다가가기
                Vector3 targetPos = chaseTarget.transform.position;

                if (targetPos.x > transform.position.x)
                {
                    MoveDir = new Vector2(1, 0);
                }
                else
                {
                    MoveDir = new Vector2(-1, 0);
                }

                // 근접 거리면 대기하기
                if (Mathf.Abs(targetPos.x - transform.position.x) < closeSightRange)
                {
                    CurrentMoveSpeed = 0;
                }
                else
                {
                    CurrentMoveSpeed = maxMoveSpeed;
                }
                
            }
            else
            {
                // 타켓 대상을 잃어버릴 때 대기 상태로 돌입
                State = EnemyState.Wait;
            }
            currentChaseTime = ChaseMaxUpdateTime;
        }

        transform.Translate(CurrentMoveSpeed * Time.deltaTime * MoveDir * 0.1f);
        //rb.MovePosition(transform.position + (Vector3)(CurrentMoveSpeed * Time.deltaTime * moveDir));
        currentChaseTime -= Time.deltaTime;
    }

    protected override void Update_Attack()
    {
        if (attackCurrentCoolTime < 0)
        {
            animator.SetTrigger(Hash_IsAttack);
            StopCoroutine(attackCoolDown);
            attackCurrentCoolTime = attackMaxCoolTime;
            StartCoroutine(attackCoolDown);
        }
    }

    protected override void Update_Hitted()
    {
        
    }

    protected override void Update_Dead()
    {

    }


    // 공격 기능 ---------------------------------------

    /// <summary>
    /// 공격 쿨다운 감소시키는 코루틴
    /// </summary>
    IEnumerator AttackCoolDown()
    {
        while (true)        // 코루틴을 변수에 저장하게 되면 Stop하고 다시 시작 할 때 처음부터가 아닌 중단한 곳부터 시작하게되기 때문에 무한 루프 붙여줌
        {
            while (attackCurrentCoolTime > 0)
            {
                attackCurrentCoolTime -= Time.deltaTime;
                yield return null;
            }
            yield return null;
            if(attackTarget != null) State = EnemyState.Attack;
        }
    }

    /// <summary>
    /// 애니메이션에서 공격이 시작할 때 호출 되는 함수
    /// </summary>
    void AttackStart()
    {
        isAttacking = true;
    }

    /// <summary>
    /// 애니메이션에서 공격이 끝날 때 호출되는 함수(공격하는 도중 방향 전환 안되게 할려고함)
    /// </summary>
    void AttackEnd()
    {
        isAttacking = false;
        if (attackTarget == null)
        {
            if (State != EnemyState.Dead)    // 죽은 상태가 아니면
            {
                State = EnemyState.Chase;   // 추적 상태로 변경
            }
        }
    }


    

    /// <summary>
    /// 애니메이션에 달릴 공격 이벤트 함수
    /// </summary>
    void AttackTargetInArea()
    {
        if (attackTarget != null)
        {
            Attack(attackTarget, 5);
        }
    }

    // 방어 기능 ---------------------------------------------------------------------------------------------------------------------------------------------------

    public override void Defence(float damage, Vector2 knockBackDir, ElemantalStates elemantal = null)
    {
        base.Defence(damage, knockBackDir, elemantal);
        if (IsAlive && !isAttacking)                    // 살아 있거나 공격 중이 아닐 때(공격 중에는 hit그로기에 빠지지 않는다)
        {
            if(State != EnemyState.Hitted) preState = State;
            State = EnemyState.Hitted;
        }
    }

    // 생존 기능 ----------------------------------------

    public override void Die()
    {
        base.Die();
        State = EnemyState.Dead;
    }


    // 감지 기능---------------------------------------------
    protected override bool SearchPlayer()
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
                // 근접 범위 밖이면(사이에 벽이 있을 때 구분해야됨)
                chaseTarget = target.transform;
                result = true;
            }
        }
        return result;
    }
}
