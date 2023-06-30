//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.5.1
//     from Assets/Action/ActionControl.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @ActionControl: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @ActionControl()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ActionControl"",
    ""maps"": [
        {
            ""name"": ""ClickAction"",
            ""id"": ""da6c30d1-972d-4ba2-9871-23c675552815"",
            ""actions"": [
                {
                    ""name"": ""Mouse_Left"",
                    ""type"": ""Button"",
                    ""id"": ""aab923d3-f35f-47a6-a0ce-a69259214274"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""52e25fe6-c7f7-4b13-ac1b-567d6681ab44"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""be159371-4559-49da-a3dd-d03ee338a458"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f7aaff1-e66a-4f89-98bb-93080ca48a48"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af4700aa-05dc-48d2-907a-a2de4655feed"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5f0c3895-22ec-4bd7-9208-78af28d211d4"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""d2fff761-4fe0-472d-b604-85a8298c2001"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4214d9b8-bf96-46b0-9701-1dea818c4015"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Click"",
                    ""action"": ""Mouse_Left"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""PlayerTest"",
            ""id"": ""4f01b39a-680a-4ec5-b285-f6f695880b26"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""30d87450-9d18-40a9-8b8c-68fec1a2f884"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""19095b0d-fde3-4e81-867b-324b1b049e1d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""624d1b5e-e331-4e01-8b9f-1da609509f21"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""eb4c3dc2-514b-4ed4-954f-497e3cfc3edb"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""bdbdc7ca-1966-4ec8-af40-9007406e80c1"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""cc9625f5-e4a8-4fd6-9444-19b67cc1718b"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8fae359f-c7ce-450b-9a76-95c847eb74d7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ef1e53de-ae35-4841-b6c8-5b8861ef9a5f"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""MouseClickMenu"",
            ""id"": ""2eaeaca6-2da3-4afd-8531-480979a86e0a"",
            ""actions"": [
                {
                    ""name"": ""MouesEvent"",
                    ""type"": ""Button"",
                    ""id"": ""df7ba992-74ee-4ad8-90a7-a8e9e597f8d4"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""9d526c54-c944-4ef1-88b2-4441c140a108"",
                    ""path"": ""<Mouse>/rightButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Click"",
                    ""action"": ""MouesEvent"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Click"",
            ""bindingGroup"": ""Click"",
            ""devices"": [
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // ClickAction
        m_ClickAction = asset.FindActionMap("ClickAction", throwIfNotFound: true);
        m_ClickAction_Mouse_Left = m_ClickAction.FindAction("Mouse_Left", throwIfNotFound: true);
        m_ClickAction_Attack = m_ClickAction.FindAction("Attack", throwIfNotFound: true);
        // PlayerTest
        m_PlayerTest = asset.FindActionMap("PlayerTest", throwIfNotFound: true);
        m_PlayerTest_Move = m_PlayerTest.FindAction("Move", throwIfNotFound: true);
        m_PlayerTest_Jump = m_PlayerTest.FindAction("Jump", throwIfNotFound: true);
        // MouseClickMenu
        m_MouseClickMenu = asset.FindActionMap("MouseClickMenu", throwIfNotFound: true);
        m_MouseClickMenu_MouesEvent = m_MouseClickMenu.FindAction("MouesEvent", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // ClickAction
    private readonly InputActionMap m_ClickAction;
    private List<IClickActionActions> m_ClickActionActionsCallbackInterfaces = new List<IClickActionActions>();
    private readonly InputAction m_ClickAction_Mouse_Left;
    private readonly InputAction m_ClickAction_Attack;
    public struct ClickActionActions
    {
        private @ActionControl m_Wrapper;
        public ClickActionActions(@ActionControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Mouse_Left => m_Wrapper.m_ClickAction_Mouse_Left;
        public InputAction @Attack => m_Wrapper.m_ClickAction_Attack;
        public InputActionMap Get() { return m_Wrapper.m_ClickAction; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ClickActionActions set) { return set.Get(); }
        public void AddCallbacks(IClickActionActions instance)
        {
            if (instance == null || m_Wrapper.m_ClickActionActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_ClickActionActionsCallbackInterfaces.Add(instance);
            @Mouse_Left.started += instance.OnMouse_Left;
            @Mouse_Left.performed += instance.OnMouse_Left;
            @Mouse_Left.canceled += instance.OnMouse_Left;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
        }

        private void UnregisterCallbacks(IClickActionActions instance)
        {
            @Mouse_Left.started -= instance.OnMouse_Left;
            @Mouse_Left.performed -= instance.OnMouse_Left;
            @Mouse_Left.canceled -= instance.OnMouse_Left;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
        }

        public void RemoveCallbacks(IClickActionActions instance)
        {
            if (m_Wrapper.m_ClickActionActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IClickActionActions instance)
        {
            foreach (var item in m_Wrapper.m_ClickActionActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_ClickActionActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public ClickActionActions @ClickAction => new ClickActionActions(this);

    // PlayerTest
    private readonly InputActionMap m_PlayerTest;
    private List<IPlayerTestActions> m_PlayerTestActionsCallbackInterfaces = new List<IPlayerTestActions>();
    private readonly InputAction m_PlayerTest_Move;
    private readonly InputAction m_PlayerTest_Jump;
    public struct PlayerTestActions
    {
        private @ActionControl m_Wrapper;
        public PlayerTestActions(@ActionControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_PlayerTest_Move;
        public InputAction @Jump => m_Wrapper.m_PlayerTest_Jump;
        public InputActionMap Get() { return m_Wrapper.m_PlayerTest; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerTestActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerTestActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerTestActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerTestActionsCallbackInterfaces.Add(instance);
            @Move.started += instance.OnMove;
            @Move.performed += instance.OnMove;
            @Move.canceled += instance.OnMove;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
        }

        private void UnregisterCallbacks(IPlayerTestActions instance)
        {
            @Move.started -= instance.OnMove;
            @Move.performed -= instance.OnMove;
            @Move.canceled -= instance.OnMove;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
        }

        public void RemoveCallbacks(IPlayerTestActions instance)
        {
            if (m_Wrapper.m_PlayerTestActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerTestActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerTestActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerTestActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerTestActions @PlayerTest => new PlayerTestActions(this);

    // MouseClickMenu
    private readonly InputActionMap m_MouseClickMenu;
    private List<IMouseClickMenuActions> m_MouseClickMenuActionsCallbackInterfaces = new List<IMouseClickMenuActions>();
    private readonly InputAction m_MouseClickMenu_MouesEvent;
    public struct MouseClickMenuActions
    {
        private @ActionControl m_Wrapper;
        public MouseClickMenuActions(@ActionControl wrapper) { m_Wrapper = wrapper; }
        public InputAction @MouesEvent => m_Wrapper.m_MouseClickMenu_MouesEvent;
        public InputActionMap Get() { return m_Wrapper.m_MouseClickMenu; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(MouseClickMenuActions set) { return set.Get(); }
        public void AddCallbacks(IMouseClickMenuActions instance)
        {
            if (instance == null || m_Wrapper.m_MouseClickMenuActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_MouseClickMenuActionsCallbackInterfaces.Add(instance);
            @MouesEvent.started += instance.OnMouesEvent;
            @MouesEvent.performed += instance.OnMouesEvent;
            @MouesEvent.canceled += instance.OnMouesEvent;
        }

        private void UnregisterCallbacks(IMouseClickMenuActions instance)
        {
            @MouesEvent.started -= instance.OnMouesEvent;
            @MouesEvent.performed -= instance.OnMouesEvent;
            @MouesEvent.canceled -= instance.OnMouesEvent;
        }

        public void RemoveCallbacks(IMouseClickMenuActions instance)
        {
            if (m_Wrapper.m_MouseClickMenuActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IMouseClickMenuActions instance)
        {
            foreach (var item in m_Wrapper.m_MouseClickMenuActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_MouseClickMenuActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public MouseClickMenuActions @MouseClickMenu => new MouseClickMenuActions(this);
    private int m_ClickSchemeIndex = -1;
    public InputControlScheme ClickScheme
    {
        get
        {
            if (m_ClickSchemeIndex == -1) m_ClickSchemeIndex = asset.FindControlSchemeIndex("Click");
            return asset.controlSchemes[m_ClickSchemeIndex];
        }
    }
    public interface IClickActionActions
    {
        void OnMouse_Left(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
    }
    public interface IPlayerTestActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
    }
    public interface IMouseClickMenuActions
    {
        void OnMouesEvent(InputAction.CallbackContext context);
    }
}
