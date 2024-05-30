using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody playerRigidbody;
        
        [Header("Movement Settings")]
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] private float maxMovementSpeed = 4f;
        [SerializeField] private float slowdownPercentage = 0.98f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 20f;

        public void UpdateMovement(Vector2 playerInput)
        {
            if (playerInput.magnitude < 0.1f) 
                SlowDownPlayer(playerInput);
            else
                MovePlayer(playerInput);
        }

        /// <summary>
        /// Take a percentage of the players current horizontal velocity away to slow them down.
        /// </summary>
        private void SlowDownPlayer(Vector2 playerInput)
        {
            return;
            Vector3 currentVelocity = playerRigidbody.velocity;
            float previousY = currentVelocity.y;
            currentVelocity.y = 0;
            currentVelocity *= slowdownPercentage;
            currentVelocity.y = previousY;
            playerRigidbody.velocity = currentVelocity;
        }

        /// <summary>
        /// Move the player based on their input.
        /// </summary>
        private void MovePlayer(Vector2 playerInput)
        {
            playerRigidbody.AddForce(playerInput.x * movementSpeed * Time.deltaTime, 0, playerInput.y * movementSpeed * Time.deltaTime, ForceMode.Force);
            Vector3 currentVelocity = playerRigidbody.velocity;
            float previousY = currentVelocity.y;
            currentVelocity.y = 0;

            if (currentVelocity.magnitude > maxMovementSpeed)
                currentVelocity = currentVelocity.normalized * maxMovementSpeed;

            currentVelocity.y = previousY;
            playerRigidbody.velocity = currentVelocity;
        }

        public void PerformJump()
        {
            //This is just so the force is always consistent even if Character mass changes
            playerRigidbody.AddForce(0, jumpForce, 0, ForceMode.VelocityChange);
        }
    }
}