using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    TestPlayer testPlayer;

    private void Awake()
    {
        testPlayer =FindObjectOfType<TestPlayer>(); 
    }


    public void OnClickBuuton()
    {
        //Input.GetMouseButton(1);
        Debug.Log("�ٶ� ����");

        testPlayer.element = Element.WInd;
      }


    }
