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
        
    }
}
