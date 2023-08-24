using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSensor : MonoBehaviour
{
	public Action<bool> OnWall;
	

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Ground"))
		{
			Debug.Log("ºÎ´ÚÄ§");
			OnWall?.Invoke(true);
		}
	}


	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Ground"))
		{
			OnWall?.Invoke(false);
		}
	}

}
