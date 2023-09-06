
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RingMenuSlotUI : MonoBehaviour, /*IPointerClickHandler,*/ IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler
    // ������ �̺�Ʈ�� �ʿ��Ѱ�
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



    // �̺�Ʈ �߻��ϴ� �������̽� �Լ�

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
