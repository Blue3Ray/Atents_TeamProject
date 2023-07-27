using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenuSlot 
{
    // ���� �ε���
    uint slotIndex;

    // �ε����� Ȯ���ϱ� ���� ������Ƽ
    public uint Index => slotIndex;

    // ���Կ� �ִ� �Ӽ� ����
    ElementalData slotElementalData = null;

    // ���Կ� �ִ� �ۼ� ������ Ȯ���ϱ� ���� ������Ƽ
    public ElementalData ElementalData
    {
        get => slotElementalData;
        private set
        {
            if(slotElementalData != value) // �Ӽ� ���� ����
            {
                slotElementalData= value;
                onSlotChange?.Invoke();
            }
        }
    }

    // ���Կ� ����ִ� �ۼ��� ����Ǿ��ٰ� �˸��� ��������Ʈ
    public Action onSlotChange;

    // ���Կ� �Ӽ��� �ִ��� ������ Ȯ�� �ϴ� ������Ƽ((true�� ����ְ�, false�� �Ӽ��� ����ִ�.)
    public bool IsEmpty => slotElementalData == null;

    //������
    public RingMenuSlot(uint index)
    {
        slotIndex= index;
    }

    // ������ ���� �Լ�
    public void ClearSlotItem()
    {
        slotElementalData = null;
    }
    

}
