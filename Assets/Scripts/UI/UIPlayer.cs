using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class UIPlayer : MonoBehaviour
{
    ActionControl actionControl;

    // elematerMenu ��ü ����
    GameObject elemanterMenu;

    //ElemanterMenu Ŭ���� ��������
    ElemanterMenu elemanter;

    private void Awake()
    {
        // UIPlayer �ڽ��� elemanterMenu 
        elemanterMenu = transform.GetChild(0).gameObject;
        
        // elemanterMenu ��ġ�� uiplayer ��ġ�� �޾ƿ���
        elemanterMenu.transform.position = transform.position;

        elemanter = GetComponent<ElemanterMenu>();
    }

    //private void OnEnable()
    //{
    //    actionControl.MouseClickMenu.MouesEvent.Enable();
    //    actionControl.MouseClickMenu.MouesEvent.performed += Onclick;
    //}

    //private void OnDisable()
    //{
    //    actionControl.MouseClickMenu.MouesEvent.performed -= Onclick;
    //    actionControl.MouseClickMenu.MouesEvent.Disable();
    //}

    //private void Onclick(InputAction.CallbackContext _)
    //{
    //    elemanterMenu.SetActive(true);
    //}


}
