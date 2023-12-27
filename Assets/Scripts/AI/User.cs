/*using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public enum SteeringState
{
    Seek = 1,
    Flee,
    Arrive,
    Wander,
    Pursuit,
    Evade
}

public class User : MonoBehaviour
{
    [SerializeField] private float _cameraMoveSpeed = 5f;
    [SerializeField] private float _cameraRotateSpeedX = 5f;
    [SerializeField] private float _cameraRotateSpeedY = 5f;
    [SerializeField] private LocationMarker _markerObject = null;
    [SerializeField] private List<Agent> _agents = new List<Agent>();

    [SerializeField] private Camera _playerCamera = null;
    private Vector2 _movementInput = Vector2.zero;
    private bool _allowCameraRotation = false;

    private PlayerInput _playerInput = null;

    private void Awake()
    {
        _playerInput = new PlayerInput();
        EnableInput();
    }

    private void EnableInput()
    {
        _playerInput.Enable();
        _playerInput.Player.Alpha1.performed += StartAlpha1;
        _playerInput.Player.Alpha2.performed += StartAlpha2;
        _playerInput.Player.Alpha3.performed += StartAlpha3;
        _playerInput.Player.Alpha4.performed += StartAlpha4;
        _playerInput.Player.Alpha5.performed += StartAlpha5;
        _playerInput.Player.Alpha6.performed += StartAlpha6;
        _playerInput.Player.Alpha7.performed += StartAlpha7;
        _playerInput.Player.Alpha8.performed += StartAlpha8;
        _playerInput.Player.Alpha9.performed += StartAlpha9;
        _playerInput.Player.Alpha0.performed += StartAlpha0;
        _playerInput.Player.LeftClick.performed += StartLeftClick;
        _playerInput.Player.RightClick.performed += StartRightClick;
        _playerInput.Player.Movement.performed += StartMovementInput;
        _playerInput.Player.Mouse.performed += StartMouseMovement;
        _playerInput.Player.Escape.performed += StartEscapeInput;

        _playerInput.Player.LeftClick.canceled += EndLeftClick;
        _playerInput.Player.RightClick.canceled += EndRightClick;
        _playerInput.Player.Movement.canceled += EndMovementInput;
        _playerInput.Player.Escape.canceled += EndEscapeInput;
    }

    private void DisableInput()
    {
        _playerInput.Player.Alpha1.performed -= StartAlpha1;
        _playerInput.Player.Alpha2.performed -= StartAlpha2;
        _playerInput.Player.Alpha3.performed -= StartAlpha3;
        _playerInput.Player.Alpha4.performed -= StartAlpha4;
        _playerInput.Player.Alpha5.performed -= StartAlpha5;
        _playerInput.Player.Alpha6.performed -= StartAlpha6;
        _playerInput.Player.Alpha7.performed -= StartAlpha7;
        _playerInput.Player.Alpha8.performed -= StartAlpha8;
        _playerInput.Player.Alpha9.performed -= StartAlpha9;
        _playerInput.Player.Alpha0.performed -= StartAlpha0;
        _playerInput.Player.LeftClick.performed -= StartLeftClick;
        _playerInput.Player.RightClick.performed -= StartRightClick;
        _playerInput.Player.Movement.performed -= StartMovementInput;
        _playerInput.Player.Mouse.performed -= StartMouseMovement;
        _playerInput.Player.Escape.performed -= StartEscapeInput;

        _playerInput.Player.LeftClick.canceled -= EndLeftClick;
        _playerInput.Player.RightClick.canceled -= EndRightClick;
        _playerInput.Player.Movement.canceled -= EndMovementInput;
        _playerInput.Player.Escape.canceled -= EndEscapeInput;
    }

    void Update()
    {
        UserMovement();
    }
    
    private void UserMovement()
    {
        if (_movementInput == Vector2.zero)
            return;
        
        Vector3 _moveDirection = 
            _movementInput.x * _playerCamera.transform.right +
            _movementInput.y * _playerCamera.transform.forward;
        
        _moveDirection = _moveDirection.normalized;
        _playerCamera.transform.position += _moveDirection * (_cameraMoveSpeed * Time.deltaTime);
    }

    private void StartAlpha1(InputAction.CallbackContext a_context)
    {
        Debug.Log("Agent state set to Seek");
        foreach(Agent agent in _agents)
        {
            agent._currentState = SteeringState.Seek;
        }
    }
    private void StartAlpha2(InputAction.CallbackContext a_context)
    {
        Debug.Log("Agent state set to Flee");
        foreach(Agent agent in _agents)
        {
            agent._currentState = SteeringState.Flee;
        }
    }
    private void StartAlpha3(InputAction.CallbackContext a_context)
    {
        Debug.Log("Agent state set to Arrive");
        foreach(Agent agent in _agents)
        {
            agent._currentState = SteeringState.Arrive;
        }
    }
    private void StartAlpha4(InputAction.CallbackContext a_context)
    {
        Debug.Log("Agent state set to Wander");
        foreach(Agent agent in _agents)
        {
            agent._currentState = SteeringState.Wander;
        }
    }
    private void StartAlpha5(InputAction.CallbackContext a_context)
    {
        Debug.Log("Agent state set to Pursuit");
        foreach(Agent agent in _agents)
        {
            agent._currentState = SteeringState.Pursuit;
        }
    }
    private void StartAlpha6(InputAction.CallbackContext a_context)
    {
        Debug.Log("Agent state set to Evade");
        foreach(Agent agent in _agents)
        {
            agent._currentState = SteeringState.Evade;
        }
    }
    private void StartAlpha7(InputAction.CallbackContext a_context)
    {
        
    }
    private void StartAlpha8(InputAction.CallbackContext a_context)
    {
        
    }
    private void StartAlpha9(InputAction.CallbackContext a_context)
    {
        
    }
    private void StartAlpha0(InputAction.CallbackContext a_context)
    {
        
    }
    private void StartLeftClick(InputAction.CallbackContext a_context)
    {
        Ray ray = _playerCamera.ScreenPointToRay(Mouse.current.position.ReadValue());

        RaycastHit hit = new RaycastHit();
            
        if (Physics.Raycast(ray, out hit, 1000))
        {
            if (hit.transform.CompareTag("Ground"))
            {
                _markerObject.SetNewPosition(hit.point);

                foreach(Agent agent in _agents)
                {
                    agent.SeekPosition(hit.point);
                }
            }
        }
    }
    private void EndLeftClick(InputAction.CallbackContext a_context)
    {
        
    }
    private void StartRightClick(InputAction.CallbackContext a_context)
    {
        
    }
    private void EndRightClick(InputAction.CallbackContext a_context)
    {
        
    }
    private void StartMouseMovement(InputAction.CallbackContext a_context)
    {
        if (_allowCameraRotation)
        {
            float mouseX = Input.GetAxis("Mouse X") * _cameraRotateSpeedX;
            float mouseY = Input.GetAxis("Mouse Y") * -_cameraRotateSpeedY;

            
            //Didn't understand how to do the rotation of the camera so just found it online
            
            Quaternion rotation = Camera.main.transform.rotation;
            Quaternion horiz = Quaternion.AngleAxis(mouseX, Vector3.up);
            Quaternion vert = Quaternion.AngleAxis(mouseY, Vector3.right);
            Camera.main.transform.rotation = horiz * rotation * vert;
            //Yeah dont know how the multiplication works with that, I will study this later.
        }
    }
    private void StartMovementInput(InputAction.CallbackContext a_context)
    {
        _movementInput = a_context.ReadValue<Vector2>();
    }
    private void EndMovementInput(InputAction.CallbackContext a_context)
    {
        _movementInput = Vector2.zero;
    }

    private void StartEscapeInput(InputAction.CallbackContext a_context)
    {
        _allowCameraRotation = !_allowCameraRotation;
        Debug.Log("Allow Rotation is currently " + _allowCameraRotation);
    }
    private void EndEscapeInput(InputAction.CallbackContext a_context)
    {
        
    }
}
*/