using System;
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

    public int CurrentLevelValue
    {
        get => currentLevelValue;
        set
        {
            currentLevelValue = value;
            onChangeCurrentLevelValue?.Invoke(currentLevelValue);
        }
     }
    public Action<int> onChangeCurrentLevelValue { get; set; }

    /// <summary>
    /// �Ӽ� ���ݷ�
    /// </summary>
    TextMeshProUGUI elemantalAttackValue;

    public ElementalType elementalType;

    ElemantalStates elemantalStates;

    private void Awake()
    {
        elemantalStates = new ElemantalStates();
        elemantalButton = transform.GetChild(0).GetComponent<Button>();
        statusZone = transform.GetChild(1);
        elemantalLevelValue = statusZone.GetChild(1).GetComponent<TextMeshProUGUI>();
        elemantalAttackValue = statusZone.GetChild(3).GetComponent<TextMeshProUGUI>();
        

    }

    private void Start()
    {
        PlayerJS player = GameManager.Ins.Player;
        elemantalLevelValue.text = CurrentLevelValue.ToString();
        elemantalAttackValue.text = player.elemantalAttack.ToString();
        elemantalButton.onClick.AddListener(ElemantalLevelUP);
    }

    public void ElemantalLevelUP()
    {
        PlayerJS player = GameManager.Ins.Player;
        CurrentLevelValue++;
        player.elemantalAttack = player.elemantalAttack + 10.0f;
        elemantalLevelValue.text = $"{CurrentLevelValue}";
        elemantalAttackValue.text = $"{player.elemantalAttack}";
        player.SkillStet--;
    }

}
