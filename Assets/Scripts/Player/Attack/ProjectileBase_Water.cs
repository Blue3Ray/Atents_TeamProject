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

	protected override void OnAttack(CharacterBase characterTarget)
	{
		MoveOrStop = false;
		base.OnAttack(characterTarget);
		anim.SetTrigger(Hash_Collision);
	}
}
