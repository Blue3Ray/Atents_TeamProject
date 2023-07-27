using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlotUI : MonoBehaviour
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
		refresh();
	}

	public void refresh()
	{
		if(invenSlot != null)
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
				itemCount.text = invenSlot.ItemData.itemDescription;
				itemImage.sprite = invenSlot.ItemData.itemIcon;

			}
		}
		else
		{
			itemImage.color = Color.clear;
			itemCount.text = string.Empty;
			itemImage.sprite = null;
		}
		
	}

	
}
