using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public class TestPlayer : MonoBehaviour, IHealth, IMana 
{
    ActionControl actions;
    Leveling leveling;

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

    // 경험치 및 레벨 설정
    float experience = 0.0f;
    float experienceMax = 100.0f;
    int level = 1;

    private void Awake()
    {
        actions= new ActionControl();
        leveling = new Leveling();
    }

    private void Start()
    {
       
    }

    private void Update()
    {
      
    }

    private void FixedUpdate()
    {
        
    }

    private void OnEnable()
    {
        actions.Test.Enable();
        actions.Test.Test1.performed += OnLevelUp;
    }

 
    private void OnDisable()
    {
        actions.Test.Test1.performed -= OnLevelUp;
        actions.Test.Disable();
    }
    public void Die()
    {
        onDie?.Invoke();
        Debug.Log("플레이어 사망");
    }

    private void OnLevelUp(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        experience += 10.0f;

        leveling.LevelUP(experience, experienceMax, level);
        Debug.Log($" {experience}");

    }

}





