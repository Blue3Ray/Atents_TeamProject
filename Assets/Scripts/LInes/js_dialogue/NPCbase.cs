using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCbase : MonoBehaviour
{
	//DialogParse Dia;

	Canvas talkCavas;

	ActionControl actionControle;

	Vector3 NewMousePosition;
	
	Vector2 mousePosition;

	private Collider2D myCollider;

	protected void Awake()
	{
		//Dia = FindAnyObjectByType<DialogParse>();
		myCollider = GetComponent<Collider2D>();
		actionControle = new ActionControl();
		talkCavas = GetComponent<Canvas>();
		if(talkCavas == null)
		{
			Debug.Log("talkcanvas가 비었습니다");
		}
	}

	private void OnEnable()
	{
		actionControle.ClickAction.Enable();
		actionControle.ClickAction.Mouse_Left.performed += OnClick;
		
	}

	private void OnDisable()
	{
		actionControle.ClickAction.Mouse_Left.performed -= OnClick;
		actionControle.ClickAction.Disable();

	}

	private void OnClick(InputAction.CallbackContext _)
	{
		mousePosition = Mouse.current.position.value;
		mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
		NewMousePosition = new Vector3(mousePosition.x, mousePosition.y, 0);
		//Debug.Log($"{NewMousePosition}");
		if (myCollider.bounds.Contains(NewMousePosition))
		{
			Speak();
		}

	}



	private void Speak()
	{
		Debug.Log($"HI");

		//talkCavas.gameObject.SetActive(true);
		
		//Dia.OnDialog("Lines", 0);
	}


	//private void OnDialogue()
	//{
	//	print(dataDialog[0]["Event Name"].ToString());

	//	//print(dataDialog[1]["Lines"].ToString());

	//	//print(dataDialog[2]["Lines"].ToString());

	//	//print(dataDialog[3]["Lines"].ToString());

	//	//print(dataDialog[1]["Event Name"].ToString());
	//	NewMousePosition = Vector2.zero;

	//}

}
