using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Water : MonoBehaviour
{
    TestPlayer testPlayer;

    private void Awake()
    {
        testPlayer = FindObjectOfType<TestPlayer>();
    }

    public void OnClickBuuton()
    {
  
        Debug.Log("�� ����");
        testPlayer.element = Element.Water;

    }

    }
