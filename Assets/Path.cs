using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{
    [SerializeField]
    private List<Vector3> points;


    void OnDrawGizmosSelected()
    {
        Vector3 _base = transform.position;
        Vector3 start = points[0];
        for (int i = 1; i < points.Count; i++)
        {
            Vector3 end = points[i];
            Gizmos.DrawLine(_base + start, _base + end);
            start = end;
        }
    }
}
