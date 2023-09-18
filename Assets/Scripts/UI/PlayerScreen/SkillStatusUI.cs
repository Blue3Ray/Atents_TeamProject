using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class SkillStatusUI : MonoBehaviour
{
    TextMeshProUGUI ename;

  

    public ElementalType elementalType = 0;

    private void Awake()
    {
        ename = GetComponent<TextMeshProUGUI>();
        
    }

    private void Start()
    {
        PlayerJS testplayer = GameManager.Ins.player;
        ename.text = testplayer.PlayerElementalStatus.ToString();
    
    }

}
