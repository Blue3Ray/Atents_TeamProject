using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InvenSlotUI : SlotUIBase, IDragHandler, IBeginDragHandler, IEndDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler

{


	public Action<uint> onDragEnter;
	public Action<uint> onDragExit;
	public Action<uint> onClick;
	public Action onTrashcan;

	//split count가 하나씩 올라갈 때마다 슬롯과 count를 외친다.
	public Action<uint> onSplit;

	float splitCountUpSeconds = 0.5f;



	public void OnDrag(PointerEventData eventData)
	{
		
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Left)
		{
			onDragEnter?.Invoke(invenSlot.Index);
		}
	}

	public void OnEndDrag(PointerEventData eventData)
	{


		GameObject gameObjecttmp = eventData.pointerCurrentRaycast.gameObject;


		if (gameObjecttmp != null)
		{
			if (gameObjecttmp.TryGetComponent<InvenSlotUI>(out InvenSlotUI invenSlotUI))
			{
				onDragExit?.Invoke(invenSlotUI.invenSlot.Index);

			}
			else if (gameObjecttmp.TryGetComponent<TrashCan>(out TrashCan _))
			{
				onTrashcan?.Invoke();
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

	public void OnPointerDown(PointerEventData eventData)
	{
		if(eventData.button == PointerEventData.InputButton.Right)
		{
			StartCoroutine(SplitItemCountUp());
		}
	}

	public void OnPointerUp(PointerEventData eventData)
	{
		StopAllCoroutines();
	}

	IEnumerator SplitItemCountUp()
	{
		while(1 < invenSlot.ItemCount)
		{
			onSplit?.Invoke(invenSlot.Index);
			yield return new WaitForSeconds(splitCountUpSeconds);
		}
		
	}
}
