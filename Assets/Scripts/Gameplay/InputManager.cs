using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static InputManager Instance;
    
    // ---- / Public Variables / ---- //
    public Vector2 NavigationInput { get; set; }
    
    public static float LookHorizontalInput;
    public static float LookVerticalInput;

    public static bool WasAttackPressed;
    public static bool WasAimPressed;
    public static bool WasAimReleased;
    
    public static bool WasNextWeaponPressed;
    public static bool WasPreviousWeaponPressed;
    
    public static bool WasEscapePressed;
    public static bool WasInteractPressed;
    
    // ---- / Private Variables / ---- //
    private InputAction _navigationAction;
    private InputAction _lookHorizontalAction;
    private InputAction _lookVerticalAction;
    
    private InputAction _attackAction;
    private InputAction _aimAction;
    
    private InputAction _nextWeaponAction;
    private InputAction _previousWeaponAction;
    
    private InputAction _interactAction;
    private InputAction _escapeAction;
    
    private static PlayerInput _playerInput;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        
        _navigationAction = _playerInput.actions["Navigate"];
        _lookHorizontalAction = _playerInput.actions["LookHorizontal"];
        _lookVerticalAction = _playerInput.actions["LookVertical"];
        
        _attackAction = _playerInput.actions["Fire"];
        _aimAction = _playerInput.actions["Aim"];
        
        _nextWeaponAction = _playerInput.actions["SwitchNextWeapon"];
        _previousWeaponAction = _playerInput.actions["SwitchPreviousWeapon"];
        
        _interactAction = _playerInput.actions["Interact"];
        _escapeAction = _playerInput.actions["Escape"];
    }

    private void Update()
    {
        NavigationInput = _navigationAction.ReadValue<Vector2>();
        LookHorizontalInput = _lookHorizontalAction.ReadValue<float>();
        LookVerticalInput = _lookVerticalAction.ReadValue<float>();

        WasAttackPressed = _attackAction.WasPressedThisFrame();
        WasAimPressed = _aimAction.WasPressedThisFrame();
        WasAimReleased = _aimAction.WasReleasedThisFrame();

        WasNextWeaponPressed = _nextWeaponAction.WasPressedThisFrame();
        WasPreviousWeaponPressed = _previousWeaponAction.WasPressedThisFrame();
        
        WasInteractPressed = _interactAction.WasPressedThisFrame();
        WasEscapePressed = _escapeAction.WasPressedThisFrame();
    }
}
