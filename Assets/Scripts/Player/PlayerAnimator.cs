using UnityEngine;

namespace Player
{
    public class PlayerAnimator : MonoBehaviour
    {
        [HideInInspector] public Animator m_animator;
        [HideInInspector] public PlayerMovement m_playerMovement;
        [SerializeField] private float m_animatorLerpSpeed = 7;
        
        [Header("Attaching Sockets for hammer")] 
        [SerializeField] private GameObject m_hammerWayObject;
        [SerializeField] private Transform m_backAttachSocket;
        [SerializeField] private Transform m_handAttachSocket;
        [SerializeField] private Transform m_groundAttachSocket;

        //Animator Ids (doing string comparison is 100% going to be an issue at some point)
        private readonly int SpeedAnimatorId = Animator.StringToHash("Speed");
        private readonly int DanceAnimatorId = Animator.StringToHash("Dance");
        private readonly int JumpStartAnimatorId = Animator.StringToHash("JumpStart");
        private readonly int FallingAnimatorId = Animator.StringToHash("Falling");
        private readonly int SoftLandAnimatorId = Animator.StringToHash("SoftLand");
        private readonly int HardLandAnimatorId = Animator.StringToHash("HardLand");
        private readonly int AttackHorizontalAnimatorId = Animator.StringToHash("AttackHorizontal");
        private readonly int ResetToBaseMovementAnimatorId = Animator.StringToHash("ResetToBaseMovement");
        private readonly int HoldingSplineAnimatorId = Animator.StringToHash("SplineRiding");

        private float m_previousAnimatorSpeedValue = 0;
        
        public void UpdateComponent()
        {
            Vector3 currentVel = m_playerMovement.m_playerRigidbody.velocity;
            currentVel.y = 0;
            float currentHorizontalVelocity = currentVel.magnitude / m_playerMovement.m_maxMovementSpeed;
            float animatorSpeedValue = Mathf.Lerp(m_previousAnimatorSpeedValue, currentHorizontalVelocity, m_animatorLerpSpeed * Time.deltaTime);
            m_previousAnimatorSpeedValue = animatorSpeedValue;

            //for now just disable the movement.
            if (m_playerMovement.m_CurrentMovementState == MovementState.HammahWay)
            {
                animatorSpeedValue = 0;
                GotoBaseMovementState(0.2f);
            }

            m_animator.SetFloat(SpeedAnimatorId, animatorSpeedValue);
        }

        public void GotoBaseMovementState(float transitionDuration)
        {
            m_animator.CrossFade("BaseMovementTree", transitionDuration);
        }
        public void GotoHammahWayState(float transitionDuration)
        {
            m_animator.CrossFade("BaseMovementTree", transitionDuration);
        }
        public void GotoSplineMovementState(float transitionDuration)
        {
            m_animator.CrossFade("SplineRiding", transitionDuration);
        }
        public void GotoJumpStartState(float transitionDuration)
        {
            
        }
        public void GotoJumpFallState(float transitionDuration)
        {
            
        }

        public void GotoJumpLandState(bool hardLanding, float transitionDuration)
        {
            m_animator.CrossFade(hardLanding ? "LandingHard" : "LandingSoft", transitionDuration);
        }

        public void PlayAttackAnimation()
        {
            
        }

        public void MoveHammerToBack()
        {
            m_hammerWayObject.transform.parent = m_backAttachSocket;
            m_hammerWayObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            m_hammerWayObject.transform.localPosition = Vector3.zero;
            m_hammerWayObject.GetComponentInChildren<Collider>().enabled = false;
        }
        public void MoveHammerToHand()
        {
            m_hammerWayObject.transform.parent = m_handAttachSocket;
            m_hammerWayObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            m_hammerWayObject.transform.localPosition = Vector3.zero;
            m_hammerWayObject.GetComponentInChildren<Collider>().enabled = false;
        }
        public void MoveHammerToRiding()
        {
            m_hammerWayObject.transform.parent = m_groundAttachSocket;
            m_hammerWayObject.transform.localRotation = Quaternion.Euler(0, 0, 0);
            m_hammerWayObject.transform.localPosition = Vector3.zero;
            m_hammerWayObject.GetComponentInChildren<Collider>().enabled = true;
        }
    }
}