
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RingMenuSlotUI : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerEnterHandler
    // ������ �̺�Ʈ�� �ʿ��Ѱ�
{
    public ElementalType elementalType;

    public System.Action<uint>  onClick;
    public System.Action<uint>  onUP;

    RingMenuSlotUI slot;
    Image image;

    protected virtual void Awake()
    {
        slot = GetComponent<RingMenuSlotUI>();
        image = GetComponent<Image>();
        onClick= null;
        elementalType = (uint)ElementalType.None;
        
    }



    // �̺�Ʈ �߻��ϴ� �������̽� �Լ�

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke((uint)elementalType);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        onUP?.Invoke((uint)elementalType);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        
        
        
    }
}
