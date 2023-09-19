using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BarBase : MonoBehaviour
{
    protected Slider slider;
    protected TextMeshProUGUI currentText;
    protected TextMeshProUGUI maxText;

    private void Awake()
    {
        slider = GetComponent<Slider>();
        Transform child = transform.GetChild(2);
        currentText = child.GetComponent<TextMeshProUGUI>();
        child = transform.GetChild(3);
        maxText = child.GetComponent<TextMeshProUGUI>();
    }

    protected virtual void Start()
    {
        
    }

    /// <summary>
    /// 바가 표시할 값이 변경되었을 때 실행될 함수
    /// </summary>
    /// <param name="ratio">변경된 비율</param>
    protected void OnValueChange(float current, float max)
    {
        float ratio = current / max;
        ratio = Mathf.Clamp01(ratio);               // ratio를 0~1로 변경
        slider.value = ratio;                       // 슬라이더 조정
        currentText.text = $"{current:f0}";         // 글자 변경
        maxText.text = $"{max:f0}";
    }
}
