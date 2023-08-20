using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;

public class RingMenu
{
    // 링 메뉴 슬롯 기본 개수
    public const int Default_RingMenu_Size = 4;

    RingMenuSlot[] slots;

    public RingMenuSlot this[uint index] => slots[index]; 
    
    // 링메뉴 슬롯의 갯수
    public int slotCount => slots.Length;

    Character owner;
    public Character Owner => owner;

    public RingMenu(Character owner, uint size = Default_RingMenu_Size)
    {
        slots= new RingMenuSlot[size];
        for(uint i = 0; i < size; i++)
        {
            slots[i] = new RingMenuSlot(i);
        }

        this.owner = owner;
    }

}
