
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RingMenuSlotUI : MonoBehaviour, IPointerClickHandler  // 포인터 이벤트들 필요한거
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

    // 이벤트 발생하는 인터페이스 함수

    public void OnPointerClick(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerClick.gameObject;

        RingMenuSlotUI clickSlot = obj.GetComponent<RingMenuSlotUI>();
        if(clickSlot != null)
        {
            onClick?.Invoke(clickSlot.index);
            Debug.Log($"드래그 종료 : {clickSlot.index}번 슬롯");
        }
    }
}
