using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Graph
{
    public class GpuGraphManager : MonoBehaviour
    {
        [SerializeField] private float m_GraphStep = 2f;
        [SerializeField] private int m_GraphCount;
        [SerializeField] private float m_GraphPointScale = 0.1f;
        [SerializeField] private float m_TimeMultiplier = 1.5f;
        [SerializeField] private bool m_UpdateGraphOnFrame = true;

        [SerializeField] private float m_TransitionDuration = 1f;
        [SerializeField] private float m_FunctionDuration = 3f;
        private float m_FunctionTimer = 0;
        private float m_TransitionTimer = 0;
        private bool m_IsInTransition = false;

        [SerializeField] private LibraryFunctions.WaveFunctions m_Function = LibraryFunctions.WaveFunctions.Wave;
        private LibraryFunctions.WaveFunctions m_TransitionFunction = LibraryFunctions.WaveFunctions.Wave;

        private ComputeBuffer m_PositionsBuffer;
        private void OnEnable()
        {
            //4 byte floats, xyz positions. = 12 byte stride.
            m_PositionsBuffer = new ComputeBuffer(m_GraphCount * m_GraphCount, 12);
            CreateGraphArray();
        }

        private void OnDisable()
        {
            m_PositionsBuffer.Release();
            m_PositionsBuffer = null;
        }

        private void Update()
        {
            if (m_UpdateGraphOnFrame)
            {
                m_FunctionTimer += Time.deltaTime;

                if (m_FunctionTimer > m_FunctionDuration)
                {
                    m_IsInTransition = true;
                    m_TransitionTimer = 0;
                    m_FunctionTimer = 0;
                    
                    m_TransitionFunction = m_Function;
                    m_Function = m_Function == LibraryFunctions.WaveFunctions.Torus ? LibraryFunctions.WaveFunctions.Wave : (LibraryFunctions.WaveFunctions)(m_Function + 1);
                }

                if (m_IsInTransition)
                {
                    m_TransitionTimer += Time.deltaTime;
                    if (m_TransitionTimer > m_TransitionDuration)
                    {
                        m_TransitionTimer = 0;
                        m_IsInTransition = false;
                    }
                }
            }
        }

        [ContextMenu("Remake Graph Array")]
        private void RemakeGraphArray()
        {
            RearrangeGraphArray();
            CreateGraphArray();
        }
        private void CreateGraphArray()
        {
        }

        private void UpdateGraphArray()
        {
        }

        private void RearrangeGraphArray()
        {
        }
    }
}