using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public class TestPlayer : MonoBehaviour, InHealth
{
    float hp = 100.0f;
    public float HP 
    { 
        get => hp; 
        set
        {
            if(IsAlive)
            {
                hp = value;
                if(hp <=  0)
                {
                    Die();
                }
                hp = Mathf.Clamp(hp, 0, MaxHP);
                onHealthChange?.Invoke(hp/MaxHP);
            }
        }
    }

    float maxHP = 100.0f;
    public float MaxHP => maxHP;

    float mp = 80.0f;
    public float MP 
    {
        get => mp;
        set
        {
            mp = value;
            mp = Mathf.Clamp(mp, 0, MaxMP);
            onHealthChange?.Invoke(mp / MaxMP);
        }
    }

    float maxMP = 80.0F;
    public float MaxMP => maxMP;


    public Action<float> onHealthChange 
    { 
        get;
        set;
    }

    public bool IsAlive => hp > 0;

    public Action onDie { get; set; }
    
    public void Die()
    {
        onDie?.Invoke();
        Debug.Log("플레이어 사망");
    }
}

 
    
   

