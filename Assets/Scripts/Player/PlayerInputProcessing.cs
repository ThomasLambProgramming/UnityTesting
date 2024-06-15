using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputProcessor : MonoBehaviour
{
    public static PlayerInputProcessor Instance; 
    [HideInInspector] public bool MouseInputFromController = false;
    public Vector2 CurrentMoveInput => m_currentMoveInput;
    public Vector2 CurrentMouseInput => m_currentMouseInput;
    public Vector2 CurrentZoomInput => m_currentZoomInput;
    
    private Vector2 m_currentMoveInput = Vector2.zero;
    private Vector2 m_currentMouseInput = Vector2.zero;
    private Vector2 m_currentZoomInput;
    
    public PlayerInput m_playerInput;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("There are two player input processors, FIX!");
        }
        SetupInput();
    }

    /// <summary>
    /// Initial Setup for player input requires creating a new input instance.
    /// </summary>
    public void SetupInput()
    {
        m_playerInput = new PlayerInput();
        EnableInput();
    }
    
    /// <summary>
    /// Todo: When menus are available add in optional bool for if it was a menu press so it only disables 
    /// </summary>
    public void EnableInput()
    {
        m_playerInput.Enable();
        SetupCallbacks();
    }
    public void SetupCallbacks()
    {
        m_playerInput.Default.Movement.started += MovementStarted;
        m_playerInput.Default.Movement.performed += MovementPerformed;
        m_playerInput.Default.Movement.canceled += MovementCancelled;
        
        m_playerInput.Default.MoveCamera.started += MoveCameraStarted;
        m_playerInput.Default.MoveCamera.performed += MoveCameraPerformed;
        m_playerInput.Default.MoveCamera.canceled += MoveCameraCancelled;
        
        m_playerInput.Default.ZoomCamera.started += ZoomCameraStarted;
        m_playerInput.Default.ZoomCamera.performed += ZoomCameraPerformed;
        m_playerInput.Default.ZoomCamera.canceled += ZoomCameraCancelled;
    }

    public void RemoveCallbacks()
    {
        m_playerInput.Default.Movement.started -= MovementStarted;
        m_playerInput.Default.Movement.performed -= MovementPerformed;
        m_playerInput.Default.Movement.canceled -= MovementCancelled;
        
        m_playerInput.Default.MoveCamera.started -= MoveCameraStarted;
        m_playerInput.Default.MoveCamera.performed -= MoveCameraPerformed;
        m_playerInput.Default.MoveCamera.canceled -= MoveCameraCancelled;
        
        m_playerInput.Default.ZoomCamera.started -= ZoomCameraStarted;
        m_playerInput.Default.ZoomCamera.performed -= ZoomCameraPerformed;
        m_playerInput.Default.ZoomCamera.canceled -= ZoomCameraCancelled;
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
        m_currentZoomInput = callback.ReadValue<Vector2>();
    }
    public void ZoomCameraPerformed(InputAction.CallbackContext callback)
    {
        m_currentZoomInput = callback.ReadValue<Vector2>();
    }
    public void ZoomCameraCancelled(InputAction.CallbackContext callback)
    {
        m_currentZoomInput = Vector2.zero;
    }
}
