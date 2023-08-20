using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HPbar : BarBase
{
    Test_PlayerCharacter testPlayer;


    void Start()
    {
        testPlayer = FindObjectOfType<Test_PlayerCharacter>();
        maxValue = testPlayer.MaxHP;
        max.text = $"  /    {maxValue}";
        current.text = testPlayer.HP.ToString("N0");
        slider.value = testPlayer.HP / maxValue;
        testPlayer.onHealthChange += OnValueChange;
    }

}
