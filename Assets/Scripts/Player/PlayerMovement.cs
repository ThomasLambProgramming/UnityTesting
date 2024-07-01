using System.Collections;
using UI;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
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
        [SerializeField] private float m_baseMovementTurningSpeed = 0.4f;

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
        [SerializeField] private float m_hammahWayMaxTurningAngle;
        
        [Header("Hammer Suspension Variables")]
        [SerializeField] private Transform m_leftHammerSide;
        [SerializeField] private Transform m_rightHammerSide;
        [SerializeField] private Transform m_leftHammerWheel;
        [SerializeField] private Transform m_rightHammerHalfTransform;
        [SerializeField] private float m_springRestDistance = 1;
        [SerializeField] private float m_springStrength = 10f;
        [SerializeField] private float m_springDamping = 5f;
        [SerializeField] private float m_wheelCheckDistance = 4f;
        [SerializeField] private float m_wheelGripFactor = 0.80f;
        [SerializeField] private float m_wheelMass = 3.0f;
        [SerializeField] private float m_correctiveRotationSpeed = 10f;

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
                    BeginBaseMovementState();
                    break;
                case MovementState.HammahWay:
                    BeginHammahWayState();
                    break;
                case MovementState.SplineRiding:
                    BeginBaseMovementState();
                    break;
            }
        }

        public void UpdateComponent()
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
                    HammahWayMove();
                    break;
            }

            velocityToApply.y = previousY;
            //return velocityToApply;
            
            switch (m_CurrentMovementState)
            {
                case MovementState.BaseMovement:
                    m_playerRigidbody.velocity = PlayerInputProcessor.Instance.CurrentMoveInput.magnitude > m_magnitudeSpeedCutoff ? BaseMovementMove() : SlowDownPlayerNoInput();
                    break;
                case MovementState.HammahWay:
                    HammahWayMove();
                    break;
                case MovementState.SplineRiding:
                    UpdateCurrentSplineMove();
                    break;
                case MovementState.Cutscene:
                    break;
            }
            
            if ((m_CurrentMovementState != MovementState.Cutscene || m_CurrentMovementState != MovementState.SplineRiding) && !m_isPlayerGrounded)
                GroundCheckUpdate();
        }

        #region MovementStateSwapping
        private void BeginHammahWayState()
        {
            Debug.Log("SetToHammahWay");
            if (m_CurrentMovementState == MovementState.SplineRiding)
                StartCorrectingRotationFromSpline();

            m_CurrentMovementState = MovementState.HammahWay;
            m_playerAnimator.MoveHammerToRiding();
            m_playerAnimator.GotoHammahWayState(0.2f);
            m_playerRigidbody.useGravity = true;
            
            Vector3 heightOffset = Vector3.up;
            m_leftHammerSide.position = m_leftHammerWheel.position + heightOffset;
            m_rightHammerSide.position = m_rightHammerHalfTransform.position + heightOffset;
            m_leftHammerWheel.parent = m_leftHammerSide;
            m_rightHammerHalfTransform.parent = m_rightHammerSide;
        }

        private void BeginBaseMovementState()
        {
            Debug.Log("SetToBaseMovement");
            if (m_CurrentMovementState == MovementState.SplineRiding)
                StartCorrectingRotationFromSpline();

            m_CurrentMovementState = MovementState.BaseMovement;
            m_playerAnimator.MoveHammerToBack();
            m_playerAnimator.GotoBaseMovementState(0.2f);
            m_playerRigidbody.useGravity = true;
        }

        private void BeginSplineRidingState()
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
                BeginHammahWayState();
            else
                BeginBaseMovementState();
        }
        #endregion

        #region MovementTypeFunctions

        private Vector3 BaseMovementMove()
        {
            Vector3 currentVelocity = m_playerRigidbody.velocity;
            float previousYVel = currentVelocity.y;
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
            
            transform.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(Mathf.Clamp(Vector3.SignedAngle(transform.forward, currentVelocity.normalized, Vector3.up) * m_baseMovementTurningSpeed * Time.deltaTime, -m_hammahWayMaxTurningAngle, m_hammahWayMaxTurningAngle), Vector3.up) * transform.forward, Vector3.up);
            
            currentVelocity.y = previousYVel;
            return currentVelocity;
        }

        private void HammahWayMove()
        {
            Vector3 heightOffset = Vector3.up;

            if (Mathf.Abs(PlayerInputProcessor.Instance.CurrentMoveInput.x) > 0.01f)
            {
                Vector3 playerForward = transform.forward;
                Vector3 wheelForward = m_leftHammerSide.forward;
                Vector3 currentTurningAmount = Quaternion.AngleAxis(PlayerInputProcessor.Instance.CurrentMoveInput.x * m_HammahWayTurningSpeed * Time.deltaTime, Vector3.up) * wheelForward;
                currentTurningAmount = currentTurningAmount.normalized;

                float angleBetween = Vector3.Angle(playerForward, currentTurningAmount);
                //if (angleBetween > m_hammahWayMaxTurningAngle)
                //    currentTurningAmount = Quaternion.AngleAxis(Mathf.Sign(PlayerInputProcessor.Instance.CurrentMoveInput.x) * m_hammahWayMaxTurningAngle, Vector3.up) * playerForward;

                m_leftHammerSide.Rotate(new Vector3(0, PlayerInputProcessor.Instance.CurrentMoveInput.x * m_HammahWayTurningSpeed * Time.deltaTime,0));
                m_rightHammerSide.Rotate(new Vector3(0, PlayerInputProcessor.Instance.CurrentMoveInput.x * m_HammahWayTurningSpeed * Time.deltaTime,0));
            }

            ProcessWheel(m_leftHammerSide, m_leftHammerSide.position - heightOffset);
            ProcessWheel(m_rightHammerSide, m_rightHammerSide.position - heightOffset);

            Vector3 currentVel = m_playerRigidbody.velocity;
            currentVel.y = 0;
            transform.rotation = Quaternion.RotateTowards(transform.rotation, Quaternion.LookRotation(currentVel.normalized, Vector3.up), m_correctiveRotationSpeed * Time.deltaTime);
        }

        void ProcessWheel(Transform wheelTransform, Vector3 actualWheelPosition)
        {
            Vector3 currentVelocity = m_playerRigidbody.velocity;
            float currentYVelocity = currentVelocity.y;
            currentVelocity.y = 0;
            Vector3 wheelCheckDirection = -Vector3.up;

            if (Physics.Raycast(wheelTransform.position, wheelCheckDirection, out RaycastHit hitInformation, m_wheelCheckDistance, 1 << LayerMask.NameToLayer("Default")))
            {
                WheelSuspensionForce(actualWheelPosition, hitInformation);
                //Make sure that the current velocity isn't 0 so friction isn't applied to it.
                if (currentVelocity.sqrMagnitude > 0.01f)
                    WheelSteeringForce(wheelTransform.right, actualWheelPosition);
                WheelAccelerationForce(wheelTransform.forward, actualWheelPosition);
            }
        }

        void WheelSuspensionForce(Vector3 wheelPosition, RaycastHit hitInformation)
        {
            Vector3 springDirection = Vector3.up;
            Vector3 hammerVelocityAtWheel = m_playerRigidbody.GetPointVelocity(wheelPosition);
            float offset = m_springRestDistance - hitInformation.distance;
            float projectedVelocity = Vector3.Dot(springDirection, hammerVelocityAtWheel);
            float force = (offset * m_springStrength) - (projectedVelocity * m_springDamping);
            m_playerRigidbody.AddForceAtPosition(springDirection * force, wheelPosition);
        }

        void WheelSteeringForce(Vector3 wheelRight, Vector3 wheelPosition)
        {
            Vector3 steeringDirection = wheelRight;
            Vector3 wheelVelocity = m_playerRigidbody.GetPointVelocity(wheelPosition);
            float steeringVelocity = Vector3.Dot(steeringDirection, wheelVelocity);
            float desiredVelocityChance = -steeringVelocity * m_wheelGripFactor;
            float desiredAcceleration = desiredVelocityChance / Time.deltaTime;
            m_playerRigidbody.AddForceAtPosition(steeringDirection * (m_wheelMass * desiredAcceleration), wheelPosition);
        }

        void WheelAccelerationForce(Vector3 wheelForward, Vector3 wheelPosition)
        {
            Vector3 accelerationDir = wheelForward;
            if (Mathf.Abs(PlayerInputProcessor.Instance.CurrentMoveInput.y) > 0.01f)
            {
                float hammerSpeed = Vector3.Dot(wheelForward, m_playerRigidbody.velocity);
                float normalizedSpeed = Mathf.Clamp01(Mathf.Abs((hammerSpeed)) / m_HammahWayMaxSpeed);
                //This needs to be changed to a powercurve / lookup curve so the speed is not just a linear force but it has difference in acceleration at all stages of speed.
                float avaliableTorque = 0.70f; //powerCurve.evaluate(normalizedspeed) * accelInput;
                
                m_playerRigidbody.AddForceAtPosition(Mathf.Sign(PlayerInputProcessor.Instance.CurrentMoveInput.y) * accelerationDir * (avaliableTorque * m_HammahWayMovementSpeed), wheelPosition);
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
                BeginBaseMovementState();
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
            BeginSplineRidingState();
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
                BeginBaseMovementState();
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

        //d_ is for debugging variables (so it is possible to search for all debugging related variables / to note not to use these variables for core gameplay)
        private Vector3 d_leftWheelUpDirection;
        private Vector3 d_rightWheelUpDirection;
        
        private Vector3 d_leftWheelRaycastHitNormal;
        private Vector3 d_rightWheelRaycastHitNormal;
        
        private Vector3 d_leftWheelRaycastHitLocation;
        private Vector3 d_rightWheelRaycastHitLocation;

        //Draw both the rest position and the difference currently from the ground or above from goal.
        private Vector3 d_leftWheelRestPosition;
        private Vector3 d_rightWheelRestPosition;

        private Vector3 d_leftWheelForceApplied;
        private Vector3 d_rightWheelForceApplied;

        private Vector3 d_leftWheelDesiredAcceleration;
        private Vector3 d_rightWheelDesiredAcceleration;

        private Vector3 d_leftWheelDesiredVelocity;
        private Vector3 d_rightWheelDesiredVelocity;
        
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
