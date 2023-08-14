using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MPbar : BarBase
{
    void Start()
    {
        TestPlayer testPlayer = new TestPlayer();
        maxValue = testPlayer.MaxMP;
        max.text = $"/    {maxValue}";
        current.text = testPlayer.MP.ToString("0");
        slider.value = testPlayer.MP / maxValue;
        testPlayer.onHealthChange += OnValueChange;
    }
}
