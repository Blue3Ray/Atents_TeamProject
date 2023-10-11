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
	//		Debug.Log("아이템의 범위 안에 플레이어 들어옴");
			
	//	}
	//}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
		{
            Debug.Log("아이템의 범위 안에 플레이어 들어옴");
            if (GameManager.Ins.Player.inven.AddItemExeptQuickSlot(itemCode))
            {
                Destroy(this.gameObject);
            }
        }
    }
}
