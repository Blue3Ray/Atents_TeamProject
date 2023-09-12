using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyBase : CharacterBase
{
    // ���� �ӽ�
    // ���� : ���, ����, ����, ����, ���
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
                        CurrentMoveSpeed = 0;
                        animator.SetTrigger(Hash_IsDead);
                        onStateUpdate = Update_Dead;
                        StartCoroutine(LifeOver(3.0f));
                        break;
                }
            }
        }
    }

    // �̵� ��� �κ�

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
                if(IsAlive)animator.SetFloat(Hash_Speed, currentMoveSpeed);
            }
        }
    }

    public float farSightRange = 5.0f;
    public float closeSightRange = 2.0f;

    // ���� ��� �κ� ------------------------------------------------------

    AttackArea attackArea;

    AttackArea detectedArea;

    /// <summary>
    /// ���� ���(1��)
    /// </summary>
    CharacterBase attackTarget;

    Transform chaseTarget;

    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    public float attackCoolTime = 5.0f;

    /// <summary>
    /// ���� ��Ÿ�� ���� ���� �ð�(0�̵Ǹ� ���� ����)
    /// </summary>
    float attackCurrentCoolTime = 5.0f;

    /// <summary>
    /// update �Լ� ���� ��������Ʈ
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

        detectedArea = areas[1];
        detectedArea.onPlayerIn += (target) =>
        {
            if (State != EnemyState.Chase)       // ���ݻ��°� �ƴ� ��
            {
                chaseTarget = target;          // ���� ��� ����
                State = EnemyState.Chase;      // ���� ���·� ����
            }
        };

        detectedArea.onPlayerOut += (target) =>
        {
            if (chaseTarget == target)          // ���� ����� ������ 
            {
                chaseTarget = null;            // ���� ��� �ʱ�ȭ
                if (State != EnemyState.Dead)    // ���� ���°� �ƴϸ�
                {
                    State = EnemyState.Wait;   // ��� ���·� ����
                }
            }
        };
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
        State = EnemyState.Wait;
    }

    // ������Ʈ �Լ��� -----------------------------------------------------

    private void Update()
    {
        onStateUpdate();
    }

    void Update_Patrol()
    {
        SearchPlayer();
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
            attackCurrentCoolTime = attackCoolTime;
        }
    }

    void Update_Dead()
    {

    }

    //--------------------------------------------------------

    /// <summary>
    /// �÷��̾ ã�� �Լ�
    /// </summary>
    /// <returns>���̸� ã��, �����̸� ã�� ����</returns>
    bool SearchPlayer()
    {
        bool result = false;
        chaseTarget = null;

        Collider2D target = Physics2D.OverlapCircle(transform.position, farSightRange, LayerMask.GetMask("Player"));
        //Physics2D.Raycast()

        if (target != null)
        {
            Vector3 player = target.transform.position;
            // ���� �����ȿ� ���� ��
            if (Vector2.SqrMagnitude(player - transform.position) < closeSightRange * closeSightRange)
            {
                chaseTarget = target.transform;
                result = true;
            }
            else
            {
                // �þ߿� ���԰�
                if (IsInSightAngle(player - transform.position))
                {
                    // �þ߿� �����°� ������ ��
                    if (IsSightClear(player - transform.position))
                    {
                        Debug.Log("�þ߿� �����°� ���� �÷��̾ ����");
                        chaseTarget = target.transform;
                        result = true;
                    }
                    else
                    {
                        Debug.Log("�þ߿� �����°� ����");
                    }
                }
            }
        }
        return result;
    }

    bool IsInSightAngle(Vector3 targetPos)
    {
        return true;
    }

    bool IsSightClear(Vector3 toTargetDir)
    {
        bool result = false;
        RaycastHit2D hit = Physics2D.Raycast(transform.position, toTargetDir, farSightRange);
        if (hit)
        {
            if (hit.collider.CompareTag("Player"))
            {
                result = true;
            }
        }
        return result;
    }

    // ���� ��ɵ�-----------------------------------------------

    public override void Attack(CharacterBase target, float knockBackPower)
    {
        base.Attack(target, knockBackPower);
        attackCurrentCoolTime = attackCoolTime;
    }

    public override void Defence(float damage, ElemantalStatus elemantal = null)
    {
        base.Defence(damage, elemantal);
        if(IsAlive) animator.SetTrigger(Hash_GetHit);
    }

    public override void Defence(float damage, float knockBackPower, ElemantalStatus elemantal = null)
    {
        base.Defence(damage, knockBackPower, elemantal);
        if (IsAlive) animator.SetTrigger(Hash_GetHit);
    }

    // ���� ���� ��� �� ----------------------

    public override void Die()
    {
        base.Die();     // �׾��ٶ�� �α�
        State = EnemyState.Dead;
    }
}
