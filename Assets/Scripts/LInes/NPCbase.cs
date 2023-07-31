using Mono.CompilerServices.SymbolWriter;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCbase : MonoBehaviour, IClickable
{

	//ĵ������ �ڽĿ� �����ؼ� Ȱ��ȭ�� ����� �ϹǷ� ĵ������ ����
	Canvas canvas;


	public void OnClicking(IClickable tmp)
	{
		if(tmp as NPCbase != null)					//Ŭ���� ���� NPCbase �� ����
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
		Transform canvasTransform = canvas.transform.GetChild(1);
		canvasTransform.gameObject.SetActive(true);
	}
}
