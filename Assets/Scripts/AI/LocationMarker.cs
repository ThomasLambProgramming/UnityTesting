using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationMarker : MonoBehaviour
{
    [SerializeField] private float _bobSpeed = 4f;
    [SerializeField] private float _offsetY = 2f;
    [SerializeField] private float _heightY = 3f;
    
    private Vector3 _initialLocation = Vector3.zero;
    private void Start() 
    {
        _initialLocation = transform.position;
    }
    public void SetNewPosition(Vector3 a_location)
    {
        _initialLocation = a_location;
        _initialLocation.y += _offsetY;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.Lerp(
            _initialLocation,
            new Vector3(_initialLocation.x, _initialLocation.y + _heightY, _initialLocation.z), 
             Mathf.Abs(Mathf.Sin(Time.time * _bobSpeed)));
    }
}
