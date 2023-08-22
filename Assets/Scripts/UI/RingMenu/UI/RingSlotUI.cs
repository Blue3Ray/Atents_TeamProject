using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class RingSlotUI : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerMoveHandler
{
    // ui�� ǥ���� ����
    RingMenuSlot ringslot;

    // ���� Ȯ�ο� ������Ƽ
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
        // ���콺 ��ġ�� ������Ʈ�� �ִ� �� Ȯ��
        if(obj != null)
        {
            RingSlotUI endSlot = obj.GetComponent<RingSlotUI>();
            if(endSlot != null)
            {
                Debug.Log($"�巡�� ���� : {endSlot.Index}�� ����");
                onDragEnd?.Invoke(endSlot.Index, true); // ���������� �ִ� ��Ȫ�� �ε����� ���������� �����ٰ� �˶� ������
            }
            else
            {
                Debug.Log($"������ �ƴմϴ�.");
                onDragEnd?.Invoke(endSlot.Index, false);
                // ���� �巡�װ� ������ �ε����� ������������ �����ٰ� �˶� ������
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
