
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RingMenuSlotUI : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerEnterHandler
    // 포인터 이벤트들 필요한거
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



    // 이벤트 발생하는 인터페이스 함수

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
