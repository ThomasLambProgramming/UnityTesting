/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalInput : MonoBehaviour
{
    public static GlobalInput Instance = null;
    private PlayerInput Input;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
        {
            Debug.LogError("More than one Global Input. Fix!");
        }

        Input = new PlayerInput();
        Input.Enable();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void InteractInput()
    {
        
    }
}
*/
