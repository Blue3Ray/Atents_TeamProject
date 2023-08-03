using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSlotUI : SlotUIBase
{

	public float addX;
	public float addY;
	public Vector3 addedPosition= Vector3.zero;

	protected override void Awake()
	{
		base.Awake();
		addedPosition = new Vector3(addX, addY, 0);
	}

	//private void Awake()
	//{
	//	base.Awake();
	//	addedPosition = new Vector3(addX, addY, 0);
	//}
	private void Update()
	{
		if (Mouse.current.rightButton.isPressed)
		{
			transform.position = new Vector3(Mouse.current.position.ReadValue().x, Mouse.current.position.ReadValue().y, 0) + addedPosition;
		}
		else
		{
			transform.position = Mouse.current.position.ReadValue();
		}
	}
}
