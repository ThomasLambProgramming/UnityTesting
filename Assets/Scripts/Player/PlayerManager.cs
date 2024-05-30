using System;
using UnityEngine;

namespace Player
{
    //Make it easier when creating a new player by forcing all components to be added at once.
    [RequireComponent(typeof(PlayerInputProcessor), typeof(Rigidbody), typeof(PlayerMovement))]
    public class PlayerManager : MonoBehaviour
    {
        private PlayerInputProcessor playerInput;
        private Rigidbody rigidbody;
        private PlayerMovement playerMovement;

        private bool isGrounded = true;

        private void Awake()
        {
            playerInput = GetComponent<PlayerInputProcessor>();
            playerInput.SetupInput();
        }

        private void Start()
        {
            rigidbody = GetComponent<Rigidbody>();
            playerMovement = GetComponent<PlayerMovement>();
            playerMovement.playerRigidbody = rigidbody;
        }

        private void Update()
        {
            if (playerInput.JumpInputDown && isGrounded)
            {
                isGrounded = false;
                playerMovement.PerformJump();
            }
            
            playerMovement.UpdateMovement(playerInput.CurrentMoveInput);
        }
    }
}