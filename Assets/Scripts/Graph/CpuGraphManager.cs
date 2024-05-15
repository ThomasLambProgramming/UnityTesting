using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Graph
{
    public class CpuGraphManager : MonoBehaviour
    {
        private Transform[] m_GraphPoints;
        [SerializeField] private GameObject m_GraphPrefab;
        [SerializeField] private float m_GraphStep = 2f;
        [SerializeField] private int m_GraphCount;
        [SerializeField] private float m_GraphPointScale = 0.1f;
        [SerializeField] private float m_TimeMultiplier = 1.5f;
        [SerializeField] private bool m_UpdateGraphOnFrame = true;

        [SerializeField] private float m_TransitionDuration = 1f;
        [SerializeField] private float m_FunctionDuration = 3f;
        [SerializeField] private float m_FunctionTimer = 0;
        [SerializeField] private float m_TransitionTimer = 0;
        [SerializeField] private bool m_IsInTransition = false;

        [SerializeField] private LibraryFunctions.WaveFunctions m_Function = LibraryFunctions.WaveFunctions.Wave;
        [SerializeField] private LibraryFunctions.WaveFunctions m_TransitionFunction = LibraryFunctions.WaveFunctions.Wave;
        private void Start()
        {
            CreateGraphArray();
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
                
                UpdateGraphArray();
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
            if (m_GraphPoints != null)
                return;
            //Graph count = amount in x and z
            m_GraphPoints = new Transform[m_GraphCount * m_GraphCount];
            for (int i = 0; i < m_GraphPoints.Length; i++)
            {
                m_GraphPoints[i] = Instantiate(m_GraphPrefab, transform).transform;
            }
            UpdateGraphArray();
        }

        private void UpdateGraphArray()
        {
            float time = Time.time;
            LibraryFunctions.GraphFunction graphFunction = LibraryFunctions.GetFunction(m_Function);
            LibraryFunctions.GraphFunction graphPreviousFunction = LibraryFunctions.GetFunction(m_TransitionFunction);
            //What the fuck. didn't know this was a thing. cool.
            for (int i = 0, x = 0, z = 0; i < m_GraphPoints.Length; i++, x++)
            {
                if (x == m_GraphCount)
                {
                    x = 0;
                    z++;
                }

                //Attempt at making the grid be centered around the 0,0,0 instead of going off 0 then continuing positively
                float halfOffset = -m_GraphCount * m_GraphStep / 2 + 1;
                
                float u = halfOffset + (x + 0.5f) * m_GraphStep - 1f;
                float v = halfOffset + (z + 0.5f) * m_GraphStep - 1f;

                if (m_IsInTransition)
                    m_GraphPoints[i].transform.localPosition = LibraryFunctions.LerpTwoPoints(u, v, time, graphPreviousFunction, graphFunction, m_TransitionTimer / m_TransitionDuration);
                else
                    m_GraphPoints[i].transform.localPosition = graphFunction(u,v,time * m_TimeMultiplier);
                
                m_GraphPoints[i].transform.localScale = new Vector3(m_GraphPointScale, m_GraphPointScale, m_GraphPointScale);
            }
        }

        private void RearrangeGraphArray()
        {
            int index = 0;
            if (transform.childCount != 0)
            {
                m_GraphPoints = new Transform[m_GraphCount * m_GraphCount];
                for (int i = 0; i < m_GraphPoints.Length; i++)
                {
                    if (i > transform.childCount - 1)
                        break;
                    
                    m_GraphPoints[i] = transform.GetChild(i);
                    index++;
                }
            }

            if (index != m_GraphCount * m_GraphCount - 1)
            {
                for (int i = index; i < m_GraphCount * m_GraphCount; i++)
                {
                    m_GraphPoints[i] = Instantiate(m_GraphPrefab, transform).transform;
                }
            }


            m_GraphPoints = null;
        }
    }
}