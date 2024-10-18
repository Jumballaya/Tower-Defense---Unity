using UnityEngine;


// https://en.wikipedia.org/wiki/B%C3%A9zier_curve
public class BezierCurve
{
    public Vector3[] points;

    public BezierCurve()
    {
        points = new Vector3[4];
    }

    public BezierCurve(Vector3[] points)
    {
        this.points = points;
    }

    public Vector3 GetSegment(float time)
    {
        time = Mathf.Clamp01(time);
        float t = 1f - time; // 1 - time is from the wiki entry for Cubic Bezier curves
        return (t * t * t * points[0])
            + (3 * t * t * time * points[1])
            + (3 * t * time * time * points[2])
            + (time * time * time * points[3]);
    }

    public Vector3[] GetSegments(int subdivisions)
    {
        Vector3[] segments = new Vector3[subdivisions];
        float t = 0;
        for (int i = 0; i < subdivisions; i++)
        {
            t = (float)i / subdivisions;
            segments[i] = GetSegment(t);
        }
        return segments;
    }

    public Vector3 Start
    {
        get
        {
            return points[0];
        }
    }

    public Vector3 End
    {
        get
        {
            return points[3];
        }
    }
}