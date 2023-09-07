
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RingMenuSlotUI : MonoBehaviour, IPointerClickHandler,/* IPointerEnterHandler, IPointerExitHandler*/ IPointerDownHandler, IPointerUpHandler
    // 포인터 이벤트들 필요한거
{
    public ElementalType elementalType;

    public System.Action<uint>  onClick;
    public System.Action<ElementalType> onUP;
    public System.Action<uint>  onEnter;
    public System.Action<uint> onExit;
    public System.Action<uint>  onDown;

    RingMenuSlotUI ringMenuSlotUI;

   

    private void Awake()
    {
        ringMenuSlotUI = GetComponent<RingMenuSlotUI>();
        onEnter = null;
        onExit = null;
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("click");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Debug.Log("Down");
    }

    //public void OnPointerEnter(PointerEventData eventData)
    //{
        
    //    if (eventData.pointerCurrentRaycast.gameObject != null)
    //    {
    //        Debug.Log("Enter");
    //    }
    //}

    //public void OnPointerExit(PointerEventData eventData)
    //{
    //    if (eventData.pointerCurrentRaycast.gameObject == null)
    //    {
    //        Debug.Log("Exit");
    //    }

    //}

    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(eventData.pointerPressRaycast.gameObject != null)
            {
                Debug.Log("Up");
            }
        }
        
    }



    //public void OnPointerUp(PointerEventData eventData)
    //{
    //    Debug.Log("Up");
    //    if(eventData.button == PointerEventData.InputButton.Right)
    //    {
    //        if(eventData.pointerCurrentRaycast.gameObject != null)
    //        {
    //            onUP?.Invoke(elementalType);
    //            Debug.Log($"in Slot : {elementalType}");
    //        }
    //    }

    //}


    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    Debug.Log("Down");
    //}
}
