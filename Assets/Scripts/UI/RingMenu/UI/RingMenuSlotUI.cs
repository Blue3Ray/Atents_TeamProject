
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingMenuSlotUI : MonoBehaviour, IPointerClickHandler  // 포인터 이벤트들 필요한거
{
    public ElementalType elementalType;

    public System.Action<uint>  onClick;


    protected virtual void Awake()
    {
        onClick= null;
        elementalType = ElementalType.None;
    }



    // 이벤트 발생하는 인터페이스 함수

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke((uint)elementalType);
    }

  
}
