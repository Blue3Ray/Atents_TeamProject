using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	public InvenSlotUI[] UISlots;
	Inventory inven;

	private void Awake()
	{
		Transform child = transform.GetChild(2);
		UISlots = GetComponentsInChildren<InvenSlotUI>();
	}

	private void Start()
	{
	}
}
