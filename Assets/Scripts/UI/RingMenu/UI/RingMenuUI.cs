using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RingMenuUI : MonoBehaviour
{
    ActionControl acionControl;

    // �Ӽ� �޴�
    Transform elemanterMenu;

    // �Ӽ� �޴� �ȿ� �ִ� ����
    Transform[] elemanterSlot;

    //�Ӽ� Ŭ����
    Fire fire;
    Water water;
    Wind wind;
    Thunder thunder;

    TestPlayer testPlayer;

    private void Awake()
    {
        acionControl = new ActionControl();

        // ���޴� �ڽ��� �Ӽ� �޴� ȣ��
        elemanterMenu = transform.GetChild(0);

        // �Ӽ� �޴� �ڽ��� �� �Ӽ� ���� ȣ�� 
        elemanterSlot = new Transform[4];
        elemanterSlot[0] = elemanterMenu.GetChild(0).transform;
        elemanterSlot[1] = elemanterMenu.GetChild(1).transform;
        elemanterSlot[2] = elemanterMenu.GetChild(2).transform;
        elemanterSlot[3] = elemanterMenu.GetChild(3).transform;

        // �ʱⰪ���� �Ӽ� �޴��� ������ �ʴ� ����
        elemanterMenu.gameObject.SetActive(false);

        // �� �Ӽ� Ŭ���� ȣ��
        fire = new Fire();
        water = new Water();
        wind = new Wind();
        thunder = new Thunder();

        testPlayer = FindObjectOfType<TestPlayer>();
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

        if (target.y > elemanterMenu.position.y)
        {
            fire.OnClickBuuton();
            testPlayer.element = Element.Fire;
            elemanterMenu.gameObject.SetActive(false);
        }
        else if (target.x > elemanterMenu.position.x)
        {
            water.OnClickBuuton();
            testPlayer.element = Element.Water;
            elemanterMenu.gameObject.SetActive(false);
        }
        else if (target.y < elemanterMenu.position.y)
        {
            wind.OnClickBuuton();
            testPlayer.element = Element.Wind;
            elemanterMenu.gameObject.SetActive(false);
        }
        else if (target.x < elemanterMenu.position.x)
        {
            thunder.OnClickBuuton();
            testPlayer.element = Element.Thunder;
            elemanterMenu.gameObject.SetActive(false);
        }
        else
        {
            testPlayer.element = Element.None;
            elemanterMenu.gameObject.SetActive(false);
        }


    }
}
