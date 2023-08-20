using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayerCharacter : Character, IExperience
{
    /// <summary>
    /// ����ġ ���� �κ�
    /// </summary>
    int experience = 0;
    public int Experience 
    { 
        get => experience; 
        set
        {
            experience = value;
            if(experience >= experieceMax)
            {
                experience -= experieceMax;
                Level++;
            }
            Debug.Log($"���� ����ġ : {experience}, �ִ� ����ġ : {experieceMax}");
            onChangeEx?.Invoke(Level, experience, experieceMax);
        }
    }

    /// <summary>
    /// �ִ� ����ġ
    /// </summary>
    int experieceMax = 100;
    public int ExperienceMax
    {
        get => experieceMax;
        set
        {
            experieceMax = value;
        }
    }

    /// <summary>
    /// ���� ����
    /// </summary>
    uint level = 1;
    public uint Level 
    {
        get => level;
        set
        {
            if (level != value)
            {
                if (value > level) onLevelUP?.Invoke();
                level = value;   
            }
        }
    }

    public Action<uint, int, int> onChangeEx { get; set ; }
    public Action onLevelUP { get; set; }

    //-------------------------------------------------------------------------------------------------

    protected override void Awake()
    {
        base.Awake();
        onLevelUP += IncreaceMax;
        onLevelUP += () => HP = MaxHP;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    void IncreaceMax()
    {
        ExperienceMax += 10;
    }
}