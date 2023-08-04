using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NPCbase : MonoBehaviour, IClickable
{

	//캔버스의 자식에 접근해서 활성화를 해줘야 하므로 캔버스에 접근
	Canvas canvas;
	TalkCanvas talkCanvas;

	private void Awake()
	{
		canvas = FindAnyObjectByType<Canvas>();
		
	}
	private void Start()
	{
		
		Transform canvasTransform = canvas.transform.GetChild(0);
		talkCanvas = canvasTransform.GetComponent<TalkCanvas>();
	}


	private void CanvasOn()
	{
		talkCanvas.CanvasLineOn();
	}

	public void OnClicking(IClickable tmp)
	{
		Debug.Log("눌림");
		CanvasOn();
	}
}
