using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class UIPlayer : MonoBehaviour
{

    public GameObject elemanterSlot;

    private void Awake()
    {
       
    }

    private void Update()
    {

        if (Input.GetMouseButtonDown(1))  // ���콺 ������ ��ư ���� �� �Ӽ� ���� �޴� Ȱ��ȭ
        {
            elemanterSlot.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1)) // ���콺 ������ ��ư ���� �� �Ӽ� ���� �޴� Ȱ��ȭ
        {
            
                    elemanterSlot.SetActive(false);
               
           
            
        }
       


    }


}
