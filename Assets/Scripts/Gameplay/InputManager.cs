using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // ---- / Singleton / ---- //
    public static InputManager Instance;
    
    // ---- / Public Variables / ---- //
    public Vector2 NavigationInput { get; set; }
    
    public static bool WasEscapePressed;
    public static bool WasGrabOrReleasePressed;
    public static bool WasReleaseAllPressed;
    public static bool WasMousePressed;
    public static bool WasCenterPresed;
    
    // ---- / Private Variables / ---- //
    private static PlayerInput _playerInput;

    private InputAction _navigationAction;
    
    private InputAction _moveAction;

    private InputAction _grabReleaseAction;
    private InputAction _releaseAllAction;
    
    private InputAction _escapeAction;
    private InputAction _pressJump;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }

        _playerInput = GetComponent<PlayerInput>();
        
        _navigationAction = _playerInput.actions["Navigate"];
        
        _moveAction = _playerInput.actions["Move"];

        _grabReleaseAction = _playerInput.actions["GrabRelease"];
        _releaseAllAction = _playerInput.actions["ReleaseAll"];
        
        _escapeAction = _playerInput.actions["Escape"];
    }

    private void Update()
    {
        NavigationInput = _navigationAction.ReadValue<Vector2>();
        
        WasMousePressed = _moveAction.IsPressed();
        
        WasGrabOrReleasePressed = _grabReleaseAction.WasPressedThisFrame();
        WasReleaseAllPressed = _releaseAllAction.WasPressedThisFrame();
        
        WasEscapePressed = _escapeAction.WasPressedThisFrame();
    }
}
