using System;
using UnityEngine;

[Serializable]
public enum NodeOrientation
{
    ZeroDegrees,
    NinetyDegrees,
    OneEightyDegrees,
    NegativeNinetyDegrees, // same as 270 degrees
};

public class Node : MonoBehaviour
{
    [Header("Base Attributes")]
    public bool canBuildHere = false;
    public int elevation = 0;
    public NodeOrientation orientiation = 0;

    [Header("Addon Spots")]
    public Transform buildSpot;

    private Vector3 position;
    private Quaternion rotation;

    private int lastElevation;
    private NodeOrientation lastOrientation;


    void Start()
    {
        lastElevation = elevation;
        lastOrientation = orientiation;
        position = new Vector3(transform.position.x, elevation * 0.2f, transform.position.z);
        rotation = Quaternion.Euler(0f, 0f, GetOrientationDegrees());
        transform.SetPositionAndRotation(position, rotation);
    }

    void FixedUpdate()
    {
        bool updated = false;
        if (elevation != lastElevation)
        {
            position.Set(transform.position.x, elevation * 0.2f, transform.position.z);
            updated = true;
        }
        if (orientiation != lastOrientation)
        {
            rotation = Quaternion.Euler(0f, GetOrientationDegrees(), 0f);
            updated = true;
        }
        if (updated)
        {
            transform.SetPositionAndRotation(position, rotation);
        }
    }

    private float GetOrientationDegrees()
    {
        switch (orientiation)
        {
            case NodeOrientation.ZeroDegrees:
                {
                    return 0f;
                }
            case NodeOrientation.NinetyDegrees:
                {
                    return 90f;
                }
            case NodeOrientation.OneEightyDegrees:
                {
                    return 180f;
                }
            case NodeOrientation.NegativeNinetyDegrees:
                {
                    return -90f;
                }
        }
        return 0f;
    }
}
