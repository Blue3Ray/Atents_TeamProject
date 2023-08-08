using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemBlock : MonoBehaviour
{
	TextMeshProUGUI itemName;
	Image itemImage;
	private void Awake()
	{
		itemName = transform.GetChild(2).GetComponent<TextMeshProUGUI>();
		itemImage = transform.GetChild(4).GetComponent<Image>();
		itemImage.color = Color.clear;
	}

	public void Refresh(ItemCode code)
	{
		
		itemName.text = GameManager.Ins.ItemData[code].itemName;
		itemImage.sprite = GameManager.Ins.ItemData[code].itemIcon;
		itemImage.color = Color.white;
	}
}
