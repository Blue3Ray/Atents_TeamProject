using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ElementalType
{
    None,
    Fire,
    Water,
    Wind,
    Thunder
}



public class ElemantalStates 
{
    /// <summary>
    /// 현재 원소 속성
    /// </summary>
    ElementalType currentElemantal = ElementalType.None;
    public ElementalType CurrentElemantal
    {

        get => currentElemantal;
        private set
        {
            currentElemantal = value;
        }
    }

    /// <summary>
    /// 현재 원소 레벨
    /// </summary>
    public int currentElemantalLevel = 0;

    /// <summary>
    /// 모든 원소에 대한 레벨
    /// ElementalType의 인덱스 순서와 같음
    /// </summary>
    public int[] elemantalevels = new int[5] {0,1,1,1,1};

    /// <summary>
    /// 원소가 변경될 시 실행되는 델리게이트
    /// </summary>
    public System.Action<ElementalType> onElemantaltypeChange;

    /// <summary>
    /// 현재 원소를 바꿀때 불리는 함수
    /// </summary>
    /// <param name="type">바꿀 원소</param>
    public void ChangeType(ElementalType type)
    {
        if (type != CurrentElemantal)       // 현재 원소와 같으면 굳이 바꾸지 않는다
        {
            if (type != 0 && elemantalevels[(int)type] == 0) return;        // 무속성이 아닌데 레벨이 0인 원소를 불러내려고 할 때 아무것도 하지 않는다.
            CurrentElemantal = type;
            currentElemantalLevel = elemantalevels[(int)type];
            onElemantaltypeChange?.Invoke(CurrentElemantal);
        }
    }

    /// <summary>
    /// 특정 원소의 레벨을 올리는 함수
    /// </summary>
    /// <param name="type">레벨 올릴 원소</param>
    public void LevelUpElemental(ElementalType type)
    {
        elemantalevels[(int)type]++;
    }
}
