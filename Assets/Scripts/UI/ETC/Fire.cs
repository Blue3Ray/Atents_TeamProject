using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fire : MonoBehaviour
{

    TestPlayer testPlayer;

    private void Awake()
    {
        testPlayer = FindObjectOfType<TestPlayer>();
    }

    public void OnClickBuuton()
    {
         //Input.GetMouseButton(1);
         Debug.Log("ºÒ »ý¼º");

        testPlayer.element = Element.Fire;
       
    }

  }


