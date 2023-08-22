using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
	public float fireBallSpeed = 1;

	private void Awake()
	{
		transform.SetParent(null);
	}
	private void Update()
	{
		if (GameManager.Ins.IsRight)
		{
			transform.Translate(transform.right*fireBallSpeed);
		}
		else
		{

			transform.Translate(-(transform.right)*fireBallSpeed);
		}
	}

	public void EndAttack()
    {
		//if(GameManager.Ins.IsRight)
		Debug.Log("이벤트키 실행");
        Destroy(this.gameObject);
    }
}
