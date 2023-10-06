using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossPos
{
    MidDown = 1,
    MidUp = 2,
    LeftDown,
    LeftUp,
    RightDown,
    RightUp
}

public class Boss : CharacterBase
{
    enum BossState
    {
        Sleep,
        Awake,      // Player�� ������� ��� ���� �� ����� ����
        Idle,
        Teleport,
        FireLaser,
        FireRocket,
        Die
    }

    [SerializeField]
    //�׽�Ʈ�� �ʵ�
    BossState state = BossState.Awake;
    BossState State
    {
        get => state;
        set
        {
            if (state == BossState.Die) return;
            if (state != value)
            {
                state = value;
                switch (state)
                {
                    case BossState.Sleep:
                        animator.SetTrigger(Hash_Sleep);
                        onStateUpdate = Update_Sleep;
                        break;
                    case BossState.Awake:
                        animator.SetTrigger(Hash_Awake);
                        onStateUpdate = Update_Awake;
                        break;
                    case BossState.Idle:
                        onStateUpdate = Update_Idle;
                        break;
                    case BossState.Teleport:
                        onStateUpdate = Update_Blink;
                        Blink();
                        break;
                    case BossState.FireLaser:
                        break;
                    case BossState.FireRocket:
                        break;
                    case BossState.Die:
                        animator.SetTrigger(Hash_Die);
                        onStateUpdate = Update_Dead;
                        gameObject.layer = LayerMask.NameToLayer("IgnorePlayer");
                        
                        // ���� ���̰ų� �´� ���� ���̴��� �ʱ�ȭ
                        StopAllCoroutines();
                        bossShader.SetFloat(Shader_Split, MaxSplit);
                        bossShader.SetFloat(Shader_Hit, 1);

                        break;
                }
            }
        }
    }

    // ���� ��� ---------------------------------------------

    public GameObject rocket;
    public GameObject laser;
    Transform laserFirePoint;
    Transform rocketFirePoint;

    public float attackCoolTime  = -1f;

    public float maxAttackCoolTime = 4.0f;

    readonly int Hash_Sleep = Animator.StringToHash("Sleep");
    readonly int Hash_Awake = Animator.StringToHash("Awake");
    readonly int Hash_GetHit = Animator.StringToHash("GetHit");
    readonly int Hash_FireLaser = Animator.StringToHash("FireLaser");
    readonly int Hash_FireRocket = Animator.StringToHash("FireRocket");
    readonly int Hash_Die = Animator.StringToHash("Die");

    // �̵� ���� ���(�⺻������ ����θ� �̵��Ѵ�)

    [Header("Shader Setting")]

    /// <summary>
    /// �����ϴ� �ð�
    /// </summary>
    public float phaseTime = 0.5f;

    const float MinSplit = -1.2f;
    const float MaxSplit = 3f;

    readonly int Shader_Split = Shader.PropertyToID("_Split");
    readonly int Shader_Hit = Shader.PropertyToID("_BaseAlpha");

    /// <summary>
    /// ������ ���� ��ġ �ε���
    /// </summary>
    int bossPosIndex = 1;

    /// <summary>
    /// ���� ���� ��� �����ð�
    /// </summary>
    float blinkCoolTime = -1.0f;

    /// <summary>
    /// ���� ���� ���ð�
    /// </summary>
    public float maxBlinkCoolTime = 5.0f;

    /// <summary>
    /// ���� ����
    /// </summary>
    protected Vector2 lookDir = Vector2.right;
    protected Vector2 LookDir
    {
        get => lookDir;
        set
        {
            if (lookDir != value)
            {
                lookDir = value;
                transform.localScale = new Vector3(lookDir.x, 1, 1);
                if (lookDir != Vector2.zero) knockBackDir = lookDir;
            }
        }
    }


    CharacterBase player;


    System.Action onStateUpdate;

    Material bossShader;
    Animator animator;
    TeleportPos teleport;

    GameObject portal;


    protected override void Awake()
    {
        base.Awake();
        bossShader = transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;
        animator = GetComponent<Animator>();

        laserFirePoint = transform.GetChild(1);
        rocketFirePoint = transform.GetChild(2);

        onDie += () => State = BossState.Die;
    }

    private void Start()
    {
        //EnterArea enter = FindObjectOfType<EnterArea>();
        //enter.onEnterPlayer += 

        teleport = FindObjectOfType<TeleportPos>();
        player = GameManager.Ins.Player;

        EnterArea enterArea = FindAnyObjectByType<EnterArea>();
        enterArea.onEnterPlayer += () => State = BossState.Awake;

        portal = FindAnyObjectByType<Portal>().gameObject;
        portal.SetActive(false);
    }


    public override void OnInitialize()
    {
        base.OnInitialize();
        State = BossState.Sleep; 
        // �׽�Ʈ�� ó����  Idle�� �����ϱ�
        //State = BossState.Idle;
    }

    private void Update()
    {
        onStateUpdate?.Invoke();
    }


    void Update_Sleep() 
    { 
        
    }

    void Update_Awake() 
    { 
    
    }

    /// <summary>
    /// Awake �ִϸ��̼��� ���� �� ȣ��� �Լ�
    /// </summary>
    void Fin_Awake()
    {
        State = BossState.Idle;
    }

    void Update_Idle()
    {
        if(player.transform.position.x > transform.position.x)
        {
            LookDir = new Vector2(1, 0);
        }
        else
        {
            LookDir = new Vector2(-1, 0);
        }

        

        if(attackCoolTime < 0)
        {
            if (blinkCoolTime < 0 && Random.value > 0.3f)
            {
                State = BossState.Teleport;
            }
            else
            {
                Attack(player);
            }
        }
    }

    void Update_Blink() { }
    void Update_Dead() { }

    // ���� ���� �Լ��� --------------------------------------


    /// <summary>
    /// �ִϸ��̼� Ÿ�ֿ̹� ���� ���� �������� �����ϴ� �Լ�
    /// </summary>
    void FireRocket()
    {
        GameObject temp = Instantiate(rocket);
        ProjectileBase tempObj = temp.GetComponent<ProjectileBase>();

        tempObj.OnInitialize(knockBackDir, ElementalType.None);
        temp.transform.position = rocketFirePoint.position;
    }

    /// <summary>
    /// ���� �Լ� ����� �ٷ� �������� �����ϴ� �Լ�
    /// </summary>
    void FireLaser()
    {
        GameObject temp = Instantiate(laser);
        ProjectileBase tempObj = temp.GetComponent<ProjectileBase>();

        tempObj.OnInitialize(knockBackDir, ElementalType.None);
        temp.transform.position = laserFirePoint.position;
    }


    public override void Attack(CharacterBase target)
    {
        //State = BossState.FireRocket;
        StartCoroutine(AttackCoolTime());

        if (Mathf.Pow((target.transform.position.y + 1) - transform.position.y, 2) > 5)
        {
            animator.SetTrigger(Hash_FireRocket);
        }
        else
        {
            animator.SetTrigger(Hash_FireLaser);
            FireLaser();
        }
    }

    IEnumerator AttackCoolTime()
    {
        attackCoolTime = maxAttackCoolTime;
        while (attackCoolTime > 0)
        {
            yield return null;
            attackCoolTime -= Time.deltaTime;
        }
        attackCoolTime = -1f;
    }

    // ��� ���� --------

    public override void Defence(float damage, Vector2 knockBackDir, ElemantalStates elemantal = null)
    {
        base.Defence(damage, knockBackDir, elemantal);
        // ���� �� �����Ÿ��� ����Ʈ �߰�
        StartCoroutine(HitEffect());
    }

    IEnumerator HitEffect()
    {
        bossShader.SetFloat(Shader_Hit, 0);
        yield return new WaitForSeconds(0.1f);
        bossShader.SetFloat(Shader_Hit, 1);
    }

    // �ڷ���Ʈ ���� �Լ��� -----------------------------------------

    BossPos GetNextTeleportPos()
    {
        bossPosIndex++;
        if (bossPosIndex > System.Enum.GetNames(typeof(BossPos)).Length) bossPosIndex = 1;
        return (BossPos) bossPosIndex;
    }


    public void Blink()
    {
        if (blinkCoolTime < 0)
        {
            StartCoroutine(BlinkCoolTime());
            StartCoroutine(BlinkCoroutine(GetNextTeleportPos()));
        }
    }

    IEnumerator BlinkCoolTime()
    {
        blinkCoolTime = maxBlinkCoolTime;
        while (blinkCoolTime > 0)
        {
            blinkCoolTime -= Time.deltaTime;
            yield return null;
        }
        blinkCoolTime = -1;
    }

    IEnumerator BlinkCoroutine(BossPos pos)
    {
        float time;
        float halfPhaseTime = phaseTime * 0.5f;
        float phaseNormalize = (MaxSplit - MinSplit) / halfPhaseTime;


        time = halfPhaseTime;
        while (time > 0)
        {
            yield return null;
            time -= Time.deltaTime;
            bossShader.SetFloat(Shader_Split, (time * phaseNormalize * 2f));
        }
        Teleport(pos);
        yield return null;

        time = 0;
        while (time < halfPhaseTime)
        {
            yield return null;
            time += Time.deltaTime;
            bossShader.SetFloat(Shader_Split, (time * phaseNormalize * 2f));
        }
        State = BossState.Idle;
        yield return null;
    }

    void Teleport(BossPos pos)
    {
        transform.position = teleport.GetCenterPos(pos);
    }

    // ������ ����� ������ �� ���� �Լ��� --------------------------------------------

    void AwakeBoss()
    {

    }

    IEnumerator BossAwake()
    {
        yield return null;
    }

    // ���� ��� ���� �Լ�

    public override void Die()
    {
        base.Die();
        if(portal != null)portal.SetActive(true);
    }
}
