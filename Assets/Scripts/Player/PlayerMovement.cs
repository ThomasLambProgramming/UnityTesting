using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Splines;

namespace Player
{
    public enum MovementState
    {
        BaseMovement,
        HammahWay,
        SplineRiding,
        Cutscene,
    }
    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] public MovementState m_CurrentMovementState;
        [HideInInspector] public Rigidbody m_playerRigidbody;
        [HideInInspector] public PlayerInputProcessor m_playerInput;
        [HideInInspector] public PlayerCameraController m_playerCameraController;

        [Header("Base Movement Variables")] 
        [SerializeField] private float m_movementSpeed = 10f;
        public float m_maxMovementSpeed = 4f;
        [SerializeField] private float m_slowdownPercentage = 0.98f;
        [SerializeField] private float m_rotateToVelocitySpeed = 10f;
        private float m_magnitudeSpeedCutoff = 0.2f;

        [Header("Jumping Variables")] 
        [SerializeField] private float m_jumpForce = 20f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float m_groundCheckDistance;
        [SerializeField] private float m_groundCheckDelay = 0.5f;
        [SerializeField] private float m_landingStopSoftInputDelay = 1f;
        [SerializeField] private float m_landingStopHardInputDelay = 2f;
        [SerializeField] private float m_softLandingVelocityLimit = 16f;
        private const int EnvironmentLayerMask = 1;
        private bool m_isPlayerGrounded = true;
        private Coroutine m_playerGroundCheckCoroutine = null;

        [Header("HammahWay Variables")] 
        [SerializeField] private float m_HammahWayMovementSpeed;
        [SerializeField] private float m_HammahWayMaxSpeed;
        [SerializeField] private float m_HammahWayTurningSpeed;
        [SerializeField] private float m_HammahWayMaxTurningSpeed;

        [Header("Gravity Variables")] 
        [SerializeField] private float m_additionalBaseMovementGravity = 9.81f;
        [SerializeField] private float m_additionalHammahWayGravity = 9.81f;

        [Header("Spline Variables")] 
        [Tooltip("Units/second traveled on a spline")] [SerializeField] private float m_splineRideSpeed = 4f;
        [SerializeField] private Vector3 m_splineRidingOffset;
        private float m_splineRidingTimer = 0;
        private float m_splineDirection = 1.0f;
        private SplineContainer m_currentSpline;

        public void UpdateComponent()
        {
            switch (m_CurrentMovementState)
            {
                case MovementState.BaseMovement:
                case MovementState.HammahWay:
                    //All updates should be done through a intermediate Vector3 so velocity is only being set once per frame otherwise jittering occurs with the camera.
                    Vector3 newVelocity;
                    if (m_playerInput.CurrentMoveInput.magnitude < m_magnitudeSpeedCutoff)
                        newVelocity = SlowDownPlayerNoInput();
                    else
                        newVelocity = MovePlayer();

                    //if (Mathf.Abs(newVelocity.y) > 1)
                    //    newVelocity.y -= m_additionalGravity;
                    m_playerRigidbody.velocity = newVelocity;
                    break;
                case MovementState.SplineRiding:
                    RideCurrentSplineMove();
                    break;
                case MovementState.Cutscene:
                    break;
            }
        }

#region MovementStateSwapping 
        private void SetToHammahWay()
        {
            m_CurrentMovementState = MovementState.HammahWay;
            //Todo: Put reference to movehammertoriding function in playeranimator
        }
        private void SetToBaseMovement()
        {
            m_CurrentMovementState = MovementState.BaseMovement;
            //Todo: Put reference to movehammertoback function in playeranimator
        }

        /// <summary>
        /// Swap movement to hammahway or basemovement unless
        /// </summary>
        public void SwapMovement()
        {
        }
#endregion

#region MovementTypeFunctions
        /// <summary>
        /// Move the player based on their input.
        /// </summary>
        private Vector3 MovePlayer()
        {
            Vector3 velocityToApply = Vector3.zero;
            
            Vector3 currentVelocity = m_playerRigidbody.velocity;
            float previousY = currentVelocity.y;
            
            switch (m_CurrentMovementState)
            {
                case MovementState.BaseMovement:
                    velocityToApply = BaseMovementMove();
                    break;
                case MovementState.HammahWay:
                    velocityToApply = HammahWayMove();
                    break;
            }

            velocityToApply.y = previousY;
            return velocityToApply;
        }
        
        private Vector3 BaseMovementMove()
        {
            Vector3 currentVelocity = m_playerRigidbody.velocity;
            currentVelocity.y = 0;

            Transform cameraTransform = m_playerCameraController.m_mainCamera.transform;
            Vector3 cameraDirectionForward = cameraTransform.forward;
            Vector3 cameraDirectionRight = cameraTransform.right;
            
            Vector3 movementRight = new Vector3(cameraDirectionRight.x, 0, cameraDirectionRight.z).normalized;
            Vector3 movementForward = new Vector3(cameraDirectionForward.x, 0, cameraDirectionForward.z).normalized;
            
            currentVelocity += (movementForward * m_playerInput.CurrentMoveInput.y + movementRight * m_playerInput.CurrentMoveInput.x).normalized * (m_movementSpeed * Time.deltaTime);

            //Reduce max speed so controller is able to walk.
            float adjustedMaxSpeed = m_maxMovementSpeed * m_playerInput.CurrentMoveInput.magnitude;
            if (currentVelocity.magnitude > adjustedMaxSpeed)
                currentVelocity = currentVelocity.normalized * adjustedMaxSpeed;

            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, currentVelocity.normalized, m_rotateToVelocitySpeed * Time.deltaTime, 1));
            return currentVelocity;
        }

        private Vector3 HammahWayMove()
        {
            Vector3 currentVelocity = m_playerRigidbody.velocity;
            currentVelocity.y = 0;
            
            Vector3 movementForward = transform.forward;
            movementForward.y = 0;
            
            if (Mathf.Abs(m_playerInput.CurrentMoveInput.x) > 0.1f)
            {
                Quaternion angleToRotate = Quaternion.AngleAxis(m_playerInput.CurrentMoveInput.x * m_HammahWayTurningSpeed * Time.deltaTime, Vector3.up);
                movementForward = angleToRotate * movementForward;
                movementForward.y = 0;
                movementForward = movementForward.normalized;
            }

            currentVelocity += (movementForward) * (m_playerInput.CurrentMoveInput.y * m_HammahWayMovementSpeed * Time.deltaTime);

            float adjustedMaxSpeed = m_HammahWayMaxSpeed * m_playerInput.CurrentMoveInput.y;
            if (currentVelocity.magnitude > adjustedMaxSpeed)
                currentVelocity = currentVelocity.normalized * adjustedMaxSpeed;

            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, movementForward, m_HammahWayMaxTurningSpeed * Time.deltaTime, 1));
            return currentVelocity;
        }

        private void RideCurrentSplineMove()
        {
            m_splineRidingTimer += Time.deltaTime * m_splineDirection;
            //length of 32 / 4 riding speed = 8 seconds, being total duration required. timer / total duration = % complete goes straight into spline get position. 
            float splinePosition = m_splineRidingTimer / (m_currentSpline.CalculateLength() / m_splineRideSpeed);
            transform.position = (Vector3)m_currentSpline.EvaluatePosition(splinePosition) + m_splineRidingOffset;

            //the 0.9 >= is to account for floating point errors.
            if (m_splineDirection >= 0.9f && splinePosition > 0.98f)
            {
                m_CurrentMovementState = MovementState.BaseMovement;
            }

            if (m_splineDirection <= -0.9f && splinePosition < 0.02f)
            {
                m_CurrentMovementState = MovementState.BaseMovement;
            }
        }

        /// <summary>
        /// Take a percentage of the players current horizontal velocity away to slow them down when no input is given
        /// </summary>
        private Vector3 SlowDownPlayerNoInput()
        {
            Vector3 currentVelocity = m_playerRigidbody.velocity;
            float previousY = currentVelocity.y;
            currentVelocity.y = 0;
            currentVelocity *= m_slowdownPercentage;
            currentVelocity.y = previousY;
            return currentVelocity;
        }
#endregion

#region SplineMovement

        public void StartSplineRiding(SplineContainer splineContainer)
        {
            m_currentSpline = splineContainer;
            m_CurrentMovementState = MovementState.SplineRiding;
            float positionOnSpline = GetCharacterPositionOnSpline();

            Vector3 forwardPosition = m_currentSpline.EvaluatePosition(positionOnSpline + 0.1f);
            Vector3 backwardPosition = m_currentSpline.EvaluatePosition(positionOnSpline - 0.1f);

            Vector3 cameraForwardDirection = m_playerCameraController.m_mainCamera.transform.forward;
            float dotForward = Vector3.Dot((forwardPosition - (Vector3)m_currentSpline.EvaluatePosition(positionOnSpline)).normalized, cameraForwardDirection);
            float dotBackward = Vector3.Dot((backwardPosition - (Vector3)m_currentSpline.EvaluatePosition(positionOnSpline)).normalized, cameraForwardDirection);

            m_splineDirection = dotForward > dotBackward ? 1.0f : -1.0f;

            m_splineRidingTimer = positionOnSpline * (m_currentSpline.CalculateLength() / m_splineRideSpeed);
        }

        private float GetCharacterPositionOnSpline()
        {
            //Thank god there was a get nearest point, i was going to do some hellish binary search.
            SplineUtility.GetNearestPoint(m_currentSpline.Spline, m_currentSpline.transform.InverseTransformPoint(transform.position), out float3 nearestPoint, out float splinePosition, 8, 4);
            return splinePosition;
        }

#endregion

#region JumpingAndGroundCheckingFunction

        public void PerformJump()
        {
            if (m_CurrentMovementState == MovementState.SplineRiding)
            {
                m_CurrentMovementState = MovementState.BaseMovement;
                //Probably need to add a previous direction and current position so i can just add a force to the player at a certain speed so it
                //seems like the player gets launched off the rail.
                return;
            }

            //This is just so the force is always consistent even if Character mass changes
            m_playerRigidbody.AddForce(0, m_jumpForce, 0, ForceMode.VelocityChange);
        }

        IEnumerator LandingInputDelay(bool softLanding)
        {
            //m_stopPlayerWASDInput = true;
            yield return new WaitForSeconds(softLanding ? m_landingStopSoftInputDelay : m_landingStopHardInputDelay);
            //m_stopPlayerWASDInput = false;
        }

        //Raycast immediately finds ground when first jumping. adding delay to stop this.
        IEnumerator GroundCheckDelay()
        {
            yield return new WaitForSeconds(m_groundCheckDelay);
            m_isPlayerGrounded = false;
            m_playerGroundCheckCoroutine = null;
        }

        private void GroundCheck()
        {
            //If the object is on the environment layer (not the non-jump environment layer)
            if (Physics.Raycast(groundCheck.position, Vector3.down, m_groundCheckDistance, EnvironmentLayerMask))
            {
                m_isPlayerGrounded = true;

                if (m_playerRigidbody.velocity.y > -m_softLandingVelocityLimit)
                {
                    StartCoroutine(LandingInputDelay(true));
                }
                else
                {
                    StartCoroutine(LandingInputDelay(false));
                }
            }
        }

#endregion

#region InputProcessing
        public void SetupInputCallbacks(ref PlayerInput playerInput)
        {
            playerInput.Default.SwapMovement.performed += SwapMovementInputStart;
            playerInput.Default.SwapMovement.canceled += SwapMovementInputEnd;
            playerInput.Default.Jump.performed += JumpInputStart;
            playerInput.Default.Jump.canceled += JumpInputEnd;
        }
        public void RemoveInputCallbacks(ref PlayerInput playerInput)
        {
            playerInput.Default.SwapMovement.performed -= SwapMovementInputStart;
            playerInput.Default.SwapMovement.canceled -= SwapMovementInputEnd;
            playerInput.Default.Jump.performed -= JumpInputStart;
            playerInput.Default.Jump.canceled -= JumpInputEnd;
        }
        
        private void SwapMovementInputStart(InputAction.CallbackContext callback)
        {
            SwapMovement();
        }
        private void SwapMovementInputEnd(InputAction.CallbackContext callback)
        {
            
        }
        private void JumpInputStart(InputAction.CallbackContext callback)
        {
            PerformJump();
        }
        private void JumpInputEnd(InputAction.CallbackContext callback)
        {
            
        }

#endregion

#region DebuggingFunctions
        private void OnDrawGizmos()
        {
            Gizmos.color = Color.cyan;
            Gizmos.DrawSphere(groundCheck.position, m_groundCheckDistance);
            Gizmos.color = Color.gray;
        }
#endregion
    }
}
