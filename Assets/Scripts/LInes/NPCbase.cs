using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class NPCbase : MonoBehaviour
{

	ActionControl actionControle;
	private float isClick;

	private void Awake()
	{
		actionControle = new ActionControl();
	}

	private void OnEnable()
	{
		actionControle.ClickAction.Enable();
		actionControle.ClickAction.Mouse_Left.performed += OnClick;
		actionControle.ClickAction.Mouse_Left.canceled += OnClick;
	}

	private void OnDisable()
	{
		actionControle.ClickAction.Mouse_Left.canceled -= OnClick;
		actionControle.ClickAction.Mouse_Left.performed -= OnClick;
		actionControle.ClickAction.Disable();
	}

	private void OnClick(InputAction.CallbackContext _)
	{
		isClick = _.ReadValue<float>();
		Debug.Log($"{isClick}");
	}
}
