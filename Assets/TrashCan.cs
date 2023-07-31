using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TrashCan : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler

{
	public Sprite Open_TrashCan;
	public Sprite Closed_TrashCan;
	Image canImage;

	public Action ClickTrashCan;

	public Action MouseOnTrashCan;

	public void Awake()
	{
		canImage = GetComponent<Image>();
		canImage.sprite = Closed_TrashCan;
		canImage.SetNativeSize();
	}


	public void OnPointerEnter(PointerEventData eventData)
	{
		canImage.sprite = Open_TrashCan;
		canImage.SetNativeSize();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		canImage.sprite = Closed_TrashCan;
		canImage.SetNativeSize();
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		ClickTrashCan?.Invoke();
	}

}
