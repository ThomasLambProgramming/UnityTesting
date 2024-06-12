using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputProcessor : MonoBehaviour
{
    [HideInInspector] public bool MouseInputFromController = false;
    public Vector2 CurrentMoveInput => m_currentMoveInput;
    public Vector2 CurrentMouseInput => m_currentMouseInput;
    public float CurrentZoomInput => m_currentZoomInput;
    
    private Vector2 m_currentMoveInput = Vector2.zero;
    private Vector2 m_currentMouseInput = Vector2.zero;
    private float m_currentZoomInput;
    
    public PlayerInput playerInput;

    /// <summary>
    /// Initial Setup for player input requires creating a new input instance.
    /// </summary>
    public void SetupInput()
    {
        playerInput = new PlayerInput();
        EnableInput();
    }
    
    /// <summary>
    /// Todo: When menus are available add in optional bool for if it was a menu press so it only disables 
    /// </summary>
    public void EnableInput()
    {
        playerInput.Enable();
        SetupCallbacks();
    }
    public void SetupCallbacks()
    {
        playerInput.Default.Movement.started += MovementStarted;
        playerInput.Default.Movement.performed += MovementPerformed;
        playerInput.Default.Movement.canceled += MovementCancelled;
        
        playerInput.Default.MoveCamera.started += MoveCameraStarted;
        playerInput.Default.MoveCamera.performed += MoveCameraPerformed;
        playerInput.Default.MoveCamera.canceled += MoveCameraCancelled;
        
        playerInput.Default.ZoomCamera.started += ZoomCameraStarted;
        playerInput.Default.ZoomCamera.performed += ZoomCameraPerformed;
        playerInput.Default.ZoomCamera.canceled += ZoomCameraCancelled;
    }

    public void RemoveCallbacks()
    {
        playerInput.Default.Movement.started -= MovementStarted;
        playerInput.Default.Movement.performed -= MovementPerformed;
        playerInput.Default.Movement.canceled -= MovementCancelled;
        
        playerInput.Default.MoveCamera.started -= MoveCameraStarted;
        playerInput.Default.MoveCamera.performed -= MoveCameraPerformed;
        playerInput.Default.MoveCamera.canceled -= MoveCameraCancelled;
        
        playerInput.Default.ZoomCamera.started -= ZoomCameraStarted;
        playerInput.Default.ZoomCamera.performed -= ZoomCameraPerformed;
        playerInput.Default.ZoomCamera.canceled -= ZoomCameraCancelled;
    }

    public void MovementStarted(InputAction.CallbackContext callback)
    {
        m_currentMoveInput = callback.ReadValue<Vector2>();
    }
    public void MovementPerformed(InputAction.CallbackContext callback)
    {
        m_currentMoveInput = callback.ReadValue<Vector2>();
    }
    public void MovementCancelled(InputAction.CallbackContext callback)
    {
        m_currentMoveInput = Vector2.zero;
    }
    
    
    private void MoveCameraStarted(InputAction.CallbackContext callbackContext)
    {
        m_currentMouseInput = Vector2.zero;
        MouseInputFromController = callbackContext.control.device is not Mouse;
    }
    private void MoveCameraPerformed(InputAction.CallbackContext callbackContext)
    {
        m_currentMouseInput = callbackContext.ReadValue<Vector2>();
        MouseInputFromController = callbackContext.control.device is not Mouse;
    }
    private void MoveCameraCancelled(InputAction.CallbackContext callbackContext)
    {
        m_currentMouseInput = callbackContext.ReadValue<Vector2>();
    }
    
    
    public void ZoomCameraStarted(InputAction.CallbackContext callback)
    {
        m_currentZoomInput = callback.ReadValue<float>();
    }
    public void ZoomCameraPerformed(InputAction.CallbackContext callback)
    {
        m_currentZoomInput = callback.ReadValue<float>();
    }
    public void ZoomCameraCancelled(InputAction.CallbackContext callback)
    {
        m_currentZoomInput = 0;
    }
}
