using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class RingSlotUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    // ui가 표현할 슬롯
    RingMenuSlot ringslot;

    // 슬롯 확인용 프로퍼티
    public RingMenuSlot RingSlot => ringslot;

    public uint Index => ringslot.Index;


    public Action<uint> onDragBegin;
    public Action<uint, bool> onDragEnd;
    public Action<uint> onClick;
    public Action<uint> onPointerEnter;
    public Action<uint> onPointerExit;
    public Action<Vector2> onPointerMove;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
    }

    public void InitializeSlot(RingMenuSlot slot)
    {
        onDragBegin = null;
        onDragEnd = null;
        onClick = null;
        onPointerEnter = null;
        onPointerExit = null;
        onPointerMove = null;

        ringslot = slot;
    }


    public void OnDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        GameObject obj = eventData.pointerCurrentRaycast.gameObject;
        // 마우스 위치에 오브젝트가 있는 지 확인
        if(obj != null)
        {
            RingSlotUI endSlot = obj.GetComponent<RingSlotUI>();
            if(endSlot != null)
            {
                Debug.Log($"드래그 종료 : {endSlot.Index}번 슬롯");
                onDragEnd?.Invoke(endSlot.Index, true); // 끝난지점에 있는 슬홋의 인덱스와 정상적으로 끝났다고 알람 보내기
            }
            else
            {
                Debug.Log($"슬롯이 아닙니다.");
                onDragEnd?.Invoke(endSlot.Index, false);
                // 원래 드래그가 시작한 인덱스와 비정상적으로 끝났다고 알람 보내기
            }
        }

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(Index);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }

}
