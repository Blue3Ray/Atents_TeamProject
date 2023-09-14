using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BoneEnemy : MonoBehaviour
{
    // ���� ��� ---------------------------------------------------------------

    /// <summary>
    /// ���� ��ٿ�
    /// </summary>
    float attackMaxCoolTime = 3.0f;
    /// <summary>
    /// ���� ��ٿ� ���� �ð�(�ʱⰪ�� 0���� �۾ƾ� ���� ���� ��ٿ��� �۵���, ����� �۵� �ȵ�)
    /// </summary>
    [SerializeField]
    float attackCurrentCoolTime = -1f;

    /// <summary>
    /// ���� ��ٿ� �ڷ�ƾ�� ������ ����
    /// </summary>
    IEnumerator attackCoolDown;

    /// <summary>
    /// ���� ����
    /// </summary>
    AttackArea attackArea;

    /// <summary>
    /// ������ ���
    /// </summary>
    CharacterBase attackTarget;

    float attackRange;
    /// <summary>
    /// ���� �ִϸ��̼� �������� Ȯ���ϴ� ����(���¸ӽ��̶� ������ ������)
    /// </summary>
    bool isAttack = false;

    // �̵� ��� ---------------------------------------------------------------
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
    /// ���� �ɶ� ���� �¿�� -3, 3����
    /// </summary>
    public Vector2[] waypoints;

    /// <summary>
    /// ���� ��ǥ�ϴ� ��������Ʈ �ε���
    /// </summary>
    int currentWaypointIndex = 0;

    /// <summary>
    /// ��� �ð�
    /// </summary>
    public float waitTime = 2.0f;

    /// <summary>
    /// ��� �ð� ���� ��
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
    /// ������ ���
    /// </summary>
    Transform chaseTarget;

    /// <summary>
    /// ���Ÿ� ���� ����
    /// </summary>
    float farSightRange = 5.0f;

    /// <summary>
    /// �ٰŸ� ���� ����
    /// </summary>
    float closeSightRange = 1.5f;

    // ���� �ӽ� ---------------------------------------------------------------
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
                        WaitTimer = waitTime;           // ��� ���� ���� �ִ� ���ð� ����
                        onStateUpdate = Update_Wait;
                        break;
                    case EnemyState.Patrol:             // Wait, (Patrol)�� Speed�� ���� �ִϸ��̼� �ٲ�
                        CurrentMoveSpeed = maxMoveSpeed;
                        onStateUpdate = Update_Patrol;
                        break;
                    case EnemyState.Chase:              // CurrentMove�� ���� �ִϸ��̼� �ٲ�� ������ ���� ���ο� �̵� �ӵ��� ������
                        //CurrentMoveSpeed = maxMoveSpeed;
                        onStateUpdate = Update_Chase;
                        break;
                    case EnemyState.Attack:

                        CurrentMoveSpeed = 0;
                        
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

    // ��Ÿ ������Ʈ ---------------------------------------------------------------

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

        //���� ������ ������ ��
        attackArea.onPlayerIn += (target) =>
        {
            if (State == EnemyState.Chase)       // ���� ���� ���� ��
            {
                attackTarget = target;          // ���� ��� ����
                if(attackCurrentCoolTime < 0) State = EnemyState.Attack;      // ���� ���·� ����
            }
        };

        //���� �������� ������ ��
        attackArea.onPlayerOut += (target) =>
        {
            if (attackTarget == target)          // ���� ����� ������ 
            {
                attackTarget = null;            // ���� ��� �ʱ�ȭ

                if (State != EnemyState.Dead && !isAttack)    // ���� ���°� �ƴϰ�, ���� ���� �ƴ� ��
                {
                    State = EnemyState.Chase;   // ���� ���·� ����
                }
            }
        };

        attackRange = Mathf.Abs(transform.position.x - attackArea.transform.position.x);

        attackCoolDown = AttackCoolDown();

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
            Debug.Log("Player�� ã��");
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
            Debug.Log("Player�� ã��");
            State = EnemyState.Chase;
        }
        else
        {
            rb.MovePosition(transform.position + (Vector3) (CurrentMoveSpeed * Time.deltaTime * 0.5f * MoveDir));
            
            if (Mathf.Abs(waypoints[currentWaypointIndex].x - transform.position.x) < 0.1f)
            {
                currentWaypointIndex++;
                currentWaypointIndex %= waypoints.Length;
                MoveDir *= new Vector2(-1, 0);
                State = EnemyState.Wait;
            }
        }
    }

    float currentChaseTime = 0.3f;
    float ChaseMaxUpdateTime = 0.3f;

    void Update_Chase()
    {
        // �÷��̾�� x���� ��ĥ �� �¿� �ܻ�ȿ���� �����ϱ� ���� ������ ������Ʈ �ּ� �ð� �� �ο�
        if (currentChaseTime < 0)
        {
            if (SearchPlayer())
            {
                // Ÿ������ �ٰ�����
                Vector3 targetPos = chaseTarget.transform.position;

                if (targetPos.x > transform.position.x)
                {
                    MoveDir = new Vector2(1, 0);
                }
                else
                {
                    MoveDir = new Vector2(-1, 0);
                }

                // ���� �Ÿ��� ����ϱ�
                if (Mathf.Abs(targetPos.x - transform.position.x) < attackRange)
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
                // Ÿ�� ����� �Ҿ���� �� ��� ���·� ����
                State = EnemyState.Wait;
            }
            currentChaseTime = ChaseMaxUpdateTime;
        }

        rb.MovePosition(transform.position + (Vector3)(CurrentMoveSpeed * Time.deltaTime * moveDir));
        currentChaseTime -= Time.deltaTime;
    }

    void Update_Attack()
    {
        if (attackCurrentCoolTime < 0)
        {
            animator.SetTrigger(Hash_IsAttack);
            StopCoroutine(attackCoolDown);
            attackCurrentCoolTime = attackMaxCoolTime;
            StartCoroutine(attackCoolDown);
            //(attackTarget);
        }
    }

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
            if(attackTarget != null) State = EnemyState.Attack;
        }
    }

    /// <summary>
    /// ������ ������ �� ȣ�� �Ǵ� �Լ�
    /// </summary>
    void AttackStart()
    {
        isAttack = true;
    }

    /// <summary>
    /// ������ ���� �� ȣ��Ǵ� �Լ�(�����ϴ� ���� ���� ��ȯ �ȵǰ� �ҷ�����)
    /// </summary>
    void AttackEnd()
    {
        isAttack = false;
        if (attackTarget == null)
        {
            if (State != EnemyState.Dead)    // ���� ���°� �ƴϸ�
            {
                State = EnemyState.Chase;   // ���� ���·� ����
            }
        }
    }

    void Update_Dead()
    {

    }


    // ���� ��� ---------------------------------------

    /// <summary>
    /// ���� �ִϸ��̼ǿ� �޸� �̺�Ʈ �Լ�
    /// </summary>
    void AttackTargetInArea()
    {
        if (attackTarget != null)
        {
            Attack(attackTarget);
        }
    }

    void Attack(CharacterBase target)
    {
        target.Defence(10, 10 * MoveDir.x);
    }

    // ���� ���---------------------------------------------
    bool SearchPlayer()
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
