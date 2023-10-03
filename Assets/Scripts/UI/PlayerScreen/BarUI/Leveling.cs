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
        Transform child = transform.GetChild(1);
        levelIndex = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(0);
        slider = child.GetComponent<Slider>();
    }

    private void Start()
    {
        PlayerJS player = GameManager.Ins.Player;
        RefreshData(player.Level, player.Experience, player.ExperienceMax);
        player.onChangeEx += RefreshData;
    }

    public void RefreshData(uint level, int  experience, int experienceMax)
    {
        slider.value = experience / (float)experienceMax; ;
        levelIndex.text = $"{level}";
    }
}
