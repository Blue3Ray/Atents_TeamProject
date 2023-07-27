using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
	Inventory inventory;
	public InvenSlotUI[] UISlots;
	public InvenSlotUI this[int index] => UISlots[index];
	

	private void Awake()
	{
		Transform child = transform.GetChild(2);
		UISlots = GetComponentsInChildren<InvenSlotUI>();
	}

	private void Start()
	{
		inventory = GameManager.Ins.inven;
		ConnetingSlots();
	}

	public void ConnetingSlots()
	{
		for(int i = 0; i < inventory.SlotCount; i++)
		{
			UISlots[i].invenSlot = inventory[(uint)i];
			
		}


	}
}
