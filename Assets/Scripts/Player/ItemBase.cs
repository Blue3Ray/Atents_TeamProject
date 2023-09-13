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
			if (GameManager.Ins.player.inven.AddItemExeptQuickSlot(itemCode))
			{
				Destroy(this.gameObject);
			}
			
		}
		
	}
}
