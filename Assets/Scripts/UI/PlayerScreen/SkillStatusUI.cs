using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillStatusUI : MonoBehaviour
{
    TextMeshProUGUI enameText;
    Image image;

    public Sprite[] eleSprites;

    public ElementalType elementalType = 0;

    private void Awake()
    {
        enameText = GetComponentInChildren<TextMeshProUGUI>();
        image = GetComponent<Image>();
    }

    private void Start()
    {
        PlayerJS player = GameManager.Ins.player;
        player.ElemantalStates.onElemantaltypeChange += Refresh;
        //ename.text = player.PlayerElementalStatus.ToString();
    }

    public void Refresh(ElementalType type)
    {
        enameText.text = type.ToString();
        image.sprite = eleSprites[(int)type];
    }
}
