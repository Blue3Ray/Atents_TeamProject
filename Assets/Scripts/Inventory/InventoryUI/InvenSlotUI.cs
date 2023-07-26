using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InvenSlotUI : MonoBehaviour
{
    Image itemImage = null;
    TextMeshProUGUI itemCount;
	InvenSlot UIslot;

	private void Awake()
	{
		Transform child = transform.GetChild(0);
		itemImage = child.GetComponent<Image>();
		itemImage.color = Color.clear;
		child = transform.GetChild(1);
		itemCount = child.GetComponent<TextMeshProUGUI>();
		itemCount.text = string.Empty;
	}

	public void InitializeSloutUI(InvenSlot slot)
	{
		UIslot = slot;
	}

	
}
