using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class SiegeWeapon : MonoBehaviour
{

    public Mesh attachment;

    public Transform rotatePoint;

    void Start()
    {
        rotatePoint.GetComponent<MeshFilter>().mesh = attachment;
    }
}
