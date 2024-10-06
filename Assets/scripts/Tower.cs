using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct TowerPiece
{
    public List<TowerPart> layers;
}

public class Tower : MonoBehaviour
{
    public Transform buildSpot;
    public GameObject weapon;
    private Transform nextBuildSpot;

    [Range(0, 3)]
    public int level;

    public List<TowerPiece> pieces = new();


    public void IncLevel()
    {
        level += 1;
        level %= pieces.Count;
        BuildTower();
    }

    public void DecLevel()
    {
        if (level == 0)
        {
            level = pieces.Count - 1;
        }
        else
        {
            level -= 1;
        }
        BuildTower();
    }


    void Start()
    {
        BuildTower();
        nextBuildSpot = buildSpot;
    }

    void BuildTower()
    {
        ClearBuilding();
        BuildPiece(level);
        BuildWeapon();
    }

    private void BuildPiece(int idx)
    {
        if (idx < 0 || idx >= pieces.Count)
        {
            return;
        }
        Transform spot = buildSpot;
        for (int i = 0; i < pieces[idx].layers.Count; i++)
        {
            var child = Instantiate(pieces[idx].layers[i], spot);
            spot = child.buildSpot;
        }
        nextBuildSpot = spot;
    }

    private void BuildWeapon()
    {
        if (weapon != null)
        {
            Instantiate(weapon, nextBuildSpot);
        }
    }

    private void ClearBuilding()
    {
        Debug.Log("Start Clearing");
        foreach (Transform child in buildSpot.transform)
        {
            Destroy(child.gameObject, 0.5f);
        }
    }
}
