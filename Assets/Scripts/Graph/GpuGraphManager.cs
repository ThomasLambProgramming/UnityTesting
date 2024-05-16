using UnityEngine;

namespace Graph
{
    public class GpuGraphManager : MonoBehaviour
    {
        [SerializeField] private ComputeShader m_ComputeShader;
        [SerializeField] private int m_Resolution;
        [SerializeField] private Material m_Material;
        [SerializeField] private Mesh m_Mesh;

        [SerializeField] private float m_TransitionDuration = 1f;
        [SerializeField] private float m_FunctionDuration = 3f;
        private float m_FunctionTimer = 0;
        private float m_TransitionTimer = 0;
        private bool m_IsInTransition = false;

        [SerializeField] private LibraryFunctions.WaveFunctions m_Function = LibraryFunctions.WaveFunctions.Wave;

        private ComputeBuffer m_PositionsBuffer;

        private int kernalId = 0;
        private static readonly int
            positionsId = Shader.PropertyToID("_Positions"),
            resolutionId = Shader.PropertyToID("_Resolution"),
            stepId = Shader.PropertyToID("_Step"),
            timeId = Shader.PropertyToID("_Time");
        
        //private LibraryFunctions.WaveFunctions m_TransitionFunction = LibraryFunctions.WaveFunctions.Wave;
        private void OnEnable()
        {
            //4 byte floats, xyz positions. = 12 byte stride.
            m_PositionsBuffer = new ComputeBuffer(m_Resolution * m_Resolution, 12);
            kernalId = m_ComputeShader.FindKernel("FunctionKernel");
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
            m_ComputeShader.SetBuffer(0, positionsId, m_PositionsBuffer);
            //8,8,1 threads on shader so we divide resolution by the same amount
            int groupAmount = Mathf.CeilToInt(m_Resolution / 8f);
            m_ComputeShader.Dispatch(kernalId,groupAmount,groupAmount,1);

            //the bounds acts as a big bounding box for the drawing of these meshes for frustum culling, since the graph is small it should fit within a small box
            Bounds bounds = new Bounds(Vector3.zero, Vector3.one * (2f + 2f / m_Resolution));
            Graphics.DrawMeshInstancedProcedural(m_Mesh, 0, m_Material, bounds, m_PositionsBuffer.count);
        }

        private void Update()
        {
            UpdateFunctionOnGpu();
            if (true)
            {
                return;
                m_FunctionTimer += Time.deltaTime;

                if (m_FunctionTimer > m_FunctionDuration)
                {
                    m_IsInTransition = true;
                    m_TransitionTimer = 0;
                    m_FunctionTimer = 0;
                    
                    //m_TransitionFunction = m_Function;
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