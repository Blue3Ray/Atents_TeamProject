using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillStatusUI : MonoBehaviour
{
    TextMeshProUGUI ename;

    public ElementalType elementalType;

    private void Awake()
    {
        ename = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        PlayerJS testPlayer = GameManager.Ins.player;
        ename.text = testPlayer.PlayerElementalStatus.ToString();
    }


}
