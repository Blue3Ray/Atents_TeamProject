using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase_Fire : ProjectileBase
{
	protected override void Awake()
	{
		base.Awake();
		elemantalStatus.ChangeType(ElementalType.Fire);
	}

	protected override void OnAttack(Character characterTarget)
	{
		base.OnAttack(characterTarget);
		dirProjectile = Vector3.zero;
	}
}
