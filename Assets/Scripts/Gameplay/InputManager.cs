using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static InputManager Instance;
    
    // ---- / Public Variables / ---- //
    public Vector2 NavigationInput { get; set; }
    
    public static bool WasEscapePressed;
    public static bool WasInteractPressed;
    public static bool WasMousePressed;
    
    // ---- / Private Variables / ---- //
    private static PlayerInput _playerInput;

    private InputAction _navigationAction;
    
    private InputAction _interactAction;
    private InputAction _escapeAction;
    private InputAction _moveAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        
        _navigationAction = _playerInput.actions["Navigate"];
        
        _interactAction = _playerInput.actions["Interact"];
        _escapeAction = _playerInput.actions["Escape"];
        _moveAction = _playerInput.actions["Move"];
    }

    private void Update()
    {
        NavigationInput = _navigationAction.ReadValue<Vector2>();
        
        WasMousePressed = _moveAction.IsPressed();
        WasInteractPressed = _interactAction.WasPressedThisFrame();
        WasEscapePressed = _escapeAction.WasPressedThisFrame();
    }
}
