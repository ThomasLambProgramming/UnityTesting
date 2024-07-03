using System;
using UnityEngine;

namespace Player
{
    /// <summary>
    /// This class is for storing values and giving debugging options to view without cluttering the player/other scripts
    /// </summary>
    public class PlayerDebug : MonoBehaviour
    {
        public static PlayerDebug Instance;
        private void Awake() { Instance = this; }
        [SerializeField] private bool m_displaySuspension;

        [Header("HammahWay Debug Settings")] 
        [SerializeField] private float m_sphereSize = 0.2f;
        [SerializeField] private Color m_forceColor;
        [SerializeField] private Color m_hitLocationColor;
        [SerializeField] private Color m_raycastColor;
        [SerializeField] private Color m_wheelPositionColor = Color.magenta;
        [SerializeField] private float m_axisDisplayLength = 0.2f;
        
        public Vector3[] m_wheelRaycastHitLocations = new Vector3[4];
        public Vector3[] m_wheelForcesApplied = new Vector3[4];

        public Transform[] m_wheelTransforms;
        
        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;
            if (m_displaySuspension)
                DisplaySuspension();
        }

        private void DisplaySuspension()
        {
            Gizmos.color = m_wheelPositionColor;
            for (int i = 0; i < m_wheelTransforms.Length; i++)
                Gizmos.DrawSphere(m_wheelTransforms[i].position, m_sphereSize);

            Gizmos.color = m_hitLocationColor;
            for (int i = 0; i < m_wheelTransforms.Length; i++)
                Gizmos.DrawSphere(m_wheelRaycastHitLocations[i], m_sphereSize);
            
            Gizmos.color = Color.blue;
            for (int i = 0; i < m_wheelTransforms.Length; i++)
                Gizmos.DrawLine(m_wheelTransforms[i].position, m_wheelTransforms[i].position + m_wheelTransforms[i].forward * m_axisDisplayLength);
            Gizmos.color = Color.red;
            for (int i = 0; i < m_wheelTransforms.Length; i++)
                Gizmos.DrawLine(m_wheelTransforms[i].position, m_wheelTransforms[i].position + m_wheelTransforms[i].right * m_axisDisplayLength);
            Gizmos.color = Color.yellow;
            for (int i = 0; i < m_wheelTransforms.Length; i++)
                Gizmos.DrawLine(m_wheelTransforms[i].position, m_wheelTransforms[i].position + m_wheelTransforms[i].up * m_axisDisplayLength);

            Gizmos.color = m_forceColor;
            for (int i = 0; i < m_wheelTransforms.Length; i++)
                Gizmos.DrawLine(m_wheelTransforms[i].position, m_wheelTransforms[i].position + m_wheelForcesApplied[i]);

            Gizmos.color = m_raycastColor;
            //Vector3.up is currently the raycast offset
            for (int i = 0; i < m_wheelTransforms.Length; i++)
                Gizmos.DrawLine(m_wheelTransforms[i].position + Vector3.up, m_wheelRaycastHitLocations[i]);
            
            Gizmos.color = Color.gray;
        }

        public void SetWheelTransformInformation(Transform[] wheels)
        {
            m_wheelTransforms = wheels;
        }
    }
}