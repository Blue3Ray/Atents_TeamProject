using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WallSensor : MonoBehaviour
{
	public Action<bool> OnWall;
	

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Wall"))
		{
			OnWall?.Invoke(true);
		}
	}


	private void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.CompareTag("Wall"))
		{
			OnWall?.Invoke(false);
		}
	}

}
