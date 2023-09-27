using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

	// Update is called once per frame
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			Debug.Log("플레이어 접근");
		}
	}
}
