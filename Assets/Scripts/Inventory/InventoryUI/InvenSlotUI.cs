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
	public Action<uint, uint> onSplit;

	public float splitCountUpSeconds = 1.0f;

	uint splitCount = 0;

	public uint SplitCount
	{
		get => splitCount;
		set
		{
			if(SplitCount != value)
			{
				splitCount = value;
				//count를 하나 만들자
			}
		}
	}
	


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
		onSplit?.Invoke(this.invenSlot.Index, splitCount);
	}

	IEnumerator SplitItemCountUp()
	{
		SplitCount = 1;
		while(splitCount < invenSlot.ItemCount - 1)
		{
			yield return new WaitForSeconds(splitCountUpSeconds);
			SplitCount++;
			Debug.Log($"{splitCount}");
		}
	}
}
