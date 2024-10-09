using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static InputManager Instance;
    
    // ---- / Public Variables / ---- //
    public Vector2 NavigationInput { get; set; }
    
    public static bool WasEscapePressed;
    public static bool WasGrabPressed;
    public static bool WasReleasePressed;
    public static bool WasMousePressed;
    public static bool WasJumpPressed;
    
    // ---- / Private Variables / ---- //
    private static PlayerInput _playerInput;

    private InputAction _navigationAction;
    
    private InputAction _moveAction;

    private InputAction _grabAction;
    private InputAction _releaseAction;
    
    private InputAction _escapeAction;
    private InputAction _jumpAction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        
        _navigationAction = _playerInput.actions["Navigate"];
        
        _moveAction = _playerInput.actions["Move"];
        _jumpAction = _playerInput.actions["Jump"];

        _grabAction = _playerInput.actions["Grab"];
        _releaseAction = _playerInput.actions["Release"];
        
        _escapeAction = _playerInput.actions["Escape"];
    }

    private void Update()
    {
        NavigationInput = _navigationAction.ReadValue<Vector2>();
        
        WasMousePressed = _moveAction.IsPressed();
        
        WasGrabPressed = _grabAction.WasPressedThisFrame();
        WasReleasePressed = _releaseAction.WasPerformedThisFrame();

        WasJumpPressed = _jumpAction.WasPressedThisFrame();
        WasEscapePressed = _escapeAction.WasPressedThisFrame();
    }
}
