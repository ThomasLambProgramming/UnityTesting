using System;
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
        
        private bool isGrounded = true;
        private bool stoppedHoldingSpace = true;
        private bool dancing = false;

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
            
            if (playerInput.JumpInput.CheckAndConsumeInput())
            {
                isGrounded = false;
                playerMovement.PerformJump();
            }

            playerCamera.UpdateCamera(playerInput.CurrentMouseInput);
            playerMovement.UpdateMovement(playerInput.CurrentMoveInput, playerCamera.mainCamera.transform.forward, playerCamera.mainCamera.transform.right);

            //float currentVelocity = playerRigidbody.velocity.magnitude / playerMovement.maxMovementSpeed;
            //playerAnimator.SetFloat("Speed", currentVelocity);
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