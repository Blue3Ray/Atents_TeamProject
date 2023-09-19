
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class NPCbase : MonoBehaviour, IClickable
{

	//ĵ������ �ڽĿ� �����ؼ� Ȱ��ȭ�� ����� �ϹǷ� ĵ������ ����
	Canvas canvas;
	TalkCanvas talkCanvas;

	private void Awake()
	{
		canvas = FindAnyObjectByType<Canvas>();
		
	}
	private void Start()
	{
		
		//Transform canvasTransform = canvas.transform.GetChild(2);
		//talkCanvas = canvasTransform.GetComponent<TalkCanvas>();

		talkCanvas = FindObjectOfType<TalkCanvas>();
	}


	private void CanvasOn()
	{
		talkCanvas.CanvasLineOn();
	}

	public void OnClicking(IClickable tmp)
	{
		Debug.Log("����");
		CanvasOn();
	}
}
