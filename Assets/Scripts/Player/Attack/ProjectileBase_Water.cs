using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase_Water : ProjectileBase
{


	protected override void Awake()
	{
		base.Awake();
		elemantalStatus.ChangeType(ElementalType.Water);
	}


	protected override void OnTriggerEnter2D(Collider2D collision)
	{
		if (elementalType == ElementalType.Water && collision.CompareTag("Ground"))
		{
			anim.SetTrigger(Hash_Collision);
			dirProjectile = Vector3.zero;
		}
		base.OnTriggerEnter2D(collision);
	}
}
