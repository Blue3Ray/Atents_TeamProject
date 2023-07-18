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

        if (Input.GetMouseButtonDown(1))  // 마우스 오른쪽 버튼 누를 시 속성 선택 메뉴 활성화
        {
            elemanterSlot.SetActive(true);
        }
        else if (Input.GetMouseButtonUp(1)) // 마우스 오른쪽 버튼 놓을 시 속성 선택 메뉴 활성화
        {
            
                    elemanterSlot.SetActive(false);
               
           
            
        }
       


    }


}
