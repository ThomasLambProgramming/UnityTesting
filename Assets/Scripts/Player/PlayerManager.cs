using System;
using System.Collections;
using UnityEngine;

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
        private PlayerInputProcessor playerInput;
        private PlayerMovement playerMovement;
        private PlayerCameraController playerCamera;

        private Rigidbody playerRigidbody;
        private Animator playerAnimator;

        [Header("Jumping Ground Check Settings")]
        [SerializeField]
        private Transform groundCheck;

        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float groundCheckDelay = 0.5f;
        [SerializeField] private float landingStopSoftInputDelay = 1f;
        [SerializeField] private float landingStopHardInputDelay = 2f;
        
        [SerializeField, Tooltip("If velocity > this value do a hard landing")]
        private float hardLandingLimit = 16f;

        private const int EnvironmentLayerMask = 1;
        private bool isGrounded = true;

        //Animator Ids (doing string comparison is 100% going to be an issue at some point)
        private int SpeedAnimatorId = Animator.StringToHash("Speed");
        private int DanceAnimatorId = Animator.StringToHash("Dance");
        private int JumpStartAnimatorId = Animator.StringToHash("JumpStart");
        private int FallingAnimatorId = Animator.StringToHash("Falling");
        private int SoftLandAnimatorId = Animator.StringToHash("SoftLand");
        private int HardLandAnimatorId = Animator.StringToHash("HardLand");
        private int AttackHorizontalAnimatorId = Animator.StringToHash("AttackHorizontal");
        private int ResetToBaseMovementAnimatorId = Animator.StringToHash("ResetToBaseMovement");

        private Coroutine playerGroundCheckCoroutine = null;
        //Debug / Random Settings that dont have anything to do with gameplay
        private bool debugDancing = false;

        private bool stopPlayerWASDInput = false;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInputProcessor>();
            playerInput.SetupInput();
        }

        private void Start()
        {
            playerAnimator = GetComponentInChildren<Animator>();
            playerMovement = GetComponent<PlayerMovement>();
            playerCamera = GetComponent<PlayerCameraController>();
            playerRigidbody = GetComponent<Rigidbody>();

            playerMovement.playerRigidbody = playerRigidbody;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            if (DebugUpdate())
                return;
            if (stopPlayerWASDInput)
                return;

            if (playerInput.JumpInput.CheckInput() && isGrounded && playerGroundCheckCoroutine == null)
            {
                playerMovement.PerformJump();
                playerAnimator.CrossFade("JumpStart", 0f);
                playerGroundCheckCoroutine = StartCoroutine(GroundCheckDelay());
            }
            if (isGrounded == false && playerRigidbody.velocity.y < 0 && playerAnimator.GetCurrentAnimatorStateInfo(0).IsName("FallingIdle"))
                playerAnimator.CrossFade("FallingIdle", 0.3f);


            if (isGrounded == false && playerGroundCheckCoroutine == null)
            {
                GroundCheck();
            }

            if (playerInput.AttackInput.CheckInput())
            {
                //playerAnimator.SetBool(AttackHorizontalAnimatorId, true);
            }

            
            playerMovement.UpdateMovement(playerInput.CurrentMoveInput, playerCamera.mainCamera.transform.forward, playerCamera.mainCamera.transform.right);
            UpdateAnimator();
        }

        private void LateUpdate()
        {
            playerCamera.UpdateCamera(playerInput.CurrentMouseInput, playerInput.MouseInputFromController);
        }

        private void UpdateAnimator()
        {
            float currentVelocity = playerRigidbody.velocity.magnitude / playerMovement.maxMovementSpeed;
            playerAnimator.SetFloat(SpeedAnimatorId, currentVelocity);
        }

        private void GroundCheck()
        {
            //If the object is on the environment layer (not the non-jump environment layer)
            if (Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, EnvironmentLayerMask))
            {
                isGrounded = true;

                if (playerRigidbody.velocity.y > -hardLandingLimit)
                {
                    playerAnimator.CrossFade("LandingSoft", 0.1f);
                    StartCoroutine(LandingInputDelay(true));
                }
                else
                {
                    playerAnimator.CrossFade("LandingHard", 0.1f);
                    StartCoroutine(LandingInputDelay(false));
                }
            }
        }
        
        IEnumerator LandingInputDelay(bool softLanding)
        {
            stopPlayerWASDInput = true;
            yield return new WaitForSeconds(softLanding ? landingStopSoftInputDelay : landingStopHardInputDelay);
            stopPlayerWASDInput = false;
            playerAnimator.CrossFade("BaseMovementTree", 0.3f);
        }

        //Raycast immediately finds ground when first jumping. adding delay to stop this.
        IEnumerator GroundCheckDelay()
        {
            yield return new WaitForSeconds(groundCheckDelay);
            isGrounded = false;
            playerGroundCheckCoroutine = null;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(groundCheck.position, groundCheckDistance);
            Gizmos.color = Color.gray;
        }

        /// <summary>
        /// Return value is for stopping the player move.
        /// </summary>
        private bool DebugUpdate()
        {
            if (playerInput.Debug1Input.CheckInput())
            {
                //if (Cursor.lockState == CursorLockMode.Locked)
                //    Cursor.lockState = CursorLockMode.None;
                //else
                //    Cursor.lockState = CursorLockMode.Locked;
                playerAnimator.CrossFade("JumpStart", 0.2f);
            }

            if (playerInput.Debug2Input.CheckInput())
                playerAnimator.CrossFade("FallingIdle", 0.3f);

            if (playerInput.Debug3Input.CheckInput())
                playerAnimator.CrossFade("LandingSoft", 0.1f);

            if (playerInput.Debug4Input.CheckInput())
                playerAnimator.CrossFade("LandingHard", 0.1f);

            if (playerInput.Debug5Input.CheckInput())
                playerAnimator.CrossFade("BaseMovementTree", 0.2f);

            return debugDancing;
        }
    }
}
