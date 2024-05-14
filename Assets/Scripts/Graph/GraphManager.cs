using System;
using UnityEngine;

namespace Graph
{
    public class GraphManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_GraphPrefab;
        private GameObject[] m_GraphPoints;
        [SerializeField] private float step = 2f;
        [SerializeField] private int m_GraphCount;
        [SerializeField] private float m_GraphPointScale = 0.1f;
        [SerializeField] private float m_TimeMultiplier = 1.5f;
        [SerializeField] private bool m_RemakeGraph = false;

        [SerializeField, Range(0.01f, 20)] private float m_TimeOffsetForMultiwave;
        [SerializeField] private LibraryFunctions.WaveFunctions m_Function = LibraryFunctions.WaveFunctions.Wave;
        private void Start()
        {
            CreateGraphArray();
        }

        private void Update()
        {
            if (m_GraphPoints == null)
            {
                m_GraphPoints = new GameObject[transform.childCount];
                for (int i = 0; i < transform.childCount; i++)
                {
                    m_GraphPoints[i] = transform.GetChild(i).gameObject;
                }
                m_RemakeGraph = true;
            }
            if (m_RemakeGraph)
            {
                RemakeGraphArray();
                m_RemakeGraph = false;
            }
            for (int i = 0; i < m_GraphPoints.Length; i++)
            {
                Vector3 localPos = m_GraphPoints[i].transform.localPosition;
                switch (m_Function)
                {
                    case LibraryFunctions.WaveFunctions.Wave:
                        localPos.y = LibraryFunctions.Wave(localPos.x, Time.time * m_TimeMultiplier);
                        break;
                    case LibraryFunctions.WaveFunctions.MultiWave:
                        localPos.y = LibraryFunctions.MultiWave(localPos.x, m_TimeOffsetForMultiwave, Time.time * m_TimeMultiplier);
                        break;
                    case LibraryFunctions.WaveFunctions.Ripple:
                        localPos.y = LibraryFunctions.Ripple(localPos.x, Time.time * m_TimeMultiplier);
                        break;
                }
                m_GraphPoints[i].transform.localPosition = localPos;
            }
        }

        [ContextMenu("Remake Graph Array")]
        private void RemakeGraphArray()
        {
            DestroyGraphArray();
            CreateGraphArray();
        }
        private void CreateGraphArray()
        {
            m_GraphPoints = new GameObject[m_GraphCount];
            for (int i = 0; i < m_GraphPoints.Length; i++)
            {
                m_GraphPoints[i] = Instantiate(m_GraphPrefab, this.transform);
                float xOffset = -m_GraphCount * step / 2 + 1;
                m_GraphPoints[i].transform.position = new Vector3(xOffset + (i + 0.5f) * step - 1f, 0, 0);
                m_GraphPoints[i].transform.localScale = new Vector3(m_GraphPointScale, m_GraphPointScale, m_GraphPointScale);
            }
        }
        private void DestroyGraphArray()
        {
            for (int i = 0; i < m_GraphPoints.Length; i++)
            {
                Destroy(m_GraphPoints[i].gameObject);
            }
        }
    }
}