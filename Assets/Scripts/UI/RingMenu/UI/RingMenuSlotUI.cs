
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingMenuSlotUI : MonoBehaviour, IPointerClickHandler  // ������ �̺�Ʈ�� �ʿ��Ѱ�
{
    public ElementalType elementalType;

    public System.Action<uint>  onClick;


    protected virtual void Awake()
    {
        onClick= null;
        elementalType = ElementalType.None;
    }



    // �̺�Ʈ �߻��ϴ� �������̽� �Լ�

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke((uint)elementalType);
    }

  
}
