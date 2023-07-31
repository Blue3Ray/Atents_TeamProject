using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TempSlotUI : SlotUIBase
{
	private void Update()
	{
		transform.position = Mouse.current.position.ReadValue();
	}
}
