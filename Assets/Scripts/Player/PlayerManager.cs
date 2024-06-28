using UI;
using UnityEngine;

namespace Player
{
    //Make it easier when creating a new player by forcing all components to be added at once.
    //RequireComponent has a limit.
    [
        RequireComponent(typeof(PlayerInputProcessor), typeof(Rigidbody), typeof(PlayerMovement)),
        RequireComponent(typeof(PlayerCameraController), typeof(PlayerInteract), typeof(PlayerAnimator))
    ]
    public class PlayerManager : MonoBehaviour
    {
        private PlayerMovement m_playerMovement;
        private PlayerCameraController m_playerCamera;
        private InGameMenuManager m_inGameMenuManager;
        private PlayerAnimator m_playerAnimator;
        private PlayerInteract m_playerInteract;

        [SerializeField] private bool m_stopPlayerWASDInput = false;

        private void Start()
        {
            m_playerAnimator = GetComponent<PlayerAnimator>();
            m_playerAnimator.m_animator = GetComponentInChildren<Animator>();
            
            m_inGameMenuManager = FindObjectOfType<InGameMenuManager>(true);
            m_playerCamera = GetComponent<PlayerCameraController>();
            
            m_playerMovement = GetComponent<PlayerMovement>();
            m_playerMovement.m_playerRigidbody = GetComponent<Rigidbody>();
            m_playerMovement.m_playerCameraController = m_playerCamera;
            m_playerMovement.m_playerAnimator = m_playerAnimator;
            m_playerAnimator.m_playerMovement = m_playerMovement;
            m_playerMovement.m_menuManager = m_inGameMenuManager;
            
            m_playerInteract = GetComponent<PlayerInteract>();
            m_playerInteract.m_playerMovement = m_playerMovement;
            
            Cursor.lockState = CursorLockMode.Locked;
            SetupInputCallbacks();
        }

        private void Update()
        {
            UpdateCursorLock();
            
            if (DebugUpdate() || m_stopPlayerWASDInput || m_inGameMenuManager.MenuActive)
                return;

            UpdateComponents();
        }

        /// <summary>
        /// On late update so movement of camera occurs after all velocities + movement has already been completed for this frame
        /// </summary>
        private void LateUpdate()
        {
            if (m_stopPlayerWASDInput || m_inGameMenuManager.MenuActive)
                return;
            m_playerCamera.UpdateCamera();
        }

        private void UpdateCursorLock()
        {
            if (m_inGameMenuManager.MenuActive)
                Cursor.lockState = CursorLockMode.None;
            else if (!m_inGameMenuManager.MenuActive && Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
        }

        private void UpdateComponents()
        {
            if (m_inGameMenuManager.MenuActive)
                return;
            
            m_playerMovement.UpdateComponent();
            m_playerAnimator.UpdateComponent();
        }

        private void SetupInputCallbacks()
        {
            m_inGameMenuManager.SetupInputCallbacks();
            m_playerMovement.SetupInputCallbacks();
            m_playerInteract.SetupInputCallbacks();
        }

        private void RemoveInputCallbacks()
        {
            m_inGameMenuManager.RemoveInputCallbacks();
            m_playerMovement.RemoveInputCallbacks();
            m_playerInteract.RemoveInputCallbacks();
        }

        /// <summary>
        /// Return value is for stopping the player move.
        /// </summary>
        private bool DebugUpdate()
        {
            return false;
        }
    }
}
