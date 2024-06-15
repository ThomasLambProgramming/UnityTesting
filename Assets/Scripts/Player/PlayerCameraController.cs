using System.Collections;
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

        [SerializeField] private float m_zoomSpeed = 1f;
        [SerializeField] private float m_minZoomRadius;
        [SerializeField] private float m_maxZoomRadius;
        [SerializeField] private float m_zoomLerpSpeed = 3f;
        
        private float m_currentCameraBottomRigRadius;
        private float m_currentCameraMiddleRigRadius;
        private float m_currentCameraTopRigRadius;
        
        private float m_previousCameraBottomRigRadius;
        private float m_previousCameraMiddleRigRadius;
        private float m_previousCameraTopRigRadius;


        private bool m_allowZoom = false;
        
        private void Start()
        {
            m_mainCamera = Camera.main;
            m_allowZoom = false;
            StartCoroutine(DelayZoom());

            m_currentCameraTopRigRadius = m_freelookCamera.m_Orbits[0].m_Radius;
            m_currentCameraMiddleRigRadius = m_freelookCamera.m_Orbits[1].m_Radius;
            m_currentCameraBottomRigRadius = m_freelookCamera.m_Orbits[2].m_Radius;
            
            m_previousCameraTopRigRadius = m_currentCameraTopRigRadius;
            m_previousCameraMiddleRigRadius = m_currentCameraMiddleRigRadius;
            m_previousCameraBottomRigRadius = m_currentCameraBottomRigRadius;
        }

        public void UpdateCamera()
        {
            if (m_delayInputAtStartTimer < m_delayInputAtStartDuration)
            {
                m_delayInputAtStartTimer += Time.deltaTime;
                return;
            }
            
            if (m_stopTrackingPlayer)
                return;

            float xAxisMultiplier = (PlayerInputProcessor.Instance.MouseInputFromController ? SettingsData.Instance.Data.MouseYSens * m_controllerScalingY : SettingsData.Instance.Data.MouseYSens);
            xAxisMultiplier /= m_rotateYSettingScaling;
            float xAxisInputValue = PlayerInputProcessor.Instance.CurrentMouseInput.x * Time.deltaTime * xAxisMultiplier;
            if (SettingsData.Instance.Data.InvertXLook)
                xAxisInputValue = -xAxisInputValue;
            
            float yAxisMultiplier = (PlayerInputProcessor.Instance.MouseInputFromController ? SettingsData.Instance.Data.MouseXSens * m_controllerScalingX : SettingsData.Instance.Data.MouseXSens);
            yAxisMultiplier /= m_rotateXSettingScaling;
            float yAxisInputValue = PlayerInputProcessor.Instance.CurrentMouseInput.y * Time.deltaTime * yAxisMultiplier;
            if (SettingsData.Instance.Data.InvertYLook)
                yAxisInputValue = -xAxisInputValue;

            float currentZoomInput = Time.deltaTime * m_zoomSpeed * -PlayerInputProcessor.Instance.CurrentZoomInput.y;
            
            if (currentZoomInput != 0 && m_allowZoom)
            {
                m_currentCameraTopRigRadius = Mathf.Clamp(Mathf.Lerp(m_previousCameraTopRigRadius, m_currentCameraTopRigRadius + currentZoomInput, Time.deltaTime * m_zoomLerpSpeed), m_minZoomRadius, m_maxZoomRadius);
                m_currentCameraMiddleRigRadius = Mathf.Clamp(Mathf.Lerp(m_previousCameraMiddleRigRadius, m_currentCameraMiddleRigRadius + currentZoomInput, Time.deltaTime * m_zoomLerpSpeed), m_minZoomRadius, m_maxZoomRadius);
                m_currentCameraBottomRigRadius = Mathf.Clamp(Mathf.Lerp(m_previousCameraBottomRigRadius, m_currentCameraBottomRigRadius + currentZoomInput, Time.deltaTime * m_zoomLerpSpeed), m_minZoomRadius, m_maxZoomRadius);

                m_previousCameraTopRigRadius = m_currentCameraTopRigRadius;
                m_previousCameraMiddleRigRadius = m_currentCameraMiddleRigRadius;
                m_previousCameraBottomRigRadius = m_currentCameraBottomRigRadius;

                m_freelookCamera.m_Orbits[0].m_Radius = m_currentCameraTopRigRadius;
                m_freelookCamera.m_Orbits[1].m_Radius = m_currentCameraMiddleRigRadius;
                m_freelookCamera.m_Orbits[2].m_Radius = m_currentCameraBottomRigRadius;
            }

            m_freelookCamera.m_YAxis.Value += yAxisInputValue;
            m_freelookCamera.m_XAxis.Value += xAxisInputValue;
        }

        IEnumerator DelayZoom()
        {
            m_allowZoom = false;
            yield return new WaitForSeconds(1.5f);
            m_allowZoom = true;
        }
    }
}