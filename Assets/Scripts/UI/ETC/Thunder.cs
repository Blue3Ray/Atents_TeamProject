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
        Debug.Log("번개 생성");
        testPlayer.element = Element.Thunder;

     }

  
    }
