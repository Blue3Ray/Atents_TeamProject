using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCbase : MonoBehaviour
{
	public Action IsClick;

	ActionControl actionControle;

	Vector3 NewMousePosition;
	
	Vector2 mousePosition;

	private Collider2D myCollider;

	protected void Awake()
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
		if (myCollider.bounds.Contains(NewMousePosition))
		{
			Speak();
		}

	}

	private void Speak()
	{
		Debug.Log("HI");
		IsClick?.Invoke();
	}

}
