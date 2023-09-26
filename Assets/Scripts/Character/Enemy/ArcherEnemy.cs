using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : EnemyBase
{
    // 공격 기능 ---------------------------------------------------------------

    /// <summary>
    /// 공격 쿨다운 코루틴을 저장할 변수
    /// </summary>
    IEnumerator attackCoolDown;

    /// <summary>
    /// 공격 사거리(farSight보다는 작아야 한다)
    /// </summary>
    float attackRange = 10;

    /// <summary>
    /// 공격 애니메이션 도중인지 확인하는 변수(상태머신이랑 별개로 움직임, 공격 도중 움직이는 것을 방지, 공격 도중 맞아도 공격지속시키는 역할함)
    /// </summary>
    bool isAttacking = false;

    /// <summary>
    /// 화살 발사 위치
    /// </summary>
    Transform firePoint;

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
                if (State == EnemyState.Wait) State = EnemyState.Patrol;
            }
        }
    }



    // 상태 머신 ---------------------------------------------------------------

    protected override EnemyState State
    {
        get => state;
        set
        {
            if (state != value || value == EnemyState.Hitted)
            {
                //Debug.Log($"이전 : {state}, 이후 : {value}");
                state = value;

                switch (state)
                {
                    case EnemyState.Wait:
                        CurrentMoveSpeed = 0;
                        WaitTimer = waitTime;           // 대기 상태 들어가면 최대 대기시간 설정
                        onStateUpdate = Update_Wait;
                        break;
                    case EnemyState.Patrol:             // Wait, (Patrol)는 Speed에 따라서 애니메이션 바뀜
                        CurrentMoveSpeed = maxMoveSpeed;
                        onStateUpdate = Update_Patrol;
                        break;
                    case EnemyState.Chase:              // CurrentMove에 따라서 애니메이션 바뀌기 때문에 추적 내부에 이동 속도를 조절함

                        onStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:
                        isAttacking = true;
                        if (chaseTarget.position.x > transform.position.x)
                        {
                            MoveDir = new Vector2(1, 0);
                        }
                        else
                        {
                            MoveDir = new Vector2(-1, 0);
                        }

                        CurrentMoveSpeed = 0;

                        onStateUpdate = Update_Attack;
                        break;
                    case EnemyState.Hitted:
                        CurrentMoveSpeed = 0;
                        animator.SetTrigger(Hash_GetHit);
                        StartCoroutine(HitDelay(1.0f));
                        onStateUpdate = Update_Hitted;
                        break;
                    case EnemyState.Dead:
                        CurrentMoveSpeed = 0;
                        animator.SetTrigger(Hash_IsDead);
                        onStateUpdate = Update_Dead;

                        rb.bodyType = RigidbodyType2D.Static;
                        GetComponent<Collider2D>().isTrigger = true;

                        StartCoroutine(LifeOver(3.0f));
                        break;
                }
            }
        }
    }

    /// <summary>
    /// 상태 머신에서 부자연스럽게 빠른 동작 전환하는 것을 방지하기위해 사용하는 기준시간 변수
    /// </summary>
    float currentChaseTime = 0.3f;
    float ChaseMaxUpdateTime = 0.3f;


    protected override void Awake()
    {
        base.Awake();
        
        AttackArea detectedArea = GetComponentInChildren<AttackArea>();
        //detectedArea.onPlayerIn += (target) =>
        //{
        //    // Player가 인지 범위 안에 들어올 때
        //    attackTarget = target;
        //    State = EnemyState.Chase;
        //};
        //detectedArea.onPlayerOut += (target) =>
        //{
        //    // Player가 인지 범위 밖에 나갈 때
        //    if (target == attackTarget)
        //    {
        //        attackTarget = null;
        //        if (State != EnemyState.Dead && !isAttacking)
        //        {
        //            State = EnemyState.Wait;
        //        }
        //    }
        //};

        firePoint = transform.GetChild(2);

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
        if (SearchPlayer())
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
                Vector3 targetPos = chaseTarget.transform.position;
                CurrentMoveSpeed = maxMoveSpeed;

                if ((attackRange * attackRange) + 2 < Mathf.Pow(targetPos.x - transform.position.x, 2))
                {
                    // 사거리 보다 바깥이면
                    // 타켓한테 다가가는 방향 설정
                    if (targetPos.x > transform.position.x)
                    {
                        MoveDir = new Vector2(1, 0);
                    }
                    else
                    {
                        MoveDir = new Vector2(-1, 0);
                    }
                }
                else
                {
                    if (attackCurrentCoolTime < 0 && Mathf.Abs(targetPos.y - transform.position.y) < 2.0f)
                    {

                        State = EnemyState.Attack;      // 공격 상태로 변경
                    }
                    else
                    {

                        if ((attackRange * attackRange) * 0.3F > Mathf.Pow(targetPos.x - transform.position.x, 2))
                        {
                            // 사거리보다 너무 안쪽이면
                            // 타켓한테 멀어지는 방향 설정
                            if (targetPos.x > transform.position.x)
                            {
                                MoveDir = new Vector2(-1, 0);
                            }
                            else
                            {
                                MoveDir = new Vector2(1, 0);
                            }
                        }
                        else
                        {
                            // 공격 사거리면 대기하기
                            if (targetPos.x > transform.position.x)
                            {
                                MoveDir = new Vector2(1, 0);
                            }
                            else
                            {
                                MoveDir = new Vector2(-1, 0);
                            }
                            CurrentMoveSpeed = 0;
                        }
                    }
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
            if (attackTarget != null) State = EnemyState.Attack;
        }
    }

    /// <summary>
    /// 애니메이션에서 공격이 시작할 때 호출 되는 함수
    /// </summary>
    void AttackStart()
    {
        
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
    void FireArrow()
    {
        Attack(attackTarget);
    }

    public override void Attack(CharacterBase _)
    {
        Vector2 targetPos = chaseTarget.transform.position;

        GameObject arrowObj = Factory.Ins.GetObject(PoolObjectType.Projectile_Arrow, firePoint.position);
        ProjectileBase objProjectile = arrowObj.GetComponent<ProjectileBase>();
        objProjectile.OnInitialize(knockBackDir, elemantalStatus.CurrentElemantal);
    }

    // 방어 기능 ---------------------------------------------------------------------------------------------------------------------------------------------------

    public override void Defence(float damage, Vector2 knockBackDir, ElemantalStates elemantal = null)
    {
        base.Defence(damage, knockBackDir, elemantal);
        if (IsAlive && !isAttacking)                    // 살아 있거나 공격 중이 아닐 때(공격 중에는 hit그로기에 빠지지 않는다)
        {
            if (State != EnemyState.Hitted) preState = State;
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
