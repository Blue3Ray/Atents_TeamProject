using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayerCharater : Character, IExperience
{



    // 경험치 구현 부분
    float experience = 0.0f;
    public float Experience 
    { 
        get => experience; 
        set
        {
            experience = value;
            if(experience >= experieceMax)
            {
                experieceMax -= experieceMax;
                level++;
            }
            onChangeEx(experience, experieceMax, level);
        }
    }
    float experieceMax = 100.0f;
    public float ExperienceMax => experieceMax;

    int level = 1;
    public int Level 
    {
        get => level;
        set
        {
            level = value;
         
        }
    }

    public Action<float, float, int> onChangeEx { get; set ; }
    public Action onLevelUP { get; set; }
  

    //-------------------------------------------------------------------------------------------------

    private void Start()
    {
        maxHP = 100.0f;
        HP = 100.0f;
    }


    public void GetEx(float ex)
    {
        
    }

    
}
