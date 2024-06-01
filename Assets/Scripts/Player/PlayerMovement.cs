using UnityEngine;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody playerRigidbody;
        
        [Header("Movement Settings")]
        [SerializeField] private float movementSpeed = 10f;
        [SerializeField] public float maxMovementSpeed = 4f;
        [SerializeField] private float slowdownPercentage = 0.98f;
        [SerializeField] private float rotateToVelocitySpeed = 10f;

        [Header("Jump Settings")]
        [SerializeField] private float jumpForce = 20f;

        [Header("Gravity Settings")] [SerializeField]
        private float m_additionalGravity = 9.81f;



        public void UpdateMovement(Vector2 playerInput, Vector3 cameraDirectionForward, Vector3 cameraDirectionRight)
        {
            if (playerInput.magnitude < 0.1f) 
                SlowDownPlayer(playerInput);
            else
                MovePlayer(playerInput, cameraDirectionForward, cameraDirectionRight);
            
            //Additional gravity to make player fall faster and feel more "weighty"
            //if (playerRigidbody.velocity.y < -1)
            playerRigidbody.AddForce(0,-m_additionalGravity, 0, ForceMode.Force);
        }

        /// <summary>
        /// Take a percentage of the players current horizontal velocity away to slow them down.
        /// </summary>
        private void SlowDownPlayer(Vector2 playerInput)
        {
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
        private void MovePlayer(Vector2 playerInput, Vector3 cameraDirectionForward, Vector3 cameraDirectionRight)
        {
            Vector3 movementRight = new Vector3(cameraDirectionRight.x, 0, cameraDirectionRight.z).normalized;
            Vector3 movementForward = new Vector3(cameraDirectionForward.x, 0, cameraDirectionForward.z).normalized;
            
            playerRigidbody.AddForce((movementForward * playerInput.y + movementRight * playerInput.x).normalized * (movementSpeed * Time.deltaTime), ForceMode.VelocityChange);
            Vector3 currentVelocity = playerRigidbody.velocity;
            float previousY = currentVelocity.y;
            currentVelocity.y = 0;
            //
            if (currentVelocity.magnitude > maxMovementSpeed)
                currentVelocity = currentVelocity.normalized * maxMovementSpeed;
            if (currentVelocity.sqrMagnitude <= 0.5f)
                return;
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, currentVelocity.normalized, rotateToVelocitySpeed * Time.deltaTime, 1));
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