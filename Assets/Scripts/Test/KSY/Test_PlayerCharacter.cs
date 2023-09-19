using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_PlayerCharacter : CharacterBase, IExperience
{
    /// <summary>
    /// 경험치 구현 부분
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
            Debug.Log($"현재 경험치 : {experience}, 최대 경험치 : {experieceMax}");
            onChangeEx?.Invoke(Level, experience, experieceMax);
        }
    }

    /// <summary>
    /// 최대 경험치
    /// </summary>
    int experieceMax = 30;
    public int ExperienceMax
    {
        get => experieceMax;
        set
        {
            experieceMax = value;
        }
    }

    /// <summary>
    /// 현재 레벨
    /// </summary>
    uint level = 1;
    public uint Level 
    {
        get => level;
        set
        {
            if (level != value)
            {
                if (value > level) onLevelUP?.Invoke(level);
                level = value;   
            }
        }
    }

    public Action<uint, int, int> onChangeEx { get; set ; }
    public Action<uint> onLevelUP { get; set; }

    //-------------------------------------------------------------------------------------------------

    protected override void Awake()
    {
        base.Awake();
        onLevelUP += (level) => IncreaceMax();
        onLevelUP += (level) => HP = MaxHP;
    }

    public override void OnInitialize()
    {
        base.OnInitialize();
    }

    void IncreaceMax()
    {
        ExperienceMax += 10;
    }


    public void LevelUp()
    {
        throw new NotImplementedException();
    }

}
