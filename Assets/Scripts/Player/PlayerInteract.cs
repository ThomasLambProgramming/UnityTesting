using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {
        public PlayerMovement m_playerMovement;
        private readonly int InteractableLayermask = 1 << LayerMask.NameToLayer("Interactable");
        private void PlayerInteractInputStarted()
        {
            Collider[] overlaps = Physics.OverlapSphere(transform.position, 5, InteractableLayermask);
            for (int i = 0; i < overlaps.Length; i++)
            {
                SplineContainer splineContainer = overlaps[i].gameObject.GetComponent<SplineContainer>();
                if (splineContainer)
                    m_playerMovement.StartSplineRiding(splineContainer);
            }
        }

        public void SetupInputCallbacks(ref PlayerInput playerInput)
        {
            playerInput.Default.Interact.performed += InteractInputStart;
            playerInput.Default.Interact.canceled += InteractInputEnd;
        }
        public void RemoveInputCallbacks(ref PlayerInput playerInput)
        {
            playerInput.Default.Interact.performed -= InteractInputStart;
            playerInput.Default.Interact.canceled -= InteractInputEnd;
        }
        
        private void InteractInputStart(InputAction.CallbackContext callback)
        {
            PlayerInteractInputStarted();
        }
        private void InteractInputEnd(InputAction.CallbackContext callback)
        {
            
        }
    }
}