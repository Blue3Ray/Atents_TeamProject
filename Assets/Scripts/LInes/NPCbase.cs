using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCbase : MonoBehaviour
{
	
	ActionControl actionControle;

	Vector3 NewMousePosition;
	
	Vector2 mousePosition;

	private Collider2D myCollider;
	

	private void Awake()
	{
		myCollider = GetComponent<Collider2D>();
		actionControle = new ActionControl();
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
		Debug.Log($"{NewMousePosition}");
	}


	private void Update()
	{
		
		if (myCollider.bounds.Contains(NewMousePosition))
		{
			OnDialogue();
		}

	}

	private void OnDialogue()
	{
		Debug.Log("안녕하세요 리안입니다");
		NewMousePosition = Vector2.zero;

	}

}
