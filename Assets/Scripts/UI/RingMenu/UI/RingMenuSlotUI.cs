
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RingMenuSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
// ������ �̺�Ʈ�� �ʿ��Ѱ�
{
    public ElementalType elementalType;
   
    public System.Action<uint> onEnter;
    public System.Action<uint> onExit;
    
    public System.Action<uint> onDown;


    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if (obj != null)
        {
            RingMenuSlotUI ringslot = obj.GetComponent<RingMenuSlotUI>();
            if (ringslot != null)
            {
                onEnter?.Invoke((uint)ringslot.elementalType);
                Debug.Log($"{ringslot.elementalType}");
            }     
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if (obj != null)
        {
            RingMenuSlotUI ringslot = obj.GetComponent<RingMenuSlotUI>();
            if (ringslot != null)
            {
                onExit?.Invoke((uint)ringslot.elementalType);
               
            }
        }

    }






}
