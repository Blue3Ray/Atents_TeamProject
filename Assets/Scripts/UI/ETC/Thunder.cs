using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thunder : MonoBehaviour
{
    TestPlayer testPlayer;

    private void Awake()
    {
        testPlayer = FindObjectOfType<TestPlayer>();
    }

    public void OnClickBuuton()
    {
        //Input.GetMouseButton(1);
        Debug.Log("���� ����");
        testPlayer.element = Element.Thunder;

     }

  
    }
