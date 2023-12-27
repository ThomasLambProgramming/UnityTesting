/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Agent : MonoBehaviour
{
    private Vector3 _target = Vector3.zero;
    
    public bool _enableMovement = false;
    [SerializeField] private SteeringBehaviours _steeringBehaviour = new SteeringBehaviours();
    public SteeringState _currentState = SteeringState.Seek;
    private Rigidbody _agentRb = null;


    //----Speed Variables----
    [SerializeField] private float _maxSpeed = 5f;
    [SerializeField] private float _maxTurningSpeed = 5f;
    [SerializeField] private float _turningSpeed = 4f;
    [SerializeField] private float _rotationSpeed = 2f;
    [SerializeField] private float _decelerationRate = 2f;
    //-----------------------

    [SerializeField] private float _stoppingDistance = 2f;

    [SerializeField] private bool _enableDebugging = true;

    void Start()
    {
        _agentRb = GetComponent<Rigidbody>();
        //_steeringBehaviour = new SteeringBehaviours();
        _steeringBehaviour.SetAgentRb(_agentRb);
        _steeringBehaviour.SetSpeed(_maxSpeed);
        _steeringBehaviour.SetAgentTransform(this.transform);
    }

    // Update is called once per frame
    void Update()
    {
        if (_enableMovement == false || Vector3.Distance(transform.position, _target) < _stoppingDistance)
            return;

        Movement();
        RotateAgent();
    }
    private void OnDrawGizmos() {
        if (_enableDebugging)
        {
            Gizmos.DrawSphere(_steeringBehaviour._newWanderPosition, 0.4f);
            Gizmos.DrawLine(_steeringBehaviour._wanderDistance * transform.forward + transform.position, transform.position);
            Gizmos.DrawWireSphere(_steeringBehaviour._wanderDistance * transform.forward + transform.position, _steeringBehaviour._wanderRadius);
        }
    }

    //Get the desired 
    private void Movement()
    {
        //Get the desiredVel from the current steering behaviour
        Vector3 desiredVelocity = Vector3.zero;
        desiredVelocity = GetDesiredVelocity();
        
        if (desiredVelocity.sqrMagnitude > (_maxTurningSpeed * _maxTurningSpeed))
        {
            desiredVelocity = desiredVelocity.normalized;
            desiredVelocity *= _maxTurningSpeed;
        }

        desiredVelocity.y = 0;
        _agentRb.velocity += desiredVelocity * (Time.deltaTime * _turningSpeed);

        if (_agentRb.velocity.sqrMagnitude > _maxSpeed * _maxSpeed)
        {
            Vector3 newVel = _agentRb.velocity.normalized;
            newVel *= _maxSpeed;
            _agentRb.velocity = newVel;
        }
    }

    private void RotateAgent()
    {
        Quaternion velocityRotation =  Quaternion.LookRotation(_agentRb.velocity);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, velocityRotation, _rotationSpeed); 
    }
    private Vector3 GetDesiredVelocity()
    {
        switch(_currentState)
        {
            case SteeringState.Seek:
                return _steeringBehaviour.Seek(_maxSpeed);

            case SteeringState.Flee:
                return _steeringBehaviour.Flee(_maxSpeed);

            case SteeringState.Arrive:
                return _steeringBehaviour.Arrive(_decelerationRate);

            case SteeringState.Wander:
                return _steeringBehaviour.Wander();

            case SteeringState.Pursuit:
                return _steeringBehaviour.Pursue(_maxSpeed);

            case SteeringState.Evade:
                return _steeringBehaviour.Evade(_maxSpeed);
        }

        //if all fails
        return Vector3.zero;
    }

    public void SeekPosition(Vector3 a_position)
    {
        _target = a_position;
        _steeringBehaviour.SetTargetLocation(a_position);
        _enableMovement = true;
    }
}
*/