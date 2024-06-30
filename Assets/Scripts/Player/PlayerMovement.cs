using System;
using System.Collections;
using UI;
using Unity.Mathematics;
using UnityEditor.Rendering;
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
        public MovementState m_CurrentMovementState;
        [HideInInspector] public Rigidbody m_playerRigidbody;
        [HideInInspector] public PlayerCameraController m_playerCameraController;
        [HideInInspector] public PlayerAnimator m_playerAnimator;
        [HideInInspector] public InGameMenuManager m_menuManager;

        [Header("Base Movement Variables")]
        [SerializeField] private float m_movementSpeed = 10f;
        public float m_maxMovementSpeed = 4f;
        [SerializeField] private float m_slowdownPercentage = 0.98f;
        [SerializeField] private float m_rotateToVelocitySpeed = 10f;
        private float m_magnitudeSpeedCutoff = 0.2f;

        [Header("Jumping Variables")]
        private const int EnvironmentLayerMask = 1;
        [SerializeField] private float m_jumpForce = 20f;
        [SerializeField] private Transform groundCheck;
        [SerializeField] private float m_groundCheckDistance;

        //this is used to stop the groundcheck from doing raycasts till the movement has moved 110% of ground check distance in y so it doesnt give false positives
        private float m_previousYPositon;
        private float m_jumpYDistanceTraveled = 0f;
        private bool m_isPlayerGrounded = true;

        [Header("HammahWay Variables")]
        [SerializeField] private float m_HammahWayMovementSpeed;
        [SerializeField] private float m_HammahWayMaxSpeed;
        [SerializeField] private float m_HammahWayTurningSpeed;
        [SerializeField] private float m_HammahWayMaxTurningSpeed;
        
        [Header("Hammer Suspension Variables")]
        [SerializeField] private Transform m_leftHammerSide;
        [SerializeField] private Transform m_rightHammerSide;
        [SerializeField] private float m_springRestDistance = 1;
        [SerializeField] private float m_springStrength = 10f;
        [SerializeField] private float m_springDamping = 5f;
        [SerializeField] private float m_wheelCheckDistance = 4f;

        [Header("Gravity Variables")]
        [SerializeField] private float m_additionalBaseMovementGravity = 9.81f;
        [SerializeField] private float m_additionalHammahWayGravity = 9.81f;

        [Header("Spline Variables")]
        [Tooltip("Units/second traveled on a spline")]
        [SerializeField] private float m_splineRideSpeed = 4f;
        [SerializeField] private Vector3 m_splineRidingOffset;
        [SerializeField] private float m_splineCorrectiveRotationSpeed = 5f;
        [SerializeField] private float m_splineCorrectiveRotationDuration = 1.0f;
        private Coroutine m_splineCorrectiveCoroutine = null;
        private float m_splineRidingTimer = 0;
        private float m_splineDirection = 1.0f;
        private SplineContainer m_currentSpline;

        
        public void BeginPlay()
        {
            switch (m_CurrentMovementState)
            {
                case MovementState.BaseMovement:
                    SetToBaseMovement();
                    break;
                case MovementState.HammahWay:
                    SetToHammahWay();
                    break;
                case MovementState.SplineRiding:
                    SetToBaseMovement();
                    break;
            }
        }

        public void UpdateComponent()
        {
            switch (m_CurrentMovementState)
            {
                case MovementState.BaseMovement:
                    //All updates should be done through a intermediate Vector3 so velocity is only being set once per frame otherwise jittering occurs with the camera.
                    Vector3 newVelocity = 
                        PlayerInputProcessor.Instance.CurrentMoveInput.magnitude < m_magnitudeSpeedCutoff ? SlowDownPlayerNoInput() : MovePlayer();

                    if (m_isPlayerGrounded == false)
                        GroundCheckUpdate();

                    m_playerRigidbody.velocity = newVelocity;
                    break;
                case MovementState.HammahWay:
                    //All updates should be done through a intermediate Vector3 so velocity is only being set once per frame otherwise jittering occurs with the camera.
                    Vector3 hammerVelocity = MovePlayer();
                    if (m_isPlayerGrounded == false)
                        GroundCheckUpdate();
                    //m_playerRigidbody.velocity = hammerVelocity;
                    break;
                case MovementState.SplineRiding:
                    UpdateCurrentSplineMove();
                    break;
                case MovementState.Cutscene:
                    break;
            }
        }

        #region MovementStateSwapping
        private void SetToHammahWay()
        {
            Debug.Log("SetToHammahWay");
            if (m_CurrentMovementState == MovementState.SplineRiding)
                StartCorrectingRotationFromSpline();

            m_CurrentMovementState = MovementState.HammahWay;
            m_playerAnimator.MoveHammerToRiding();
            m_playerAnimator.GotoHammahWayState(0.2f);
            m_playerRigidbody.useGravity = true;
        }

        private void SetToBaseMovement()
        {
            Debug.Log("SetToBaseMovement");
            if (m_CurrentMovementState == MovementState.SplineRiding)
                StartCorrectingRotationFromSpline();

            m_CurrentMovementState = MovementState.BaseMovement;
            m_playerAnimator.MoveHammerToBack();
            m_playerAnimator.GotoBaseMovementState(0.2f);
            m_playerRigidbody.useGravity = true;
        }

        private void SetToSplineRiding()
        {
            Debug.Log("SetToSplineRiding");
            m_CurrentMovementState = MovementState.SplineRiding;
            m_playerAnimator.GotoSplineMovementState(0.2f);
            m_playerAnimator.MoveHammerToHand();
        }

        /// <summary>
        /// Swap movement to hammahway or basemovement unless
        /// </summary>
        public void SwapMovement()
        {
            if (m_menuManager.MenuActive || m_CurrentMovementState == MovementState.SplineRiding || m_CurrentMovementState == MovementState.Cutscene)
                return;

            if (m_CurrentMovementState == MovementState.BaseMovement)
                SetToHammahWay();
            else
                SetToBaseMovement();
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

            currentVelocity += (movementForward * PlayerInputProcessor.Instance.CurrentMoveInput.y + movementRight * PlayerInputProcessor.Instance.CurrentMoveInput.x).normalized * (m_movementSpeed * Time.deltaTime);

            //Reduce max speed so controller is able to walk.
            float adjustedMaxSpeed = m_maxMovementSpeed * PlayerInputProcessor.Instance.CurrentMoveInput.magnitude;
            if (currentVelocity.magnitude > adjustedMaxSpeed)
                currentVelocity = currentVelocity.normalized * adjustedMaxSpeed;

            transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, currentVelocity.normalized, m_rotateToVelocitySpeed * Time.deltaTime, 1));
            return currentVelocity;
        }

        private Vector3 HammahWayMove()
        {
            //sprintDirection = up
            //getvelocity at point for each half
            //offset = rest distance - raydistance
            //project halfpoint velocity onto spring direction
            //force = offset * springstrength - projection * springDamping
            //apply force at position.
            
            WheelSuspensionForce(m_leftHammerSide);
            WheelSuspensionForce(m_rightHammerSide);
            
            //Vector3 currentVelocity = m_playerRigidbody.velocity;
            //currentVelocity.y = 0;
            //
            //Vector3 movementForward = transform.forward;
            //movementForward.y = 0;
            //
            //if (Mathf.Abs(PlayerInputProcessor.Instance.CurrentMoveInput.x) > 0.1f)
            //{
            //    Quaternion angleToRotate = Quaternion.AngleAxis(PlayerInputProcessor.Instance.CurrentMoveInput.x * m_HammahWayTurningSpeed * Time.deltaTime, Vector3.up);
            //    movementForward = angleToRotate * movementForward;
            //    movementForward.y = 0;
            //    movementForward = movementForward.normalized;
            //}
            //
            //currentVelocity += (movementForward) * (PlayerInputProcessor.Instance.CurrentMoveInput.y * m_HammahWayMovementSpeed * Time.deltaTime);
            //
            //float adjustedMaxSpeed = m_HammahWayMaxSpeed * PlayerInputProcessor.Instance.CurrentMoveInput.y;
            //if (currentVelocity.magnitude > adjustedMaxSpeed)
            //    currentVelocity = currentVelocity.normalized * adjustedMaxSpeed;
            //
            //transform.rotation = Quaternion.LookRotation(Vector3.RotateTowards(transform.forward, movementForward, m_HammahWayMaxTurningSpeed * Time.deltaTime, 1));
            return Vector3.zero;

        }
        void WheelSuspensionForce(Transform wheelTransform)
        {
            Vector3 wheelCheckDirection = -Vector3.up;
            if (Physics.Raycast(wheelTransform.position, wheelCheckDirection, out RaycastHit hitInformation, m_wheelCheckDistance, 1 << LayerMask.NameToLayer("Default")))
            {
                Vector3 springDirection = Vector3.up;
                Vector3 hammerVelocityAtWheel = m_playerRigidbody.GetPointVelocity(wheelTransform.position);
                float offset = m_springRestDistance - hitInformation.distance;
                float projectedVelocity = Vector3.Dot(springDirection, hammerVelocityAtWheel);
                float force = (offset * m_springStrength) - (projectedVelocity * m_springDamping);
                m_playerRigidbody.AddForceAtPosition(springDirection * force, wheelTransform.position);
            }
        }

        private void UpdateCurrentSplineMove()
        {
            m_splineRidingTimer += Time.deltaTime * m_splineDirection;
            //length of 32 / 4 riding speed = 8 seconds, being total duration required. timer / total duration = % complete goes straight into spline get position.
            float splinePosition = m_splineRidingTimer / (m_currentSpline.CalculateLength() / m_splineRideSpeed);
            transform.position =
                (Vector3)m_currentSpline.EvaluatePosition(splinePosition)
                +
                //Get normal of spline through cross product of up and tangent vectors and offset using spline direction and preset offset
                Vector3.Cross(m_currentSpline.EvaluateTangent(splinePosition), m_currentSpline.EvaluateUpVector(splinePosition)).normalized * (m_splineRidingOffset.x * m_splineDirection)
                + (Vector3)m_currentSpline.EvaluateUpVector(splinePosition) * m_splineRidingOffset.y;

            Vector3 splineDirection = (Vector3)m_currentSpline.EvaluatePosition(splinePosition + 0.001f * m_splineDirection) - (Vector3)m_currentSpline.EvaluatePosition(splinePosition);
            if (splineDirection != Vector3.zero)
            {
                splineDirection = splineDirection.normalized;
                transform.rotation = Quaternion.LookRotation(splineDirection);
            }

            //Insuring that the player doesnt get moved causing jittering.
            m_playerRigidbody.velocity = Vector3.zero;
            //the 0.9 >= is to account for floating point errors and we want to cut off before the above checking distance so comparisons between spline 1.0 and 1.0 arent done giving zero vectors.
            if ((m_splineDirection >= 0.9f && splinePosition > 0.98f) || (m_splineDirection <= -0.9f && splinePosition < 0.02f))
            {
                SetToBaseMovement();
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
            SetToSplineRiding();
            m_currentSpline = splineContainer;
            float positionOnSpline = GetCharacterPositionOnSpline();

            Vector3 forwardPosition = m_currentSpline.EvaluatePosition(positionOnSpline + 0.1f);
            Vector3 backwardPosition = m_currentSpline.EvaluatePosition(positionOnSpline - 0.1f);

            Vector3 cameraForwardDirection = m_playerCameraController.m_mainCamera.transform.forward;
            float dotForward = Vector3.Dot((forwardPosition - (Vector3)m_currentSpline.EvaluatePosition(positionOnSpline)).normalized, cameraForwardDirection);
            float dotBackward = Vector3.Dot((backwardPosition - (Vector3)m_currentSpline.EvaluatePosition(positionOnSpline)).normalized, cameraForwardDirection);

            m_splineDirection = dotForward > dotBackward ? 1.0f : -1.0f;
            m_splineRidingTimer = positionOnSpline * (m_currentSpline.CalculateLength() / m_splineRideSpeed);

            m_playerRigidbody.velocity = Vector3.zero;
            //While updating the player position gravity will continue to apply so by the end of the velocity the player could already have -100s of y velocity
            m_playerRigidbody.useGravity = false;
        }

        private float GetCharacterPositionOnSpline()
        {
            //Thank god there was a get nearest point, i was going to do some hellish binary search.
            SplineUtility.GetNearestPoint(m_currentSpline.Spline, m_currentSpline.transform.InverseTransformPoint(transform.position), out float3 nearestPoint, out float splinePosition, 8, 4);
            return splinePosition;
        }

        private void StartCorrectingRotationFromSpline()
        {
            if (m_splineCorrectiveCoroutine != null)
                StopCoroutine(m_splineCorrectiveCoroutine);
            m_splineCorrectiveCoroutine = StartCoroutine(CorrectRotationFromSplineRiding());
        }

        IEnumerator CorrectRotationFromSplineRiding()
        {
            float timer = 0;
            while (timer < m_splineCorrectiveRotationDuration)
            {
                timer += Time.deltaTime;

                Vector3 forwardDirection = transform.forward;
                forwardDirection.y = 0;
                forwardDirection = forwardDirection.normalized;
                transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(forwardDirection), m_splineCorrectiveRotationSpeed * Time.deltaTime);

                yield return null;
            }

            m_splineCorrectiveCoroutine = null;
        }

        #endregion

        #region JumpingAndGroundCheckingFunction

        public void PerformJump()
        {
            if (m_menuManager.MenuActive)
                return;
            if (m_CurrentMovementState == MovementState.SplineRiding)
            {
                SetToBaseMovement();
                return;
            }

            if (m_isPlayerGrounded)
            {
                m_playerAnimator.GotoJumpStartState(0.1f);
                m_playerRigidbody.AddForce(0, m_jumpForce, 0, ForceMode.VelocityChange);
                m_jumpYDistanceTraveled = 0;
                m_isPlayerGrounded = false;
                m_previousYPositon = transform.position.y;
            }
        }

        private void GroundCheckUpdate()
        {
            //Until the player has gone up more than ground distance check so no false positives
            //or until the player is falling after coyote time (for when they go off a cliff not just on jump we dont want to raycast for ground checking.
            if (m_jumpYDistanceTraveled < m_groundCheckDistance * 1.1f)
            {
                float currentYPos = transform.position.y;
                float yDifference = currentYPos - m_previousYPositon;
                m_jumpYDistanceTraveled += Mathf.Abs(yDifference);
                m_previousYPositon = currentYPos;
            }
            else
            {
                //If the object is on the environment layer (not the non-jump environment layer)
                if (Physics.Raycast(groundCheck.position, Vector3.down, m_groundCheckDistance, EnvironmentLayerMask))
                {
                    m_isPlayerGrounded = true;
                    //for now no landing needed.
                    m_playerAnimator.GotoBaseMovementState(0.2f);
                    //if (m_playerRigidbody.velocity.y > -m_softLandingVelocityLimit)
                    //    StartCoroutine(LandingInputDelay(true));
                    //else
                    //    StartCoroutine(LandingInputDelay(false));
                }
            }
        }

        #endregion

        #region InputProcessing
        public void SetupInputCallbacks()
        {
            PlayerInputProcessor.Instance.m_playerInput.Default.SwapMovement.performed += SwapMovementInputStart;
            PlayerInputProcessor.Instance.m_playerInput.Default.SwapMovement.canceled += SwapMovementInputEnd;
            PlayerInputProcessor.Instance.m_playerInput.Default.Jump.performed += JumpInputStart;
            PlayerInputProcessor.Instance.m_playerInput.Default.Jump.canceled += JumpInputEnd;
        }

        public void RemoveInputCallbacks()
        {
            PlayerInputProcessor.Instance.m_playerInput.Default.SwapMovement.performed -= SwapMovementInputStart;
            PlayerInputProcessor.Instance.m_playerInput.Default.SwapMovement.canceled -= SwapMovementInputEnd;
            PlayerInputProcessor.Instance.m_playerInput.Default.Jump.performed -= JumpInputStart;
            PlayerInputProcessor.Instance.m_playerInput.Default.Jump.canceled -= JumpInputEnd;
        }

        private void SwapMovementInputStart(InputAction.CallbackContext callback)
        {
            SwapMovement();
        }

        private void SwapMovementInputEnd(InputAction.CallbackContext callback) { }

        private void JumpInputStart(InputAction.CallbackContext callback)
        {
            PerformJump();
        }

        private void JumpInputEnd(InputAction.CallbackContext callback) { }

        #endregion

        #region DebuggingFunctions
        private void OnDrawGizmos()
        {
            //Gizmos.color = Color.cyan;
            //Gizmos.DrawSphere(groundCheck.position, m_groundCheckDistance);
            //Gizmos.color = Color.gray;
            
            Gizmos.color = Color.green;
            Vector3 wheelCheckDirection = -Vector3.up;
            Gizmos.DrawLine(m_leftHammerSide.position, m_leftHammerSide.position + wheelCheckDirection * m_wheelCheckDistance);
            Gizmos.DrawLine(m_rightHammerSide.position, m_rightHammerSide.position + wheelCheckDirection * m_wheelCheckDistance);
            Gizmos.color = Color.gray;
        }
        #endregion
    }
}
