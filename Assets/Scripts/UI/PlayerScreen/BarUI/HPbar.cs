using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : BarBase
{
    protected override void Start()
    {
        PlayerJS player = GameManager.Ins.Player;
        OnValueChange(player.HP, player.MaxHP);
        player.onHealthChange += (current, max) => OnValueChange(current, max);
        //base.Start();
    }

}
