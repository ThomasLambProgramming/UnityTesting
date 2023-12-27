using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Bezier curves are defined by a sequence of points, it starts at first point
//and ends at the last point but does not need to go though any middle points

//instead though points pull the curve away from being a straight line
public class BezierCurve : MonoBehaviour
{
    public Vector3[] points;

    public Vector3 GetPoint (float t) {
		return transform.TransformPoint(Bezier.GetPoint(points[0], points[1], points[2], points[3], t));
	}

    public Vector3 GetVelocity(float t) 
    {
        //get first derivate gives a velocity vector so we dont need the current position of the object and we 
        //want the world position of the points to know its velocity direction in world space only

        //small note about transformdirection while im looking at it, the vector that direction takes will be the same length
        //but transform point will apply the scale as well
        return transform.TransformPoint(Bezier.GetFirstDerivate(points[0], points[1], points[2], points[3], t)) - transform.position;
    }
    public Vector3 GetDirection(float t) => GetVelocity(t).normalized;

    //When the component is reset or created this will be called by default (thats really cool)
    public void Reset()
    {
        points = new Vector3[] {
			new Vector3(1f, 0f, 0f),
			new Vector3(2f, 0f, 0f),
			new Vector3(3f, 0f, 0f),
            new Vector3(4f, 0f, 0f)
        };
    }
}



