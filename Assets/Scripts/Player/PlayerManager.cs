using System;
using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.Serialization;

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

        private Rigidbody m_playerRigidbody;
        private Animator m_playerAnimator;

        //[Header("Required References")] 
        private InGameMenuManager m_mainMenuManager;

        [Header("Jumping Ground Check Settings"), Space(5f)]
        [SerializeField] private Transform groundCheck;

        [SerializeField] private float m_groundCheckDistance;
        [SerializeField] private float m_groundCheckDelay = 0.5f;
        [SerializeField] private float m_landingStopSoftInputDelay = 1f;
        [SerializeField] private float m_landingStopHardInputDelay = 2f;
        
        [SerializeField, Tooltip("If velocity > this value do a hard landing")]
        private float m_softLandingVelocityLimit = 16f;

        private const int EnvironmentLayerMask = 1;
        private bool m_isPlayerGrounded = true;
        private Coroutine m_playerGroundCheckCoroutine = null;

        //Animator Ids (doing string comparison is 100% going to be an issue at some point)
        private readonly int SpeedAnimatorId = Animator.StringToHash("Speed");
        private readonly int DanceAnimatorId = Animator.StringToHash("Dance");
        private readonly int JumpStartAnimatorId = Animator.StringToHash("JumpStart");
        private readonly int FallingAnimatorId = Animator.StringToHash("Falling");
        private readonly int SoftLandAnimatorId = Animator.StringToHash("SoftLand");
        private readonly int HardLandAnimatorId = Animator.StringToHash("HardLand");
        private readonly int AttackHorizontalAnimatorId = Animator.StringToHash("AttackHorizontal");
        private readonly int ResetToBaseMovementAnimatorId = Animator.StringToHash("ResetToBaseMovement");

        private float m_previousAnimatorSpeedValue = 0;
        [SerializeField] private float m_animatorLerpSpeed = 7;
        [SerializeField] private bool m_stopPlayerWASDInput = false;

        [Header("Attaching Sockets for hammer")] 
        [SerializeField] private GameObject m_hammerWayObject;
        [SerializeField] private Transform m_backAttachSocket;
        [SerializeField] private Transform m_handAttachSocket;
        [SerializeField] private Transform m_groundAttachSocket;
        

        private void Awake()
        {
            m_playerInput = GetComponent<PlayerInputProcessor>();
            m_playerInput.SetupInput();
        }

        private void Start()
        {
            m_playerAnimator = GetComponentInChildren<Animator>();
            m_playerMovement = GetComponent<PlayerMovement>();
            m_playerCamera = GetComponent<PlayerCameraController>();
            m_playerRigidbody = GetComponent<Rigidbody>();

            m_playerMovement.playerRigidbody = m_playerRigidbody;
            Cursor.lockState = CursorLockMode.Locked;
            m_mainMenuManager = FindObjectOfType<InGameMenuManager>(true);
        }

        private void Update()
        {
            if (m_playerInput.PauseInput.CheckInput())
            {
                m_mainMenuManager.ToggleActive();
            }
            if (m_mainMenuManager.MenuActive)
                Cursor.lockState = CursorLockMode.None;
            else if (!m_mainMenuManager.MenuActive && Cursor.lockState == CursorLockMode.None)
                Cursor.lockState = CursorLockMode.Locked;
                
            
            if (DebugUpdate() || m_stopPlayerWASDInput || m_mainMenuManager.MenuActive)
                return;

            m_playerMovement.UpdateMovement(m_playerInput.CurrentMoveInput, m_playerCamera.m_mainCamera.transform.forward, m_playerCamera.m_mainCamera.transform.right);

            if (m_isPlayerGrounded == false && m_playerGroundCheckCoroutine == null)
            {
                GroundCheck();
            }

            if (m_playerInput.InteractInput.CheckInput())
            {
                if (m_playerMovement.m_CurrentMovementState == PlayerMovement.MovementState.BaseMovement)
                    SetToHammahWay();
                else
                {
                    SetToBaseMovement();
                }
            }

            if (m_playerMovement.m_CurrentMovementState == PlayerMovement.MovementState.HammahWay)
                return;

            UpdateAnimator();
            if (m_playerInput.JumpInput.CheckInput() && m_isPlayerGrounded && m_playerGroundCheckCoroutine == null)
            {
                m_playerMovement.PerformJump();
                m_playerAnimator.CrossFade("JumpStart", 0f);
                m_playerGroundCheckCoroutine = StartCoroutine(GroundCheckDelay());
            }
            if (m_isPlayerGrounded == false && m_playerRigidbody.velocity.y < 0 && m_playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("FallingIdle"))
                m_playerAnimator.CrossFade("FallingIdle", 0.3f);
            
        }

        private void LateUpdate()
        {
            if (m_stopPlayerWASDInput || m_mainMenuManager.MenuActive)
                return;
            m_playerCamera.UpdateCamera(m_playerInput.CurrentMouseInput, m_playerInput.MouseInputFromController);
        }

        private void UpdateAnimator()
        {
            Vector3 currentVel = m_playerRigidbody.velocity;
            currentVel.y = 0;
            float currentHorizontalVelocity = currentVel.magnitude / m_playerMovement.MaxMovementSpeed;
            float animatorSpeedValue = Mathf.Lerp(m_previousAnimatorSpeedValue, currentHorizontalVelocity, m_animatorLerpSpeed * Time.deltaTime);
            m_previousAnimatorSpeedValue = animatorSpeedValue;
            m_playerAnimator.SetFloat(SpeedAnimatorId, animatorSpeedValue);
        }

        private void GroundCheck()
        {
            //If the object is on the environment layer (not the non-jump environment layer)
            if (Physics.Raycast(groundCheck.position, Vector3.down, m_groundCheckDistance, EnvironmentLayerMask))
            {
                m_isPlayerGrounded = true;

                if (m_playerRigidbody.velocity.y > -m_softLandingVelocityLimit)
                {
                    m_playerAnimator.CrossFade("LandingSoft", 0.1f);
                    StartCoroutine(LandingInputDelay(true));
                }
                else
                {
                    m_playerAnimator.CrossFade("LandingHard", 0.1f);
                    StartCoroutine(LandingInputDelay(false));
                }
            }
        }

        private void SetToHammahWay()
        {
            m_hammerWayObject.transform.parent = m_groundAttachSocket;
            m_hammerWayObject.transform.localRotation = Quaternion.Euler(0,0,0);
            m_hammerWayObject.transform.localPosition = Vector3.zero;
            m_hammerWayObject.GetComponentInChildren<Collider>().enabled = true;
            m_playerMovement.m_CurrentMovementState = PlayerMovement.MovementState.HammahWay;
            m_playerAnimator.SetFloat(SpeedAnimatorId, 0);
        }
        private void SetToBaseMovement()
        {
            m_hammerWayObject.transform.parent = m_backAttachSocket;
            m_hammerWayObject.transform.localRotation = Quaternion.Euler(0,0,0);
            m_hammerWayObject.transform.localPosition = Vector3.zero;
            m_hammerWayObject.GetComponentInChildren<Collider>().enabled = false;
            m_playerMovement.m_CurrentMovementState = PlayerMovement.MovementState.BaseMovement;
        }
        
        
        IEnumerator LandingInputDelay(bool softLanding)
        {
            m_stopPlayerWASDInput = true;
            yield return new WaitForSeconds(softLanding ? m_landingStopSoftInputDelay : m_landingStopHardInputDelay);
            m_stopPlayerWASDInput = false;
            m_playerAnimator.CrossFade("BaseMovementTree", 0.3f);
        }

        //Raycast immediately finds ground when first jumping. adding delay to stop this.
        IEnumerator GroundCheckDelay()
        {
            yield return new WaitForSeconds(m_groundCheckDelay);
            m_isPlayerGrounded = false;
            m_playerGroundCheckCoroutine = null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(groundCheck.position, m_groundCheckDistance);
            Gizmos.color = Color.gray;
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
