using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Graph
{
    public class GraphManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_GraphPrefab;
        private Transform[] m_GraphPoints;
        [SerializeField] private float m_GraphStep = 2f;
        [SerializeField] private int m_GraphCount;
        [SerializeField] private float m_GraphPointScale = 0.1f;
        [SerializeField] private float m_TimeMultiplier = 1.5f;
        [SerializeField] private bool m_RemakeGraph = false;
        [SerializeField] private bool m_UpdateGraphOnFrame = true;

        [SerializeField, Range(0.01f, 20)] private float m_TimeOffsetForMultiwave;
        [SerializeField] private LibraryFunctions.WaveFunctions m_Function = LibraryFunctions.WaveFunctions.Wave;
        private void Start()
        {
            CreateGraphArray();
        }

        private void Update()
        {
            if (m_RemakeGraph || m_GraphPoints == null || (m_GraphPoints.Length != m_GraphCount * m_GraphCount))
            {
                RemakeGraphArray();
                m_RemakeGraph = false;
            }
            if (m_UpdateGraphOnFrame)
                UpdateGraphArray();
        }

        [ContextMenu("Remake Graph Array")]
        private void RemakeGraphArray()
        {
            DestroyGraphArray();
            CreateGraphArray();
        }
        private void CreateGraphArray()
        {
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
                
                m_GraphPoints[i].transform.localPosition = graphFunction(u,v,time * m_TimeMultiplier);;
                m_GraphPoints[i].transform.localScale = new Vector3(m_GraphPointScale, m_GraphPointScale, m_GraphPointScale);
            }
        }

        private void DestroyGraphArray()
        {
            if (transform.childCount != 0)
            {
                m_GraphPoints = new Transform[transform.childCount];
                for (int i = 0; i < transform.childCount; i++)
                    m_GraphPoints[i] = transform.GetChild(i);
            }

            if (m_GraphPoints != null)
            {
                for (int i = 0; i < m_GraphPoints.Length; i++)
                    Destroy(m_GraphPoints[i].gameObject);
            }

            m_GraphPoints = null;
        }
    }
}