using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leveling : MonoBehaviour
{
    TextMeshProUGUI levelIndex;
    Slider slider;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        levelIndex = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(1);
        slider = child.GetComponent<Slider>();
        
    }

    private void Start()
    {
        PlayerJS testPlayer = GameManager.Ins.player;
        testPlayer.onChangeEx += RefreshData;
    }

    public void RefreshData(uint level, int  experience, int experienceMax)
    {
        slider.value = (float) experience / (float) experienceMax;
        levelIndex.text = level.ToString();
    }
}
