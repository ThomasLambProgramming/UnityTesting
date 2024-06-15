using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

namespace Player
{
    public class PlayerInteract : MonoBehaviour
    {
        [HideInInspector] public PlayerMovement m_playerMovement;
        private int InteractableLayermask;

        private void Awake()
        {
            InteractableLayermask = 1 << LayerMask.NameToLayer("Interactable");
        }
        
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

        public void SetupInputCallbacks()
        {
            PlayerInputProcessor.Instance.m_playerInput.Default.Interact.performed += InteractInputStart;
            PlayerInputProcessor.Instance.m_playerInput.Default.Interact.canceled += InteractInputEnd;
        }
        public void RemoveInputCallbacks()
        {
            PlayerInputProcessor.Instance.m_playerInput.Default.Interact.performed -= InteractInputStart;
            PlayerInputProcessor.Instance.m_playerInput.Default.Interact.canceled -= InteractInputEnd;
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