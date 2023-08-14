using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketControle : MonoBehaviour
{
	Transform content;

	public GameObject itemBlock;

	[SerializeField]
	public ItemCode[] itemCodes;

	public int ItemCodes => itemCodes.Length;

    ItemBlock[] itemBlocks;

	private void Awake()
	{
		itemBlocks = GetComponentsInChildren<ItemBlock>();
		content = GetComponentInChildren<ContentSizeFitter>().transform;
		
	}

	private void Start()
	{
		for(int i = 0; i<ItemCodes; i++)
		{
			GameObject tempItemBlok = Instantiate(itemBlock, content);
			tempItemBlok.name = "ItemBlock" + $"_{i}";
			ItemBlock tempItemBlockComponent = tempItemBlok.GetComponent<ItemBlock>();
			tempItemBlockComponent.Refresh(itemCodes[i]);
		}
	}


}
