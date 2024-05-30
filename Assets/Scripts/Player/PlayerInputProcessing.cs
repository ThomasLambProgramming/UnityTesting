using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputProcessor : MonoBehaviour
{
    public Vector2 CurrentMoveInput => m_CurrentMoveInput;
    public bool AttackInputDown => m_AttackInputDown > 0.1f;
    public bool JumpInputDown => m_JumpInputDown > 0.1f;
    public bool InteractInputDown => m_InteractInputDown > 0.1f;
    
    private Vector2 m_CurrentMoveInput = Vector2.zero;
    private float m_AttackInputDown = 0;
    private float m_JumpInputDown = 0;
    private float m_InteractInputDown = 0;
    
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
        
        playerInput.Default.Attack.started+= AttackStarted;
        playerInput.Default.Attack.performed += AttackPerformed;
        playerInput.Default.Attack.canceled += AttackCancelled;

        playerInput.Default.Jump.started+= JumpStarted;
        playerInput.Default.Jump.performed += JumpPerformed;
        playerInput.Default.Jump.canceled += JumpCancelled;
        
        playerInput.Default.Interact.started+= InteractStarted;
        playerInput.Default.Interact.performed += InteractPerformed;
        playerInput.Default.Interact.canceled += InteractCancelled;
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
    }

    public void MovementStarted(InputAction.CallbackContext callback)
    {
        m_CurrentMoveInput = callback.ReadValue<Vector2>();
    }
    public void MovementPerformed(InputAction.CallbackContext callback)
    {
        m_CurrentMoveInput = callback.ReadValue<Vector2>();
    }
    public void MovementCancelled(InputAction.CallbackContext callback)
    {
        m_CurrentMoveInput = Vector2.zero;
    }
    
    
    public void AttackStarted(InputAction.CallbackContext callback)
    {
        m_AttackInputDown = callback.ReadValue<float>();
    }
    public void AttackPerformed(InputAction.CallbackContext callback)
    {
        m_AttackInputDown = callback.ReadValue<float>();
    }
    public void AttackCancelled(InputAction.CallbackContext callback)
    {
        m_AttackInputDown = 0;
    }
    
    
    public void JumpStarted(InputAction.CallbackContext callback)
    {
        m_JumpInputDown = callback.ReadValue<float>();
    }
    public void JumpPerformed(InputAction.CallbackContext callback)
    {
        m_JumpInputDown = callback.ReadValue<float>();
    }
    public void JumpCancelled(InputAction.CallbackContext callback)
    {
        m_JumpInputDown = 0;
    }
    
    
    public void InteractStarted(InputAction.CallbackContext callback)
    {
        m_InteractInputDown = callback.ReadValue<float>();
    }
    public void InteractPerformed(InputAction.CallbackContext callback)
    {
        m_InteractInputDown = callback.ReadValue<float>();
    }
    public void InteractCancelled(InputAction.CallbackContext callback)
    {
        m_InteractInputDown = 0;
    }
}
