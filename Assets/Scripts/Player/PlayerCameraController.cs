using Cinemachine;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Separate Camera controller to manage when the player can move the camera themselves or if it is during a cutscene.
    /// </summary>
    public class PlayerCameraController : MonoBehaviour
    {
        public Camera m_mainCamera;
        
        //We want to rotate the camera around the player from far away so making a pivot point is easier.
        [SerializeField] private float m_cameraRotateSpeedX;
        [SerializeField] private float m_cameraRotateSpeedY;

        [SerializeField] private float m_controllerScalingX;
        [SerializeField] private float m_controllerScalingY;

        //It over rotates when first loading in, possibly due to delta time being large or input values being wrong on setup.
        [SerializeField] private float m_delayInputAtStartDuration = 1.0f;
        private float m_delayInputAtStartTimer = 0;

        //Where do we want the camera to follow to.
        [SerializeField] private CinemachineFreeLook m_freelookCamera;
        [SerializeField] private bool m_stopTrackingPlayer = false;
        
        private void Start()
        {
            m_mainCamera = Camera.main;
        }

        public void UpdateCamera(Vector2 playerInput, bool fromController)
        {
            if (m_delayInputAtStartTimer < m_delayInputAtStartDuration)
            {
                m_delayInputAtStartTimer += Time.deltaTime;
                return;
            }
            
            if (m_stopTrackingPlayer)
                return;
            
            m_freelookCamera.m_YAxis.Value += playerInput.y * Time.deltaTime * (fromController ? m_cameraRotateSpeedX * m_controllerScalingX : m_cameraRotateSpeedX);
            m_freelookCamera.m_XAxis.Value += playerInput.x * Time.deltaTime * (fromController ? m_cameraRotateSpeedY * m_controllerScalingY : m_cameraRotateSpeedY);
        }
    }
}