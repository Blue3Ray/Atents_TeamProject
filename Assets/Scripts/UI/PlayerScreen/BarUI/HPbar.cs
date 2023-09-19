using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : BarBase
{
    protected override void Start()
    {
        GameManager.Ins.player.onHealthChange += (current, max) => OnValueChange(current, max);
        base.Start();
    }

}
