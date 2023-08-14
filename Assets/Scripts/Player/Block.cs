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
	/// �Ʒ� Ű�� �����̽��ٸ� ���ÿ� ������ ��
	/// �� ��ũ��Ʈ�� ���� ���ӿ�����Ʈ�� �ݶ��̴���
	/// </summary>
	void PassBlock()
	{
		Debug.Log("��������Ʈ ������ ��");
		if (InBlockCollider)
		{
			StartCoroutine(BlockKinematicChange());
		}
	}

	/// <summary>
	/// 2�ʵ��� ���� rigid ������Ʈ�� Ű�׸�ƽ���� ���� ����
	/// �˾Ƽ� ������ �ڷ�ƾ
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
