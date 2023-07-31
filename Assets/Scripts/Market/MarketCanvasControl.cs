using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.UI;
public class MarketCanvasControl : MonoBehaviour
{
	ExitButton exitButton;
	GameObject marketWindow;
	TalkCanvas talkCanvas;
	Transform blocking;

	private void Awake()
	{
		blocking = transform.GetChild(0);
		Transform transformT = transform.GetChild(1);
		talkCanvas = transformT.GetComponent<TalkCanvas>();
		Transform transformM = transform.GetChild(2);
		marketWindow = transformM.gameObject;
		Transform transformB = transformM.GetChild(4);
		exitButton = transformB.GetComponent<ExitButton>();
		talkCanvas.EndTalk += MarketOn;
		exitButton.IsExitMarket += ExitMarket;

		
	}

	public void ExitMarket()
	{
		marketWindow.gameObject.SetActive(false);
		blocking.gameObject.SetActive(false);
	}

	private void MarketOn()
	{
		marketWindow.gameObject.SetActive(true);
		blocking.gameObject.SetActive(true);
	}
}
