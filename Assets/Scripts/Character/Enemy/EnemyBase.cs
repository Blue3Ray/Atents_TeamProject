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
        Hitted,
        Dead
    }

    /// <summary>
    /// ���� ���¸� �����ϴ� ����(Hit������ �� �����)
    /// </summary>
    protected EnemyState preState = EnemyState.Wait;

    /// <summary>
    /// ���� ����
    /// </summary>
    [SerializeField]
    protected EnemyState state = EnemyState.Patrol;
    
    /// <summary>
    /// ���� ���¸� �����ϴ� ������Ƽ
    /// </summary>
    protected virtual EnemyState State
    {
        get => state;
        set
        {
            if (state != value)
            {
                if (state == EnemyState.Dead) return;
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
                        GetComponent<Collider2D>().isTrigger = true;

                        StartCoroutine(LifeOver(3.0f));
                        break;
                }
            }
        }
    }

    /// <summary>
    /// �¾��� �� ������ �ɸ��� ���� �ð�
    /// </summary>
    protected float currentDelayTime = 0;
    protected virtual IEnumerator HitDelay(float delayTime)
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

    // ------------------------------------------------------------------------------------------

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
                //
                // State = EnemyState.Patrol;
            }
        }
    }

    /// <summary>
    /// ����� �÷��̾�� �ִ� ����ġ
    /// </summary>
    public int experiencePoint = 10;

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
                if(moveDir != Vector2.zero) knockBackDir = moveDir;
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
    }

    public override void OnInitialize()
    {
        gameObject.layer = LayerMask.NameToLayer("Enemy");
        base.OnInitialize();
    }

    // ������Ʈ �Լ��� -----------------------------------------------------

    protected virtual void FixedUpdate()
    {
        onStateUpdate();
    }

    protected virtual void Update_Wait() { }

    protected virtual void Update_Patrol() { }

    protected virtual void Update_Chase() { }

    protected virtual void Update_Attack() { }

    protected virtual void Update_Hitted() { }

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

    public override void Defence(float damage, ElemantalStates elemantal = null)
    {
        base.Defence(damage, elemantal);
        //if(IsAlive) animator.SetTrigger(Hash_GetHit);
    }

    public override void Defence(float damage, Vector2 knockBackDir, ElemantalStates elemantal = null)
    {
        base.Defence(damage, knockBackDir, elemantal);
        //if (IsAlive) animator.SetTrigger(Hash_GetHit);
    }

    // ���� ���� ��� �� ----------------------

    public override void Die()
    {
        base.Die();     // �׾��ٶ�� �α�
        gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
        GameManager.Ins.Player.Experience += experiencePoint;
        GameObject dropItem = Instantiate(GameManager.Ins.ItemData[ItemCode.Potion].modelPrefab);
        
        dropItem.transform.position = this.transform.position;

    }
}
