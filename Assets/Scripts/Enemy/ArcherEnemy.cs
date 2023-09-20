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
    /// ���� ��Ÿ�
    /// </summary>
    float attackRange = 10;

    /// <summary>
    /// ���� �ִϸ��̼� �������� Ȯ���ϴ� ����(���¸ӽ��̶� ������ ������, ���� ���� �����̴� ���� ����, ���� ���� �¾Ƶ� �������ӽ�Ű�� ������)
    /// </summary>
    bool isAttacking = false;


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


    // ���� �ӽ� ---------------------------------------------------------------

    /// <summary>
    /// ���� ���¸� �����ϴ� ����(Hit������ �� �����)
    /// </summary>
    EnemyState preState = EnemyState.Wait;

    EnemyState state = EnemyState.Patrol;
    EnemyState State
    {
        get => state;
        set
        {
            if (state != value)
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
                        // GetComponent<CapsuleCollider2D>().enabled = false;

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
        // animator = GetComponent<Animator>();
        //rb = GetComponent<Rigidbody2D>();

        //AttackArea[] areas = GetComponentsInChildren<AttackArea>();
        //attackArea = areas[0];

        //���� ������ ������ ��
        attackArea.onPlayerIn += (target) =>
        {
            if (State == EnemyState.Chase)       // ���� ���� ���� ��
            {
                attackTarget = target;          // ���� ��� ����
                if (attackCurrentCoolTime < 0) State = EnemyState.Attack;      // ���� ���·� ����
            }
        };

        //���� �������� ������ ��
        attackArea.onPlayerOut += (target) =>
        {
            if (attackTarget == target)          // ���� ����� ������ 
            {
                attackTarget = null;            // ���� ��� �ʱ�ȭ

                if (State != EnemyState.Dead && !isAttacking)    // ���� ���°� �ƴϰ�, ���� ���� �ƴ� ��
                {
                    State = EnemyState.Chase;   // ���� ���·� ����
                }
            }
        };

        // ���� ���� ������ ���� �˾�è(?) ������ ����
        // closeSightRange = Mathf.Abs(transform.position.x - attackArea.transform.position.x);

        attackCoolDown = AttackCoolDown();

        OnInitialize();
    }

    public override void OnInitialize()
    {
        base.OnInitialize();

        rb.bodyType = RigidbodyType2D.Dynamic;
        GetComponent<CapsuleCollider2D>().enabled = true;

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
        // �÷��̾�� x���� ��ĥ �� �¿� �ܻ�ȿ���� �����ϱ� ���� ������ ������Ʈ �ּ� �ð� �� �ο�
        if (currentChaseTime < 0)
        {
            if (SearchPlayer())
            {
                Vector3 targetPos = chaseTarget.transform.position;
                CurrentMoveSpeed = maxMoveSpeed;

                if ((attackRange * attackRange) + 2 < Mathf.Pow(targetPos.x - transform.position.x, 2))
                {
                    Debug.Log("��Ÿ��� ��");
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
                else if ((attackRange * attackRange) + 2 > Mathf.Pow(targetPos.x - transform.position.x, 2))
                {
                    Debug.Log("��Ÿ��� �����");
                    // ��Ÿ����� �����̸�
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
                    Debug.Log("��Ÿ��� ������");
                    // ���� ��Ÿ��� ����ϱ�
                    CurrentMoveSpeed = 0;
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
        isAttacking = true;
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
    void AttackTargetInArea()
    {
        if (attackTarget != null)
        {
            Attack(attackTarget, 5);
        }
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


    float currentDelayTime = 0;
    IEnumerator HitDelay(float delayTime)
    {
        if (currentDelayTime <= 0) currentDelayTime = delayTime;
        while (currentDelayTime > 0)
        {
            currentDelayTime -= Time.deltaTime;
            yield return null;
        }
        if (State != EnemyState.Dead)
        {
            State = preState;
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