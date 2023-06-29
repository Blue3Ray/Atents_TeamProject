using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCbase : MonoBehaviour, IClickable
{

	//캔버스의 자식에 접근해서 활성화를 해줘야 하므로 캔버스에 접근
	Canvas canvas;


	public void OnClicking(IClickable tmp)
	{
		if(tmp as NPCbase != null)
		{
			CanvasOn();
		}
	}

	private void Awake()
	{
		canvas = FindAnyObjectByType<Canvas>();
		
	}

	private void CanvasOn()
	{

		Transform canvasTransform = canvas.transform.GetChild(0);
		canvasTransform.gameObject.SetActive(true);

	}
}
