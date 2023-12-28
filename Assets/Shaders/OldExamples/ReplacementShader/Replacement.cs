using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Replacement : MonoBehaviour
{
    public Shader m_ReplacementShader;  
    // Start is called before the first frame update
    private void OnEnable()
    {
        if (m_ReplacementShader != null)
        {
            //The camera will replace all shaders in the scene with the replacement one the render type configuration. so all that are labelled 
            //will be replaced.
            GetComponent<Camera>().SetReplacementShader(m_ReplacementShader, "RenderType");
        }
    }

    private void OnDisable()
    {
        GetComponent<Camera>().ResetReplacementShader();
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
