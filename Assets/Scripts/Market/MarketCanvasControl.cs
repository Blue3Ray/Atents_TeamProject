using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MarketCanvasControl : MonoBehaviour
{
	ExitButton exitButton;
	GameObject marketWindow;
	TalkCanvas talkCanvas;

	private void Awake()
	{
		Transform transformT = transform.GetChild(0);
		talkCanvas = transformT.GetComponent<TalkCanvas>();
		Transform transformM = transform.GetChild(1);
		marketWindow = transformM.gameObject;
		Transform transformB = transformM.GetChild(1);
		exitButton = transformB.GetComponent<ExitButton>();
		talkCanvas.EndTalk += MarketOn;
		exitButton.IsExitMarket += ExitMarket;
		
	}

	private void ExitMarket()
	{
		marketWindow.gameObject.SetActive(false);
	}

	private void MarketOn()
	{
		marketWindow.gameObject.SetActive(true);
	}
}
