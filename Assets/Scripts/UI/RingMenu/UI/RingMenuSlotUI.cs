
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingMenuSlotUI : MonoBehaviour, IPointerClickHandler  // ������ �̺�Ʈ�� �ʿ��Ѱ�
{
    public ElementalType elementalType;

    public System.Action<uint>  onClick;

    private void Awake()
    {
        onClick= null;
        elementalType = ElementalType.None;
    }

    public void SlotSelect()
    {

    }

    // �̺�Ʈ �߻��ϴ� �������̽� �Լ�

    public void OnPointerClick(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
