using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCbase : MonoBehaviour
{
	//�÷��̾�Լ� �Է��� �޾ƿ;� �ϹǷ� �÷��̾ ����
	PlayerTest playerTest;

	//ĵ������ �ڽĿ� �����ؼ� Ȱ��ȭ�� ����� �ϹǷ� ĵ������ ����
	Canvas canvas;

	private void Awake()
	{
		canvas = FindAnyObjectByType<Canvas>();
		playerTest = FindAnyObjectByType<PlayerTest>();

		//�÷��̾�Test���� Ŭ������ �� invoke�Ǵ� �׼ǿ� CanvaOn ����
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
