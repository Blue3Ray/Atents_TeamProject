using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using System;

public class RingMenuUI : MonoBehaviour
{
    TextMeshProUGUI equippedText;


    ActionControl acionControl;

    // �Ӽ� �޴�
    Transform elemanterMenu;

    // �Ӽ� �޴� �ȿ� �ִ� ����
    Transform[] elemanterSlot;

    RingMenuSlot ringslot;

    // ���� Ȯ�ο� ������Ƽ
    public RingMenuSlot RingSlot => ringslot;

    public uint Index => ringslot.Index;

    private void Awake()
    {
        acionControl = new ActionControl();

        // ���޴� �ڽ��� �Ӽ� �޴� ȣ��
        elemanterMenu = transform.GetChild(0);

        // �ʱⰪ���� �Ӽ� �޴��� ������ �ʴ� ����
        elemanterMenu.gameObject.SetActive(false);
    }

    
   
    private void OnEnable()
    {
        acionControl.MouseClickMenu.Enable();
        acionControl.MouseClickMenu.MouesEvent.performed += OnElemanterMenu;
        acionControl.MouseClickMenu.MouesEvent.canceled += OffElemanterMenu;
    }



    private void OnDisable()
    {
        acionControl.MouseClickMenu.MouesEvent.canceled -= OffElemanterMenu;
        acionControl.MouseClickMenu.MouesEvent.performed -= OnElemanterMenu;
        acionControl.MouseClickMenu.Disable();
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    // ���콺 ������ ��ư ���� �� ���콺 ������ ��ġ�� �Ӽ� �޴� Ȱ��ȭ
    private void OnElemanterMenu(InputAction.CallbackContext context)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();

        elemanterMenu.position = mousePos;
        elemanterMenu.gameObject.SetActive(true);

    }

    // �Ӽ� �޴� Ȱ��ȭ �� ���¿��� �Ӽ��޴� ��ġ���� ���콺 ������ ��ư ���� �� �Ӽ� ����
    private void OffElemanterMenu(InputAction.CallbackContext context)
    {
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Vector3 target = context.ReadValue<Vector2>();
        target = mousePos;

    }

   
}
