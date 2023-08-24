using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class FireBall : ProjectileBase
{
	protected override void InvokeOnHit(Character targetHit)
	{
		OnHit?.Invoke(targetHit, ElementalType.Fire);
	}
}
