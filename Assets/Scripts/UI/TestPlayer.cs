using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.Switch;

public class TestPlayer : MonoBehaviour, IHealth, IMana 
{
    ActionControl actions;

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


    public System.Action<float> onHealthChange 
    { 
        get;
        set;
    }

    public bool IsAlive => hp > 0;
    public System.Action onDie { get; set; }

    // ����ġ �� ���� ����
    float experience = 0.0f;
    public float Experience
    {
        get => experience;
        set
        {
            experience = value;

            if (experience >= experienceMax)
            {
                experience -= experienceMax;
                level++;
                Debug.Log($"Level up : {level}");
            }
            onChangeEx(experience, experienceMax, level);
        }
    }
    float experienceMax = 100.0f;
    int level = 1;
    public int Level
    {
        get => level;
        set
        {
            level = value;
            // ���� ��½� �߻��� �̺�Ʈ��
        }
    }

    public System.Action<float, float, int> onChangeEx;

    private void Awake()
    {
        actions= new ActionControl();
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

    public void GetEx(float ex)
    {
        Experience += ex;
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
        Debug.Log("�÷��̾� ���");
    }

    private void OnLevelUp(UnityEngine.InputSystem.InputAction.CallbackContext _)
    {
        GetEx(10.0f);
        Debug.Log($"����ġ �ο���");
    }

}





