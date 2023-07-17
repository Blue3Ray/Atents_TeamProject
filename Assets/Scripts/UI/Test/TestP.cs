using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TestP : MonoBehaviour
{
    ActionControl actionControl;

    // elematerMenu ��ü ����
    GameObject elemanterMenu;

    private GameObject fireSelect;
    private GameObject waterSelect;
    private GameObject windSelect;
    private GameObject thunderSelect;

    

    private void Awake()
    {
        actionControl = new ActionControl();

        // UIPlayer �ڽ��� elemanterMenu 
        elemanterMenu = transform.GetChild(0).gameObject;
        elemanterMenu.transform.position = transform.position;

        fireSelect = elemanterMenu.transform.GetChild(0).gameObject;
        waterSelect = elemanterMenu.transform.GetChild(1).gameObject;
        windSelect = elemanterMenu.transform.GetChild(2).gameObject;
        thunderSelect = elemanterMenu.transform.GetChild(3).gameObject;

        

    }

  

    private void Update()
    {
        if(Input.GetMouseButtonDown(0))
        {
            elemanterMenu.SetActive(true);

            
        }
        else if(Input.GetMouseButtonUp(0)) 
        {
            Vector3 mousepostion = Mouse.current.position.ReadValue();
           

            elemanterMenu.SetActive(false); 
        }
    }

  

}
