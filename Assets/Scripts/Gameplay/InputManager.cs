using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static InputManager Instance;
    
    // ---- / Public Variables / ---- //
    public Vector2 NavigationInput { get; set; }
    
    public static float LookHorizontalInput;    
    public static bool WasEscapePressed;
    public static bool WasInteractPressed;
    public static bool WasMousePressed;
    
    // ---- / Private Variables / ---- //
    private InputAction _navigationAction;
    private InputAction _lookHorizontalAction;
    
    private InputAction _interactAction;
    private InputAction _escapeAction;
    
    private static PlayerInput _playerInput;
    private InputAction _move;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        
        _navigationAction = _playerInput.actions["Navigate"];
        _lookHorizontalAction = _playerInput.actions["LookHorizontal"];
        
       
        
        _interactAction = _playerInput.actions["Interact"];
        _escapeAction = _playerInput.actions["Escape"];
        _move = _playerInput.actions["Move"];
    }

    private void Update()
    {
        NavigationInput = _navigationAction.ReadValue<Vector2>();
        LookHorizontalInput = _lookHorizontalAction.ReadValue<float>();
        
        
        WasMousePressed = _move.IsPressed();
        WasInteractPressed = _interactAction.WasPressedThisFrame();
        WasEscapePressed = _escapeAction.WasPressedThisFrame();
    }
}
