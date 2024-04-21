using System;
using UnityEngine;

namespace Graph
{
    public class GraphManager : MonoBehaviour
    {
        [SerializeField] private GameObject m_GraphPrefab;
        private GameObject[] m_GraphPoints;
        [SerializeField] private float step = 2f;
        [Range(10, 150)]
        [SerializeField] private int m_GraphCount;

        private int m_previousGraphCount;

        [Range(0.01f, 3)]
        [SerializeField] private float m_GraphPointScale = 0.1f;

        [SerializeField] private float m_TimeMultiplier = 1.5f;
        private void Start()
        {
            CreateGraphArray();
            m_previousGraphCount = m_GraphCount;
        }

        private void Update()
        {
            if (m_previousGraphCount != m_GraphCount)
            {
                DestroyGraphArray();
                CreateGraphArray();
                Debug.Log("Graph count not the same");
                m_previousGraphCount = m_GraphCount;
            }
            for (int i = 0; i < m_GraphPoints.Length; i++)
            {
                m_GraphPoints[i].transform.localScale = new Vector3(m_GraphPointScale, m_GraphPointScale, m_GraphPointScale);
                Vector3 localPos = m_GraphPoints[i].transform.localPosition;
                //Local pos setting of x acts as an offset to make curve, sin time*pi = 2 seconds for full change? something like that
                localPos.y = Mathf.Sin(Mathf.PI * (localPos.x + Time.time * m_TimeMultiplier));
                localPos.x = (i + 0.5f) * step - 1f;
                m_GraphPoints[i].transform.localPosition = localPos;
            }
        }

        private void CreateGraphArray()
        {
            m_GraphPoints = new GameObject[m_GraphCount];
            for (int i = 0; i < m_GraphPoints.Length; i++)
            {
                m_GraphPoints[i] = Instantiate(m_GraphPrefab, this.transform);
                m_GraphPoints[i].transform.position = new Vector3((i + 0.5f) * step - 1f, 0, 0);
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