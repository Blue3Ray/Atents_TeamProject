using System;
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
        Awake,      // Player가 방안으로 들어 왔을 때 실행될 예정
        Idle,
        Teleport,
        FireLaser,
        FireRocket,
        Die
    }


    BossState state = BossState.Idle;
    BossState State
    {
        get => state;
        set
        {
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
                        break;
                    case BossState.FireLaser:
                        break;
                    case BossState.FireRocket:
                        break;
                    case BossState.Die:
                        break;
                }
            }
        }
    }

    readonly int Hash_Sleep = Animator.StringToHash("Sleep");
    readonly int Hash_Awake = Animator.StringToHash("Awake");
    readonly int Hash_GetHit = Animator.StringToHash("GetHit");
    readonly int Hash_FireLaser = Animator.StringToHash("FireLaser");
    readonly int Hash_FireRocket = Animator.StringToHash("FireRocket");
    readonly int Hash_IsDead = Animator.StringToHash("IsDead");


    [Header("Shader Setting")]
    public float phaseTime = 0.5f;

    const float MinSplit = -1.2f;
    const float MaxSplit = 3f;

    readonly int Sharder_Split = Shader.PropertyToID("_Split");

    System.Action onStateUpdate;

    Material bossShader;
    Animator animator;
    TeleportPos teleport;


    protected override void Awake()
    {
        base.Awake();
        bossShader = transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        //EnterArea enter = FindObjectOfType<EnterArea>();
        //enter.onEnterPlayer += 

        teleport = FindObjectOfType<TeleportPos>();
    }


    public override void OnInitialize()
    {
        base.OnInitialize();
        // State = BossState.Sleep; 
        // 테스트로 처음에  Idle로 시작하기
        State = BossState.Idle;
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
    /// Awake 애니메이션이 끝날 때 호출될 함수
    /// </summary>
    void Fin_Awake()
    {
        State = BossState.Idle;
    }

    void Update_Idle()
    {

    }

    void Update_Hitted() { }

    void Update_Dead() { }

    // 텔레포트 관련 함수들 -----------------------------------------

    int bossPos = 1;

    BossPos GetTeleportPos()
    {
        bossPos++;
        if (bossPos > Enum.GetNames(typeof(BossPos)).Length) bossPos = 1;
        return (BossPos) bossPos;
    }


    public void Blink()
    {
        StartCoroutine(BlinkCoroutine(GetTeleportPos()));
    }

    IEnumerator BlinkCoroutine(BossPos pos)
    {
        float time = 0.0f;
        float halfPhaseTime = phaseTime * 0.5f;
        float phaseNormalize = (MaxSplit - MinSplit) / halfPhaseTime;

        time = halfPhaseTime;
        while (time > 0)
        {
            time -= Time.deltaTime;
            bossShader.SetFloat(Sharder_Split, (time * phaseNormalize * 2f));
            yield return null;
        }
        Teleport(pos);
        yield return null;

        time = 0;
        while (time < halfPhaseTime)
        {
            time += Time.deltaTime;
            bossShader.SetFloat(Sharder_Split, (time * phaseNormalize * 2f));
            yield return null;
        }
        yield return null;
    }

    void Teleport(BossPos pos)
    {
        transform.position = teleport.GetCenterPos(pos);
    }

    // 보스가 깨어날때 보여질 씬 진행 함수들 --------------------------------------------

    void AwakeBoss()
    {

    }

    IEnumerator BossAwake()
    {
        yield return null;
    }
}
