using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class SteeringBehaviours
{
    private Transform _agentTransform = null;
    private Rigidbody _agentRb = null;
    private Rigidbody _targetRb = null;
    private Vector3 _targetPosition = Vector3.zero;
    private float _maxSpeed = 5f;
    public void SetAgentRb(Rigidbody a_rigid) => _agentRb = a_rigid;
    public void SetTargetRb(Rigidbody a_rigid) => _targetRb = a_rigid;
    public void SetTargetLocation(Vector3 a_pos) => _targetPosition = a_pos;
    public void SetSpeed(float a_maxSpeed) => _maxSpeed = a_maxSpeed;
    public void SetAgentTransform(Transform a_transform) => _agentTransform = a_transform;
    
    public float _moveSpeed = 0;
    
    
    //The following behaviours are marked down as 1,2,3,4,5 etc on the controls for the user.
    public Vector3 Seek(float a_maxSpeed)
    {
        Vector3 desiredVel = (_targetPosition - _agentTransform.position).normalized * a_maxSpeed;
        //Remember left is goal vector, right is the one we want to find out how to get to the left from
        return desiredVel - _agentRb.velocity;
    }
    //Made a second one with a parameter overload
    public Vector3 Seek(Vector3 a_seekPosition, float a_maxSpeed)
    {
        Vector3 desiredVel = (a_seekPosition - _agentTransform.position).normalized * a_maxSpeed;
        //Remember left is goal vector, right is the one we want to find out how to get to the left from
        return desiredVel - _agentRb.velocity;
    }

    public Vector3 Flee(float a_maxSpeed)
    {
        Vector3 desiredVel = (_agentTransform.position - _targetPosition).normalized * a_maxSpeed;
        //Remember left is goal vector, right is the one we want to find out how to get to the left from
        return desiredVel - _agentRb.velocity;
    } 
    public Vector3 Flee(Vector3 a_fleePosition, float a_maxSpeed)
    {
        Vector3 desiredVel = (_agentTransform.position - a_fleePosition).normalized * a_maxSpeed;
        //Remember left is goal vector, right is the one we want to find out how to get to the left from
        return desiredVel - _agentRb.velocity;
    } 
    public Vector3 Arrive(float a_decelRate)
    {
        Vector3 toTarget = _targetPosition - _agentTransform.position;
        float distance = toTarget.magnitude;

        if (distance > 0)
        {
            //This is just a variable to help with the scaling and values of the decel
            float decelerationTweaker = 0.6f;

            //calculate the speed required to reach the target
            float velNeeded = distance / (decelerationTweaker * a_decelRate);

            if (velNeeded > _maxSpeed)
                velNeeded = _maxSpeed;

            return toTarget * (velNeeded / distance);
        }
        return Vector3.zero;
    }
    
    public Vector3 Pursue(float a_maxSpeed)
    {
        Vector3 toTarget = _targetPosition - _agentTransform.position;

        float relativeHeading = Vector3.Dot(_agentRb.velocity.normalized, _targetRb.velocity.normalized);

        //Acos(0.95) = 18 degrees
        //small note i did not know that is how you get angles from the dot product, very useful for designers variable setting.
        
        //this is a check that the agent is moving at least toward the target and the target is heading straight for the agent
        //then we do not need to look ahead and instead just seek the target
        if (Vector3.Dot(toTarget, _agentRb.velocity.normalized) > 0 && relativeHeading < -0.95)
        {
            return Seek(a_maxSpeed);
        }

        float lookAheadAmount = toTarget.magnitude / (_maxSpeed + _targetRb.velocity.magnitude);

        return Seek(_targetPosition + _targetRb.velocity * lookAheadAmount, a_maxSpeed);
    }
    public Vector3 Evade(float a_maxSpeed)
    {
        //the heading chekcing is not needed for the evade function
        Vector3 toTarget = _targetPosition - _agentTransform.position;

        float lookAheadAmount = toTarget.magnitude / (_maxSpeed + _targetRb.velocity.magnitude);

        return Flee(_targetPosition + _targetRb.velocity * lookAheadAmount, a_maxSpeed);
    }

    //This is to get a time to wait for the steering behaviour to turn around to face the current target (think how long a tank takes to turn around)
    public float TurnAroundTime()
    {
        Vector3 toTarget = (_targetPosition - _agentTransform.position).normalized;

        float dot = Vector3.Dot(_agentRb.velocity.normalized, toTarget);

        /*
        There was a whole explaination of this however
        get a coefficient for the turning rate of the vehicle (make it a param probably)
        0.5 = this function will return a time of 1 second for the vehicle to turn around
        */
        float coefficientScale = 0.5f;

        //since dot will give 1 if forward then no time is really needed
        //but if -1 then it will scale accordingly
        return (dot - 1) * -coefficientScale;
    }





    //radius of the constraining circle
    public float _wanderRadius = 1.0f;

    //distance away from the agent for wandering
    public float _wanderDistance = 2.0f;

    //maximum amount of displacement that can be added to the target each second
    public float _wanderJitter = 1.0f;
    public Vector3 _newWanderPosition;
    //Target to wander too
    public Vector2 _wanderTarget = Vector2.zero;
    public Vector3 Wander()
    {
        _wanderTarget.x += UnityEngine.Random.Range(-1.0f, 1.0f) * _wanderJitter;
        _wanderTarget.y += UnityEngine.Random.Range(-1.0f, 1.0f) * _wanderJitter;
           
        //Turn into direction
        _wanderTarget = _wanderTarget.normalized;

        //make the vector the same size as the circle we want for the wander
        _wanderTarget *= _wanderRadius;

        //make a position that is based on the position of the agent then move it forward by
        //the wander distance then add the random offset to seek too
        Vector3 wanderPosition = _agentTransform.position;
        wanderPosition += _agentTransform.transform.forward * _wanderDistance;
        
        wanderPosition.x += _wanderTarget.x;
        wanderPosition.z += _wanderTarget.y;
        _newWanderPosition = wanderPosition;
        return wanderPosition - _agentTransform.position;
    }
}
