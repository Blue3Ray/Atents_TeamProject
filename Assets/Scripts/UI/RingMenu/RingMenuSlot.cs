using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenuSlot 
{
    // 슬롯 인덱스
    uint slotIndex;

    // 인덱스를 확인하기 위한 프로퍼티
    public uint Index => slotIndex;

    // 슬롯에 있는 속성 종류
    ElementalData slotElementalData = null;

    // 슬롯에 있는 송성 종류를 확인하기 위한 프로퍼티
    public ElementalData ElementalData
    {
        get => slotElementalData;
        private set
        {
            if(slotElementalData != value) // 속성 종류 변경
            {
                slotElementalData= value;
                onSlotChange?.Invoke();
            }
        }
    }

    // 슬롯에 들어있는 송성이 변경되었다고 알리는 델리게이트
    public Action onSlotChange;

    // 슬롯에 속성이 있는지 없는지 확인 하는 프로퍼티((true면 비어있고, false면 속성이 들어있다.)
    public bool IsEmpty => slotElementalData == null;

    //생성자
    public RingMenuSlot(uint index)
    {
        slotIndex= index;
    }

    // 슬롯을 비우는 함수
    public void ClearSlotItem()
    {
        slotElementalData = null;
    }
    

}
