using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileBase_Thunder : ProjectileBase
{
	Thunder_Child_Trigger childTrigger;

	
	protected override void Awake()
	{
		base.Awake();
		elemantalStatus.ChangeType(ElementalType.Thunder);
		childTrigger = transform.GetChild(0).GetComponent<Thunder_Child_Trigger>();
		childTrigger.childTriggerOn += (hitTarget) => OnAttack(hitTarget);
	}

	
}
