using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StatusSlot : MonoBehaviour
{
    /// <summary>
    /// 속성 레벨 증가 버튼
    /// </summary>
    Button elemantalButton;

    /// <summary>
    /// 스테이터스 존 구역 객체
    /// </summary>
    Transform statusZone;

    /// <summary>
    /// 속성 레벨 텍스트 부분
    /// </summary>
    TextMeshProUGUI elemantalLevelValue;

    /// <summary>
    /// 현재 속성 레벱 값
    /// </summary>
    int currentLevelValue = 0;

    /// <summary>
    /// 속성 공격력
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
        elemantalLevelValue.text = currentLevelValue.ToString();
        elemantalAttackValue.text = player.elemantalAttack.ToString();
        elemantalButton.onClick.AddListener(ElemantalLevelUP);
    }

    public void ElemantalLevelUP()
    {
        PlayerJS player = GameManager.Ins.Player;
        currentLevelValue++;
        player.elemantalAttack = player.elemantalAttack + 10.0f;
        elemantalLevelValue.text = $"{currentLevelValue}";
        elemantalAttackValue.text = $"{player.elemantalAttack}";
    }

}
