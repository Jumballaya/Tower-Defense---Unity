using System;
using System.Collections.Generic;
using System.Collections;
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
    private GameObject weaponInstance;
    public Material dissolveMaterial;
    private Transform nextBuildSpot;

    [Range(0, 3)]
    public int level;

    public List<TowerPiece> pieces = new();

    private float upgradeTimer = 0f;

    public void IncLevel()
    {
        if (upgradeTimer < 2f)
        {
            return;
        }
        upgradeTimer = 0f;
        level += 1;
        level %= pieces.Count;
        RemoveChildren();
        BuildTower();
    }

    public void DecLevel()
    {
        if (upgradeTimer < 2f)
        {
            return;
        }
        upgradeTimer = 0f;
        if (level == 0)
        {
            level = pieces.Count - 1;
        }
        else
        {
            level -= 1;
        }
        StartCoroutine(DissolveTower());
    }

    void Update()
    {
        upgradeTimer += Time.deltaTime;
    }


    void Start()
    {
        BuildTower();
        nextBuildSpot = buildSpot;
    }

    private void BuildTower()
    {
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
            var child = Instantiate(pieces[idx].layers[i], buildSpot);
            child.transform.position += spot.transform.position;
            spot = child.buildSpot;
        }
        nextBuildSpot = spot;
    }

    private void BuildWeapon()
    {
        if (weapon != null)
        {
            weaponInstance = Instantiate(weapon, buildSpot);
            weaponInstance.transform.position = nextBuildSpot.position;
        }
    }

    private void RemoveChildren()
    {
        List<GameObject> objectsToDelete = new();
        if (weaponInstance)
        {
            objectsToDelete.Add(weaponInstance);
            objectsToDelete.Add(weaponInstance.transform.GetChild(0).gameObject);
        }
        foreach (Transform child in buildSpot.transform)
        {
            objectsToDelete.Add(child.gameObject);
        }
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }
    }

    // @TODO: Figure out how to get a different 't' for each object.
    private IEnumerator DissolveTower()
    {
        // Get a list of items to delete
        List<GameObject> objectsToDelete = new();
        if (weaponInstance)
        {
            objectsToDelete.Add(weaponInstance);
            objectsToDelete.Add(weaponInstance.transform.GetChild(0).gameObject);
        }
        foreach (Transform child in buildSpot.transform)
        {
            objectsToDelete.Add(child.gameObject);
        }

        // Set their material to the dissolve material
        foreach (GameObject obj in objectsToDelete)
        {
            obj.GetComponent<Renderer>().material = dissolveMaterial;
        }

        // Run the dissolve 
        yield return StartCoroutine(Dissolve());

        // Delete the items
        foreach (GameObject obj in objectsToDelete)
        {
            Destroy(obj);
        }
    }

    private IEnumerator Dissolve()
    {
        float elapse = 0f;
        float str;
        float duration = 2f;
        while (elapse < duration)
        {
            elapse += Time.deltaTime;
            str = Mathf.Lerp(0, 1, elapse / duration);
            dissolveMaterial.SetFloat("_t", str);
            yield return null;
        }
    }
}
