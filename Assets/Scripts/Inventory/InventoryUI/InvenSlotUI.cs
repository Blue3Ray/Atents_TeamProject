using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InvenSlotUI : SlotUIBase,  IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler

{
   

	public Action<uint> onDragEnter;
	public Action onDragging;
	public Action<uint> onDragExit;
	public Action<uint> onClick;
	public Action onTrashcan;
	

	public void OnDrag(PointerEventData eventData)
	{
		onDragging?.Invoke();
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		//Debug.Log($"드래그 시작{invenSlot.Index}");
		onDragEnter?.Invoke(invenSlot.Index);
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		GameObject gameObjecttmp = eventData.pointerCurrentRaycast.gameObject;
		
		
		if(gameObjecttmp != null)
		{
			if (gameObjecttmp.TryGetComponent<InvenSlotUI>(out InvenSlotUI invenSlotUI))
			{
				onDragExit?.Invoke(invenSlotUI.invenSlot.Index);

			}
			else if (gameObjecttmp.TryGetComponent<TrashCan>(out TrashCan _))
			{
				onTrashcan?.Invoke();
				Debug.Log("ray TrashCan");
			}
			else
			{
				onDragExit?.Invoke(this.invenSlot.Index);
			}

		}
		else
		{
			onDragExit?.Invoke(this.invenSlot.Index);
		}
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		onClick?.Invoke(this.invenSlot.Index);
	}
}
