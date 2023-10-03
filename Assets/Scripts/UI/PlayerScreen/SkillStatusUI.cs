using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillStatusUI : MonoBehaviour
{
    Image image;

    public Sprite[] eleSprites;

    public ElementalType elementalType = 0;

    private void Awake()
    {    
        image = GetComponent<Image>();
    }

    private void Start()
    {
        PlayerJS player = GameManager.Ins.Player;
        player.ElemantalStates.onElemantaltypeChange += Refresh;
        //ename.text = player.PlayerElementalStatus.ToString();
    }

    public void Refresh(ElementalType type)
    {
        image.sprite = eleSprites[(int)type];
    }
}
