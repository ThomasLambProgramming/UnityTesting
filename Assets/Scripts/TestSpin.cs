using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestSpin : MonoBehaviour
{
    [SerializeField] private Transform m_LeftWheel;
    [SerializeField] private Transform m_RightWheel;

    [SerializeField] private float m_SpinSpeed = 10f;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        m_LeftWheel.transform.Rotate(new Vector3(0,0, m_SpinSpeed * Time.deltaTime));    
        m_RightWheel.transform.Rotate(new Vector3(0,0, m_SpinSpeed * Time.deltaTime));    
    }
}
