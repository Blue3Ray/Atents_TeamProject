using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Character
{
    Element element;
	ActionControl ac;
	public Action MouseJustclick_Left;

	protected override void Awake()
	{
		base.Awake();
		ac = new();
	}

	private void OnEnable()
	{
		ac.ClickAction.Enable();
		ac.ClickAction.Mouse_Left.performed += OnClickMouse_Left;
	}

	private void OnDisable()
	{
		ac.ClickAction.Disable();
	}
	private void OnClickMouse_Left(InputAction.CallbackContext obj)
	{
		Vector3 mousePosition = Input.mousePosition;

		MouseJustclick_Left?.Invoke();


		Ray ray = Camera.main.ScreenPointToRay(mousePosition);

		RaycastHit2D hit;


		if (hit = Physics2D.Raycast(ray.origin, ray.direction, 50.0f))
		{
			Debug.Log($"{hit.transform.name}");
			if (hit.transform.TryGetComponent<IClickable>(out IClickable temp))
			{
				temp.OnClicking(temp);
			}
		}
	}

}
