using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemBase : MonoBehaviour
{
	readonly int PlayerLayerMask = 1 << 9;

	public float rangeDetect;

	public ItemCode itemCode;

	//private void FixedUpdate()
	//{
	//	if(Physics2D.OverlapCircle(transform.position, rangeDetect, PlayerLayerMask))
	//	{
	//		Debug.Log("�������� ���� �ȿ� �÷��̾� ����");
			
	//	}
	//}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
		{
            Debug.Log("�������� ���� �ȿ� �÷��̾� ����");
            if (GameManager.Ins.Player.inven.AddItemExeptQuickSlot(itemCode))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
