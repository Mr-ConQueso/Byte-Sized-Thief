using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static InputManager Instance;
    
    // ---- / Public Variables / ---- //
    public Vector2 NavigationInput { get; set; }
    
    public static float LookHorizontalInput;

    public static Vector2 MoveInput;
    
    public static bool WasEscapePressed;
    public static bool WasInteractPressed;
    
    // ---- / Private Variables / ---- //
    private InputAction _navigationAction;
    private InputAction _lookHorizontalAction;
    
    private InputAction _moveAction;
    
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
        
        _moveAction = _playerInput.actions["Move"];
        
        _interactAction = _playerInput.actions["Interact"];
        _escapeAction = _playerInput.actions["Escape"];
    }

    private void Update()
    {
        NavigationInput = _navigationAction.ReadValue<Vector2>();
        LookHorizontalInput = _lookHorizontalAction.ReadValue<float>();
        
        MoveInput = _moveAction.ReadValue<Vector2>();
        
        WasInteractPressed = _interactAction.WasPressedThisFrame();
        WasEscapePressed = _escapeAction.WasPressedThisFrame();
    }
}
