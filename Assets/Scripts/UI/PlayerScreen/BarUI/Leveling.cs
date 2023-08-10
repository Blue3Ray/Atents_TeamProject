using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Leveling : MonoBehaviour
{
    TextMeshProUGUI levelIndex;
    Slider slider;

    float experience;
    float experienceMax ;
    int level = 1;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        levelIndex = child.GetComponent<TextMeshProUGUI>();

        child = transform.GetChild(1);
        slider = child.GetComponent<Slider>();
        
    }
    private void Start()
    {
        slider.value = experience;
        slider.maxValue = experienceMax;

        
    }

    public void LevelUP(float  experience, float experienceMax, int level)
    {
        this.experience = experience;
        this.experienceMax = experienceMax; 
        this.level = level;

        if(this.experience >= this.experienceMax)
        {
            this.level++;
            this.experience = 0.0f;
        
            Debug.Log($"Level up : {level}");
        }


    }
}
