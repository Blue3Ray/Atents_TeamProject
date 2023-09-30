
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RingMenuSlotUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
// 포인터 이벤트들 필요한거
{
    public ElementalType elementalType;
   
    public System.Action<uint> onEnter;
    public System.Action<uint> onExit;
    
    public System.Action<uint> onDown;

    CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = transform.GetChild(0).GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        canvasGroup.alpha = 0.0f;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if (obj != null)
        {
            onEnter?.Invoke((uint)elementalType);
            Debug.Log($"{elementalType}");
            canvasGroup.alpha = 1.0f;
        }
        
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        if (obj != null)
        {
            onExit?.Invoke((uint)elementalType);
            canvasGroup.alpha = 0.0f;
        }

    }






}
