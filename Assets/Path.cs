using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    public List<Vector3> points;
    public float smoothingRadius = 2f;
    public int smoothingSections = 10;

    private BezierCurve[] curves;

    private Vector3[] path;

    void OnValidate()
    {
        GenerateCurves();
        GeneratePath();
    }

    void OnDrawGizmosSelected()
    {
        if (path.Length < 1)
        {
            return;
        }
        Vector3 start = transform.position + path[0];
        for (int i = 1; i < path.Length; i++)
        {
            Vector3 end = transform.position + path[i];
            Gizmos.DrawLine(start, end);
            start = end;
        }
    }

    private void GenerateCurves()
    {
        if (points.Count < 1)
        {
            return;
        }
        curves = new BezierCurve[points.Count - 1];
        Vector3 start = points[0];
        for (int i = 1; i < points.Count; i++)
        {
            Vector3 end = points[i];
            float halfHeight = (start.y - end.y) * 0.5f;
            Vector3 offset = Vector3.up * halfHeight;
            Vector3 t1 = start;
            Vector3 t2 = end;
            curves[i - 1] = new BezierCurve(new Vector3[] { start, t1 + offset, t2 - offset, end });
            start = end;
        }
    }

    private void GeneratePath()
    {
        if (curves.Length < 1)
        {
            return;
        }
        path = new Vector3[curves.Length * smoothingSections];
        for (int i = 1; i < curves.Length; i++)
        {
            Vector3[] segments = curves[i].GetSegments(smoothingSections);
            for (int j = 0; j < segments.Length; j++)
            {
                int idx = (i * smoothingSections) + j;
                path[idx] = segments[j];
            }
        }
    }
}
