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

        [SerializeField] private Transform groundCheck;
        [SerializeField] private float groundCheckDistance;
        [SerializeField] private float groundCheckDelay = 0.5f;
        
        private bool checkGround = false;
        private bool isGrounded = true;
        private bool dancing = false;
        
        private static readonly int Speed = Animator.StringToHash("Speed");

        private const int EnvironmentLayerMask = 1 << 6;

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
                StartCoroutine(GroundCheckDelay());
            }
            
            if (isGrounded == false && checkGround)
                GroundCheck();

            playerCamera.UpdateCamera(playerInput.CurrentMouseInput);
            playerMovement.UpdateMovement(playerInput.CurrentMoveInput, playerCamera.mainCamera.transform.forward, playerCamera.mainCamera.transform.right);

            float currentVelocity = playerRigidbody.velocity.magnitude / playerMovement.maxMovementSpeed;
            playerAnimator.SetFloat(Speed, currentVelocity);
        }

        private void GroundCheck()
        {
            //If the object is on the environment layer (not the non-jump environment layer)
            if (Physics.Raycast(groundCheck.position, Vector3.down, groundCheckDistance, EnvironmentLayerMask))
            {
                isGrounded = true;
                checkGround = false;
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
                playerAnimator.SetBool("Dance", dancing);
            
            return dancing;
        }
    }
}