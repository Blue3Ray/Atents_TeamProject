
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingMenuSlotUI : MonoBehaviour, IPointerClickHandler  // ������ �̺�Ʈ�� �ʿ��Ѱ�
{
    public ElementalType elementalType;

    public System.Action<uint>  onClick;

    uint index = 0; 
    private void Awake()
    {
        onClick= null;
        elementalType = ElementalType.None;
    }

    public void SlotSelect()
    {
        switch (elementalType)
        {
            case ElementalType.Fire:
                index = 1;
                
                break;
            case ElementalType.Wind:
                index = 2;
                break;
            case ElementalType.Water:
                index = 3;
                break;
            case ElementalType.Thunder:
                index = 4;
                break;

        }
    }

    // �̺�Ʈ �߻��ϴ� �������̽� �Լ�

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerClick.gameObject;

        RingMenuSlotUI clickSlot = obj.GetComponent<RingMenuSlotUI>();
        if(clickSlot != null)
        {
            onClick?.Invoke(clickSlot.index);
            Debug.Log($"�巡�� ���� : {clickSlot.index}�� ����");
        }
    }
}
