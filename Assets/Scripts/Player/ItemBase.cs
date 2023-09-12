using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
	readonly int PlayerLayerMask = 1 << 9;

	public float rangeDetect;

	public ItemCode itemCode;

	private void FixedUpdate()
	{
		if(Physics2D.OverlapCircle(transform.position, rangeDetect, PlayerLayerMask))
		{
			Debug.Log("�������� ���� �ȿ� �÷��̾� ����");
			GameManager.Ins.player.inven.AddItem(itemCode);
			Destroy(this.gameObject);
		}
		
	}
}
