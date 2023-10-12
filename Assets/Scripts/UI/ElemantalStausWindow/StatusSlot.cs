using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusSlot : MonoBehaviour
{
    /// <summary>
    /// �Ӽ� ���� ���� ��ư
    /// </summary>
    Button elemantalButton;

    /// <summary>
    /// �������ͽ� �� ���� ��ü
    /// </summary>
    Transform statusZone;

    /// <summary>
    /// �Ӽ� ���� �ؽ�Ʈ �κ�
    /// </summary>
    TextMeshProUGUI elemantalLevelValue;

    /// <summary>
    /// ���� �Ӽ� ���� ��
    /// </summary>
    int currentLevelValue = 0;

    public ElementalType elementalType;

    private void Awake()
    {
        elemantalButton = transform.GetChild(0).GetComponent<Button>();
        statusZone = transform.GetChild(1);
        elemantalLevelValue = statusZone.GetChild(1).GetComponent<TextMeshProUGUI>();

    }

    private void Start()
    {
        elemantalLevelValue.text = currentLevelValue.ToString();

        elemantalButton.onClick.AddListener(ElemantalLevelUP);

    }

    public void ElemantalLevelUP()
    {
        currentLevelValue++;
        elemantalLevelValue.text = $"{currentLevelValue}";
    }

}
