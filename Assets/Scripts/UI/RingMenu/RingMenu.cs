using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.VisualScripting;
using UnityEditor.SceneManagement;

public class RingMenu
{
    // �� �޴� ���� �⺻ ����
    public const int Default_RingMenu_Size = 4;

    RingMenuSlot[] slots;

    public RingMenuSlot this[uint index] => slots[index]; 
    
    // ���޴� ������ ����
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
