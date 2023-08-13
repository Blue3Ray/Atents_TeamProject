using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
	BoxCollider2D colliderBlock;
	PlayerJM playerJM;

	bool InBlockCollider = false;

	private void Awake()
	{
		colliderBlock = transform.GetComponentInParent<BoxCollider2D>();
	}
	private void Start()
	{
		playerJM = GameManager.Ins.playerTest;
		playerJM.OnBlockCommand += PassBlock;
	}

	/// <summary>
	/// 아래 키와 스페이스바를 동시에 눌렀을 때
	/// 이 스크립트를 가진 게임오브젝트의 콜라이더에
	/// </summary>
	void PassBlock()
	{
		Debug.Log("델리게이트 실행은 됨");
		if (InBlockCollider)
		{
			StartCoroutine(BlockKinematicChange());
		}
	}

	/// <summary>
	/// 2초동안 벽의 rigid 컴포넌트를 키네마틱으로 만든 다음
	/// 알아서 꺼지는 코루틴
	/// </summary>
	/// <returns></returns>
	IEnumerator BlockKinematicChange()
	{
		colliderBlock.isTrigger = true;
		yield return new WaitForSeconds(2);
		colliderBlock.isTrigger = false;
		StopCoroutine((BlockKinematicChange()));

	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			InBlockCollider = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		
		if (collision.CompareTag("Player"))
		{
			InBlockCollider = false;
		}
	}



}
