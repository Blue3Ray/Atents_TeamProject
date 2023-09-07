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

	protected override void OnAttack(Character characterTarget)
	{
		base.OnAttack(characterTarget);
		dirProjectile = Vector3.zero;
		anim.SetTrigger(Hash_Collision);
	}
}
