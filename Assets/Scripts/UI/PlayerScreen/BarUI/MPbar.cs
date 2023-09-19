using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPbar : BarBase
{
    protected override void Start()
    {
        GameManager.Ins.player.onMpChange += (current, max) => OnValueChange(current, max);
        base.Start();
	}

}
