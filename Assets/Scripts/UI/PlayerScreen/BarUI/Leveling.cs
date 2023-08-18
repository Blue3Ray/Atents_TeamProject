using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leveling : MonoBehaviour
{
    TextMeshProUGUI levelIndex;
    Slider slider;
   Test_PlayerCharater testPlayer;

    private void Awake()
    {
        testPlayer = FindObjectOfType<Test_PlayerCharater>();

        Transform child = transform.GetChild(0);
        levelIndex = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(1);
        slider = child.GetComponent<Slider>();
        
    }
    private void Start()
    {
        
    }

    public void RefreshData(float  experience, float experienceMax, int level)
    {
        slider.value = experience;
        slider.maxValue = experienceMax;
        levelIndex.text = level.ToString();
    }
}
