using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffector : MonoBehaviour
{

	Animator anim;

	readonly int Hash_LevelUp = Animator.StringToHash("LevelUp");


	private void Awake()
	{
		anim = GetComponent<Animator>();
	}
	private void Start()
	{
		PlayerJS player = transform.parent.GetComponent<PlayerJS>();
		player.onLevelUP += (_) => { OnEffectLevelUp(); };
	}

	private void OnEffectLevelUp()
	{
		anim.SetTrigger(Hash_LevelUp);
	}
}
