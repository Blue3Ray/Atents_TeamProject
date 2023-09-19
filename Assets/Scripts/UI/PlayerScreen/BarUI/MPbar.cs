using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPbar : BarBase
{
    protected override void Start()
    {
        PlayerJS player = GameManager.Ins.player;
        OnValueChange(player.MP, player.MaxMP);
        player.onMpChange += (current, max) => OnValueChange(current, max);
	}

}
