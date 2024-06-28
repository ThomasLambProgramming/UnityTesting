//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/PlayerInput.inputactions
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

public partial class @PlayerInput: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInput()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInput"",
    ""maps"": [
        {
            ""name"": ""Default"",
            ""id"": ""358da7df-b65e-42da-b550-b510bfd20d45"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""87390103-6f65-4cee-a8a7-a6460af1d373"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Interact"",
                    ""type"": ""Button"",
                    ""id"": ""750e7c08-9304-4144-95ea-4dc60dc93b08"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""SwapMovement"",
                    ""type"": ""Button"",
                    ""id"": ""8e0c8c67-95da-4db5-add1-8642853b65c1"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""0d9d91ef-fcba-42a2-8ef3-141c5af531f5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Attack"",
                    ""type"": ""Button"",
                    ""id"": ""18ef622f-7fa2-4053-8b60-246e75d7dd7f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MoveCamera"",
                    ""type"": ""Value"",
                    ""id"": ""fa75f1ce-fae1-4d8f-878b-2e3a0e1dd9ec"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""ZoomCamera"",
                    ""type"": ""Value"",
                    ""id"": ""fd5cb627-7d2d-4c1e-9dac-b6d0bed2ec1d"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Debug1"",
                    ""type"": ""Button"",
                    ""id"": ""7b9ef180-84ad-4b79-bbde-fc4f96e402a9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Debug2"",
                    ""type"": ""Button"",
                    ""id"": ""369082a9-5b38-4692-9b85-1c3bc7cebb23"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Debug3"",
                    ""type"": ""Button"",
                    ""id"": ""fac79d5f-ab7f-44f4-ad48-ea64c153f9cb"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Debug4"",
                    ""type"": ""Button"",
                    ""id"": ""c81a4b10-3946-47d3-a20d-add9ea1ec918"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Debug5"",
                    ""type"": ""Button"",
                    ""id"": ""e070f000-50f6-4753-b956-53e0e2de3a75"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Pause"",
                    ""type"": ""Button"",
                    ""id"": ""1e6e65b4-6ea5-4ce3-858c-a63445ba5d31"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeMenuTabRight"",
                    ""type"": ""Button"",
                    ""id"": ""c619dca1-c560-438c-a07f-294b7065f6a0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""ChangeMenuTabLeft"",
                    ""type"": ""Button"",
                    ""id"": ""046612e3-d4a9-4913-a7b2-65624ef1b3e7"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MenuAccept"",
                    ""type"": ""Button"",
                    ""id"": ""cd117ee4-b939-4108-86f5-535cde4061e2"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""MenuBack"",
                    ""type"": ""Button"",
                    ""id"": ""34307c39-3c48-4c9a-a5d8-0c1c142d4ffd"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""WASD"",
                    ""id"": ""df425956-eb2e-4238-ba5a-e87fd64fae3a"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""b135045b-4943-454f-ba69-cc1bf1bb3d21"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9d293053-b887-477b-be8c-77570248babd"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""16239a0c-ae43-4a70-8145-f675790f6f79"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""cd388ed8-7e94-47cc-a52c-fa8693884800"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""3c6f350a-3d02-4c10-96ce-c1a625182986"",
                    ""path"": ""<Gamepad>/leftStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7d6b5a7d-9ba9-4d4c-890c-07f81f6e1d17"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4bfb6e1a-34ef-4f86-8cdf-d8add5a50f78"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interact"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""beed4808-c265-432d-83f7-727bb0bff937"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a2e5a52-2dad-44cd-90dc-ae56cfaaafd7"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7706e43c-954b-448c-9555-4f6c1dad1037"",
                    ""path"": ""<Mouse>/leftButton"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""633b05fc-12f9-4c94-97cb-efae3c76cd9f"",
                    ""path"": ""<Gamepad>/rightTrigger"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Attack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7744829b-95f9-4790-b60d-bca01d55cb38"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""2cbd3317-c24a-4654-966b-8fd9d4fc66b1"",
                    ""path"": ""<Gamepad>/rightStick"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MoveCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f783ccfa-7a29-46c6-8c1d-13cd5063a477"",
                    ""path"": ""<Mouse>/scroll"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ZoomCamera"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""b969ad38-1a23-4990-8b71-82747067a46d"",
                    ""path"": ""<Keyboard>/1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Debug1"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a1ce8566-412a-48d4-8843-b40e917adb04"",
                    ""path"": ""<Keyboard>/2"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Debug2"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0a9ad8ac-abd0-44fb-babd-80e7d66e66dc"",
                    ""path"": ""<Keyboard>/3"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Debug3"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a6de36f1-bbb9-4740-8ea9-9805e9cdd84e"",
                    ""path"": ""<Keyboard>/4"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Debug4"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""83c9a11e-fc35-4426-bb13-c5ebaa48bffd"",
                    ""path"": ""<Keyboard>/5"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Debug5"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a4098b38-6879-4307-b748-44f65253f0b3"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""885013e2-f066-4902-aa56-41d1ee120eb8"",
                    ""path"": ""<Gamepad>/start"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Pause"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""49d3de31-3d91-4c33-97d2-8f6bc87bd4f0"",
                    ""path"": ""<Keyboard>/f"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwapMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5c838441-4e12-413d-a3f4-f4c22f374d44"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""SwapMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""60a7fff7-1406-451d-aaf1-44eb678aafaa"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeMenuTabRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""57914e8d-69ed-4f66-9e77-0479ae819c87"",
                    ""path"": ""<Gamepad>/rightShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeMenuTabRight"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""835d3588-89de-4814-a939-9b03a10a3c09"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeMenuTabLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""4261d16a-ed30-423b-bdb5-9a9a514033bb"",
                    ""path"": ""<Gamepad>/leftShoulder"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ChangeMenuTabLeft"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3b5a7372-2af1-44e3-a21a-50ff9ca12aad"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuAccept"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""9a853ca8-7a6f-4fee-b1fb-5822a5216597"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuAccept"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""11b86b46-dfb1-4dc1-b709-f414e509f9e4"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuBack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""13baf7cb-e380-42c7-b7d3-6322b6c8d606"",
                    ""path"": ""<Keyboard>/backspace"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MenuBack"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Default
        m_Default = asset.FindActionMap("Default", throwIfNotFound: true);
        m_Default_Movement = m_Default.FindAction("Movement", throwIfNotFound: true);
        m_Default_Interact = m_Default.FindAction("Interact", throwIfNotFound: true);
        m_Default_SwapMovement = m_Default.FindAction("SwapMovement", throwIfNotFound: true);
        m_Default_Jump = m_Default.FindAction("Jump", throwIfNotFound: true);
        m_Default_Attack = m_Default.FindAction("Attack", throwIfNotFound: true);
        m_Default_MoveCamera = m_Default.FindAction("MoveCamera", throwIfNotFound: true);
        m_Default_ZoomCamera = m_Default.FindAction("ZoomCamera", throwIfNotFound: true);
        m_Default_Debug1 = m_Default.FindAction("Debug1", throwIfNotFound: true);
        m_Default_Debug2 = m_Default.FindAction("Debug2", throwIfNotFound: true);
        m_Default_Debug3 = m_Default.FindAction("Debug3", throwIfNotFound: true);
        m_Default_Debug4 = m_Default.FindAction("Debug4", throwIfNotFound: true);
        m_Default_Debug5 = m_Default.FindAction("Debug5", throwIfNotFound: true);
        m_Default_Pause = m_Default.FindAction("Pause", throwIfNotFound: true);
        m_Default_ChangeMenuTabRight = m_Default.FindAction("ChangeMenuTabRight", throwIfNotFound: true);
        m_Default_ChangeMenuTabLeft = m_Default.FindAction("ChangeMenuTabLeft", throwIfNotFound: true);
        m_Default_MenuAccept = m_Default.FindAction("MenuAccept", throwIfNotFound: true);
        m_Default_MenuBack = m_Default.FindAction("MenuBack", throwIfNotFound: true);
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

    // Default
    private readonly InputActionMap m_Default;
    private List<IDefaultActions> m_DefaultActionsCallbackInterfaces = new List<IDefaultActions>();
    private readonly InputAction m_Default_Movement;
    private readonly InputAction m_Default_Interact;
    private readonly InputAction m_Default_SwapMovement;
    private readonly InputAction m_Default_Jump;
    private readonly InputAction m_Default_Attack;
    private readonly InputAction m_Default_MoveCamera;
    private readonly InputAction m_Default_ZoomCamera;
    private readonly InputAction m_Default_Debug1;
    private readonly InputAction m_Default_Debug2;
    private readonly InputAction m_Default_Debug3;
    private readonly InputAction m_Default_Debug4;
    private readonly InputAction m_Default_Debug5;
    private readonly InputAction m_Default_Pause;
    private readonly InputAction m_Default_ChangeMenuTabRight;
    private readonly InputAction m_Default_ChangeMenuTabLeft;
    private readonly InputAction m_Default_MenuAccept;
    private readonly InputAction m_Default_MenuBack;
    public struct DefaultActions
    {
        private @PlayerInput m_Wrapper;
        public DefaultActions(@PlayerInput wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Default_Movement;
        public InputAction @Interact => m_Wrapper.m_Default_Interact;
        public InputAction @SwapMovement => m_Wrapper.m_Default_SwapMovement;
        public InputAction @Jump => m_Wrapper.m_Default_Jump;
        public InputAction @Attack => m_Wrapper.m_Default_Attack;
        public InputAction @MoveCamera => m_Wrapper.m_Default_MoveCamera;
        public InputAction @ZoomCamera => m_Wrapper.m_Default_ZoomCamera;
        public InputAction @Debug1 => m_Wrapper.m_Default_Debug1;
        public InputAction @Debug2 => m_Wrapper.m_Default_Debug2;
        public InputAction @Debug3 => m_Wrapper.m_Default_Debug3;
        public InputAction @Debug4 => m_Wrapper.m_Default_Debug4;
        public InputAction @Debug5 => m_Wrapper.m_Default_Debug5;
        public InputAction @Pause => m_Wrapper.m_Default_Pause;
        public InputAction @ChangeMenuTabRight => m_Wrapper.m_Default_ChangeMenuTabRight;
        public InputAction @ChangeMenuTabLeft => m_Wrapper.m_Default_ChangeMenuTabLeft;
        public InputAction @MenuAccept => m_Wrapper.m_Default_MenuAccept;
        public InputAction @MenuBack => m_Wrapper.m_Default_MenuBack;
        public InputActionMap Get() { return m_Wrapper.m_Default; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(DefaultActions set) { return set.Get(); }
        public void AddCallbacks(IDefaultActions instance)
        {
            if (instance == null || m_Wrapper.m_DefaultActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_DefaultActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Interact.started += instance.OnInteract;
            @Interact.performed += instance.OnInteract;
            @Interact.canceled += instance.OnInteract;
            @SwapMovement.started += instance.OnSwapMovement;
            @SwapMovement.performed += instance.OnSwapMovement;
            @SwapMovement.canceled += instance.OnSwapMovement;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Attack.started += instance.OnAttack;
            @Attack.performed += instance.OnAttack;
            @Attack.canceled += instance.OnAttack;
            @MoveCamera.started += instance.OnMoveCamera;
            @MoveCamera.performed += instance.OnMoveCamera;
            @MoveCamera.canceled += instance.OnMoveCamera;
            @ZoomCamera.started += instance.OnZoomCamera;
            @ZoomCamera.performed += instance.OnZoomCamera;
            @ZoomCamera.canceled += instance.OnZoomCamera;
            @Debug1.started += instance.OnDebug1;
            @Debug1.performed += instance.OnDebug1;
            @Debug1.canceled += instance.OnDebug1;
            @Debug2.started += instance.OnDebug2;
            @Debug2.performed += instance.OnDebug2;
            @Debug2.canceled += instance.OnDebug2;
            @Debug3.started += instance.OnDebug3;
            @Debug3.performed += instance.OnDebug3;
            @Debug3.canceled += instance.OnDebug3;
            @Debug4.started += instance.OnDebug4;
            @Debug4.performed += instance.OnDebug4;
            @Debug4.canceled += instance.OnDebug4;
            @Debug5.started += instance.OnDebug5;
            @Debug5.performed += instance.OnDebug5;
            @Debug5.canceled += instance.OnDebug5;
            @Pause.started += instance.OnPause;
            @Pause.performed += instance.OnPause;
            @Pause.canceled += instance.OnPause;
            @ChangeMenuTabRight.started += instance.OnChangeMenuTabRight;
            @ChangeMenuTabRight.performed += instance.OnChangeMenuTabRight;
            @ChangeMenuTabRight.canceled += instance.OnChangeMenuTabRight;
            @ChangeMenuTabLeft.started += instance.OnChangeMenuTabLeft;
            @ChangeMenuTabLeft.performed += instance.OnChangeMenuTabLeft;
            @ChangeMenuTabLeft.canceled += instance.OnChangeMenuTabLeft;
            @MenuAccept.started += instance.OnMenuAccept;
            @MenuAccept.performed += instance.OnMenuAccept;
            @MenuAccept.canceled += instance.OnMenuAccept;
            @MenuBack.started += instance.OnMenuBack;
            @MenuBack.performed += instance.OnMenuBack;
            @MenuBack.canceled += instance.OnMenuBack;
        }

        private void UnregisterCallbacks(IDefaultActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Interact.started -= instance.OnInteract;
            @Interact.performed -= instance.OnInteract;
            @Interact.canceled -= instance.OnInteract;
            @SwapMovement.started -= instance.OnSwapMovement;
            @SwapMovement.performed -= instance.OnSwapMovement;
            @SwapMovement.canceled -= instance.OnSwapMovement;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Attack.started -= instance.OnAttack;
            @Attack.performed -= instance.OnAttack;
            @Attack.canceled -= instance.OnAttack;
            @MoveCamera.started -= instance.OnMoveCamera;
            @MoveCamera.performed -= instance.OnMoveCamera;
            @MoveCamera.canceled -= instance.OnMoveCamera;
            @ZoomCamera.started -= instance.OnZoomCamera;
            @ZoomCamera.performed -= instance.OnZoomCamera;
            @ZoomCamera.canceled -= instance.OnZoomCamera;
            @Debug1.started -= instance.OnDebug1;
            @Debug1.performed -= instance.OnDebug1;
            @Debug1.canceled -= instance.OnDebug1;
            @Debug2.started -= instance.OnDebug2;
            @Debug2.performed -= instance.OnDebug2;
            @Debug2.canceled -= instance.OnDebug2;
            @Debug3.started -= instance.OnDebug3;
            @Debug3.performed -= instance.OnDebug3;
            @Debug3.canceled -= instance.OnDebug3;
            @Debug4.started -= instance.OnDebug4;
            @Debug4.performed -= instance.OnDebug4;
            @Debug4.canceled -= instance.OnDebug4;
            @Debug5.started -= instance.OnDebug5;
            @Debug5.performed -= instance.OnDebug5;
            @Debug5.canceled -= instance.OnDebug5;
            @Pause.started -= instance.OnPause;
            @Pause.performed -= instance.OnPause;
            @Pause.canceled -= instance.OnPause;
            @ChangeMenuTabRight.started -= instance.OnChangeMenuTabRight;
            @ChangeMenuTabRight.performed -= instance.OnChangeMenuTabRight;
            @ChangeMenuTabRight.canceled -= instance.OnChangeMenuTabRight;
            @ChangeMenuTabLeft.started -= instance.OnChangeMenuTabLeft;
            @ChangeMenuTabLeft.performed -= instance.OnChangeMenuTabLeft;
            @ChangeMenuTabLeft.canceled -= instance.OnChangeMenuTabLeft;
            @MenuAccept.started -= instance.OnMenuAccept;
            @MenuAccept.performed -= instance.OnMenuAccept;
            @MenuAccept.canceled -= instance.OnMenuAccept;
            @MenuBack.started -= instance.OnMenuBack;
            @MenuBack.performed -= instance.OnMenuBack;
            @MenuBack.canceled -= instance.OnMenuBack;
        }

        public void RemoveCallbacks(IDefaultActions instance)
        {
            if (m_Wrapper.m_DefaultActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IDefaultActions instance)
        {
            foreach (var item in m_Wrapper.m_DefaultActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_DefaultActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public DefaultActions @Default => new DefaultActions(this);
    public interface IDefaultActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnInteract(InputAction.CallbackContext context);
        void OnSwapMovement(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnAttack(InputAction.CallbackContext context);
        void OnMoveCamera(InputAction.CallbackContext context);
        void OnZoomCamera(InputAction.CallbackContext context);
        void OnDebug1(InputAction.CallbackContext context);
        void OnDebug2(InputAction.CallbackContext context);
        void OnDebug3(InputAction.CallbackContext context);
        void OnDebug4(InputAction.CallbackContext context);
        void OnDebug5(InputAction.CallbackContext context);
        void OnPause(InputAction.CallbackContext context);
        void OnChangeMenuTabRight(InputAction.CallbackContext context);
        void OnChangeMenuTabLeft(InputAction.CallbackContext context);
        void OnMenuAccept(InputAction.CallbackContext context);
        void OnMenuBack(InputAction.CallbackContext context);
    }
}
