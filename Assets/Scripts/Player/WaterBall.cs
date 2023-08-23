using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterBall : ProjectileBase
{
	protected override void InvokeOnHit(Character targetHit)
	{
		OnHit?.Invoke(targetHit, ElementalType.Water);
	}
}
