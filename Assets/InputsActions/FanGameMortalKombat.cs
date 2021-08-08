// GENERATED AUTOMATICALLY FROM 'Assets/InputsActions/FanGameMortalKombat.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @FanGameMortalKombat : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @FanGameMortalKombat()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""FanGameMortalKombat"",
    ""maps"": [
        {
            ""name"": ""PlayerMovement"",
            ""id"": ""b75f81d7-e94a-46dc-810b-718d5d653af5"",
            ""actions"": [
                {
                    ""name"": ""MovementControllers"",
                    ""type"": ""Value"",
                    ""id"": ""6aefe311-c9cc-44ae-90ab-859cc9ed2c58"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""CameraMovement"",
                    ""type"": ""Value"",
                    ""id"": ""530395bd-3399-4ea5-a78f-d28de2dd1d88"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ButtonsToAction"",
                    ""type"": ""Button"",
                    ""id"": ""496749ed-d84d-45dd-a865-dd726c2f6704"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press(behavior=2)""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""2D Vector KeyBoardAWSD"",
                    ""id"": ""e59b8392-ea84-4085-8bd4-daa5ea8083f1"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""7f3ad06c-33c5-4d5f-ad5c-21d8c85fcf89"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""a2972a41-479b-4dcd-bdeb-f63d5129d049"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b5532d8f-ae23-4017-8bf2-9fcf091dfcac"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""5a212008-6cf9-4cfe-9451-89aee05b8a95"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector KeyBoard Arrows"",
                    ""id"": ""eb1a1dee-cffb-4d22-8803-025985486506"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""2a7b5cca-34a0-48cb-a286-089c09194aa5"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""f035fa09-2524-4133-8890-91f83bf10c8b"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""86d6a1f8-f1c8-4393-8ee9-04ea091f71a5"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""76ca750b-9f1c-45ac-bd46-f3a433994607"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector Control"",
                    ""id"": ""b972a2ed-e0c7-432d-acbf-0272450290ff"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""abf75051-521c-4ce1-b804-b548ad547e72"",
                    ""path"": ""<Joystick>/stick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""72300149-c71d-4a36-a6cb-ebb12ed218e7"",
                    ""path"": ""<Joystick>/stick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""48e3f3ec-acf8-402d-8291-f5a4215f2b59"",
                    ""path"": ""<Joystick>/stick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""83172c39-a0fa-4bf3-a705-16ef8077dee2"",
                    ""path"": ""<Joystick>/stick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""MovementControllers"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector Control"",
                    ""id"": ""1ea4354d-e8de-42f1-a80b-28db36e49980"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""caddf4d7-bd9d-476a-8278-6fd7d8f4582b"",
                    ""path"": ""<HID::DragonRise Inc.   Generic   USB  Joystick  >/hat/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Joystick;Gamepad"",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""66046792-8f75-483d-b24f-f82f61e1e58a"",
                    ""path"": ""<HID::DragonRise Inc.   Generic   USB  Joystick  >/hat/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""1e6754ab-c0e8-4e9c-a9f9-e2adac1ad9e2"",
                    ""path"": ""<HID::DragonRise Inc.   Generic   USB  Joystick  >/hat/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""75bd2325-d246-4e09-b37b-48ae15fe5505"",
                    ""path"": ""<HID::DragonRise Inc.   Generic   USB  Joystick  >/hat/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad;Joystick"",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector KeyBoard"",
                    ""id"": ""eb9b1ba4-697b-4d47-bd04-d95f00f3fbaa"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""53bba0f9-6655-4768-bef0-dca9e6e405a9"",
                    ""path"": ""<Keyboard>/i"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""5f6a4ed1-87f9-4580-af3b-f7b61327e7d5"",
                    ""path"": ""<Keyboard>/k"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""f0cd0167-a44f-4af6-8d58-fb0534425216"",
                    ""path"": ""<Keyboard>/j"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""55f88a31-e4b2-4deb-891c-be27c939993d"",
                    ""path"": ""<Keyboard>/l"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""2D Vector stickRight"",
                    ""id"": ""95fdf5fb-9c0a-4404-bf02-d692a8e85687"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""86d16e93-c7bd-460f-a4ea-d8accaa1ebf9"",
                    ""path"": ""<HID::DragonRise Inc.   Generic   USB  Joystick  >/rz"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""af6eb5d2-85e7-49f2-ad70-1722673cb732"",
                    ""path"": ""<HID::DragonRise Inc.   Generic   USB  Joystick  >/rz"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""b9e0e13c-1d50-45dc-a3c3-8cd0307c882c"",
                    ""path"": ""<HID::DragonRise Inc.   Generic   USB  Joystick  >/z1"",
                    ""interactions"": """",
                    ""processors"": ""Invert"",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""d4621495-87f1-4ee4-8821-9a01e2263dad"",
                    ""path"": ""<HID::DragonRise Inc.   Generic   USB  Joystick  >/z1"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CameraMovement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""ad4f9e66-c240-417f-b627-65c822e4ae94"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ButtonsToAction"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // PlayerMovement
        m_PlayerMovement = asset.FindActionMap("PlayerMovement", throwIfNotFound: true);
        m_PlayerMovement_MovementControllers = m_PlayerMovement.FindAction("MovementControllers", throwIfNotFound: true);
        m_PlayerMovement_CameraMovement = m_PlayerMovement.FindAction("CameraMovement", throwIfNotFound: true);
        m_PlayerMovement_ButtonsToAction = m_PlayerMovement.FindAction("ButtonsToAction", throwIfNotFound: true);
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

    // PlayerMovement
    private readonly InputActionMap m_PlayerMovement;
    private IPlayerMovementActions m_PlayerMovementActionsCallbackInterface;
    private readonly InputAction m_PlayerMovement_MovementControllers;
    private readonly InputAction m_PlayerMovement_CameraMovement;
    private readonly InputAction m_PlayerMovement_ButtonsToAction;
    public struct PlayerMovementActions
    {
        private @FanGameMortalKombat m_Wrapper;
        public PlayerMovementActions(@FanGameMortalKombat wrapper) { m_Wrapper = wrapper; }
        public InputAction @MovementControllers => m_Wrapper.m_PlayerMovement_MovementControllers;
        public InputAction @CameraMovement => m_Wrapper.m_PlayerMovement_CameraMovement;
        public InputAction @ButtonsToAction => m_Wrapper.m_PlayerMovement_ButtonsToAction;
        public InputActionMap Get() { return m_Wrapper.m_PlayerMovement; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerMovementActions set) { return set.Get(); }
        public void SetCallbacks(IPlayerMovementActions instance)
        {
            if (m_Wrapper.m_PlayerMovementActionsCallbackInterface != null)
            {
                @MovementControllers.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovementControllers;
                @MovementControllers.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovementControllers;
                @MovementControllers.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnMovementControllers;
                @CameraMovement.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraMovement;
                @CameraMovement.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraMovement;
                @CameraMovement.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnCameraMovement;
                @ButtonsToAction.started -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnButtonsToAction;
                @ButtonsToAction.performed -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnButtonsToAction;
                @ButtonsToAction.canceled -= m_Wrapper.m_PlayerMovementActionsCallbackInterface.OnButtonsToAction;
            }
            m_Wrapper.m_PlayerMovementActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MovementControllers.started += instance.OnMovementControllers;
                @MovementControllers.performed += instance.OnMovementControllers;
                @MovementControllers.canceled += instance.OnMovementControllers;
                @CameraMovement.started += instance.OnCameraMovement;
                @CameraMovement.performed += instance.OnCameraMovement;
                @CameraMovement.canceled += instance.OnCameraMovement;
                @ButtonsToAction.started += instance.OnButtonsToAction;
                @ButtonsToAction.performed += instance.OnButtonsToAction;
                @ButtonsToAction.canceled += instance.OnButtonsToAction;
            }
        }
    }
    public PlayerMovementActions @PlayerMovement => new PlayerMovementActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IPlayerMovementActions
    {
        void OnMovementControllers(InputAction.CallbackContext context);
        void OnCameraMovement(InputAction.CallbackContext context);
        void OnButtonsToAction(InputAction.CallbackContext context);
    }
}
