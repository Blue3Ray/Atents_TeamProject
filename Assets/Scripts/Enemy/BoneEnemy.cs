using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneEnemy : MonoBehaviour
{
    // ���� ��� ---------------------------------------------------------------

    float attackMaxCoolTime = 3.0f;
    float attackCurrentCoolTime = 3.0f;

    /// <summary>
    /// ���� ����
    /// </summary>
    AttackArea attackArea;

    /// <summary>
    /// ������ ���
    /// </summary>
    CharacterBase attackTarget;

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
                    case EnemyState.Patrol:             // Wait, Patrol, Chase�� Speed�� ���� �ִϸ��̼� �ٲ�
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
        attackArea.onPlayerIn += (target) =>
        {
            if (State == EnemyState.Chase)       // ���� ���� ���� ��
            {
                attackTarget = target;          // ���� ��� ����
                State = EnemyState.Attack;      // ���� ���·� ����
            }
        };

        attackArea.onPlayerOut += (target) =>
        {
            if (attackTarget == target)          // ���� ����� ������ 
            {
                attackTarget = null;            // ���� ��� �ʱ�ȭ
                if (State != EnemyState.Dead)    // ���� ���°� �ƴϸ�
                {
                    State = EnemyState.Chase;   // ���� ���·� ����
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
            // Ÿ������ �ٰ�����
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
            // Ÿ�� ����� �Ҿ���� �� ��� ���·� ����
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
                // ���� ���� ���̸�
                chaseTarget = target.transform;
                result = true;
            }
        }
        return result;
    }
}
