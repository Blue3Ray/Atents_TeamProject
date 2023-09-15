using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class EnemyBase : CharacterBase
{
    // 상태 머신
    // 상태 : 대기, 순찰, 추적, 공격, 사망
    public enum EnemyState
    {
        Wait = 0,
        Patrol,
        Chase,
        Attack,
        Dead
    }

    /// <summary>
    /// 패트롤 대기 시간(또는 대상 노쳤을 시 대기 시간)
    /// </summary>
    public float waitTime = 2.0f;

    /// <summary>
    /// 대기 시간 측정 용
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

    // 이동 기능 부분 ----------------------------------------------

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
    /// 움직이는 방향
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
    /// 원거리 추적 거리
    /// </summary>
    public float farSightRange = 5.0f;

    /// <summary>
    /// 근거리 추적 거리
    /// </summary>
    public float closeSightRange = 2.0f;

    // 공격 기능 부분 ------------------------------------------------------

    protected AttackArea attackArea;

    /// <summary>
    /// 공격 대상(1개)
    /// </summary>
    protected CharacterBase attackTarget;

    /// <summary>
    /// 추적 대상
    /// </summary>
    protected Transform chaseTarget;

    /// <summary>
    /// 공격 쿨타임
    /// </summary>
    public float attackMaxCoolTime = 5.0f;

    /// <summary>
    /// 공격 쿨다운 남은 시간(초기값은 0보다 작아야 이후 공격 쿨다운이 작동됨, 양수면 작동 안됨)
    /// </summary>
    protected float attackCurrentCoolTime = -1.0f;

    /// <summary>
    /// update 함수 돌릴 델리게이트
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

    // 업데이트 함수들 -----------------------------------------------------

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
    /// 플레이어를 찾는 함수
    /// </summary>
    /// <returns>참이면 찾음, 거짓이면 찾지 못함</returns>
    protected virtual bool SearchPlayer() { return false; } 

    // 공격 기능들-----------------------------------------------

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

    // 생존 관련 기능 들 ----------------------

    public override void Die()
    {
        base.Die();     // 죽었다라는 로그
    }
}
