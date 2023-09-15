using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyBase : CharacterBase
{
    // ���� �ӽ�
    // ���� : ���, ����, ����, ����, ���
    public enum EnemyState
    {
        Wait = 0,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    /// <summary>
    /// ��Ʈ�� ��� �ð�(�Ǵ� ��� ������ �� ��� �ð�)
    /// </summary>
    public float waitTime = 2.0f;

    /// <summary>
    /// ��� �ð� ���� ��
    /// </summary>
    protected float waitTimer = 2.0f;
    protected virtual float WaitTimer
    {
        get => waitTimer;
        set
        {
            waitTimer = value;
            if (waitTimer < 0)
            {
                //State = EnemyState.Patrol;
            }
        }
    }

    // �̵� ��� �κ� ----------------------------------------------

    public float maxMoveSpeed;

    protected float currentMoveSpeed;
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

    /// <summary>
    /// �����̴� ����
    /// </summary>
    protected Vector2 moveDir = Vector2.right;
    protected Vector2 MoveDir
    {
        get => moveDir;
        set
        {
            if (moveDir != value)
            {
                moveDir = value;
                transform.localScale = new Vector3(moveDir.x, 1, 1);
            }
        }
    }

    /// <summary>
    /// ���Ÿ� ���� �Ÿ�
    /// </summary>
    public float farSightRange = 5.0f;

    /// <summary>
    /// �ٰŸ� ���� �Ÿ�
    /// </summary>
    public float closeSightRange = 2.0f;

    // ���� ��� �κ� ------------------------------------------------------

    protected AttackArea attackArea;

    /// <summary>
    /// ���� ���(1��)
    /// </summary>
    protected CharacterBase attackTarget;

    /// <summary>
    /// ���� ���
    /// </summary>
    protected Transform chaseTarget;

    /// <summary>
    /// ���� ��Ÿ��
    /// </summary>
    public float attackMaxCoolTime = 5.0f;

    /// <summary>
    /// ���� ��ٿ� ���� �ð�(�ʱⰪ�� 0���� �۾ƾ� ���� ���� ��ٿ��� �۵���, ����� �۵� �ȵ�)
    /// </summary>
    protected float attackCurrentCoolTime = -1.0f;

    /// <summary>
    /// update �Լ� ���� ��������Ʈ
    /// </summary>
    protected System.Action onStateUpdate;

    protected Animator animator;

    protected readonly int Hash_GetHit = Animator.StringToHash("GetHit");
    protected readonly int Hash_IsDead = Animator.StringToHash("IsDead");
    protected readonly int Hash_Speed = Animator.StringToHash("Speed");
    protected readonly int Hash_IsAttack = Animator.StringToHash("IsAttack");

    protected Rigidbody2D rb;


    protected override void Awake()
    {
        base.Awake();
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();

        AttackArea[] areas = GetComponentsInChildren<AttackArea>();
        attackArea = areas[0];
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    // ������Ʈ �Լ��� -----------------------------------------------------

    protected virtual void Update()
    {
        onStateUpdate();
    }

    protected virtual void Update_Wait() { }

    protected virtual void Update_Patrol() { }

    protected virtual void Update_Chase() { }

    protected virtual void Update_Attack() { }

    protected virtual void Update_Dead() { }

    //--------------------------------------------------------

    /// <summary>
    /// �÷��̾ ã�� �Լ�
    /// </summary>
    /// <returns>���̸� ã��, �����̸� ã�� ����</returns>
    protected virtual bool SearchPlayer() { return false; } 

    // ���� ��ɵ�-----------------------------------------------

    public override void Attack(CharacterBase target, float knockBackPower)
    {
        base.Attack(target, knockBackPower);
        attackCurrentCoolTime = attackMaxCoolTime;
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
    }
}
