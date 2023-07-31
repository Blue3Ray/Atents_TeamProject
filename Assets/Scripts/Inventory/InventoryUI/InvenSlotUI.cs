using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InvenSlotUI : MonoBehaviour,  IDragHandler, IBeginDragHandler, IEndDragHandler

{
    Image itemImage = null;
    TextMeshProUGUI itemCount;
	public InvenSlot invenSlot = null;

	public Action<uint> onDragEnter;
	public Action onDragging;
	public Action<uint> onDragExit;


	private void Awake()
	{
		Transform child = transform.GetChild(0);
		itemImage = child.GetComponent<Image>();
		child = transform.GetChild(1);
		itemCount = child.GetComponent<TextMeshProUGUI>();
		itemImage.color = Color.clear;
		itemCount.text = string.Empty;
		itemImage.sprite = null;
		//refresh();
	}

	public void refresh()
	{

		if (invenSlot.ItemData == null)
		{
			itemImage.color = Color.clear;
			itemCount.text = string.Empty;
			itemImage.sprite = null;
		}
		else
		{
			itemImage.color = Color.white;
			itemCount.text = "" + (invenSlot.ItemCount);
			itemImage.sprite = invenSlot.ItemData.itemIcon;

		}

	}
	

	public void OnDrag(PointerEventData eventData)
	{
		onDragEnter?.Invoke(invenSlot.Index);
	}

	public void OnBeginDrag(PointerEventData eventData)
	{
		//Debug.Log($"드래그 시작{invenSlot.Index}");
		onDragging?.Invoke();
	}

	public void OnEndDrag(PointerEventData eventData)
	{
		GameObject gameObjecttmp = eventData.pointerCurrentRaycast.gameObject;
		//Debug.Log($"드래그 끝{gameObject.name}");

		InvenSlotUI invenSlotUI = gameObjecttmp.GetComponent<InvenSlotUI>();
		if(invenSlotUI != null)
		{
			onDragExit?.Invoke(invenSlotUI.invenSlot.Index);
			//Debug.Log($"드래그 끝{invenSlotUI.invenSlot.Index}");

		}

	}
}
