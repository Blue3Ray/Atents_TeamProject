using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldUI : MonoBehaviour
{
    TextMeshProUGUI goldIndex;

    private void Awake()
    {
        Transform child = transform.GetChild(0);
        goldIndex = child.GetComponent<TextMeshProUGUI>();
    }

    public void RefreshData(int gold)
    {
        
        goldIndex.text = gold.ToString();
    }
}
