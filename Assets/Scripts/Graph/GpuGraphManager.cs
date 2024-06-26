﻿using UnityEngine;

namespace Graph
{
    public class GpuGraphManager : MonoBehaviour
    {
        [SerializeField] private ComputeShader m_ComputeShader;
        
        [Range(1, m_MaxResolution), SerializeField] private int m_Resolution;
        [SerializeField] private Material m_Material;
        [SerializeField] private Mesh m_Mesh;

        [SerializeField] private float m_TransitionDuration = 1f;
        [SerializeField] private float m_FunctionDuration = 3f;
        private float m_FunctionTimer = 0;
        private float m_TransitionTimer = 0;
        private bool m_IsInTransition = false;

        [SerializeField] private LibraryFunctions.WaveFunctions m_Function = LibraryFunctions.WaveFunctions.Wave;
        private LibraryFunctions.WaveFunctions m_TransitionFunction = LibraryFunctions.WaveFunctions.Wave;

        private ComputeBuffer m_PositionsBuffer;

        private const int m_MaxResolution = 1000;
        
        private int kernalId = 0;
        private static readonly int
            positionsId = Shader.PropertyToID("_Positions"),
            resolutionId = Shader.PropertyToID("_Resolution"),
            stepId = Shader.PropertyToID("_Step"),
            timeId = Shader.PropertyToID("_Time"),
            transitionProgress = Shader.PropertyToID("_TransitionProgress");
        
        private void OnEnable()
        {
            //4 byte floats, xyz positions. = 12 byte stride.
            m_PositionsBuffer = new ComputeBuffer(m_MaxResolution * m_MaxResolution, 12);
            CreateGraphArray();
        }

        private void OnDisable()
        {
            m_PositionsBuffer.Release();
            m_PositionsBuffer = null;
        }

        private void UpdateFunctionOnGpu()
        {
            float step = 2f / m_Resolution;
            m_ComputeShader.SetInt(resolutionId, m_Resolution);
            m_ComputeShader.SetFloat(stepId, step);
            m_ComputeShader.SetFloat(timeId, Time.time);
            
            //Dont need to do an extra instruction for nothing
            if (m_IsInTransition)
                m_ComputeShader.SetFloat(transitionProgress, m_TransitionTimer / m_TransitionDuration);
            
            //5 is the current function count, dont really want to add it as i am now done with this section and ill rewrite it when i come back to it.
            kernalId = (int)m_Function + (int)(m_IsInTransition ? m_TransitionFunction : m_Function) * 5;
            
            m_ComputeShader.SetBuffer(kernalId, positionsId, m_PositionsBuffer);
            //8,8,1 threads on shader so we divide resolution by the same amount
            int groupAmount = Mathf.CeilToInt(m_Resolution / 8f);
            m_ComputeShader.Dispatch(kernalId,groupAmount,groupAmount,1);

            m_Material.SetBuffer(positionsId, m_PositionsBuffer);
            m_Material.SetFloat(stepId, step);
            
            //the bounds acts as a big bounding box for the drawing of these meshes for frustum culling, since the graph is small it should fit within a small box
            Bounds bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / m_Resolution));
            Graphics.DrawMeshInstancedProcedural(m_Mesh, 0, m_Material, bounds, m_Resolution * m_Resolution);
        }

        private void Update()
        {
            UpdateFunctionOnGpu();
            if (true)
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