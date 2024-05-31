using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputProcessor : MonoBehaviour
{
    /// <summary>
    /// Used this struct as a way to press down key, allow the check for pressed this frame and to be
    /// checked that the player isn't being held down
    /// </summary>
    public struct BaseInput
    {
        public float RawInputValue;
        public bool ConsumedValue;
        public bool HoldingInput;

        public bool CheckAndConsumeInput()
        {
            if (ConsumedValue == false && RawInputValue > 0.1f)
            {
                ConsumedValue = true;
                return true;
            }
            return false;
        }
    }
    
    public Vector2 CurrentMoveInput => currentMoveInput;
    public Vector2 CurrentMouseInput => currentMouseInput;
    
    private Vector2 currentMoveInput = Vector2.zero;
    private Vector2 currentMouseInput = Vector2.zero;
    
    public BaseInput AttackInput;
    public BaseInput JumpInput;
    public BaseInput InteractInput;
    public BaseInput CameraZoomInput;
    
    public BaseInput Debug1Input;
    public BaseInput Debug2Input;
    public BaseInput Debug3Input;
    public BaseInput Debug4Input;
    public BaseInput Debug5Input;
    
    private PlayerInput playerInput;

    /// <summary>
    /// Initial Setup for player input requires creating a new input instance.
    /// </summary>
    public void SetupInput()
    {
        playerInput = new PlayerInput();
        EnableInput();
    }
    
    /// <summary>
    /// Todo: When menus are avaliable add in optional bool for if it was a menu press so it only disables 
    /// </summary>
    public void EnableInput()
    {
        playerInput.Enable();
        SetupCallbacks();
    }
    public void DisableInput()
    {
        RemoveCallbacks();
        playerInput.Disable();
    }

    public void SetupCallbacks()
    {
        playerInput.Default.Movement.started += MovementStarted;
        playerInput.Default.Movement.performed += MovementPerformed;
        playerInput.Default.Movement.canceled += MovementCancelled;
        
        playerInput.Default.Attack.started += AttackStarted;
        playerInput.Default.Attack.performed += AttackPerformed;
        playerInput.Default.Attack.canceled += AttackCancelled;

        playerInput.Default.Jump.started += JumpStarted;
        playerInput.Default.Jump.performed += JumpPerformed;
        playerInput.Default.Jump.canceled += JumpCancelled;
        
        playerInput.Default.Interact.started += InteractStarted;
        playerInput.Default.Interact.performed += InteractPerformed;
        playerInput.Default.Interact.canceled += InteractCancelled;
        
        playerInput.Default.MoveCamera.started += MoveCameraStarted;
        playerInput.Default.MoveCamera.performed += MoveCameraPerformed;
        playerInput.Default.MoveCamera.canceled += MoveCameraCancelled;
        
        playerInput.Default.ZoomCamera.started += ZoomCameraStarted;
        playerInput.Default.ZoomCamera.performed += ZoomCameraPerformed;
        playerInput.Default.ZoomCamera.canceled += ZoomCameraCancelled;
        
        playerInput.Default.Debug1.started += Debug1Started;
        playerInput.Default.Debug1.performed += Debug1Performed;
        playerInput.Default.Debug1.canceled += Debug1Cancelled;
        
        playerInput.Default.Debug2.started += Debug2Started;
        playerInput.Default.Debug2.performed += Debug2Performed;
        playerInput.Default.Debug2.canceled += Debug2Cancelled;
        
        playerInput.Default.Debug3.started += Debug3Started;
        playerInput.Default.Debug3.performed += Debug3Performed;
        playerInput.Default.Debug3.canceled += Debug3Cancelled;
        
        playerInput.Default.Debug4.started += Debug4Started;
        playerInput.Default.Debug4.performed += Debug4Performed;
        playerInput.Default.Debug4.canceled += Debug4Cancelled;
        
        playerInput.Default.Debug5.started += Debug5Started;
        playerInput.Default.Debug5.performed += Debug5Performed;
        playerInput.Default.Debug5.canceled += Debug5Cancelled;
    }

    public void RemoveCallbacks()
    {
        playerInput.Default.Movement.started -= MovementStarted;
        playerInput.Default.Movement.performed -= MovementPerformed;
        playerInput.Default.Movement.canceled -= MovementCancelled;
        
        playerInput.Default.Attack.started -= AttackStarted;
        playerInput.Default.Attack.performed -= AttackPerformed;
        playerInput.Default.Attack.canceled -= AttackCancelled;

        playerInput.Default.Jump.started -= JumpStarted;
        playerInput.Default.Jump.performed -= JumpPerformed;
        playerInput.Default.Jump.canceled -= JumpCancelled;
        
        playerInput.Default.Interact.started -= InteractStarted;
        playerInput.Default.Interact.performed -= InteractPerformed;
        playerInput.Default.Interact.canceled -= InteractCancelled;
        
        playerInput.Default.ZoomCamera.started -= ZoomCameraStarted;
        playerInput.Default.ZoomCamera.performed -= ZoomCameraPerformed;
        playerInput.Default.ZoomCamera.canceled -= ZoomCameraCancelled;
        
        playerInput.Default.Debug1.started -= Debug1Started;
        playerInput.Default.Debug1.performed -= Debug1Performed;
        playerInput.Default.Debug1.canceled -= Debug1Cancelled;
        
        playerInput.Default.Debug2.started -= Debug2Started;
        playerInput.Default.Debug2.performed -= Debug2Performed;
        playerInput.Default.Debug2.canceled -= Debug2Cancelled;
        
        playerInput.Default.Debug3.started -= Debug3Started;
        playerInput.Default.Debug3.performed -= Debug3Performed;
        playerInput.Default.Debug3.canceled -= Debug3Cancelled;
        
        playerInput.Default.Debug4.started -= Debug4Started;
        playerInput.Default.Debug4.performed -= Debug4Performed;
        playerInput.Default.Debug4.canceled -= Debug4Cancelled;
        
        playerInput.Default.Debug5.started -= Debug5Started;
        playerInput.Default.Debug5.performed -= Debug5Performed;
        playerInput.Default.Debug5.canceled -= Debug5Cancelled;
    }

    public void MovementStarted(InputAction.CallbackContext callback)
    {
        currentMoveInput = callback.ReadValue<Vector2>();
    }
    public void MovementPerformed(InputAction.CallbackContext callback)
    {
        currentMoveInput = callback.ReadValue<Vector2>();
    }
    public void MovementCancelled(InputAction.CallbackContext callback)
    {
        currentMoveInput = Vector2.zero;
    }
    
    
    private void MoveCameraStarted(InputAction.CallbackContext callbackContext)
    {
        currentMouseInput = Vector2.zero;
    }
    private void MoveCameraPerformed(InputAction.CallbackContext callbackContext)
    {
        currentMouseInput = callbackContext.ReadValue<Vector2>();
    }
    private void MoveCameraCancelled(InputAction.CallbackContext callbackContext)
    {
        currentMouseInput = callbackContext.ReadValue<Vector2>();
    }
    

    /// <summary>
    /// Below is an attempt to try and have a universal check if an input has been used once eg jump input and if it is still being held
    /// </summary>
    
    private void StartInput(ref BaseInput baseInput, float rawValue)
    {
        baseInput.RawInputValue = rawValue;
        baseInput.HoldingInput = true;
        baseInput.ConsumedValue = false;
    }

    private void ProcessInput(ref BaseInput baseInput, float rawValue)
    {
        baseInput.RawInputValue = rawValue;
        baseInput.HoldingInput = true;
        baseInput.ConsumedValue = false;
    }

    private void CancelInput(ref BaseInput baseInput)
    {
        baseInput.HoldingInput = false;
        baseInput.RawInputValue = 0;
    }
    
    public void AttackStarted(InputAction.CallbackContext callback)
    {
        StartInput(ref AttackInput, callback.ReadValue<float>());
    }
    public void AttackPerformed(InputAction.CallbackContext callback)
    {
        ProcessInput(ref AttackInput, callback.ReadValue<float>());
    }
    public void AttackCancelled(InputAction.CallbackContext callback)
    {
        CancelInput(ref AttackInput);
    }
    
    
    public void JumpStarted(InputAction.CallbackContext callback)
    {
        StartInput(ref JumpInput, callback.ReadValue<float>());
    }
    public void JumpPerformed(InputAction.CallbackContext callback)
    {
        ProcessInput(ref JumpInput, callback.ReadValue<float>());
    }
    public void JumpCancelled(InputAction.CallbackContext callback)
    {
        CancelInput(ref JumpInput);
    }
    
    
    public void InteractStarted(InputAction.CallbackContext callback)
    {
        StartInput(ref InteractInput, callback.ReadValue<float>());
    }
    public void InteractPerformed(InputAction.CallbackContext callback)
    {
        ProcessInput(ref InteractInput, callback.ReadValue<float>());
    }
    public void InteractCancelled(InputAction.CallbackContext callback)
    {
        CancelInput(ref InteractInput);
    }
    
    
    public void ZoomCameraStarted(InputAction.CallbackContext callback)
    {
        StartInput(ref CameraZoomInput, callback.ReadValue<float>());
    }
    public void ZoomCameraPerformed(InputAction.CallbackContext callback)
    {
        ProcessInput(ref CameraZoomInput, callback.ReadValue<float>());
    }
    public void ZoomCameraCancelled(InputAction.CallbackContext callback)
    {
        CancelInput(ref CameraZoomInput);
    }
    
    
    public void Debug1Started(InputAction.CallbackContext callback)
    {
        StartInput(ref Debug1Input, callback.ReadValue<float>());
    }
    public void Debug1Performed(InputAction.CallbackContext callback)
    {
        ProcessInput(ref Debug1Input, callback.ReadValue<float>());
    }
    public void Debug1Cancelled(InputAction.CallbackContext callback)
    {
        CancelInput(ref Debug1Input);
    }
    
    
    public void Debug2Started(InputAction.CallbackContext callback)
    {
        StartInput(ref Debug2Input, callback.ReadValue<float>());
    }
    public void Debug2Performed(InputAction.CallbackContext callback)
    {
        ProcessInput(ref Debug2Input, callback.ReadValue<float>());
    }
    public void Debug2Cancelled(InputAction.CallbackContext callback)
    {
        CancelInput(ref Debug2Input);
    }
    
    
    public void Debug3Started(InputAction.CallbackContext callback)
    {
        StartInput(ref Debug3Input, callback.ReadValue<float>());
    }
    public void Debug3Performed(InputAction.CallbackContext callback)
    {
        ProcessInput(ref Debug3Input, callback.ReadValue<float>());
    }
    public void Debug3Cancelled(InputAction.CallbackContext callback)
    {
        CancelInput(ref Debug3Input);
    }
    
    
    public void Debug4Started(InputAction.CallbackContext callback)
    {
        StartInput(ref Debug4Input, callback.ReadValue<float>());
    }
    public void Debug4Performed(InputAction.CallbackContext callback)
    {
        ProcessInput(ref Debug4Input, callback.ReadValue<float>());
    }
    public void Debug4Cancelled(InputAction.CallbackContext callback)
    {
        CancelInput(ref Debug4Input);
    }
    
    
    public void Debug5Started(InputAction.CallbackContext callback)
    {
        StartInput(ref Debug5Input, callback.ReadValue<float>());
    }
    public void Debug5Performed(InputAction.CallbackContext callback)
    {
        ProcessInput(ref Debug5Input, callback.ReadValue<float>());
    }
    public void Debug5Cancelled(InputAction.CallbackContext callback)
    {
        CancelInput(ref Debug5Input);
    }
}
