using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SlotUIBase : MonoBehaviour
{

	Image itemImage = null;
	TextMeshProUGUI itemCount;
	public InvenSlot invenSlot = null;

	private void Awake()
	{
		Transform child = transform.GetChild(0);
		itemImage = child.GetComponent<Image>();
		child = transform.GetChild(1);
		itemCount = child.GetComponent<TextMeshProUGUI>();
		itemImage.color = Color.clear;
		itemCount.text = string.Empty;
		itemImage.sprite = null;
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

	/// <summary>
	/// SlotUIBase�� Refresh �Լ��� InvenSlot�� ��������Ʈ onSlotItemChange�� �����ϱ� �����̴�.
	/// </summary>
	public void InitializeSlot()
	{
		invenSlot.onSlotItemChange += refresh;
	}
}