using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HPbar : BarBase
{ 
    void Start()
    {
        TestPlayer testPlayer = new TestPlayer();
        maxValue = testPlayer.MaxHP;
        max.text = $"/    {maxValue}";
        current.text = testPlayer.HP.ToString("0");
        slider.value = testPlayer.HP / maxValue;
        testPlayer.onHealthChange += OnValueChange;
    }

   

}
