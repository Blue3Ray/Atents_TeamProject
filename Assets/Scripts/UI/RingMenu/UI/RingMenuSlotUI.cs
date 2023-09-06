
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RingMenuSlotUI : MonoBehaviour, /*IPointerClickHandler,*/ IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler
    // 포인터 이벤트들 필요한거
{
    public ElementalType elementalType;

    public System.Action<uint>  onClick;
    public System.Action<ElementalType> onUP;
    public System.Action<uint>  onEnter;
    public System.Action<uint>  onDown;

    


    private void Awake()
    {
        onClick= null;
        onUP= null;
        onEnter= null;
        onDown= null;
    }



    // 이벤트 발생하는 인터페이스 함수

    //public void OnPointerClick(PointerEventData eventData)
    //{
        

    //}

    public void OnPointerUp(PointerEventData eventData)
    {
        if(eventData.button == PointerEventData.InputButton.Right)
        {
            if(eventData.pointerCurrentRaycast.gameObject != null)
            {
                onUP?.Invoke(elementalType);
                Debug.Log($"in Slot : {elementalType}");
            }
        }

    }

    public void OnPointerEnter(PointerEventData eventData)
    {     
       
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
       
    }
}
