using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;




public class UIPlayer : MonoBehaviour
{
    ActionControl actionControl;

    // elematerMenu ��ü ����
    public GameObject elemanterMenu;

    //ElemanterMenu Ŭ���� ��������
    ElemanterMenu elemanter;

    private void Awake()
    {
        actionControl = new ActionControl();

        elemanter = new ElemanterMenu();
        // UIPlayer �ڽ��� elemanterMenu 
        //elemanterMenu = transform.GetChild(0).gameObject;
        
        // elemanterMenu ��ġ�� uiplayer ��ġ�� �޾ƿ���
        elemanterMenu.transform.position = transform.position;

        elemanter = GetComponent<ElemanterMenu>();
    }

    private void OnEnable()
    {
        actionControl.MouseClickMenu.Enable();
        actionControl.MouseClickMenu.MouesEvent.performed += Onclick;
    }

    private void OnDisable()
    {
        actionControl.MouseClickMenu.MouesEvent.performed -= Onclick;
        actionControl.MouseClickMenu.Disable();
    }

    private void Onclick(InputAction.CallbackContext _)
    {
        elemanterMenu.SetActive(true);
    }


}
