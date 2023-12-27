using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BezierSpline : MonoBehaviour
{
    [SerializeField] public Vector3[] points;

    [SerializeField] private BezierControlPointMode[] modes;
    public int ControlPointCount {get {return points.Length;}}

    public BezierControlPointMode GetControlPointMode(int index)
    {
        return modes[(index + 1) / 3];
    }
    public void SetControlPointMode(int index, BezierControlPointMode mode)
    {
        modes[(index + 1) / 3] = mode;
        EnforceMode(index);
    }
    public Vector3 GetControlPoint(int a_index)
    {
        return points[a_index];
    }

    public void SetControlPoint(int a_index, Vector3 point) 
    {
        if (a_index % 3 == 0)
        {
            Vector3 delta = point - points[a_index];
            if (a_index > 0)
            {
                points[a_index - 1] += delta;
            }
            if (a_index + 1 < points.Length)
            {
                points[a_index + 1] += delta;
            }
        }

        points[a_index] = point;
        EnforceMode(a_index);
    }
    private void EnforceMode(int index)
    {
        int modeIndex = (index + 1) / 3;
        BezierControlPointMode mode = modes[modeIndex];
        if (mode == BezierControlPointMode.Free || modeIndex == 0 || modeIndex == modes.Length - 1)
            return;

        int middleIndex = modeIndex * 3;
        int fixedIndex, enforcedIndex;

        if (index <= middleIndex)
        {
            fixedIndex = middleIndex - 1;
            enforcedIndex = middleIndex + 1;
        }
        else
        {
            fixedIndex = middleIndex + 1;
            enforcedIndex = middleIndex -1;
        }
        Vector3 middle = points[middleIndex];
        Vector3 enforcedTangent = middle - points[fixedIndex];

        if (mode == BezierControlPointMode.Aligned)
        {
            enforcedTangent = enforcedTangent.normalized * Vector3.Distance(middle, points[enforcedIndex]);
        }

        points[enforcedIndex] = middle + enforcedTangent;
    }
    public void AddCurve()
    {
        //Resize array to have more and fill new elements with new points from the last array element
        Vector3 point = points[points.Length - 1];
        Array.Resize(ref points, points.Length + 3);
        point.x += 1f;
        points[points.Length - 3] = point;
        point.x += 1f;
        points[points.Length - 2] = point;
        point.x += 1f;
        points[points.Length - 1] = point;

        Array.Resize(ref modes, modes.Length + 1);
        modes[modes.Length - 1] = modes[modes.Length - 2];
    }
    public int CurveCount() => (points.Length - 1) / 3;
    public Vector3 GetPoint (float t) 
    {
        //this i and t business here just determines what curve index we are currently on
        //so the one time float can be used to cross all splines with just 0-1
        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount();
            i = (int)t;
            t -= i;
            i *= 3;
        }
		return transform.TransformPoint(Bezier.GetPoint(points[i], points[i + 1], points[i + 2], points[i + 3], t));
	}

    public Vector3 GetVelocity(float t) 
    {
        //get first derivate gives a velocity vector so we dont need the current position of the object and we 
        //want the world position of the points to know its velocity direction in world space only

        //small note about transformdirection while im looking at it, the vector that direction takes will be the same length
        //but transform point will apply the scale as well

        int i;
        if (t >= 1f)
        {
            t = 1f;
            i = points.Length - 4;
        }
        else
        {
            t = Mathf.Clamp01(t) * CurveCount();
            i = (int)t;
            t -= i;
            i *= 3;
        }
        return transform.TransformPoint(Bezier.GetFirstDerivate(points[i], points[i + 1], points[i + 2], points[i + 3], t)) - transform.position;
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
        modes = new BezierControlPointMode[]
        {
            BezierControlPointMode.Free,
            BezierControlPointMode.Free
        };
    }
}
