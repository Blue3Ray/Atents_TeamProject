using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcherEnemy : EnemyBase
{
    // ���� ��� ---------------------------------------------------------------

    /// <summary>
    /// ���� ��ٿ� �ڷ�ƾ�� ������ ����
    /// </summary>
    IEnumerator attackCoolDown;

    /// <summary>
    /// ���� ��Ÿ�(farSight���ٴ� �۾ƾ� �Ѵ�)
    /// </summary>
    float attackRange = 10;

    /// <summary>
    /// ���� �ִϸ��̼� �������� Ȯ���ϴ� ����(���¸ӽ��̶� ������ ������, ���� ���� �����̴� ���� ����, ���� ���� �¾Ƶ� �������ӽ�Ű�� ������)
    /// </summary>
    bool isAttacking = false;

    /// <summary>
    /// ȭ�� �߻� ��ġ
    /// </summary>
    Transform firePoint;

    // �̵� ��� ---------------------------------------------------------------

    /// <summary>
    /// ���� �ɶ� ���� �¿�� -3, 3����
    /// </summary>
    [SerializeField]
    Vector2[] waypoints;

    /// <summary>
    /// ���� ��ǥ�ϴ� ��������Ʈ �ε���
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



    // ���� �ӽ� ---------------------------------------------------------------

    protected override EnemyState State
    {
        get => state;
        set
        {
            if (state != value || value == EnemyState.Hitted)
            {
                //Debug.Log($"���� : {state}, ���� : {value}");
                state = value;

                switch (state)
                {
                    case EnemyState.Wait:
                        CurrentMoveSpeed = 0;
                        WaitTimer = waitTime;           // ��� ���� ���� �ִ� ���ð� ����
                        onStateUpdate = Update_Wait;
                        break;
                    case EnemyState.Patrol:             // Wait, (Patrol)�� Speed�� ���� �ִϸ��̼� �ٲ�
                        CurrentMoveSpeed = maxMoveSpeed;
                        onStateUpdate = Update_Patrol;
                        break;
                    case EnemyState.Chase:              // CurrentMove�� ���� �ִϸ��̼� �ٲ�� ������ ���� ���ο� �̵� �ӵ��� ������

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
    /// ���� �ӽſ��� ���ڿ������� ���� ���� ��ȯ�ϴ� ���� �����ϱ����� ����ϴ� ���ؽð� ����
    /// </summary>
    float currentChaseTime = 0.3f;
    float ChaseMaxUpdateTime = 0.3f;


    protected override void Awake()
    {
        base.Awake();
        
        AttackArea detectedArea = GetComponentInChildren<AttackArea>();
        //detectedArea.onPlayerIn += (target) =>
        //{
        //    // Player�� ���� ���� �ȿ� ���� ��
        //    attackTarget = target;
        //    State = EnemyState.Chase;
        //};
        //detectedArea.onPlayerOut += (target) =>
        //{
        //    // Player�� ���� ���� �ۿ� ���� ��
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
            Debug.Log("Player�� ã��");
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
            Debug.Log("Player�� ã��");
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
        // �÷��̾�� x���� ��ĥ �� �¿� �ܻ�ȿ���� �����ϱ� ���� ������ ������Ʈ �ּ� �ð� �� �ο�
        if (currentChaseTime < 0)
        {
            if (SearchPlayer())
            {
                Vector3 targetPos = chaseTarget.transform.position;
                CurrentMoveSpeed = maxMoveSpeed;

                if ((attackRange * attackRange) + 2 < Mathf.Pow(targetPos.x - transform.position.x, 2))
                {
                    // ��Ÿ� ���� �ٱ��̸�
                    // Ÿ������ �ٰ����� ���� ����
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

                        State = EnemyState.Attack;      // ���� ���·� ����
                    }
                    else
                    {

                        if ((attackRange * attackRange) * 0.3F > Mathf.Pow(targetPos.x - transform.position.x, 2))
                        {
                            // ��Ÿ����� �ʹ� �����̸�
                            // Ÿ������ �־����� ���� ����
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
                            // ���� ��Ÿ��� ����ϱ�
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
                // Ÿ�� ����� �Ҿ���� �� ��� ���·� ����
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


    // ���� ��� ---------------------------------------

    /// <summary>
    /// ���� ��ٿ� ���ҽ�Ű�� �ڷ�ƾ
    /// </summary>
    IEnumerator AttackCoolDown()
    {
        while (true)        // �ڷ�ƾ�� ������ �����ϰ� �Ǹ� Stop�ϰ� �ٽ� ���� �� �� ó�����Ͱ� �ƴ� �ߴ��� ������ �����ϰԵǱ� ������ ���� ���� �ٿ���
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
    /// �ִϸ��̼ǿ��� ������ ������ �� ȣ�� �Ǵ� �Լ�
    /// </summary>
    void AttackStart()
    {
        
    }

    /// <summary>
    /// �ִϸ��̼ǿ��� ������ ���� �� ȣ��Ǵ� �Լ�(�����ϴ� ���� ���� ��ȯ �ȵǰ� �ҷ�����)
    /// </summary>
    void AttackEnd()
    {
        isAttacking = false;
        if (attackTarget == null)
        {
            if (State != EnemyState.Dead)    // ���� ���°� �ƴϸ�
            {
                State = EnemyState.Chase;   // ���� ���·� ����
            }
        }
    }

    /// <summary>
    /// �ִϸ��̼ǿ� �޸� ���� �̺�Ʈ �Լ�
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

    // ��� ��� ---------------------------------------------------------------------------------------------------------------------------------------------------

    public override void Defence(float damage, Vector2 knockBackDir, ElemantalStates elemantal = null)
    {
        base.Defence(damage, knockBackDir, elemantal);
        if (IsAlive && !isAttacking)                    // ��� �ְų� ���� ���� �ƴ� ��(���� �߿��� hit�׷α⿡ ������ �ʴ´�)
        {
            if (State != EnemyState.Hitted) preState = State;
            State = EnemyState.Hitted;
        }
    }


    // ���� ��� ----------------------------------------

    public override void Die()
    {
        base.Die();
        State = EnemyState.Dead;
    }


    // ���� ���---------------------------------------------
    protected override bool SearchPlayer()
    {
        bool result = false;
        chaseTarget = null;

        Collider2D target = Physics2D.OverlapCircle(transform.position, farSightRange, LayerMask.GetMask("Player"));
        //Physics2D.Raycast()

        //���Ÿ� ���� ������ ���� target�� ������(���� state�ٲ����� ����)
        if (target != null)
        {
            Vector3 player = target.transform.position;

            if (Vector2.SqrMagnitude(player - transform.position) < closeSightRange * closeSightRange)
            {
                // ���� �����̸�
                chaseTarget = target.transform;
                result = true;
            }
            else
            {
                // ���� ���� ���̸�(���̿� ���� ���� �� �����ؾߵ�)
                chaseTarget = target.transform;
                result = true;
            }
        }
        return result;
    }
}
