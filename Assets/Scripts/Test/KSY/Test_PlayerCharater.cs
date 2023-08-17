using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayerCharater : Character
{
       

    // ����ġ ���� �κ�
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
//-------------------------------------------------------------------------------------------------
    private void Awake()
    {
       

    }

    private void Start()
    {
        hp = 100.0f;
        maxHP = 100.0f;
    }


    public void GetEx(float ex)
    {
        Experience += ex;
    }
}
