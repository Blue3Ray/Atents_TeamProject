using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase_Thunder : ProjectileBase
{
	protected override void Awake()
	{
		base.Awake();
		elemantalStatus.ChangeType(ElementalType.Thunder);
	}
}
