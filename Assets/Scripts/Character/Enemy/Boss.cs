using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : CharacterBase
{
    enum BossState
    {
        Sleep,
        Awake,
        Idle,
        Teleport,
        AttackLaser,
        AttackRocket,
        Die
    }

    BossState state;
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
                        
                        break;
                    case BossState.Awake:
                        break;
                    case BossState.Idle:
                        break;
                    case BossState.Teleport:
                        break;
                    case BossState.AttackLaser:
                        break;
                    case BossState.AttackRocket:
                        break;
                    case BossState.Die:
                        break;
                }
            }
        }
    }

    System.Action onStateUpdate;

    Material bossShader;

    protected override void Awake()
    {
        base.Awake();
        bossShader = transform.GetChild(0).GetComponent<Renderer>().sharedMaterial;
    }


    private void Update()
    {
        onStateUpdate?.Invoke();
    }
}
