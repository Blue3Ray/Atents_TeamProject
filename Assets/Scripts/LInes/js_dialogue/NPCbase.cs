using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCbase : MonoBehaviour
{
	//플레이어에게서 입력을 받아와야 하므로 플레이어에 접근
	PlayerTest playerTest;

	//캔버스의 자식에 접근해서 활성화를 해줘야 하므로 캔버스에 접근
	Canvas canvas;

	private void Awake()
	{
		canvas = FindAnyObjectByType<Canvas>();
		playerTest = FindAnyObjectByType<PlayerTest>();

		//플레이어Test에서 클릭했을 때 invoke되는 액션에 CanvaOn 실행
		playerTest.ClickedObject = CanvasOn;
		
	}

	private void CanvasOn(Transform clickTransform)
	{
		
		if(clickTransform == transform)
		{
			Transform canvasTransform = canvas.transform.GetChild(0);
			canvasTransform.gameObject.SetActive(true);	
		}
	}
}
