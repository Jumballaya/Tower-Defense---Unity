using System;
using UnityEditor.Rendering.Universal;
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
    private NodeManager nodeManager;
    private BuildManager buildManager;

    private Tower tower = null;


    void Start()
    {
        position = new Vector3(transform.position.x, elevation * 0.2f, transform.position.z);
        rotation = Quaternion.Euler(0f, 0f, GetOrientationDegrees());
        transform.SetPositionAndRotation(position, rotation);
        nodeManager = NodeManager.GetInstance();
        buildManager = BuildManager.GetInstance();
    }

    void OnEnable() => NodeManager.GetInstance().AddNode(this);
    void OnDisable() => NodeManager.GetInstance().RemoveNode(this);

    void OnValidate()
    {
        position.Set(transform.position.x, elevation * 0.2f, transform.position.z);
        rotation = Quaternion.Euler(0f, GetOrientationDegrees(), 0f);
        transform.SetPositionAndRotation(position, rotation);
    }

    void OnMouseDown()
    {
        if (!canBuildHere) return;
        buildManager.ShowMenu(transform);
        nodeManager.SelectNode(this);
    }

    public void BuildOrUpgrade()
    {
        if (tower == null)
        {
            Build();
            return;
        }
        Upgrade();
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

    private void Build()
    {
        GameObject towerObj = TowerManager.GetInstance().CreateTower(buildSpot);
        tower = towerObj.GetComponent<Tower>();
    }

    private void Upgrade()
    {
        tower.Upgrade();
    }
}
