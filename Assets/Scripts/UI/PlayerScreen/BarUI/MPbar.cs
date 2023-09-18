using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPbar : BarBase
{
    void Start()
    {
        GameManager.Ins.player.onMpchange += (ratio) => OnValueChange(ratio);
        maxValue = GameManager.Ins.player.MaxMP;
		max.text = maxValue.ToString();
		OnValueChange(1);
		//Test_PlayerCharater testPlayer = FindObjectOfType<Test_PlayerCharater>();
		//maxValue = testPlayer.MaxMP;
		//max.text = $"/    {maxValue}";
		//current.text = testPlayer.MP.ToString("0");
		//slider.value = testPlayer.MP / maxValue;
		//testPlayer.onHealthChange += OnValueChange;
	}

}
