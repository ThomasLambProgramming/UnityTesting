using System.Collections;
using UnityEngine;

namespace Player
{
    //Make it easier when creating a new player by forcing all components to be added at once.
    [RequireComponent(typeof(PlayerInputProcessor), typeof(Rigidbody), typeof(PlayerMovement)), 
     //RequireComponent has a limit.
     RequireComponent(typeof(PlayerCameraController))]
    public class PlayerManager : MonoBehaviour
    {
        private PlayerInputProcessor playerInput;
        private PlayerMovement playerMovement;
        private PlayerCameraController playerCamera;
        
        private Rigidbody playerRigidbody;
        private Animator playerAnimator;

        [Header("Jumping Ground Check Settings")]
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float groundCheckDelay = 0.5f;
        [SerializeField, Tooltip("If velocity > this value do a hard landing")] private float hardLandingLimit = 4f;
        
        private const int EnvironmentLayerMask = 1 << 6;
        private bool checkGround = false;
        private bool isGrounded = true;
        
        
        //Animator Ids (doing string comparison is 100% going to be an issue at some point)
        private static readonly int SpeedAnimatorId = Animator.StringToHash("Speed");
        private static readonly int DanceAnimatorId  = Animator.StringToHash("Dance");
        private static readonly int JumpStartAnimatorId  = Animator.StringToHash("JumpStart");
        private static readonly int FallingAnimatorId  = Animator.StringToHash("Falling");
        private static readonly int SoftLandAnimatorId  = Animator.StringToHash("SoftLand");
        private static readonly int HardLandAnimatorId  = Animator.StringToHash("HardLand");
        private static readonly int AttackHorizontalAnimatorId  = Animator.StringToHash("AttackHorizontal");
        private static readonly int ResetToBaseMovementAnimatorId = Animator.StringToHash("ResetToBaseMovement");

        //Debug / Random Settings that dont have anything to do with gameplay
        private bool debugDancing = false;
        

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
        }

        private void Update()
        {
            if (DebugUpdate())
                return;
            
            if (playerInput.JumpInput.CheckAndConsumeInput() && isGrounded)
            {
                isGrounded = false;
                checkGround = false;
                playerMovement.PerformJump();
                playerAnimator.SetBool(JumpStartAnimatorId, true);
                playerAnimator.SetBool(ResetToBaseMovementAnimatorId, false);
                StartCoroutine(GroundCheckDelay());
            }

            if (isGrounded == false && checkGround)
            {
                if (playerRigidbody.velocity.y <= 0)
                    playerAnimator.SetBool(FallingAnimatorId, true);
                GroundCheck();
            }

            if (playerInput.AttackInput.CheckAndConsumeInput())
            {
                //playerAnimator.SetBool(AttackHorizontalAnimatorId, true);
            }
            
            playerCamera.UpdateCamera(playerInput.CurrentMouseInput);
            playerMovement.UpdateMovement(playerInput.CurrentMoveInput, playerCamera.mainCamera.transform.forward, playerCamera.mainCamera.transform.right);
            UpdateAnimator();
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
                checkGround = false;
                playerAnimator.SetBool(FallingAnimatorId, false);
                playerAnimator.SetBool(ResetToBaseMovementAnimatorId, true);
                playerAnimator.SetBool(HardLandAnimatorId, playerRigidbody.velocity.y > hardLandingLimit);
            }
        }

        //Raycast immediately finds ground when first jumping. adding delay to stop this.
        IEnumerator GroundCheckDelay()
        {
            yield return new WaitForSeconds(groundCheckDelay);
            checkGround = true;
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
            if (playerInput.Debug1Input.CheckAndConsumeInput())
            {
                if (Cursor.lockState == CursorLockMode.Locked)
                    Cursor.lockState = CursorLockMode.None;
                else
                    Cursor.lockState = CursorLockMode.Locked;
            }

            if (playerInput.Debug2Input.CheckAndConsumeInput())
                playerCamera.ToggleControllerSpeed();
            
            if (playerInput.Debug3Input.CheckAndConsumeInput())
                playerAnimator.SetBool(DanceAnimatorId, debugDancing);
            
            return debugDancing;
        }
    }
}