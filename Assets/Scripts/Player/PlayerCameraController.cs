using Cinemachine;
using UI;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// Separate Camera controller to manage when the player can move the camera themselves or if it is during a cutscene.
    /// </summary>
    public class PlayerCameraController : MonoBehaviour
    {
        [HideInInspector] public Camera m_mainCamera;
        
        //Due to the x axis having crazy rotation + not wanting really small numbers the settings menu needs to be / by a set amount so players
        //can have higher (1-10) sens but its still scaled nicely
        [SerializeField] private float m_rotateXSettingScaling = 40;
        [SerializeField] private float m_rotateYSettingScaling = 8;

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

            float xAxisMultiplier = (fromController ? SettingsData.Instance.Data.MouseYSens * m_controllerScalingY : SettingsData.Instance.Data.MouseYSens);
            xAxisMultiplier /= m_rotateYSettingScaling;
            float xAxisInputValue = playerInput.x * Time.deltaTime * xAxisMultiplier;
            if (SettingsData.Instance.Data.InvertXLook)
                xAxisInputValue = -xAxisInputValue;
            
            float yAxisMultiplier = (fromController ? SettingsData.Instance.Data.MouseXSens * m_controllerScalingX : SettingsData.Instance.Data.MouseXSens);
            yAxisMultiplier /= m_rotateXSettingScaling;
            float yAxisInputValue = playerInput.y * Time.deltaTime * yAxisMultiplier;
            if (SettingsData.Instance.Data.InvertYLook)
                yAxisInputValue = -xAxisInputValue;
            
            
            m_freelookCamera.m_YAxis.Value += yAxisInputValue;
            m_freelookCamera.m_XAxis.Value += xAxisInputValue;
        }

        public void SetupInputCallbacks(ref PlayerInput playerInput)
        {
            
        }
        
        public void RemoveInputCallbacks(ref PlayerInput playerInput)
        {
            
        }
    }
}