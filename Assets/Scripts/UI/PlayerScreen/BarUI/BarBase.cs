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
    /// �ٰ� ǥ���� ���� ����Ǿ��� �� ����� �Լ�
    /// </summary>
    /// <param name="ratio">����� ����</param>
    protected void OnValueChange(float current, float max)
    {
        float ratio = current / max;
        ratio = Mathf.Clamp01(ratio);               // ratio�� 0~1�� ����
        slider.value = ratio;                       // �����̴� ����
        currentText.text = $"{current:f0}";         // ���� ����
        maxText.text = $"{max:f0}";
    }
}
