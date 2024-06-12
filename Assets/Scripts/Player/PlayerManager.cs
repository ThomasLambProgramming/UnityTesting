using UI;
using UnityEngine;
using UnityEngine.Splines;

namespace Player
{
    //Make it easier when creating a new player by forcing all components to be added at once.
    [
        RequireComponent(typeof(PlayerInputProcessor), typeof(Rigidbody), typeof(PlayerMovement)),
        //RequireComponent has a limit.
        RequireComponent(typeof(PlayerCameraController))
    ]
    public class PlayerManager : MonoBehaviour
    {
        private PlayerInputProcessor m_playerInput;
        private PlayerMovement m_playerMovement;
        private PlayerCameraController m_playerCamera;
        private InGameMenuManager m_inGameMenuManager;
        private PlayerAnimator m_playerAnimator;

        [SerializeField] private bool m_stopPlayerWASDInput = false;

        private void Awake()
        {
            m_playerInput = GetComponent<PlayerInputProcessor>();
            m_playerInput.SetupInput();
        }

        private void Start()
        {
            m_playerAnimator = GetComponentInChildren<PlayerAnimator>();
            m_playerMovement = GetComponent<PlayerMovement>();
            m_playerCamera = GetComponent<PlayerCameraController>();
            m_inGameMenuManager = FindObjectOfType<InGameMenuManager>(true);
            m_playerMovement.m_playerRigidbody = GetComponent<Rigidbody>();
            
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

        private void UpdateCursorLock()
        {
            if (m_inGameMenuManager.MenuActive)
                Cursor.lockState = CursorLockMode.None;
            else if (!m_inGameMenuManager.MenuActive && Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
        }

        private void UpdateComponents()
        {
            m_playerMovement.UpdateComponent();
        }

        private void SetupInputCallbacks()
        {
            m_inGameMenuManager.SetupInputCallbacks(ref m_playerInput.playerInput);
            m_playerMovement.SetupInputCallbacks(ref m_playerInput.playerInput);
            m_playerCamera.SetupInputCallbacks(ref m_playerInput.playerInput);
        }

        private void RemoveInputCallbacks()
        {
            m_inGameMenuManager.RemoveInputCallbacks(ref m_playerInput.playerInput);
            m_playerMovement.RemoveInputCallbacks(ref m_playerInput.playerInput);
            m_playerCamera.RemoveInputCallbacks(ref m_playerInput.playerInput);
        }

        /// <summary>
        /// On late update so movement of camera occurs after all velocities + movement has already been completed for this frame
        /// </summary>
        private void LateUpdate()
        {
            if (m_stopPlayerWASDInput || m_inGameMenuManager.MenuActive)
                return;
            m_playerCamera.UpdateCamera(m_playerInput.CurrentMouseInput, m_playerInput.MouseInputFromController);
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
