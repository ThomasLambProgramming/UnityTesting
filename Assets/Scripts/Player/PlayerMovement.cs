using UnityEngine;
using UnityEngine.Serialization;

namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {
        public Rigidbody playerRigidbody;

        [Header("Movement Settings")]
        [SerializeField] private float m_movementSpeed = 10f;
        [SerializeField] private float m_maxMovementSpeed = 4f;
        public float MaxMovementSpeed => m_maxMovementSpeed;
        [SerializeField] private float m_slowdownPercentage = 0.98f;
        [SerializeField] private float m_rotateToVelocitySpeed = 10f;

        [Header("Jump Settings")]
        [SerializeField] private float m_jumpForce = 20f;

        //[Header("Gravity Settings")]
        //[SerializeField] private float m_additionalGravity = 9.81f;

        private float m_magnitudeSpeedCutoff = 0.2f;
        public void UpdateMovement(Vector2 playerInput, Vector3 cameraDirectionForward, Vector3 cameraDirectionRight)
        {
            //All updates should be done through a intermediate Vector3 so velocity is only being set once per frame otherwise jittering occurs with the camera.
            Vector3 newVelocity;
            if (playerInput.magnitude < m_magnitudeSpeedCutoff) 
                newVelocity = SlowDownPlayer();
            else
                newVelocity = MovePlayer(playerInput, cameraDirectionForward, cameraDirectionRight);

            //if (Mathf.Abs(newVelocity.y) > 1)
            //    newVelocity.y -= m_additionalGravity;
            playerRigidbody.velocity = newVelocity;
        }

        /// <summary>
        /// Take a percentage of the players current horizontal velocity away to slow them down.
        /// </summary>
        private Vector3 SlowDownPlayer()
        {
            Vector3 currentVelocity = playerRigidbody.velocity;
            float previousY = currentVelocity.y;
            currentVelocity.y = 0;
            currentVelocity *= m_slowdownPercentage;
            currentVelocity.y = previousY;
            return currentVelocity;
        }

        /// <summary>
        /// Move the player based on their input.
        /// </summary>
        private Vector3 MovePlayer(Vector2 playerInput, Vector3 cameraDirectionForward, Vector3 cameraDirectionRight)
        {
            Vector3 movementRight = new Vector3(cameraDirectionRight.x, 0, cameraDirectionRight.z).normalized;
            Vector3 movementForward = new Vector3(cameraDirectionForward.x, 0, cameraDirectionForward.z).normalized;

            Vector3 currentVelocity = playerRigidbody.velocity;
            float previousY = currentVelocity.y;
            currentVelocity.y = 0;

            currentVelocity += (movementForward * playerInput.y + movementRight * playerInput.x).normalized * (m_movementSpeed * Time.deltaTime);

            //Reduce max speed so controller is able to walk.
            float adjustedMaxSpeed = m_maxMovementSpeed * playerInput.magnitude;
            if (currentVelocity.magnitude > adjustedMaxSpeed)
                currentVelocity = currentVelocity.normalized * adjustedMaxSpeed;
            
            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, currentVelocity.normalized, m_rotateToVelocitySpeed * Time.deltaTime, 1));
            currentVelocity.y = previousY;
            return currentVelocity;
        }

        public void PerformJump()
        {
            //This is just so the force is always consistent even if Character mass changes
            playerRigidbody.AddForce(0, m_jumpForce, 0, ForceMode.VelocityChange);
        }
    }
}
