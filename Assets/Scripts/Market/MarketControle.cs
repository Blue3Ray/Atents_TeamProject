using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MarketControle : MonoBehaviour
{
	public GameObject itemBlock;

	[SerializeField]
	public ItemCode[] itemCodes;

	public int ItemCodes => itemCodes.Length;

	Transform content;
	CanvasGroup canvasGroup;

	private void Awake()
	{
		content = GetComponentInChildren<ContentSizeFitter>().transform;
		canvasGroup = transform.GetComponent<CanvasGroup>();
		
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
		MarketOff();
	}

	public void MarketOff()
	{
		canvasGroup.alpha = 0;
		canvasGroup.interactable = false;
		canvasGroup.blocksRaycasts = false;
		GameManager.Ins.Player.EnableInputAction();
	}

	public void MarketOn()
	{
		canvasGroup.alpha = 1;
		canvasGroup.interactable = true;
		canvasGroup.blocksRaycasts = true;
		GameManager.Ins.Player.DisableInputAction();
	}
}
